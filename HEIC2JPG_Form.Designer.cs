namespace HEIC2JPG
{
    partial class HEIC2JPG
    {
        /// <summary>
        /// 設計工具所需的變數。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清除任何使用中的資源。
        /// </summary>
        /// <param name="disposing">如果應該處置受控資源則為 true，否則為 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 設計工具產生的程式碼

        /// <summary>
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器修改
        /// 這個方法的內容。
        /// </summary>
        private void InitializeComponent()
        {
            this.buttonOpenFiles = new System.Windows.Forms.Button();
            this.button_OpenDir = new System.Windows.Forms.Button();
            this.listViewFile = new System.Windows.Forms.ListView();
            this.checkBoxIncludeSubDirs = new System.Windows.Forms.CheckBox();
            this.buttonConvert = new System.Windows.Forms.Button();
            this.button_ClearListView = new System.Windows.Forms.Button();
            this.trackBar_JpgCompressRate = new System.Windows.Forms.TrackBar();
            this.label_JpgCompressRate = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar_JpgCompressRate)).BeginInit();
            this.SuspendLayout();
            // 
            // buttonOpenFiles
            // 
            this.buttonOpenFiles.Font = new System.Drawing.Font("微軟正黑體", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.buttonOpenFiles.Location = new System.Drawing.Point(11, 10);
            this.buttonOpenFiles.Name = "buttonOpenFiles";
            this.buttonOpenFiles.Size = new System.Drawing.Size(100, 40);
            this.buttonOpenFiles.TabIndex = 0;
            this.buttonOpenFiles.Text = "開啟檔案";
            this.buttonOpenFiles.UseVisualStyleBackColor = true;
            this.buttonOpenFiles.Click += new System.EventHandler(this.buttonOpenFiles_Click);
            // 
            // button_OpenDir
            // 
            this.button_OpenDir.Font = new System.Drawing.Font("微軟正黑體", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.button_OpenDir.Location = new System.Drawing.Point(120, 10);
            this.button_OpenDir.Name = "button_OpenDir";
            this.button_OpenDir.Size = new System.Drawing.Size(100, 40);
            this.button_OpenDir.TabIndex = 1;
            this.button_OpenDir.Text = "開啟目錄";
            this.button_OpenDir.UseVisualStyleBackColor = true;
            this.button_OpenDir.Click += new System.EventHandler(this.button_OpenDir_Click);
            // 
            // listViewFile
            // 
            this.listViewFile.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.listViewFile.HideSelection = false;
            this.listViewFile.Location = new System.Drawing.Point(0, 60);
            this.listViewFile.Name = "listViewFile";
            this.listViewFile.Size = new System.Drawing.Size(933, 550);
            this.listViewFile.TabIndex = 2;
            this.listViewFile.UseCompatibleStateImageBehavior = false;
            this.listViewFile.View = System.Windows.Forms.View.Details;
            this.listViewFile.DoubleClick += new System.EventHandler(this.listViewFile_DoubleClick);
            // 
            // checkBoxIncludeSubDirs
            // 
            this.checkBoxIncludeSubDirs.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.checkBoxIncludeSubDirs.Location = new System.Drawing.Point(226, 27);
            this.checkBoxIncludeSubDirs.Name = "checkBoxIncludeSubDirs";
            this.checkBoxIncludeSubDirs.Size = new System.Drawing.Size(109, 24);
            this.checkBoxIncludeSubDirs.TabIndex = 3;
            this.checkBoxIncludeSubDirs.Text = "包含子目錄";
            // 
            // buttonConvert
            // 
            this.buttonConvert.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonConvert.Enabled = false;
            this.buttonConvert.Font = new System.Drawing.Font("微軟正黑體", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.buttonConvert.Location = new System.Drawing.Point(800, 12);
            this.buttonConvert.Name = "buttonConvert";
            this.buttonConvert.Size = new System.Drawing.Size(123, 40);
            this.buttonConvert.TabIndex = 4;
            this.buttonConvert.Text = "轉換成JPG";
            this.buttonConvert.UseVisualStyleBackColor = true;
            this.buttonConvert.Click += new System.EventHandler(this.buttonConvert_Click);
            // 
            // button_ClearListView
            // 
            this.button_ClearListView.Font = new System.Drawing.Font("微軟正黑體", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.button_ClearListView.Location = new System.Drawing.Point(341, 10);
            this.button_ClearListView.Name = "button_ClearListView";
            this.button_ClearListView.Size = new System.Drawing.Size(100, 40);
            this.button_ClearListView.TabIndex = 5;
            this.button_ClearListView.Text = "清除列表";
            this.button_ClearListView.UseVisualStyleBackColor = true;
            this.button_ClearListView.Click += new System.EventHandler(this.button_ClearListView_Click);
            // 
            // trackBar_JpgCompressRate
            // 
            this.trackBar_JpgCompressRate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.trackBar_JpgCompressRate.Location = new System.Drawing.Point(583, 14);
            this.trackBar_JpgCompressRate.Maximum = 100;
            this.trackBar_JpgCompressRate.Minimum = 1;
            this.trackBar_JpgCompressRate.Name = "trackBar_JpgCompressRate";
            this.trackBar_JpgCompressRate.Size = new System.Drawing.Size(180, 45);
            this.trackBar_JpgCompressRate.TabIndex = 6;
            this.trackBar_JpgCompressRate.TickFrequency = 5;
            this.trackBar_JpgCompressRate.Value = 80;
            this.trackBar_JpgCompressRate.ValueChanged += new System.EventHandler(this.trackBar_JpgCompressRate_ValueChanged);
            // 
            // label_JpgCompressRate
            // 
            this.label_JpgCompressRate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label_JpgCompressRate.AutoSize = true;
            this.label_JpgCompressRate.Font = new System.Drawing.Font("微軟正黑體", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label_JpgCompressRate.Location = new System.Drawing.Point(533, 14);
            this.label_JpgCompressRate.Name = "label_JpgCompressRate";
            this.label_JpgCompressRate.Size = new System.Drawing.Size(64, 31);
            this.label_JpgCompressRate.TabIndex = 7;
            this.label_JpgCompressRate.Text = "80%";
            this.label_JpgCompressRate.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("微軟正黑體", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label1.Location = new System.Drawing.Point(763, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 24);
            this.label1.TabIndex = 8;
            this.label1.Text = "高";
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("微軟正黑體", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label2.Location = new System.Drawing.Point(451, 19);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(80, 24);
            this.label2.TabIndex = 9;
            this.label2.Text = "JPG品質";
            // 
            // HEIC2JPG
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(934, 611);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label_JpgCompressRate);
            this.Controls.Add(this.trackBar_JpgCompressRate);
            this.Controls.Add(this.button_ClearListView);
            this.Controls.Add(this.buttonConvert);
            this.Controls.Add(this.listViewFile);
            this.Controls.Add(this.button_OpenDir);
            this.Controls.Add(this.buttonOpenFiles);
            this.Controls.Add(this.checkBoxIncludeSubDirs);
            this.Name = "HEIC2JPG";
            this.Text = "HEIC2JPG";
            this.Resize += new System.EventHandler(this.HEIC2JPG_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.trackBar_JpgCompressRate)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonOpenFiles;
        private System.Windows.Forms.Button button_OpenDir;
        private System.Windows.Forms.ListView listViewFile;
        private System.Windows.Forms.Button buttonConvert;
        private System.Windows.Forms.Button button_ClearListView;
        private System.Windows.Forms.TrackBar trackBar_JpgCompressRate;
        private System.Windows.Forms.Label label_JpgCompressRate;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
    }
}

