namespace FaceDatabaseExplorer {
    partial class Main {
        /// <summary>
        /// 設計工具所需的變數。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清除任何使用中的資源。
        /// </summary>
        /// <param name="disposing">如果應該處置 Managed 資源則為 true，否則為 false。</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 設計工具產生的程式碼

        /// <summary>
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器修改
        /// 這個方法的內容。
        /// </summary>
        private void InitializeComponent() {
            this.components = new System.ComponentModel.Container();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.檔案FToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.開啟OToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.儲存SToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.關閉XToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.userContextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.editUserToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.DeleteUserToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.listView1 = new System.Windows.Forms.ListView();
            this.imageContextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.SaveImageMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.DeleteImageMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.saveImageFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.SaveUserToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.SaveUserFolderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.SaveOtherStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.ConvertToDirToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.userContextMenuStrip1.SuspendLayout();
            this.imageContextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.檔案FToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(796, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // 檔案FToolStripMenuItem
            // 
            this.檔案FToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.開啟OToolStripMenuItem,
            this.儲存SToolStripMenuItem,
            this.SaveOtherStripMenuItem1,
            this.toolStripSeparator5,
            this.toolStripMenuItem1,
            this.toolStripSeparator1,
            this.關閉XToolStripMenuItem});
            this.檔案FToolStripMenuItem.Name = "檔案FToolStripMenuItem";
            this.檔案FToolStripMenuItem.Size = new System.Drawing.Size(57, 20);
            this.檔案FToolStripMenuItem.Text = "檔案(&F)";
            // 
            // 開啟OToolStripMenuItem
            // 
            this.開啟OToolStripMenuItem.Name = "開啟OToolStripMenuItem";
            this.開啟OToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.開啟OToolStripMenuItem.Text = "開啟(&O)";
            this.開啟OToolStripMenuItem.Click += new System.EventHandler(this.開啟OToolStripMenuItem_Click);
            // 
            // 儲存SToolStripMenuItem
            // 
            this.儲存SToolStripMenuItem.Name = "儲存SToolStripMenuItem";
            this.儲存SToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.儲存SToolStripMenuItem.Text = "儲存(&S)";
            this.儲存SToolStripMenuItem.Click += new System.EventHandler(this.儲存SToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(149, 6);
            // 
            // 關閉XToolStripMenuItem
            // 
            this.關閉XToolStripMenuItem.Name = "關閉XToolStripMenuItem";
            this.關閉XToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.關閉XToolStripMenuItem.Text = "關閉(&X)";
            this.關閉XToolStripMenuItem.Click += new System.EventHandler(this.關閉XToolStripMenuItem_Click);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 24);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.listBox1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.listView1);
            this.splitContainer1.Size = new System.Drawing.Size(796, 266);
            this.splitContainer1.SplitterDistance = 145;
            this.splitContainer1.TabIndex = 1;
            // 
            // listBox1
            // 
            this.listBox1.ContextMenuStrip = this.userContextMenuStrip1;
            this.listBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBox1.FormattingEnabled = true;
            this.listBox1.ItemHeight = 12;
            this.listBox1.Location = new System.Drawing.Point(0, 0);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(145, 266);
            this.listBox1.TabIndex = 0;
            this.listBox1.SelectedIndexChanged += new System.EventHandler(this.listBox1_SelectedIndexChanged);
            // 
            // userContextMenuStrip1
            // 
            this.userContextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.SaveUserToolStripMenuItem,
            this.toolStripSeparator4,
            this.editUserToolStripMenuItem,
            this.toolStripSeparator3,
            this.DeleteUserToolStripMenuItem});
            this.userContextMenuStrip1.Name = "userContextMenuStrip1";
            this.userContextMenuStrip1.Size = new System.Drawing.Size(215, 82);
            // 
            // editUserToolStripMenuItem
            // 
            this.editUserToolStripMenuItem.Name = "editUserToolStripMenuItem";
            this.editUserToolStripMenuItem.Size = new System.Drawing.Size(214, 22);
            this.editUserToolStripMenuItem.Text = "編輯資訊";
            this.editUserToolStripMenuItem.Click += new System.EventHandler(this.editUserToolStripMenuItem_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(211, 6);
            // 
            // DeleteUserToolStripMenuItem
            // 
            this.DeleteUserToolStripMenuItem.Name = "DeleteUserToolStripMenuItem";
            this.DeleteUserToolStripMenuItem.Size = new System.Drawing.Size(214, 22);
            this.DeleteUserToolStripMenuItem.Text = "刪除";
            this.DeleteUserToolStripMenuItem.Click += new System.EventHandler(this.DeleteUserToolStripMenuItem_Click);
            // 
            // listView1
            // 
            this.listView1.ContextMenuStrip = this.imageContextMenuStrip1;
            this.listView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listView1.LargeImageList = this.imageList1;
            this.listView1.Location = new System.Drawing.Point(0, 0);
            this.listView1.MultiSelect = false;
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(647, 266);
            this.listView1.TabIndex = 0;
            this.listView1.UseCompatibleStateImageBehavior = false;
            // 
            // imageContextMenuStrip1
            // 
            this.imageContextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.SaveImageMenuItem,
            this.toolStripSeparator2,
            this.DeleteImageMenuItem});
            this.imageContextMenuStrip1.Name = "imageContextMenuStrip1";
            this.imageContextMenuStrip1.Size = new System.Drawing.Size(123, 54);
            // 
            // SaveImageMenuItem
            // 
            this.SaveImageMenuItem.Name = "SaveImageMenuItem";
            this.SaveImageMenuItem.Size = new System.Drawing.Size(122, 22);
            this.SaveImageMenuItem.Text = "另存圖片";
            this.SaveImageMenuItem.Click += new System.EventHandler(this.SaveImageMenuItem_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(119, 6);
            // 
            // DeleteImageMenuItem
            // 
            this.DeleteImageMenuItem.Name = "DeleteImageMenuItem";
            this.DeleteImageMenuItem.Size = new System.Drawing.Size(122, 22);
            this.DeleteImageMenuItem.Text = "刪除";
            this.DeleteImageMenuItem.Click += new System.EventHandler(this.DeleteImageMenuItem_Click);
            // 
            // imageList1
            // 
            this.imageList1.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
            this.imageList1.ImageSize = new System.Drawing.Size(128, 128);
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(211, 6);
            // 
            // SaveUserToolStripMenuItem
            // 
            this.SaveUserToolStripMenuItem.Name = "SaveUserToolStripMenuItem";
            this.SaveUserToolStripMenuItem.Size = new System.Drawing.Size(214, 22);
            this.SaveUserToolStripMenuItem.Text = "另存使用者圖片(新增目錄)";
            this.SaveUserToolStripMenuItem.Click += new System.EventHandler(this.SaveUserToolStripMenuItem_Click);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(149, 6);
            // 
            // SaveOtherStripMenuItem1
            // 
            this.SaveOtherStripMenuItem1.Name = "SaveOtherStripMenuItem1";
            this.SaveOtherStripMenuItem1.Size = new System.Drawing.Size(152, 22);
            this.SaveOtherStripMenuItem1.Text = "另存新檔(&N)";
            this.SaveOtherStripMenuItem1.Click += new System.EventHandler(this.SaveOtherStripMenuItem1_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ConvertToDirToolStripMenuItem});
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(152, 22);
            this.toolStripMenuItem1.Text = "匯出(&E)";
            // 
            // ConvertToDirToolStripMenuItem
            // 
            this.ConvertToDirToolStripMenuItem.Name = "ConvertToDirToolStripMenuItem";
            this.ConvertToDirToolStripMenuItem.Size = new System.Drawing.Size(266, 22);
            this.ConvertToDirToolStripMenuItem.Text = "至目錄，且依ID新建使用者圖檔目錄";
            this.ConvertToDirToolStripMenuItem.Click += new System.EventHandler(this.ConvertToDirToolStripMenuItem_Click);
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(796, 290);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Main";
            this.Text = "臉部辨識資料庫瀏覽器";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.userContextMenuStrip1.ResumeLayout(false);
            this.imageContextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 檔案FToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 開啟OToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem 關閉XToolStripMenuItem;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.ContextMenuStrip imageContextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem SaveImageMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem DeleteImageMenuItem;
        private System.Windows.Forms.SaveFileDialog saveImageFileDialog1;
        private System.Windows.Forms.ToolStripMenuItem 儲存SToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip userContextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem editUserToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem DeleteUserToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem SaveUserToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.FolderBrowserDialog SaveUserFolderBrowserDialog1;
        private System.Windows.Forms.ToolStripMenuItem SaveOtherStripMenuItem1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem ConvertToDirToolStripMenuItem;
    }
}

