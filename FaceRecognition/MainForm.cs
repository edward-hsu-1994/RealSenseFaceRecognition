using Microsoft.VisualBasic;
using RealSenseSdkExtensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FaceRecognition {
    public partial class MainForm : Form {
        /// <summary>
        /// 所有設備資訊
        /// </summary>
        private PXCMCapture.DeviceInfo[] DeviceInfos;

        /// <summary>
        /// 所有設備支援的串流類型
        /// </summary>
        private PXCMCapture.Device.StreamProfileSet[][] DeviceStreamProfiles;

        /// <summary>
        /// 使用者選擇的設備
        /// </summary>
        public PXCMCapture.DeviceInfo SelectedDevice { get; private set; }

        public PXCMFaceConfiguration.TrackingModeType ModeType {
            get {
                if (ColorPlusToolStripMenuItem.Checked) {
                    return PXCMFaceConfiguration.TrackingModeType.FACE_MODE_COLOR_PLUS_DEPTH;
                } else if (IRToolStripMenuItem.Checked) {
                    return PXCMFaceConfiguration.TrackingModeType.FACE_MODE_IR;
                } else {
                    return PXCMFaceConfiguration.TrackingModeType.FACE_MODE_COLOR;
                }
            }
        }

        /// <summary>
        /// 使用者選擇的色彩
        /// </summary>
        public PXCMCapture.Device.StreamProfileSet SelectedDeviceStreamProfile { get; set; }


        /// <summary>
        /// 使用者Id對應字典
        /// </summary>
        public Dictionary<int, string> UserTable { get; set; } = new Dictionary<int, string>();

        /// <summary>
        /// 臉部資訊資料庫
        /// </summary>
        public RecognitionFaceData[] FaceData { get; set; } = new RecognitionFaceData[0];



        private FaceRecognitionProgram realSenseProgram;
        public MainForm() {
            InitializeComponent();
            realSenseProgram = new FaceRecognitionProgram(this);
            realSenseProgram.OnStart += RealSenseProgram_OnStart;
            realSenseProgram.OnStop += RealSenseProgram_OnStop;
            realSenseProgram.OnFoundFace += RealSenseProgram_OnFoundFace;
            realSenseProgram.OnNotFoundFace += RealSenseProgram_OnNotFoundFace;
        }

        private void RealSenseProgram_OnNotFoundFace(object sender, FaceRecognitionEventArgs args) {
            label1.Text = "無使用者";
            FacePicturebox.Image = null;
            registerButton.Enabled = false;
            unregisterButton.Enabled = false;
        }
        private bool CurentDataLocked;
        public PXCMFaceData.RecognitionData Current { get; private set; }
        public PXCMFaceData CurrentData { get; private set; }
        public string CurrentName { get; private set; }
        private void RealSenseProgram_OnFoundFace(object sender, FaceRecognitionEventArgs args) {
            try {
                int UserId = args.Faces[0].QueryRecognition().QueryUserID();
                if (UserId == -1) {
                    label1.Text = "未註冊使用者";
                    CurrentName = "";
                    registerButton.Enabled = true;
                    unregisterButton.Enabled = false;
                } else {
                    string name;
                    UserTable.TryGetValue(UserId, out name);
                    CurrentName = name;
                    label1.Text = "已註冊使用者: " + name;
                    registerButton.Enabled = true;
                    unregisterButton.Enabled = true;
                }

                if (!CurentDataLocked) {
                    Current = args.FirstRecognition;
                    CurrentData = args.Output;
                }
            } catch { }
        }

        private void RealSenseProgram_OnStop(object sender, FaceRecognitionEventArgs e) {
            #region 控制項作用切換
            Invoke(new UpdatePanelDelegate(() => {
                DeviceToolStripMenuItem.Enabled = true;
                ResolutionToolStripMenuItem.Enabled = true;
                ModeToolStripMenuItem.Enabled = true;
                startButton.Enabled = true;
                stopButton.Enabled = false;
                registerButton.Enabled = false;
                unregisterButton.Enabled = false;
            }));
            #endregion
        }

        private void RealSenseProgram_OnStart(object sender, FaceRecognitionEventArgs e) {
            #region 控制項作用切換
            Invoke(new UpdatePanelDelegate(() => {
                DeviceToolStripMenuItem.Enabled = false;
                ResolutionToolStripMenuItem.Enabled = false;
                ModeToolStripMenuItem.Enabled = false;
                startButton.Enabled = false;
                stopButton.Enabled = true;
                registerButton.Enabled = true;
                unregisterButton.Enabled = true;
            }));
            #endregion
        }

        #region 視窗事件
        /// <summary>
        /// 視窗讀入事件
        /// </summary>
        private void Main_Load(object sender, EventArgs e) {
            DeviceInfos = GetDeviceInfos();
            DeviceStreamProfiles = GetDeviceStreamProfiles();

            InitMenuBar();

        }

        /// <summary>
        /// 視窗關閉中事件
        /// </summary>
        private void Main_FormClosing(object sender, FormClosingEventArgs e) {
            if (MessageBox.Show(
                "您確定要關閉本程式嗎?\r\n請注意尚未儲存的結果將會遺失!",
                "結束應用程式",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
                ) == DialogResult.No) {
                e.Cancel = true;
            }
        }
        #endregion

        #region 更新主選單
        private void InitMenuBar() {
            #region 設備列表
            DeviceInfos.Select((x, i) => {
                var item = new ToolStripMenuItem(x.name, null);
                DeviceToolStripMenuItem.DropDownItems.Add(item);
                item.Click += (sender, e) => {
                    var typedSender = (ToolStripMenuItem)sender;
                    int index = DeviceToolStripMenuItem
                        .DropDownItems
                        .IndexOf(typedSender);

                    foreach (var otherItem in DeviceToolStripMenuItem.DropDownItems) {
                        ((ToolStripMenuItem)otherItem).Checked = false;
                    }

                    SelectedDevice = DeviceInfos[index];
                    typedSender.Checked = true;
                };
                if (i == 0) {
                    SelectedDevice = DeviceInfos.First();
                    item.Checked = true;
                }
                return 0;
            }).ToArray();
            #endregion

            UpdateColorMenuBar();

            SetStatus("主選單初始化完成");
            SetStatus("主選單初始化完成2");

        }

        private void UpdateColorMenuBar() {
            int index = Array.IndexOf(DeviceInfos, SelectedDevice);
            if (index == -1) return;

            //清除列表
            ResolutionToolStripMenuItem.DropDownItems.Clear();

            ResolutionToolStripMenuItem.DropDownItems.AddRange(
                DeviceStreamProfiles[index].Select((x, i) => {
                    var item = new ToolStripMenuItem();
                    item.Text =
                        $"[{x.color.imageInfo.format.ToString().Split('_').Last()}]-" +
                        $"{x.color.imageInfo.width} x {x.color.imageInfo.height}" +
                        $"({x.color.frameRate.max} FPS)";
                    item.Click += (sender, e) => {
                        ToolStripMenuItem typedSender = (ToolStripMenuItem)sender;

                        foreach (var colorItem in ResolutionToolStripMenuItem.DropDownItems) {
                            ((ToolStripMenuItem)colorItem).Checked = false;
                        }

                        typedSender.Checked = true;
                        int index2 = ResolutionToolStripMenuItem.DropDownItems.IndexOf(typedSender);

                        SelectedDeviceStreamProfile = DeviceStreamProfiles[index][index2];
                    };

                    if (i == 1) {
                        item.Checked = true;
                    }
                    return item;
                }).ToArray()
            );
        }

        /// <summary>
        /// 追蹤模式變更事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ModeChange(object sender, EventArgs e) {
            var typedSender = (ToolStripMenuItem)sender;
            foreach (var item in ModeToolStripMenuItem.DropDownItems) {
                ((ToolStripMenuItem)item).Checked = false;
            }
            typedSender.Checked = true;
        }
        #endregion

        #region RealSense 設備資訊取得
        /// <summary>
        /// 取得所有裝置資訊
        /// </summary>
        /// <returns>裝置資訊陣列</returns>
        private PXCMCapture.DeviceInfo[] GetDeviceInfos() {
            List<PXCMCapture.DeviceInfo> result = new List<PXCMCapture.DeviceInfo>();

            var desc = new PXCMSession.ImplDesc {
                group = PXCMSession.ImplGroup.IMPL_GROUP_SENSOR,
                subgroup = PXCMSession.ImplSubgroup.IMPL_SUBGROUP_VIDEO_CAPTURE
            };

            for (int i = 0; ; i++) {
                PXCMSession.ImplDesc desc1;
                if (RealSenseObjects.Session.QueryImpl(desc, i, out desc1).IsError()) break;

                PXCMCapture capture;
                if (RealSenseObjects.Session.CreateImpl(desc1, out capture).IsError()) continue;

                for (int j = 0; ; j++) {
                    PXCMCapture.DeviceInfo dinfo;
                    if (capture.QueryDeviceInfo(j, out dinfo).IsError()) break;
                    result.Add(dinfo);
                }

                capture.Dispose();
            }

            return result.ToArray();
        }

        /// <summary>
        /// 取得所有裝置支援的顏色與解析度
        /// </summary>
        private PXCMCapture.Device.StreamProfileSet[][] GetDeviceStreamProfiles() {
            var result = new List<PXCMCapture.Device.StreamProfileSet[]>();

            var desc = new PXCMSession.ImplDesc {
                group = PXCMSession.ImplGroup.IMPL_GROUP_SENSOR,
                subgroup = PXCMSession.ImplSubgroup.IMPL_SUBGROUP_VIDEO_CAPTURE
            };

            for (int i = 0; ; i++) {
                PXCMSession.ImplDesc desc1;
                if (RealSenseObjects.Session.QueryImpl(desc, i, out desc1) < pxcmStatus.PXCM_STATUS_NO_ERROR) break;

                PXCMCapture capture;
                if (RealSenseObjects.Session.CreateImpl(desc1, out capture) < pxcmStatus.PXCM_STATUS_NO_ERROR) continue;

                for (int j = 0; ; j++) {
                    PXCMCapture.DeviceInfo info;
                    if (capture.QueryDeviceInfo(j, out info) < pxcmStatus.PXCM_STATUS_NO_ERROR) break;

                    PXCMCapture.Device device = capture.CreateDevice(j);
                    if (device == null) {
                        throw new Exception("PXCMCapture.Device null");
                    }
                    var deviceResolutions = new List<PXCMCapture.Device.StreamProfileSet>();
                    for (int k = 0; k < device.QueryStreamProfileSetNum(PXCMCapture.StreamType.STREAM_TYPE_COLOR); k++) {
                        PXCMCapture.Device.StreamProfileSet profileSet;
                        device.QueryStreamProfileSet(PXCMCapture.StreamType.STREAM_TYPE_COLOR, k, out profileSet);
                        PXCMCapture.DeviceInfo dinfo;
                        device.QueryDeviceInfo(out dinfo);

                        if (IsProfileSupported(profileSet, dinfo))
                            continue;

                        deviceResolutions.Add(profileSet);
                    }

                    try {
                        result.Add(deviceResolutions.ToArray());
                    } catch (Exception e) {
                    }
                    device.Dispose();
                }

                capture.Dispose();
            }
            return result.ToArray();
        }

        private static bool IsProfileSupported(PXCMCapture.Device.StreamProfileSet profileSet, PXCMCapture.DeviceInfo dinfo) {
            return
                (profileSet.color.frameRate.min < 30) ||
                (dinfo != null && dinfo.model == PXCMCapture.DeviceModel.DEVICE_MODEL_DS4 &&
                (profileSet.color.imageInfo.width == 1920 || profileSet.color.frameRate.min > 30 || profileSet.color.imageInfo.format == PXCMImage.PixelFormat.PIXEL_FORMAT_YUY2)) ||
                (profileSet.color.options == PXCMCapture.Device.StreamOption.STREAM_OPTION_UNRECTIFIED);
        }

        private void ColorSelectedToolStripMenuItem_Click(object sender, EventArgs e) {
            var typedSender = (ToolStripMenuItem)sender;
            foreach (var item in ModeToolStripMenuItem.DropDownItems) {
                ((ToolStripMenuItem)item).Checked = false;
            }

            typedSender.Checked = true;
        }
        #endregion

        private CancellationTokenSource _token = null;
        public void SetStatus(string text) {
            this.Invoke(new UpdatePanelDelegate(() => {
                try {
                    if (_token.Token.CanBeCanceled) {
                        _token.Cancel();
                    }
                } catch { }
            }));
            _token = new CancellationTokenSource();
            Task.Run(() => {
                this.Invoke(new UpdatePanelDelegate(() => {
                    this.toolStripStatusLabel1.Text = text;
                }));
                Thread.Sleep(1000);
                this.Invoke(new UpdatePanelDelegate(() => {
                    this.toolStripStatusLabel1.Text = "";
                }));
            }, _token.Token);
        }

        /// <summary>
        /// 結束程式選單操作
        /// </summary>
        private void EndToolStripMenuItem_Click(object sender, EventArgs e) {
            this.Close();
        }

        private void startButton_Click(object sender, EventArgs e) {
            realSenseProgram.Start();
        }

        private void stopButton_Click(object sender, EventArgs e) {
            realSenseProgram.Stop();
        }

        private void registerButton_Click(object sender, EventArgs e) {
            CurentDataLocked = true;

            realSenseProgram.Paush();//暫停影像更新

            //按下註冊瞬間的名稱
            var onClickName = CurrentName;
            var registerForm = new RegisterForm() {
                UserName = onClickName,
                Picture = FacePicturebox.Image
            };

            if (registerForm.ShowDialog() != DialogResult.OK) {
                realSenseProgram.UnPaush();
                return;
            }

            int? registedUserId = null;

            if (UserTable.ContainsValue(registerForm.UserName)) {
                //重複項目，選取使用者後registedUserId
                Dictionary<int, Image> mapping = new Dictionary<int, Image>();
                var userIds = UserTable.Where(x => x.Value == registerForm.UserName).Select(x => x.Key).ToArray();
                for (int i = 0; i < userIds.Length; i++) {
                    mapping[userIds[i]] =
                        FaceData.Where(x =>
                        x.ForeignKey == userIds[i]).FirstOrDefault()?.Image;
                    if (mapping[userIds[i]] == null) {
                        mapping[userIds[i]] = new Bitmap(128, 128);
                        using(Graphics g = Graphics.FromImage(mapping[userIds[i]])) {
                            g.DrawString("找不到圖片", new Font("Arial", 16), Brushes.Black, 0, 0);
                        }
                    }
                }

                var dupForm = new DuplicateUserForm() {
                    IdFaceMapping = mapping
                };

                if (dupForm.ShowDialog() == DialogResult.OK) {
                    registedUserId = dupForm.UserId;
                }
            }

            //註冊並寫入資料庫
            var userId = Current.RegisterUser();
            if (userId == -1) {
                MessageBox.Show(
                    "註冊過程出現異常，請再嘗試一次",
                    "註冊失敗", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                CurentDataLocked = false;
                realSenseProgram.UnPaush();
                return;
            }

            //取得資料庫緩衝區
            var buffer =
                CurrentData.QueryRecognitionModule()
                .GetDatabaseBuffer();

            FaceData = RecognitionFaceData
                .FromDatabaseBuffer(buffer);

            //有選擇使用者，但是註冊後ID不合，修正ID
            if (registedUserId.HasValue && userId != registedUserId.Value) {
                var userItem = FaceData.Last();
                userItem.ForeignKey = registedUserId.Value;
                userId = userItem.ForeignKey;
                FaceDatabaseManager.UpdateBuffer(buffer, userItem);
                //FaceDatabaseManager.ClearRemovedBuffer(buffer);

                realSenseProgram.recognitionConfig
                    .SetDatabaseBuffer(buffer);
                realSenseProgram.moduleConfiguration.ApplyChanges();
            } else if (!registedUserId.HasValue && UserTable.ContainsKey(userId)) {
                //新建使用者，但是UserID指向了現有使用者
                var userItem = FaceData.Last();
                //新建使用者ID
                userItem.ForeignKey = UserTable.ToArray().Select(x => x.Key).Max() + 1;
                userId = userItem.ForeignKey;
                FaceDatabaseManager.UpdateBuffer(buffer, userItem);

                //FaceDatabaseManager.ClearRemovedBuffer(buffer);
                realSenseProgram.recognitionConfig
                    .SetDatabaseBuffer(buffer);
                realSenseProgram.moduleConfiguration.ApplyChanges();
            }

            UserTable[userId] = registerForm.UserName;

            realSenseProgram.UnPaush();//暫停影像更新
            CurentDataLocked = false;
        }

        private void unregisterButton_Click(object sender, EventArgs e) {
            var user = Current.QueryUserID();

            realSenseProgram.Paush();

            if(MessageBox.Show(
                $"本操作將清除該使用者({user} - {UserTable[user]})的所有臉部資料，您確定要進行此操作?",
                "解除註冊確認",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question) != DialogResult.OK) {
                realSenseProgram.UnPaush();
                return;
            }

            Current.UnregisterUser();
            FaceData = CurrentData.QueryRecognitionModule()
                .GetDatabase();
            
            /*if (FaceData.Where(x => x.ForeignKey == user).Count() == 0) {
                UserTable.Remove(user);
            }*/

            realSenseProgram.UnPaush();
        }

        public Bitmap Image;
        private object PicLock = new object();
        private delegate void UpdatePanelDelegate();
        public void DrawBitmap(Bitmap image) {
            if (Image != null) Image.Dispose();
            Image = image;
            pictureBox1.Invoke(//重新繪製
                new UpdatePanelDelegate(() => {
                    pictureBox1.Invalidate();
                }));
        }
        /// <summary>
        /// 當呼叫重新繪製時候
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pictureBox1_Paint(object sender, PaintEventArgs e) {
            lock (PicLock) {
                if (Image == null) return;
                e.Graphics.DrawImage(Image, 0, 0, pictureBox1.Width, pictureBox1.Height);
            }
        }

        /// <summary>
        /// 取得臉部圖片並且匯出
        /// </summary>
        /// <param name="moduleOutput"></param>
        public void DrawInformation(PXCMFaceData moduleOutput) {
            for (var i = 0; i < moduleOutput.QueryNumberOfDetectedFaces(); i++) {
                PXCMFaceData.Face face = moduleOutput.QueryFaceByIndex(i);
                if (face == null) continue;

                PXCMFaceData.DetectionData detection = face.QueryDetection();
                if (detection == null) continue;

                PXCMRectI32 range;
                detection.QueryBoundingRect(out range);
                Bitmap faceImage = new Bitmap(128, 128);

                lock (PicLock) {
                    using (Graphics g = Graphics.FromImage(faceImage)) {
                        g.DrawImage(Image,
                            new Rectangle(0, 0, 128, 128),
                            new Rectangle(range.x, range.y, range.w, range.h)
                            , GraphicsUnit.Pixel);
                    }
                }
                this.Invoke(new UpdatePanelDelegate(() => {
                    FacePicturebox.Image = faceImage;
                }));
            }
        }

        /// <summary>
        /// Real time預覽畫面轉灰階
        /// </summary>
        /// <param name="original"></param>
        /// <returns></returns>
        public static Bitmap MakeGrayscale3(Bitmap original) {
            //create a blank bitmap the same size as original
            Bitmap newBitmap = new Bitmap(original.Width, original.Height);

            //get a graphics object from the new image
            Graphics g = Graphics.FromImage(newBitmap);

            //create the grayscale ColorMatrix
            ColorMatrix colorMatrix = new ColorMatrix(
               new float[][]{
                 new float[] {.3f, .3f, .3f, 0, 0},
                 new float[] {.59f, .59f, .59f, 0, 0},
                 new float[] {.11f, .11f, .11f, 0, 0},
                 new float[] {0, 0, 0, 1, 0},
                 new float[] {0, 0, 0, 0, 1}
               });

            //create some image attributes
            ImageAttributes attributes = new ImageAttributes();

            //set the color matrix attribute
            attributes.SetColorMatrix(colorMatrix);

            //draw the original image on the new image
            //using the grayscale color matrix
            g.DrawImage(original, new Rectangle(0, 0, original.Width, original.Height),
               0, 0, original.Width, original.Height, GraphicsUnit.Pixel, attributes);

            //dispose the Graphics object
            g.Dispose();
            return newBitmap;
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e) {
            if (tabControl1.SelectedIndex != 1) return;

            UserListBox.Items.Clear();
            UserFaceListView.Items.Clear();

            foreach (var user in UserTable) {
                UserListBox.Items.Add($"{user.Key} - {user.Value}");
            }
        }

        private void UserListBox_SelectedIndexChanged(object sender, EventArgs e) {
            if (UserListBox.SelectedIndex == -1) return;
            int UserId = UserTable.Keys.ToArray()[UserListBox.SelectedIndex];
            var UserFaceData = FaceData
                .Where(x => x.ForeignKey == UserId)
                .ToArray();

            FaceImageList.Images.Clear();
            FaceImageList.Images.AddRange(UserFaceData.Select(x =>
                x.Image
            ).ToArray());

            UserFaceListView.Items.Clear();
            for (int i = 0; i < UserFaceData.Count(); i++) {
                UserFaceListView.Items.Add(new ListViewItem() {
                    ImageIndex = i,
                    Text = UserFaceData[i].PrimaryKey.ToString()
                });
            }
        }


        #region File
        private string FilePath;
        private void OpenFileToolStripMenuItem_Click(object sender, EventArgs e) {
            var open = new OpenFileDialog() {
                FileName = "",
                Multiselect = false,
                Filter = "Zip Files(*.zip)|*.zip"
            };

            if (open.ShowDialog() != DialogResult.OK) return;

            if (UserTable.Count != 0) {
                if (MessageBox.Show(
                    "您確定要開啟檔案嗎?目前尚未儲存的結果將會遺失。",
                    "開啟舊檔", 
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question
                    ) != DialogResult.OK) return;
            }
            FilePath = open.FileName;

            UserTable.Clear();
            RecognitionFaceData[] faceData = null;
            Dictionary<int, string> userTable = null;
            FaceDatabaseFile.Load(
                open.FileName,
                ref faceData,
                ref userTable);
            FaceData = faceData;
            UserTable = userTable;
            SaveFileToolStripMenuItem.Enabled = true;
            if (realSenseProgram.recognitionConfig != null) {
                realSenseProgram.recognitionConfig.SetDatabase(FaceData);
                realSenseProgram.moduleConfiguration.ApplyChanges();
            }
            tabControl1_SelectedIndexChanged(null, null);
        }
        private void SaveFileToolStripMenuItem_Click(object sender, EventArgs e) {
            FaceDatabaseFile.Save(FilePath,
                FaceData,
                UserTable);
        }
        private void SaveOtherFileToolStripMenuItem_Click(object sender, EventArgs e) {
            var save = new SaveFileDialog() {
                FileName = "",
                Filter = "Zip Files(*.zip)|*.zip"
            };

            if (save.ShowDialog() != DialogResult.OK) return;

            FilePath = save.FileName;
            FaceDatabaseFile.Save(FilePath, FaceData, UserTable);
            SaveFileToolStripMenuItem.Enabled = true;
        }        
        private void ExportToolStripMenuItem_Click(object sender, EventArgs e) {
            var folder = new FolderBrowserDialog();
            if (folder.ShowDialog() != DialogResult.OK) return;

            //realSenseProgram.Paush();

            var path = folder.SelectedPath + $"\\Database-{DateTime.UtcNow.ToString("yyyyMMdd_HHmmss")}";
            Directory.CreateDirectory(path);
            byte[] userTable  =FaceDatabaseFile.UserTableToCSVBinary(UserTable);

            FileStream userTableFile = new FileStream(path + "\\UserTable.csv", FileMode.Create);
            BinaryWriter userTableFileWriter = new BinaryWriter(userTableFile);
            userTableFileWriter.Write(userTable);
            userTableFileWriter.Flush();
            userTableFileWriter.Close();
            userTableFile.Close();
            
            foreach(var user in UserTable.Keys) {
                var path2 = path + "\\" + user;
                Directory.CreateDirectory(path2);
                var userFaces = FaceData.Where(x => x.ForeignKey == user);
                foreach(var face in userFaces) {
                    face.Image.Save(path2 + "\\" + face.PrimaryKey + ".jpg");
                }
            }
            MessageBox.Show(
                "資料庫已成功匯出至指定目錄",
                "匯出成功", 
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
            //realSenseProgram.UnPaush();
        }

        #endregion

        #region UserList
        private void DeleteUserToolStripMenuItem_Click(object sender, EventArgs e) {
            if (UserListBox.SelectedIndex == -1) return;
            if (MessageBox.Show(
                "您確定刪除該使用者所有的臉部辨識資料嗎?",
                "刪除使用者", MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
                ) != DialogResult.Yes) return;
            var key = UserTable.Keys.ToArray()[UserListBox.SelectedIndex];
            UserTable.Remove(key);
            FaceData = FaceData.Where(x => x.ForeignKey != key).ToArray();

            if (realSenseProgram.recognitionConfig != null) {
                realSenseProgram.recognitionConfig.SetDatabase(FaceData);
                realSenseProgram.moduleConfiguration.ApplyChanges();
            }
            tabControl1_SelectedIndexChanged(null, null);
        }

        private void AddUserToolStripMenuItem_Click(object sender, EventArgs e) {
            string NewUser = Interaction.InputBox("請輸入新使用者名稱", "新增使用者","新使用者");
            int NewUserId = 100;
            if (UserTable.Keys.Count != 0) {
                NewUserId = UserTable.Keys.Max() + 1;
            }
            UserTable[NewUserId] = NewUser;
            tabControl1_SelectedIndexChanged(null, null);
            UserListBox.SelectedIndex = UserListBox.Items.Count - 1;
        }

        private void EditUserToolStripMenuItem_Click(object sender, EventArgs e) {
            if (UserListBox.SelectedIndex == -1) return;
            int UserId = UserTable.Keys.ToArray()[UserListBox.SelectedIndex];
            string UserName = Interaction.InputBox("請輸入新使用者名稱", "新增使用者",UserTable[UserId]);
            UserTable[UserId] = UserName;
            tabControl1_SelectedIndexChanged(null, null);
        }
        #endregion

        #region ImageList        
        private void DeleteImageToolStripMenuItem_Click(object sender, EventArgs e) {
            if (UserFaceListView.SelectedItems.Count == 0) return;
            foreach (var item in UserFaceListView.SelectedItems) {
                var id = int.Parse(((ListViewItem)item).Text);
                FaceData = FaceData.Where(x => x.PrimaryKey != id)
                    .ToArray();
            }
            FaceData = FaceData.Select((x, i) => {
                x.PrimaryKey = i;
                return x;
            }).ToArray();

            if (realSenseProgram.recognitionConfig != null) {
                realSenseProgram.recognitionConfig.SetDatabase(FaceData);
                realSenseProgram.moduleConfiguration.ApplyChanges();
            }

            UserListBox_SelectedIndexChanged(null, null);
        }
        private void AddImageToolStripMenuItem_Click(object sender, EventArgs e) {
            if (UserListBox.SelectedIndex == -1) return;
            var open = new OpenFileDialog() {
                FileName = "",
                Multiselect = true,
                Filter = "JPEG Files(*.jpg)|*.jpg|" +
                    "PNG Files(*.png)|*.png|" +
                    "BMP Files(*.bmp)|*.bmp|" +
                    "所有圖片類型|*.jpg;*.png;*.bmp"
            };

            if (open.ShowDialog() != DialogResult.OK) return;
            var key = UserTable.Keys.ToArray()[UserListBox.SelectedIndex];

            foreach (var file in open.FileNames) {
                var newData = new RecognitionFaceData();
                newData.PrimaryKey = FaceData.Count();
                newData.ForeignKey = key;
                newData.Image = new Bitmap(new Bitmap(file), 128, 128);

                var faceData = FaceData;
                Array.Resize(ref faceData, FaceData.Length + 1);
                faceData[faceData.Length - 1] = newData;
                FaceData = faceData;
            }

            if (realSenseProgram.recognitionConfig != null) {
                realSenseProgram.recognitionConfig.SetDatabase(FaceData);
                realSenseProgram.moduleConfiguration.ApplyChanges();
            }

            UserListBox_SelectedIndexChanged(null, null);
        }
        #endregion
    }
}