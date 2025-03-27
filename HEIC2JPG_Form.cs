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

        private void InitializeListView()
        {
            listViewFile.View = View.Details;
            listViewFile.Columns.Add("檔案名稱", 200);
            listViewFile.Columns.Add("目錄", 600);
        }

        private void buttonOpenFiles_Click(object sender, EventArgs e)
        {
            Console.WriteLine("buttonOpenFiles_Click");
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                //openFileDialog.Filter = "HEIC文件|*.heic;*.HEIC;*.HEIF;*.heif|JPG文件|*.jpg;*.jpeg;*.JPG;*.JPEG|所有文件|*.*";
                openFileDialog.Filter = "HEIC文件|*.heic;*.HEIC;*.HEIF;*.heif|所有文件|*.*";
                openFileDialog.Multiselect = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string[] files = openFileDialog.FileNames;
                    AddFileToListView(files);
                }
            }
        }

        private void button_OpenDir_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog folderDialog = new FolderBrowserDialog())
            {
                if (folderDialog.ShowDialog() == DialogResult.OK)
                {
                    SearchOption searchOption = checkBoxIncludeSubDirs.Checked ? 
                        SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;

                    string[] files = Directory.GetFiles(folderDialog.SelectedPath, "*.HEIC", searchOption)
                        .Concat(Directory.GetFiles(folderDialog.SelectedPath, "*.HEIF", searchOption))
                        //.Concat(Directory.GetFiles(folderDialog.SelectedPath, "*.jpg", searchOption))
                        //.Concat(Directory.GetFiles(folderDialog.SelectedPath, "*.jpeg", searchOption))
                        .ToArray();
                    //請改成一次將所有檔案傳給AddFileToListView 
                    AddFileToListView(files);   
                }
            }
        }

        private void AddFileToListView(string[] filePaths)
        {
            //先讓listViewFile顯示暫停  
            listViewFile.SuspendLayout();
            foreach (string filePath in filePaths)
            {
                //檢查是否已經存在  
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
                    //改成跳對話框
                    MessageBox.Show(filePath + "已經存在");
                }   
            }
            //再讓listViewFile顯示恢復
            listViewFile.ResumeLayout();
            listViewFile.Refresh();
            buttonConvert.Enabled = true;
        }

        private void buttonConvert_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in listViewFile.Items)
            {
                string heicFileName = Path.Combine(item.SubItems[1].Text, item.Text);
                Console.WriteLine(heicFileName);
                string jpgFileName = Path.ChangeExtension(heicFileName, ".jpg");
                //檢查是否已經存在
                if (File.Exists(jpgFileName))
                {
                    //改成可以選擇是否覆寫  
                    DialogResult result = MessageBox.Show(jpgFileName + "已經存在，是否覆寫？", "確認覆寫", MessageBoxButtons.YesNo);
                    if (result == DialogResult.Yes)
                    {
                        ConvertHEICToJPG(heicFileName, jpgFileName);
                    }
                }
                else
                {
                    // 调用转换方法（假设您有一个 ConvertHEICToJPG 方法）
                    ConvertHEICToJPG(heicFileName, jpgFileName);
                }
            }
        }

        // 假设您有一个转换方法
        private void ConvertHEICToJPG(string inputFileName, string outputFileName)
        {
  
            // 使用 ImageMagick 进行 HEIC 到 JPG 的转换
            using (var image = new MagickImage(inputFileName))
            {
                image.Format = MagickFormat.Jpg; // 设置输出格式为 JPG
                image.Quality = (uint)trackBar_JpgCompressRate.Value; // 设置输出质量为 100
                image.Write(outputFileName); // 写入输出文件
            }
        }

        private void HEIC2JPG_Resize(object sender, EventArgs e)
        {
            this.listViewFile.Size = new Size(this.Width - 20, this.Height - 80);
        }

        private void listViewFile_DoubleClick(object sender, EventArgs e)
        {
            if (listViewFile.SelectedItems.Count > 0)
            {
                string heicFilePath = Path.Combine(listViewFile.SelectedItems[0].SubItems[1].Text, listViewFile.SelectedItems[0].Text);
                ShowImage(heicFilePath);
            }
        }

        private void ShowImage(string filePath)
        {
            //Console.WriteLine("ShowImage");
            //Console.WriteLine(filePath);
            //確認檔案存在
            if (!File.Exists(filePath))
            {
                MessageBox.Show("檔案不存在");
                return;
            }
            // 创建一个新的窗体来显示图像，並且把主視窗的控制權交給這個新視窗   
            Form imageForm = new Form();
            var imageTmp = new MagickImage(filePath);
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
            //把視窗主控權交還給主視窗
            // this.Activate(); // 不再需要此行
        }

        private void button_ClearListView_Click(object sender, EventArgs e)
        {
            listViewFile.Items.Clear();
            buttonConvert.Enabled = false;
        }

        private void trackBar_JpgCompressRate_ValueChanged(object sender, EventArgs e)
        {
            label_JpgCompressRate.Text  = trackBar_JpgCompressRate.Value.ToString();
        }
    }
}
