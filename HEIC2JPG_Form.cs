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

namespace HEIC2JPG
{
    public partial class HEIC2JPG : Form
    {
        private System.Windows.Forms.CheckBox checkBoxIncludeSubDirs;

        public HEIC2JPG()
        {
            InitializeComponent();
            InitializeListView();
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
            // 先讓 listViewFile 暫停顯示，避免頻繁更新 UI
            listViewFile.SuspendLayout();
            foreach (string filePath in filePaths)
            {
                // 檢查是否已經存在於列表視圖中
                if (!listViewFile.Items.Cast<ListViewItem>()
                    .Any(x => Path.Combine(x.SubItems[1].Text, x.Text) == filePath))
                {
                    ListViewItem item = new ListViewItem(Path.GetFileName(filePath));
                    item.SubItems.Add(Path.GetDirectoryName(filePath));
                    listViewFile.Items.Add(item);
                    Console.WriteLine(Path.GetFileName(filePath));
                }
                else
                {
                    // 顯示對話框，提示檔案已存在
                    MessageBox.Show(filePath + " 已經存在");
                }
            }
            // 恢復 listViewFile 的顯示，並刷新 UI
            listViewFile.ResumeLayout();
            listViewFile.Refresh();
            buttonConvert.Enabled = true; // 啟用轉換按鈕
        }

        // 轉換按鈕點擊事件
        private async void buttonConvert_Click(object sender, EventArgs e)
        {
            // 顯示進度條
            progressBar.Visible = true;
            progressBar.Minimum = 0;
            progressBar.Maximum = listViewFile.Items.Count;
            progressBar.Value = 0;

            // 使用非同步處理，避免 UI 卡頓
            await Task.Run(() =>
            {
                int processedCount = 0;
                foreach (ListViewItem item in listViewFile.Items)
                {
                    string heicFileName = Path.Combine(item.SubItems[1].Text, item.Text);
                    Console.WriteLine(heicFileName);
                    string jpgFileName = Path.ChangeExtension(heicFileName, ".jpg");

                    // 檢查是否已經存在
                    if (File.Exists(jpgFileName))
                    {
                        // 顯示對話框，詢問是否覆寫
                        DialogResult result = MessageBox.Show(jpgFileName + " 已經存在，是否覆寫？", "確認覆寫", MessageBoxButtons.YesNo);
                        if (result == DialogResult.Yes)
                        {
                            ConvertHEICToJPG(heicFileName, jpgFileName);
                        }
                    }
                    else
                    {
                        ConvertHEICToJPG(heicFileName, jpgFileName);
                    }
                    processedCount++;
                    // 更新進度條
                    this.Invoke((MethodInvoker)delegate
                    {
                        progressBar.Value = processedCount;
                    });
                }
            });
            // 隱藏進度條
            progressBar.Visible = false;
        }

        // 轉換 HEIC 到 JPG 的方法
        private void ConvertHEICToJPG(string inputFileName, string outputFileName)
        {
            try
            {
                // 使用 ImageMagick 進行 HEIC 到 JPG 的轉換
                using (var image = new MagickImage(inputFileName))
                {
                    image.Format = MagickFormat.Jpg; // 設定輸出格式為 JPG
                    image.Quality = (uint)trackBar_JpgCompressRate.Value; // 設定輸出品質
                    image.Write(outputFileName); // 寫入輸出檔案
                }
            }
            catch (MagickException ex)
            {
                // 捕捉 ImageMagick 相關的異常
                MessageBox.Show($"轉換 {inputFileName} 失敗：\n{ex.Message}", "轉換錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                // 捕捉其他類型的異常
                MessageBox.Show($"轉換 {inputFileName} 失敗：\n{ex.Message}", "轉換錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // 表單大小調整事件
        private void HEIC2JPG_Resize(object sender, EventArgs e)
        {
            this.listViewFile.Size = new Size(this.Width - 20, this.Height - 80);
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
            // 確認檔案存在
            if (!File.Exists(filePath))
            {
                MessageBox.Show("檔案不存在");
                return;
            }

            try
            {
                // 創建一個新的表單來顯示圖像，並且把主視窗的控制權交給這個新視窗
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
                }
            }
            catch (MagickException ex)
            {
                // 捕捉 ImageMagick 相關的異常
                MessageBox.Show($"載入圖片 {filePath} 失敗：\n{ex.Message}", "圖片載入錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                // 捕捉其他類型的異常
                MessageBox.Show($"載入圖片 {filePath} 失敗：\n{ex.Message}", "圖片載入錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
    }
}
