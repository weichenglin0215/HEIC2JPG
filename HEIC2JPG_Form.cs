using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using ImageMagick;
using System.Diagnostics;

namespace HEIC2JPG
{
    public partial class HEIC2JPG : Form
    {
        private System.Windows.Forms.CheckBox checkBoxIncludeSubDirs;
        private readonly ILogger _logger;

        public HEIC2JPG()
        {
            InitializeComponent();
            InitializeListView();
            _logger = new FileLogger();
            _logger.LogInfo("應用程式啟動");
        }

        // 初始化列表視圖
        private void InitializeListView()
        {
            listViewFile.View = View.Details;
            listViewFile.Columns.Add("檔案名稱", 200);
            listViewFile.Columns.Add("目錄", 600);
        }

        // 開啟檔案按鈕點擊事件
        private void buttonOpenFiles_Click(object sender, EventArgs e)
        {
            Console.WriteLine("buttonOpenFiles_Click");
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                // 設定檔案過濾器，只顯示 HEIC 和 HEIF 檔案
                openFileDialog.Filter = "HEIC/HEIF 檔案|*.heic;*.HEIC;*.HEIF;*.heif|所有檔案|*.*";
                openFileDialog.Multiselect = true; // 允許多選檔案

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string[] files = openFileDialog.FileNames;
                    AddFileToListView(files);
                }
            }
        }

        // 開啟目錄按鈕點擊事件
        private void button_OpenDir_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog folderDialog = new FolderBrowserDialog())
            {
                if (folderDialog.ShowDialog() == DialogResult.OK)
                {
                    // 根據是否勾選「包含子目錄」來設定搜尋選項
                    SearchOption searchOption = checkBoxIncludeSubDirs.Checked ?
                        SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;

                    // 使用 GetFiles 方法一次性獲取所有 HEIC 和 HEIF 檔案
                    string[] files = Directory.GetFiles(folderDialog.SelectedPath, "*.*", searchOption)
                        .Where(file => file.EndsWith(".heic", StringComparison.OrdinalIgnoreCase) || file.EndsWith(".heif", StringComparison.OrdinalIgnoreCase))
                        .ToArray();
                    AddFileToListView(files);
                }
            }
        }

        // 將檔案加入到列表視圖
        private void AddFileToListView(string[] filePaths)
        {
            _logger.LogInfo($"開始添加檔案到列表，共 {filePaths.Length} 個檔案");
            
            listViewFile.SuspendLayout();
            foreach (string filePath in filePaths)
            {
                if (!listViewFile.Items.Cast<ListViewItem>()
                    .Any(x => Path.Combine(x.SubItems[1].Text, x.Text) == filePath))
                {
                    ListViewItem item = new ListViewItem(Path.GetFileName(filePath));
                    item.SubItems.Add(Path.GetDirectoryName(filePath));
                    listViewFile.Items.Add(item);
                    _logger.LogDebug($"添加檔案: {filePath}");
                }
                else
                {
                    _logger.LogInfo($"檔案已存在，跳過: {filePath}");
                    MessageBox.Show(filePath + " 已經存在");
                }
            }
            listViewFile.ResumeLayout();
            listViewFile.Refresh();
            buttonConvert.Enabled = true;
            statusStrip1.Items[0].Text = $"總共 {listViewFile.Items.Count} 個檔案";
            
            _logger.LogInfo($"檔案列表更新完成，共 {listViewFile.Items.Count} 個檔案");
        }

        // 轉換按鈕點擊事件
        private async void buttonConvert_Click(object sender, EventArgs e)
        {
            // 顯示進度條
            ToolStripProgressBar toolStripProgressBar1 = (ToolStripProgressBar)statusStrip1.Items[2];
            //statusStrip1.Items[2].
            toolStripProgressBar1.Visible = true;
            toolStripProgressBar1.Minimum = 0;
            toolStripProgressBar1.Maximum = listViewFile.Items.Count;
            toolStripProgressBar1.Step = listViewFile.Items.Count;
            toolStripProgressBar1.Value = 0;

            // 在启动异步任务前获取所有需要的信息
            var itemsToProcess = new List<(string FilePath, string Directory)>();
            foreach (ListViewItem item in listViewFile.Items)
            {
                itemsToProcess.Add((item.Text, item.SubItems[1].Text));
            }
            int processedCount = 0;

            // 使用非同步處理，避免 UI 卡頓
            await Task.Run(() =>
            {
                foreach (var item in itemsToProcess)
                {
                    string heicFileName = Path.Combine(item.Directory, item.FilePath);
                    Console.WriteLine(heicFileName);
                    string jpgFileName = Path.ChangeExtension(heicFileName, ".jpg");

                    // 檢查是否已經存在
                    if (File.Exists(jpgFileName))
                    {
                        // 使用Invoke确保在UI线程上执行
                        this.Invoke((MethodInvoker)delegate
                        {
                            // 顯示對話框，詢問是否覆寫
                            DialogResult result = MessageBox.Show(jpgFileName + " 已經存在，是否覆寫？", "確認覆寫", MessageBoxButtons.YesNo);
                            if (result == DialogResult.Yes)
                            {
                                ConvertHEICToJPG(heicFileName, jpgFileName);
                            }
                        });
                    }
                    else
                    {
                        this.Invoke((MethodInvoker)delegate
                        {
                            ConvertHEICToJPG(heicFileName, jpgFileName);
                        });
                    }
                    processedCount++;
                    // 更新進度條
                    this.Invoke((MethodInvoker)delegate
                    {
                        toolStripProgressBar1.Value = processedCount;
                    });
                }
            });
            // 隱藏進度條
            //toolStripProgressBar1.Visible = false;
            statusStrip1.Items[1].Text = $"轉換 {processedCount} 檔案 成功";
        }

        // 轉換 HEIC 到 JPG 的方法
        private void ConvertHEICToJPG(string inputFileName, string outputFileName)
        {
            _logger.LogInfo($"開始轉換檔案: {inputFileName}");
            
            try
            {
                this.Invoke((MethodInvoker)delegate
                {
                    using (var image = new MagickImage(inputFileName))
                    using (var outputStream = new FileStream(outputFileName, FileMode.Create))
                    {
                        image.Format = MagickFormat.Jpg;
                        image.Quality = (uint)trackBar_JpgCompressRate.Value;
                        image.Write(outputStream);
                        
                        string displayFileName = inputFileName.Length > 50
                            ? inputFileName.Substring(0, 23) + "...." + inputFileName.Substring(inputFileName.Length - 23)
                            : inputFileName;

                        statusStrip1.Items[1].Text = $"轉換 {displayFileName} 成功";
                        statusStrip1.Items[1].ToolTipText = inputFileName;
                        
                        _logger.LogInfo($"檔案轉換成功: {inputFileName} -> {outputFileName}");
                    }
                });
            }
            catch (Exception ex)
            {
                _logger.LogError($"檔案轉換失敗: {inputFileName}", ex);
                MessageBox.Show($"轉換 {inputFileName} 失敗：\n{ex.Message}", "轉換錯誤", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // 表單大小調整事件
        private void HEIC2JPG_Resize(object sender, EventArgs e)
        {
            this.listViewFile.Size = new Size(this.Width - 20, this.Height - 80 - 24);
        }

        // 列表視圖雙擊事件
        private void listViewFile_DoubleClick(object sender, EventArgs e)
        {
            if (listViewFile.SelectedItems.Count > 0)
            {
                string heicFilePath = Path.Combine(listViewFile.SelectedItems[0].SubItems[1].Text, listViewFile.SelectedItems[0].Text);
                ShowImage(heicFilePath);
            }
        }

        // 顯示圖片的方法
        private void ShowImage(string filePath)
        {
            _logger.LogInfo($"嘗試顯示圖片: {filePath}");
            
            if (!File.Exists(filePath))
            {
                _logger.LogError($"檔案不存在: {filePath}", new FileNotFoundException());
                MessageBox.Show("檔案不存在");
                return;
            }

            try
            {
                Form imageForm = new Form();
                using (var imageTmp = new MagickImage(filePath))
                {
                    imageTmp.Format = MagickFormat.Bmp;
                    PictureBox pictureBox = new PictureBox
                    {
                        Image = Bitmap.FromStream(new MemoryStream(imageTmp.ToByteArray())),
                        SizeMode = PictureBoxSizeMode.Zoom,
                        Dock = DockStyle.Fill
                    };
                    imageForm.Controls.Add(pictureBox);
                    imageForm.Text = Path.GetFileName(filePath);
                    imageForm.Size = new Size(800, 600);
                    imageForm.Show();
                    _logger.LogInfo($"圖片顯示成功: {filePath}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"圖片顯示失敗: {filePath}", ex);
                MessageBox.Show($"載入圖片 {filePath} 失敗：\n{ex.Message}", 
                    "圖片載入錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // 清除列表按鈕點擊事件
        private void button_ClearListView_Click(object sender, EventArgs e)
        {
            listViewFile.Items.Clear();
            buttonConvert.Enabled = false; // 停用轉換按鈕
        }

        // JPG 壓縮率軌跡條數值改變事件
        private void trackBar_JpgCompressRate_ValueChanged(object sender, EventArgs e)
        {
            label_JpgCompressRate.Text = trackBar_JpgCompressRate.Value.ToString();
        }

        private void buttonViewLog_ButtonClick(object sender, EventArgs e)
        {
            try
            {
                if (File.Exists("conversion.log"))
                {
                    Process.Start(new ProcessStartInfo
                    {
                        FileName = "conversion.log",
                        UseShellExecute = true
                    });
                }
                else
                {
                    MessageBox.Show("日誌檔案不存在", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("開啟日誌檔案失敗", ex);
                MessageBox.Show($"開啟日誌檔案失敗：\n{ex.Message}", "錯誤", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            _logger.LogInfo("應用程式關閉");
        }

    }

    // 建議添加結構化日誌記錄
    public interface ILogger
    {
        void LogInfo(string message);
        void LogError(string message, Exception ex);
        void LogDebug(string message);
    }

    public class FileLogger : ILogger
    {
        private readonly string _logFilePath;
        private readonly object _lockObj = new object();

        public FileLogger(string logFilePath = "conversion.log")
        {
            _logFilePath = logFilePath;
        }

        public void LogInfo(string message)
        {
            WriteLog("INFO", message);
        }

        public void LogError(string message, Exception ex)
        {
            WriteLog("ERROR", $"{message}\nException: {ex.Message}\nStackTrace: {ex.StackTrace}");
        }

        public void LogDebug(string message)
        {
            WriteLog("DEBUG", message);
        }

        private void WriteLog(string level, string message)
        {
            try
            {
                string logMessage = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] [{level}] {message}";
                lock (_lockObj)
                {
                    File.AppendAllText(_logFilePath, logMessage + Environment.NewLine);
                }
            }
            catch (Exception ex)
            {
                // 如果寫入日誌失敗，至少要在控制台顯示
                Console.WriteLine($"寫入日誌失敗: {ex.Message}");
            }
        }
    }
}
