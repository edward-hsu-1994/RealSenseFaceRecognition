namespace FaceRecognition {
    partial class MainForm {
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
            this.FormMenuBar = new System.Windows.Forms.MenuStrip();
            this.檔案FToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.OpenFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.SaveFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.SaveOtherFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.ExportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.EndToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.DeviceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ResolutionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ModeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ColorPlusToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.IRToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ColorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.說明HToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.關於AToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.panel3 = new System.Windows.Forms.Panel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.FacePicturebox = new System.Windows.Forms.PictureBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.unregisterButton = new System.Windows.Forms.Button();
            this.registerButton = new System.Windows.Forms.Button();
            this.stopButton = new System.Windows.Forms.Button();
            this.startButton = new System.Windows.Forms.Button();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.UserListBox = new System.Windows.Forms.ListBox();
            this.userListContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.EditUserToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.AddUserToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.DeleteUserToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.UserFaceListView = new System.Windows.Forms.ListView();
            this.userImageContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.AddImageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.DeleteImageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.FaceImageList = new System.Windows.Forms.ImageList(this.components);
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.FormMenuBar.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.FacePicturebox)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.userListContextMenuStrip.SuspendLayout();
            this.userImageContextMenuStrip.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // FormMenuBar
            // 
            this.FormMenuBar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.檔案FToolStripMenuItem,
            this.DeviceToolStripMenuItem,
            this.ResolutionToolStripMenuItem,
            this.ModeToolStripMenuItem,
            this.說明HToolStripMenuItem});
            this.FormMenuBar.Location = new System.Drawing.Point(0, 0);
            this.FormMenuBar.Name = "FormMenuBar";
            this.FormMenuBar.Size = new System.Drawing.Size(930, 24);
            this.FormMenuBar.TabIndex = 1;
            this.FormMenuBar.Text = "menuStrip1";
            // 
            // 檔案FToolStripMenuItem
            // 
            this.檔案FToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.OpenFileToolStripMenuItem,
            this.SaveFileToolStripMenuItem,
            this.SaveOtherFileToolStripMenuItem,
            this.toolStripSeparator1,
            this.ExportToolStripMenuItem,
            this.toolStripSeparator2,
            this.EndToolStripMenuItem});
            this.檔案FToolStripMenuItem.Name = "檔案FToolStripMenuItem";
            this.檔案FToolStripMenuItem.Size = new System.Drawing.Size(57, 20);
            this.檔案FToolStripMenuItem.Text = "檔案(&F)";
            // 
            // OpenFileToolStripMenuItem
            // 
            this.OpenFileToolStripMenuItem.Name = "OpenFileToolStripMenuItem";
            this.OpenFileToolStripMenuItem.Size = new System.Drawing.Size(140, 22);
            this.OpenFileToolStripMenuItem.Text = "開啟舊檔(&O)";
            this.OpenFileToolStripMenuItem.Click += new System.EventHandler(this.OpenFileToolStripMenuItem_Click);
            // 
            // SaveFileToolStripMenuItem
            // 
            this.SaveFileToolStripMenuItem.Enabled = false;
            this.SaveFileToolStripMenuItem.Name = "SaveFileToolStripMenuItem";
            this.SaveFileToolStripMenuItem.Size = new System.Drawing.Size(140, 22);
            this.SaveFileToolStripMenuItem.Text = "儲存(&S)";
            this.SaveFileToolStripMenuItem.Click += new System.EventHandler(this.SaveFileToolStripMenuItem_Click);
            // 
            // SaveOtherFileToolStripMenuItem
            // 
            this.SaveOtherFileToolStripMenuItem.Name = "SaveOtherFileToolStripMenuItem";
            this.SaveOtherFileToolStripMenuItem.Size = new System.Drawing.Size(140, 22);
            this.SaveOtherFileToolStripMenuItem.Text = "另存新檔(&N)";
            this.SaveOtherFileToolStripMenuItem.Click += new System.EventHandler(this.SaveOtherFileToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(137, 6);
            // 
            // ExportToolStripMenuItem
            // 
            this.ExportToolStripMenuItem.Name = "ExportToolStripMenuItem";
            this.ExportToolStripMenuItem.Size = new System.Drawing.Size(140, 22);
            this.ExportToolStripMenuItem.Text = "匯出(&E)";
            this.ExportToolStripMenuItem.Click += new System.EventHandler(this.ExportToolStripMenuItem_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(137, 6);
            // 
            // EndToolStripMenuItem
            // 
            this.EndToolStripMenuItem.Name = "EndToolStripMenuItem";
            this.EndToolStripMenuItem.Size = new System.Drawing.Size(140, 22);
            this.EndToolStripMenuItem.Text = "結束(&X)";
            this.EndToolStripMenuItem.Click += new System.EventHandler(this.EndToolStripMenuItem_Click);
            // 
            // DeviceToolStripMenuItem
            // 
            this.DeviceToolStripMenuItem.Name = "DeviceToolStripMenuItem";
            this.DeviceToolStripMenuItem.Size = new System.Drawing.Size(60, 20);
            this.DeviceToolStripMenuItem.Text = "裝置(&D)";
            // 
            // ResolutionToolStripMenuItem
            // 
            this.ResolutionToolStripMenuItem.Name = "ResolutionToolStripMenuItem";
            this.ResolutionToolStripMenuItem.Size = new System.Drawing.Size(71, 20);
            this.ResolutionToolStripMenuItem.Text = "解析度(&R)";
            // 
            // ModeToolStripMenuItem
            // 
            this.ModeToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ColorPlusToolStripMenuItem,
            this.IRToolStripMenuItem,
            this.ColorToolStripMenuItem});
            this.ModeToolStripMenuItem.Name = "ModeToolStripMenuItem";
            this.ModeToolStripMenuItem.Size = new System.Drawing.Size(63, 20);
            this.ModeToolStripMenuItem.Text = "模式(&M)";
            // 
            // ColorPlusToolStripMenuItem
            // 
            this.ColorPlusToolStripMenuItem.Checked = true;
            this.ColorPlusToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ColorPlusToolStripMenuItem.Name = "ColorPlusToolStripMenuItem";
            this.ColorPlusToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.ColorPlusToolStripMenuItem.Text = "3D 追蹤";
            this.ColorPlusToolStripMenuItem.Click += new System.EventHandler(this.ModeChange);
            // 
            // IRToolStripMenuItem
            // 
            this.IRToolStripMenuItem.Name = "IRToolStripMenuItem";
            this.IRToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.IRToolStripMenuItem.Text = "紅外線追蹤";
            this.IRToolStripMenuItem.Click += new System.EventHandler(this.ModeChange);
            // 
            // ColorToolStripMenuItem
            // 
            this.ColorToolStripMenuItem.Name = "ColorToolStripMenuItem";
            this.ColorToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.ColorToolStripMenuItem.Text = "2D 追蹤";
            this.ColorToolStripMenuItem.Click += new System.EventHandler(this.ModeChange);
            // 
            // 說明HToolStripMenuItem
            // 
            this.說明HToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.關於AToolStripMenuItem});
            this.說明HToolStripMenuItem.Name = "說明HToolStripMenuItem";
            this.說明HToolStripMenuItem.Size = new System.Drawing.Size(60, 20);
            this.說明HToolStripMenuItem.Text = "說明(&H)";
            // 
            // 關於AToolStripMenuItem
            // 
            this.關於AToolStripMenuItem.Name = "關於AToolStripMenuItem";
            this.關於AToolStripMenuItem.Size = new System.Drawing.Size(114, 22);
            this.關於AToolStripMenuItem.Text = "關於(&A)";
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(930, 542);
            this.tabControl1.TabIndex = 2;
            this.tabControl1.SelectedIndexChanged += new System.EventHandler(this.tabControl1_SelectedIndexChanged);
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.splitContainer1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(922, 516);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "影像";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(3, 3);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.panel3);
            this.splitContainer1.Panel1.Controls.Add(this.panel2);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.tableLayoutPanel1);
            this.splitContainer1.Size = new System.Drawing.Size(916, 510);
            this.splitContainer1.SplitterDistance = 800;
            this.splitContainer1.TabIndex = 0;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.pictureBox1);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(800, 430);
            this.panel3.TabIndex = 1;
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.Gray;
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(800, 430);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Paint += new System.Windows.Forms.PaintEventHandler(this.pictureBox1_Paint);
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.panel2.Controls.Add(this.label1);
            this.panel2.Controls.Add(this.FacePicturebox);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 430);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(800, 80);
            this.panel2.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Font = new System.Drawing.Font("新細明體", 20F);
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(80, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(720, 80);
            this.label1.TabIndex = 1;
            this.label1.Text = "無使用者";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // FacePicturebox
            // 
            this.FacePicturebox.BackColor = System.Drawing.Color.Black;
            this.FacePicturebox.Dock = System.Windows.Forms.DockStyle.Left;
            this.FacePicturebox.Location = new System.Drawing.Point(0, 0);
            this.FacePicturebox.Name = "FacePicturebox";
            this.FacePicturebox.Size = new System.Drawing.Size(80, 80);
            this.FacePicturebox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.FacePicturebox.TabIndex = 0;
            this.FacePicturebox.TabStop = false;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.unregisterButton, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.registerButton, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.stopButton, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.startButton, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 6;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(112, 510);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // unregisterButton
            // 
            this.unregisterButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.unregisterButton.Enabled = false;
            this.unregisterButton.Location = new System.Drawing.Point(3, 108);
            this.unregisterButton.Name = "unregisterButton";
            this.unregisterButton.Size = new System.Drawing.Size(106, 29);
            this.unregisterButton.TabIndex = 3;
            this.unregisterButton.Text = "解除註冊";
            this.unregisterButton.UseVisualStyleBackColor = true;
            this.unregisterButton.Click += new System.EventHandler(this.unregisterButton_Click);
            // 
            // registerButton
            // 
            this.registerButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.registerButton.Enabled = false;
            this.registerButton.Location = new System.Drawing.Point(3, 73);
            this.registerButton.Name = "registerButton";
            this.registerButton.Size = new System.Drawing.Size(106, 29);
            this.registerButton.TabIndex = 2;
            this.registerButton.Text = "註冊";
            this.registerButton.UseVisualStyleBackColor = true;
            this.registerButton.Click += new System.EventHandler(this.registerButton_Click);
            // 
            // stopButton
            // 
            this.stopButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.stopButton.Enabled = false;
            this.stopButton.Location = new System.Drawing.Point(3, 38);
            this.stopButton.Name = "stopButton";
            this.stopButton.Size = new System.Drawing.Size(106, 29);
            this.stopButton.TabIndex = 1;
            this.stopButton.Text = "停止";
            this.stopButton.UseVisualStyleBackColor = true;
            this.stopButton.Click += new System.EventHandler(this.stopButton_Click);
            // 
            // startButton
            // 
            this.startButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.startButton.Location = new System.Drawing.Point(3, 3);
            this.startButton.Name = "startButton";
            this.startButton.Size = new System.Drawing.Size(106, 29);
            this.startButton.TabIndex = 0;
            this.startButton.Text = "開始";
            this.startButton.UseVisualStyleBackColor = true;
            this.startButton.Click += new System.EventHandler(this.startButton_Click);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.splitContainer2);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(922, 516);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "資料庫";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(3, 3);
            this.splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.UserListBox);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.UserFaceListView);
            this.splitContainer2.Size = new System.Drawing.Size(916, 510);
            this.splitContainer2.SplitterDistance = 156;
            this.splitContainer2.TabIndex = 0;
            // 
            // UserListBox
            // 
            this.UserListBox.ContextMenuStrip = this.userListContextMenuStrip;
            this.UserListBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.UserListBox.FormattingEnabled = true;
            this.UserListBox.ItemHeight = 12;
            this.UserListBox.Location = new System.Drawing.Point(0, 0);
            this.UserListBox.Name = "UserListBox";
            this.UserListBox.Size = new System.Drawing.Size(156, 510);
            this.UserListBox.TabIndex = 0;
            this.UserListBox.SelectedIndexChanged += new System.EventHandler(this.UserListBox_SelectedIndexChanged);
            // 
            // userListContextMenuStrip
            // 
            this.userListContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.EditUserToolStripMenuItem,
            this.AddUserToolStripMenuItem,
            this.DeleteUserToolStripMenuItem});
            this.userListContextMenuStrip.Name = "userListContextMenuStrip";
            this.userListContextMenuStrip.Size = new System.Drawing.Size(135, 70);
            // 
            // EditUserToolStripMenuItem
            // 
            this.EditUserToolStripMenuItem.Name = "EditUserToolStripMenuItem";
            this.EditUserToolStripMenuItem.Size = new System.Drawing.Size(134, 22);
            this.EditUserToolStripMenuItem.Text = "變更名稱";
            this.EditUserToolStripMenuItem.Click += new System.EventHandler(this.EditUserToolStripMenuItem_Click);
            // 
            // AddUserToolStripMenuItem
            // 
            this.AddUserToolStripMenuItem.Name = "AddUserToolStripMenuItem";
            this.AddUserToolStripMenuItem.Size = new System.Drawing.Size(134, 22);
            this.AddUserToolStripMenuItem.Text = "新增使用者";
            this.AddUserToolStripMenuItem.Click += new System.EventHandler(this.AddUserToolStripMenuItem_Click);
            // 
            // DeleteUserToolStripMenuItem
            // 
            this.DeleteUserToolStripMenuItem.Name = "DeleteUserToolStripMenuItem";
            this.DeleteUserToolStripMenuItem.Size = new System.Drawing.Size(134, 22);
            this.DeleteUserToolStripMenuItem.Text = "刪除使用者";
            this.DeleteUserToolStripMenuItem.Click += new System.EventHandler(this.DeleteUserToolStripMenuItem_Click);
            // 
            // UserFaceListView
            // 
            this.UserFaceListView.ContextMenuStrip = this.userImageContextMenuStrip;
            this.UserFaceListView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.UserFaceListView.LargeImageList = this.FaceImageList;
            this.UserFaceListView.Location = new System.Drawing.Point(0, 0);
            this.UserFaceListView.Name = "UserFaceListView";
            this.UserFaceListView.Size = new System.Drawing.Size(756, 510);
            this.UserFaceListView.TabIndex = 0;
            this.UserFaceListView.UseCompatibleStateImageBehavior = false;
            // 
            // userImageContextMenuStrip
            // 
            this.userImageContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.AddImageToolStripMenuItem,
            this.DeleteImageToolStripMenuItem});
            this.userImageContextMenuStrip.Name = "userImageContextMenuStrip";
            this.userImageContextMenuStrip.Size = new System.Drawing.Size(123, 48);
            // 
            // AddImageToolStripMenuItem
            // 
            this.AddImageToolStripMenuItem.Name = "AddImageToolStripMenuItem";
            this.AddImageToolStripMenuItem.Size = new System.Drawing.Size(122, 22);
            this.AddImageToolStripMenuItem.Text = "新增圖片";
            this.AddImageToolStripMenuItem.Click += new System.EventHandler(this.AddImageToolStripMenuItem_Click);
            // 
            // DeleteImageToolStripMenuItem
            // 
            this.DeleteImageToolStripMenuItem.Name = "DeleteImageToolStripMenuItem";
            this.DeleteImageToolStripMenuItem.Size = new System.Drawing.Size(122, 22);
            this.DeleteImageToolStripMenuItem.Text = "刪除圖片";
            this.DeleteImageToolStripMenuItem.Click += new System.EventHandler(this.DeleteImageToolStripMenuItem_Click);
            // 
            // FaceImageList
            // 
            this.FaceImageList.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
            this.FaceImageList.ImageSize = new System.Drawing.Size(128, 128);
            this.FaceImageList.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 566);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(930, 22);
            this.statusStrip1.TabIndex = 3;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(31, 17);
            this.toolStripStatusLabel1.Text = "就緒";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.tabControl1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 24);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(930, 542);
            this.panel1.TabIndex = 4;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(930, 588);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.FormMenuBar);
            this.MainMenuStrip = this.FormMenuBar;
            this.Name = "MainForm";
            this.Text = "臉部辨識";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Main_FormClosing);
            this.Load += new System.EventHandler(this.Main_Load);
            this.FormMenuBar.ResumeLayout(false);
            this.FormMenuBar.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.FacePicturebox)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.userListContextMenuStrip.ResumeLayout(false);
            this.userImageContextMenuStrip.ResumeLayout(false);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.MenuStrip FormMenuBar;
        private System.Windows.Forms.ToolStripMenuItem 檔案FToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem OpenFileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem SaveFileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem SaveOtherFileToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem ExportToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem EndToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ResolutionToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem DeviceToolStripMenuItem;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.ToolStripMenuItem 說明HToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 關於AToolStripMenuItem;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Button startButton;
        private System.Windows.Forms.Button stopButton;
        private System.Windows.Forms.Button unregisterButton;
        private System.Windows.Forms.Button registerButton;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ToolStripMenuItem ModeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ColorPlusToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem IRToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ColorToolStripMenuItem;
        private System.Windows.Forms.ImageList FaceImageList;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.PictureBox FacePicturebox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.ListBox UserListBox;
        private System.Windows.Forms.ListView UserFaceListView;
        private System.Windows.Forms.ContextMenuStrip userListContextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem AddUserToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem DeleteUserToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem EditUserToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip userImageContextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem AddImageToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem DeleteImageToolStripMenuItem;
    }
}

