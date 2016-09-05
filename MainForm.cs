using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Threading;
using System.Windows.Forms;

namespace DF_FaceTracking.cs
{
    public partial class MainForm : Form
    {
        public enum Label
        {
            StatusLabel,
            AlertsLabel
        };

        public PXCMSession Session;
        public volatile bool Register = false;
        public volatile bool Unregister = false;
        public volatile bool Stopped = false;
        public readonly Dictionary<PXCMFaceConfiguration.TrackingModeType, string> FaceModesMap =
           new Dictionary<PXCMFaceConfiguration.TrackingModeType, string>()
           {
                { PXCMFaceConfiguration.TrackingModeType.FACE_MODE_COLOR_PLUS_DEPTH , "3D Tracking" },
                { PXCMFaceConfiguration.TrackingModeType.FACE_MODE_COLOR, "2D Tracking" },
                { PXCMFaceConfiguration.TrackingModeType.FACE_MODE_IR, "IR Tracking" },
                { PXCMFaceConfiguration.TrackingModeType.FACE_MODE_COLOR_STILL, "2D Still" }
           };

        private readonly object m_bitmapLock = new object();
        private readonly FaceTextOrganizer m_faceTextOrganizer;
        private Dictionary<String, ModuleSettings> m_moduleSettings;
        private IEnumerable<CheckBox> m_modulesCheckBoxes;
        private IEnumerable<TextBox> m_modulesTextBoxes;
        private Bitmap m_bitmap;
        private string m_filename;
        private Tuple<PXCMImage.ImageInfo, PXCMRangeF32> m_selectedColorResolution;
        private volatile bool m_closing;
        private static ToolStripMenuItem m_deviceMenuItem;
        private static ToolStripMenuItem m_moduleMenuItem;
        private const int LandmarkAlignment = -3;
        private const int DefaultNumberOfFaces = 4;

       

        public MainForm(PXCMSession session)
        {
            InitializeComponent();
            InitializeTextBoxes();

            m_faceTextOrganizer = new FaceTextOrganizer();
            m_deviceMenuItem = new ToolStripMenuItem("Device");
            m_moduleMenuItem = new ToolStripMenuItem("Module");
            Session = session;

            CreateResolutionMap();
            PopulateDeviceMenu();
            PopulateModuleMenu();
            PopulateProfileMenu();

            InitializeUserSettings();
            InitializeCheckboxes();
            DisableUnsupportedAlgos();
            RestoreUserSettings();

            FormClosing += MainForm_FormClosing;
            Panel2.Paint += Panel_Paint;
        }

        private void InitializeUserSettings()
        {
            m_moduleSettings = new Dictionary<String, ModuleSettings>();

            var moduleList = new List<String>
            {
                "Detection",
                "Landmarks",
                "Pose",
                "Expressions",
                "Recognition",
                "Pulse"
            };

            SetDefaultSettings();
        }

        private string GetDeviceName()
        {
            var deviceMenuString = GetCheckedDevice();

            if (deviceMenuString == null)
                return "InvalidCamera";

            if (deviceMenuString.Contains("R200") || deviceMenuString.Contains("DS4"))
            {
                return "DS4";
            }

            return "IVcam";
        }

        private void InitializeTextBoxes()
        {
            m_modulesTextBoxes = new List<TextBox>
            {
                NumDetectionText,
                NumLandmarksText,
                NumPoseText,
                NumPulseText,
                NumExpressionsText,
            };

            foreach (var textBox in m_modulesTextBoxes)
            {
                textBox.Text = DefaultNumberOfFaces.ToString();
            }
        }

        private void DisableUnsupportedAlgos()
        {
            string deviceMenuString = GetCheckedDevice();

            if (deviceMenuString != null && (deviceMenuString.Contains("R200") || deviceMenuString.Contains("DS4")))
            {
                DisablePose();
                DisableExpressions();
                DisablePulse();
            }
        }

        private void InitializeCheckboxes()
        {
            m_modulesCheckBoxes = new List<CheckBox>
            {
                Detection,
                Landmarks,
                Pose,
                Pulse,
                Expressions,
                Recognition
            };
        }

        public Dictionary<string, PXCMCapture.DeviceInfo> Devices { get; set; }
        public Dictionary<string, IEnumerable<Tuple<PXCMImage.ImageInfo, PXCMRangeF32>>> ColorResolutions { get; set; }

        private readonly List<Tuple<int, int>> SupportedColorResolutions = new List<Tuple<int, int>>
        {
            Tuple.Create(1920, 1080),
            Tuple.Create(1280, 720),
            Tuple.Create(960, 540),
            Tuple.Create(640, 480),
            Tuple.Create(640, 360),
        };

        public int NumDetection
        {
            get
            {
                int val;
                try
                {
                    val = Convert.ToInt32(NumDetectionText.Text);
                }
                catch
                {
                    val = 0;
                }
                return val;
            }
        }

        public int NumLandmarks
        {
            get
            {
                int val;
                try
                {
                    val = Convert.ToInt32(NumLandmarksText.Text);
                }
                catch
                {
                    val = 0;
                }
                return val;
            }
        }

        public int NumPose
        {
            get
            {
                int val;
                try
                {
                    val = Convert.ToInt32(NumPoseText.Text);
                }
                catch
                {
                    val = 0;
                }
                return val;
            }
        }

        public int NumExpressions
        {
            get
            {
                int val;
                try
                {
                    val = Convert.ToInt32(NumExpressionsText.Text);
                }
                catch
                {
                    val = 0;
                }
                return val;
            }
        }

        public int NumPulse
        {
            get
            {
                int val;
                try
                {
                    val = Convert.ToInt32(NumPulseText.Text);
                }
                catch
                {
                    val = 0;
                }
                return val;
            }
        }

        public string GetFileName()
        {
            return m_filename;
        }

        public bool IsRecognitionChecked()
        {
            return Recognition.Checked;
        }

        private void CreateResolutionMap()
        {
            ColorResolutions = new Dictionary<string, IEnumerable<Tuple<PXCMImage.ImageInfo, PXCMRangeF32>>>();
            var desc = new PXCMSession.ImplDesc
            {
                group = PXCMSession.ImplGroup.IMPL_GROUP_SENSOR,
                subgroup = PXCMSession.ImplSubgroup.IMPL_SUBGROUP_VIDEO_CAPTURE
            };

            for (int i = 0; ; i++)
            {
                PXCMSession.ImplDesc desc1;
                if (Session.QueryImpl(desc, i, out desc1) < pxcmStatus.PXCM_STATUS_NO_ERROR) break;

                PXCMCapture capture;
                if (Session.CreateImpl(desc1, out capture) < pxcmStatus.PXCM_STATUS_NO_ERROR) continue;

                for (int j = 0; ; j++)
                {
                    PXCMCapture.DeviceInfo info;
                    if (capture.QueryDeviceInfo(j, out info) < pxcmStatus.PXCM_STATUS_NO_ERROR) break;

                    PXCMCapture.Device device = capture.CreateDevice(j);
                    if (device == null)
                    {
                        throw new Exception("PXCMCapture.Device null");
                    }
                    var deviceResolutions = new List<Tuple<PXCMImage.ImageInfo, PXCMRangeF32>>();

                    for (int k = 0; k < device.QueryStreamProfileSetNum(PXCMCapture.StreamType.STREAM_TYPE_COLOR); k++)
                    {
                        PXCMCapture.Device.StreamProfileSet profileSet;
                        device.QueryStreamProfileSet(PXCMCapture.StreamType.STREAM_TYPE_COLOR, k, out profileSet);
                        PXCMCapture.DeviceInfo dinfo;
                        device.QueryDeviceInfo(out dinfo);

                        var currentRes = new Tuple<PXCMImage.ImageInfo, PXCMRangeF32>(profileSet.color.imageInfo, profileSet.color.frameRate);

                        if (IsProfileSupported(profileSet, dinfo))
                            continue;

                        if (SupportedColorResolutions.Contains(new Tuple<int, int>(currentRes.Item1.width, currentRes.Item1.height)))
                            deviceResolutions.Add(currentRes);

                    }

                    try
                    {
                        ColorResolutions.Add(info.name, deviceResolutions);
                    }
                    catch (Exception e)
                    {
                    }
                    device.Dispose();
                }

                capture.Dispose();
            }
        }

        private static bool IsProfileSupported(PXCMCapture.Device.StreamProfileSet profileSet, PXCMCapture.DeviceInfo dinfo)
        {
            return 
                (profileSet.color.frameRate.min < 30) ||
                (dinfo != null && dinfo.model == PXCMCapture.DeviceModel.DEVICE_MODEL_DS4 &&
                (profileSet.color.imageInfo.width == 1920 || profileSet.color.frameRate.min > 30 || profileSet.color.imageInfo.format == PXCMImage.PixelFormat.PIXEL_FORMAT_YUY2)) || 
                (profileSet.color.options == PXCMCapture.Device.StreamOption.STREAM_OPTION_UNRECTIFIED);
        }

        public void PopulateDeviceMenu()
        {
            Devices = new Dictionary<string, PXCMCapture.DeviceInfo>();
            var desc = new PXCMSession.ImplDesc
            {
                group = PXCMSession.ImplGroup.IMPL_GROUP_SENSOR,
                subgroup = PXCMSession.ImplSubgroup.IMPL_SUBGROUP_VIDEO_CAPTURE
            };

            for (int i = 0; ; i++)
            {
                PXCMSession.ImplDesc desc1;
                if (Session.QueryImpl(desc, i, out desc1) < pxcmStatus.PXCM_STATUS_NO_ERROR) break;

                PXCMCapture capture;
                if (Session.CreateImpl(desc1, out capture) < pxcmStatus.PXCM_STATUS_NO_ERROR) continue;

                for (int j = 0; ; j++)
                {
                    PXCMCapture.DeviceInfo dinfo;
                    if (capture.QueryDeviceInfo(j, out dinfo) < pxcmStatus.PXCM_STATUS_NO_ERROR) break;
                    string name = dinfo.name;
                    if (Devices.ContainsKey(dinfo.name))
                        name += j;
                    Devices.Add(name, dinfo);
                    var sm1 = new ToolStripMenuItem(dinfo.name, null, Device_Item_Click);
                    m_deviceMenuItem.DropDownItems.Add(sm1);
                    sm1.Click += (sender, eventArgs) =>
                    {
                        DisableUnsupportedAlgos();
                    };
                }

                capture.Dispose();
            }

            if (m_deviceMenuItem.DropDownItems.Count > 0)
            {
                ((ToolStripMenuItem)m_deviceMenuItem.DropDownItems[0]).Checked = true;
                PopulateColorResolutionMenu(m_deviceMenuItem.DropDownItems[0].ToString());
            }

            try
            {
                MainMenu.Items.RemoveAt(0);
            }
            catch (NotSupportedException)
            {
                m_deviceMenuItem.Dispose();
                throw;
            }
            MainMenu.Items.Insert(0, m_deviceMenuItem);
        }

        public void PopulateColorResolutionMenu(string deviceName)
        {
            bool foundDefaultResolution = false;
            var sm = new ToolStripMenuItem("Color");
            foreach (var resolution in ColorResolutions[deviceName])
            {
                var resText = PixelFormat2String(resolution.Item1.format) + " " + resolution.Item1.width + "x"
                              + resolution.Item1.height + " " + resolution.Item2.max + " fps";
                var sm1 = new ToolStripMenuItem(resText, null);
                var selectedResolution = resolution;
                sm1.Click += (sender, eventArgs) =>
                {
                    m_selectedColorResolution = selectedResolution;
                    ColorResolution_Item_Click(sender);
                };

                sm.DropDownItems.Add(sm1);

                int width = selectedResolution.Item1.width;
                int height = selectedResolution.Item1.height;
                PXCMImage.PixelFormat format = selectedResolution.Item1.format;
                float fps = selectedResolution.Item2.min;

                if (DefaultCameraConfig.IsDefaultDeviceConfig(deviceName, width, height, format, fps))
                {
                    foundDefaultResolution = true;
                    sm1.Checked = true;
                    sm1.PerformClick();
                }
            }

            if (!foundDefaultResolution && sm.DropDownItems.Count > 0)
            {
                ((ToolStripMenuItem)sm.DropDownItems[0]).Checked = true;
                ((ToolStripMenuItem)sm.DropDownItems[0]).PerformClick();
            }

            try
            {
                MainMenu.Items.RemoveAt(1);
            }
            catch (NotSupportedException)
            {
                sm.Dispose();
                throw;
            }
            MainMenu.Items.Insert(1, sm);
        }

        public class DefaultCameraConfig
        {
            public static bool IsDefaultDeviceConfig(string deviceName, int width, int height, PXCMImage.PixelFormat format, float fps)
            {
                if (deviceName.Contains("R200"))
                {
                    return width == DefaultDs4Width && height == DefaultDs4Height && format == DefaultDs4PixelFormat && fps == DefaultDs4Fps;
                }

                if (deviceName.Contains("F200") || deviceName.Contains("SR300"))
                {
                    return width == DefaultIvcamWidth && height == DefaultIvcamHeight && format == DefaultIvcamPixelFormat && fps == DefaultIvcamFps;
                }

                return false;
            }

            private static readonly int DefaultDs4Width = 640;
            private static readonly int DefaultDs4Height = 480;
            private static readonly PXCMImage.PixelFormat DefaultDs4PixelFormat = PXCMImage.PixelFormat.PIXEL_FORMAT_RGB32;
            private static readonly float DefaultDs4Fps = 30f;

            private static readonly int DefaultIvcamWidth = 640;
            private static readonly int DefaultIvcamHeight = 360;
            private static readonly PXCMImage.PixelFormat DefaultIvcamPixelFormat = PXCMImage.PixelFormat.PIXEL_FORMAT_YUY2;
            private static readonly float DefaultIvcamFps = 30f;
        }



        private void PopulateModuleMenu()
        {
            var desc = new PXCMSession.ImplDesc();
            desc.cuids[0] = PXCMFaceModule.CUID;

            for (int i = 0; ; i++)
            {
                PXCMSession.ImplDesc desc1;
                if (Session.QueryImpl(desc, i, out desc1) < pxcmStatus.PXCM_STATUS_NO_ERROR) break;
                var mm1 = new ToolStripMenuItem(desc1.friendlyName, null, Module_Item_Click);
                m_moduleMenuItem.DropDownItems.Add(mm1);
            }
            if (m_moduleMenuItem.DropDownItems.Count > 0)
                ((ToolStripMenuItem)m_moduleMenuItem.DropDownItems[0]).Checked = true;
            try
            {
                MainMenu.Items.RemoveAt(2);
            }
            catch (NotSupportedException)
            {
                m_moduleMenuItem.Dispose();
                throw;
            }
            MainMenu.Items.Insert(2, m_moduleMenuItem);

        }

        private void PopulateProfileMenu()
        {
            var pm = new ToolStripMenuItem("Profile");

            foreach (
                var trackingMode in FaceModesMap.Keys)
            {
                var pm1 = new ToolStripMenuItem(FaceModesMap[trackingMode], null, Profile_Item_Click);
                pm.DropDownItems.Add(pm1);

                if (trackingMode == PXCMFaceConfiguration.TrackingModeType.FACE_MODE_COLOR_PLUS_DEPTH) //3d = default
                {
                    pm1.Checked = true;
                }
            }
            try
            {
                MainMenu.Items.RemoveAt(3);
            }
            catch (NotSupportedException)
            {
                pm.Dispose();
                throw;
            }
            MainMenu.Items.Insert(3, pm);
        }

        private static string PixelFormat2String(PXCMImage.PixelFormat format)
        {
            switch (format)
            {
                case PXCMImage.PixelFormat.PIXEL_FORMAT_YUY2:
                    return "YUY2";
                case PXCMImage.PixelFormat.PIXEL_FORMAT_RGB32:
                    return "RGB32";
                case PXCMImage.PixelFormat.PIXEL_FORMAT_RGB24:
                    return "RGB24";
            }
            return "NA";
        }

        private void RadioCheck(object sender, string name)
        {
            foreach (ToolStripMenuItem m in MainMenu.Items)
            {
                if (!m.Text.Equals(name)) continue;
                foreach (ToolStripMenuItem e1 in m.DropDownItems)
                {
                    e1.Checked = (sender == e1);
                }
            }
        }

        private void ColorResolution_Item_Click(object sender)
        {
            RadioCheck(sender, "Color");
        }

        private void Device_Item_Click(object sender, EventArgs e)
        {
            PopulateColorResolutionMenu(sender.ToString());
            RadioCheck(sender, "Device");
        }

        private void Module_Item_Click(object sender, EventArgs e)
        {
            RadioCheck(sender, "Module");
            PopulateProfileMenu();
        }

        private void Profile_Item_Click(object sender, EventArgs e)
        {
            RadioCheck(sender, "Profile");
        }

        private void Start_Click(object sender, EventArgs e)
        {
            SaveUserSettings();
            DisableModuleCheckBoxes();
            DisableTextBoxes();

            MainMenu.Enabled = false;
            Start.Enabled = false;
            Stop.Enabled = true;

            RegisterUser.Enabled = Recognition.Checked;
            UnregisterUser.Enabled = Recognition.Checked;

            Stopped = false;

            var thread = new Thread(DoTracking);
            thread.Start();
        }

        private void DoTracking()
        {
            var ft = new FaceTracking(this);
            ft.SimplePipeline();

            Invoke(new DoTrackingCompleted(() =>
            {
                EnableModuleCheckBoxes();
                EnableTextBoxes();
                DisableUnsupportedAlgos();
                RestoreUserSettings();

                MainMenu.Enabled = true;
                Start.Enabled = true;
                Stop.Enabled = false;

                RegisterUser.Enabled = false;
                UnregisterUser.Enabled = false;

                if (m_closing) Close();
            }));
        }

        private void Stop_Click(object sender, EventArgs e)
        {
            Stopped = true;
        }

        private void SaveUserSettings()
        {
            m_moduleSettings["Detection"].IsEnabled = Detection.Checked;
            m_moduleSettings["Detection"].NumberOfFaces = NumDetection;

            m_moduleSettings["Landmarks"].IsEnabled = Landmarks.Checked;
            m_moduleSettings["Landmarks"].NumberOfFaces = NumLandmarks;

            m_moduleSettings["Pose"].IsEnabled = Pose.Checked;
            m_moduleSettings["Pose"].NumberOfFaces = NumPose;

            m_moduleSettings["Expressions"].IsEnabled = Expressions.Checked;
            m_moduleSettings["Expressions"].NumberOfFaces = NumExpressions;

            m_moduleSettings["Pulse"].IsEnabled = Pulse.Checked;
            m_moduleSettings["Pulse"].NumberOfFaces = NumPulse;

            m_moduleSettings["Recognition"].IsEnabled = Recognition.Checked;
        }

        private void RestoreUserSettings()
        {
            Detection.Checked = m_moduleSettings["Detection"].IsEnabled;
            NumDetectionText.Text = m_moduleSettings["Detection"].NumberOfFaces.ToString();

            Landmarks.Checked = m_moduleSettings["Landmarks"].IsEnabled;
            NumLandmarksText.Text = m_moduleSettings["Landmarks"].NumberOfFaces.ToString();

            Pose.Checked = m_moduleSettings["Pose"].IsEnabled;
            NumPoseText.Text = m_moduleSettings["Pose"].NumberOfFaces.ToString();

            Expressions.Checked = m_moduleSettings["Expressions"].IsEnabled;
            NumExpressionsText.Text = m_moduleSettings["Expressions"].NumberOfFaces.ToString();

            Pulse.Checked = m_moduleSettings["Pulse"].IsEnabled;
            NumPulseText.Text = m_moduleSettings["Pulse"].NumberOfFaces.ToString();

            Recognition.Checked = m_moduleSettings["Recognition"].IsEnabled;
        }

        public string GetCheckedDevice()
        {
            return (from ToolStripMenuItem m in MainMenu.Items
                    where m.Text.Equals("Device")
                    from ToolStripMenuItem e in m.DropDownItems
                    where e.Checked
                    select e.Text).FirstOrDefault();
        }

        public PXCMCapture.DeviceInfo GetCheckedDeviceInfo()
        {
            foreach (ToolStripMenuItem m in MainMenu.Items)
            {
                if (!m.Text.Equals("Device")) continue;
                for (int i = 0; i < m.DropDownItems.Count; i++)
                {
                    if (((ToolStripMenuItem)m.DropDownItems[i]).Checked)
                        return Devices.ElementAt(i).Value;
                }
            }
            return new PXCMCapture.DeviceInfo();
        }

        public Tuple<PXCMImage.ImageInfo, PXCMRangeF32> GetCheckedColorResolution()
        {
            return m_selectedColorResolution;
        }

        public string GetCheckedModule()
        {
            return (from ToolStripMenuItem m in MainMenu.Items
                    where m.Text.Equals("Module")
                    from ToolStripMenuItem e in m.DropDownItems
                    where e.Checked
                    select e.Text).FirstOrDefault();
        }

        public string GetCheckedProfile()
        {
            foreach (var m in from ToolStripMenuItem m in MainMenu.Items where m.Text.Equals("Profile") select m)
            {
                for (var i = 0; i < m.DropDownItems.Count; i++)
                {
                    if (((ToolStripMenuItem)m.DropDownItems[i]).Checked)
                        return m.DropDownItems[i].Text;
                }
            }
            return "";
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Stopped = true;
            e.Cancel = Stop.Enabled;
            m_closing = true;
        }

        public void UpdateStatus(string status, Label label)
        {
            if (label == Label.StatusLabel)
                Status2.Invoke(new UpdateStatusDelegate(delegate(string s) { StatusLabel.Text = s; }),
                    new object[] { status });

            if (label == Label.AlertsLabel)
                Status2.Invoke(new UpdateStatusDelegate(delegate(string s) { AlertsLabel.Text = s; }),
                    new object[] { status });
        }

        private void Panel_Paint(object sender, PaintEventArgs e)
        {
            lock (m_bitmapLock)
            {
                if (m_bitmap == null) return;
                if (Scale.Checked)
                {
                    e.Graphics.DrawImage(m_bitmap, Panel2.ClientRectangle);
                }
                else
                {
                    e.Graphics.DrawImageUnscaled(m_bitmap, 0, 0);
                }
            }
        }

        public void UpdatePanel()
        {
            Panel2.Invoke(new UpdatePanelDelegate(() => Panel2.Invalidate()));
        }

        public void DrawBitmap(Bitmap picture)
        {
            lock (m_bitmapLock)
            {
                if (m_bitmap != null)
                {
                    m_bitmap.Dispose();
                }
                m_bitmap = new Bitmap(picture);
            }
        }

        public void DrawGraphics(PXCMFaceData moduleOutput)
        {
            Debug.Assert(moduleOutput != null);

            for (var i = 0; i < moduleOutput.QueryNumberOfDetectedFaces(); i++)
            {
                PXCMFaceData.Face face = moduleOutput.QueryFaceByIndex(i);
                if (face == null)
                {
                    throw new Exception("DrawGraphics::PXCMFaceData.Face null");
                }

                lock (m_bitmapLock)
                {
                    m_faceTextOrganizer.ChangeFace(i, face, m_bitmap.Height, m_bitmap.Width);
                }

                DrawLocation(face);
                DrawLandmark(face);
                DrawPose(face);
                DrawPulse(face);
                DrawExpressions(face);
                DrawRecognition(face);
            }
        }

        private void RegisterUser_Click(object sender, EventArgs e)
        {
            Register = true;
        }

        private void UnregisterUser_Click(object sender, EventArgs e)
        {
            Unregister = true;
        }

        #region Playback / Record

        private void Live_Click(object sender, EventArgs e)
        {
            Playback.Checked = Record.Checked = false;
            Live.Checked = true;
        }

        private void Playback_Click(object sender, EventArgs e)
        {
            Live.Checked = Record.Checked = false;
            Playback.Checked = true;
            var ofd = new OpenFileDialog
            {
                Filter = @"RSSDK clip|*.rssdk|Old format clip|*.pcsdk|All files|*.*",
                CheckFileExists = true,
                CheckPathExists = true
            };
            try
            {
                m_filename = (ofd.ShowDialog() == DialogResult.OK) ? ofd.FileName : null;
            }
            catch (Exception)
            {
                ofd.Dispose();
                throw;
            }
            ofd.Dispose();
        }

        public bool IsInPlaybackState()
        {
            return Playback.Checked;
        }

        private void Record_Click(object sender, EventArgs e)
        {
            Live.Checked = Playback.Checked = false;
            Record.Checked = true;
            var sfd = new SaveFileDialog
            {
                Filter = @"RSSDK clip|*.rssdk|All files|*.*",
                CheckPathExists = true,
                OverwritePrompt = true,
                AddExtension = true
            };
            try
            {
                m_filename = (sfd.ShowDialog() == DialogResult.OK) ? sfd.FileName : null;
            }
            catch (Exception)
            {
                sfd.Dispose();
                throw;
            }
            sfd.Dispose();
        }

        public bool GetRecordState()
        {
            return Record.Checked;
        }

        public string GetPlaybackFile()
        {
            return Invoke(new GetFileDelegate(() =>
            {
                var ofd = new OpenFileDialog
                {
                    Filter = @"All files (*.*)|*.*",
                    CheckFileExists = true,
                    CheckPathExists = true
                };
                return ofd.ShowDialog() == DialogResult.OK ? ofd.FileName : null;
            })) as string;
        }

        public string GetRecordFile()
        {
            return Invoke(new GetFileDelegate(() =>
            {
                var sfd = new SaveFileDialog
                {
                    Filter = @"All files (*.*)|*.*",
                    CheckFileExists = true,
                    OverwritePrompt = true
                };
                if (sfd.ShowDialog() == DialogResult.OK) return sfd.FileName;
                return null;
            })) as string;
        }

        private delegate string GetFileDelegate();

        #endregion

        #region Modules Drawing

        private static readonly Assembly m_assembly = Assembly.GetExecutingAssembly();

        private readonly ResourceSet m_resources =
            new ResourceSet(m_assembly.GetManifestResourceStream(@"DF_FaceTracking.cs.Properties.Resources.resources"));

        private readonly Dictionary<PXCMFaceData.ExpressionsData.FaceExpression, Bitmap> m_cachedExpressions =
            new Dictionary<PXCMFaceData.ExpressionsData.FaceExpression, Bitmap>();

        private readonly Dictionary<PXCMFaceData.ExpressionsData.FaceExpression, string> m_expressionDictionary =
            new Dictionary<PXCMFaceData.ExpressionsData.FaceExpression, string>
            {
                {PXCMFaceData.ExpressionsData.FaceExpression.EXPRESSION_MOUTH_OPEN, @"MouthOpen"},
                {PXCMFaceData.ExpressionsData.FaceExpression.EXPRESSION_SMILE, @"Smile"},
                {PXCMFaceData.ExpressionsData.FaceExpression.EXPRESSION_KISS, @"Kiss"},
                {PXCMFaceData.ExpressionsData.FaceExpression.EXPRESSION_EYES_UP, @"Eyes_Turn_Up"},
                {PXCMFaceData.ExpressionsData.FaceExpression.EXPRESSION_EYES_DOWN, @"Eyes_Turn_Down"},
                {PXCMFaceData.ExpressionsData.FaceExpression.EXPRESSION_EYES_TURN_LEFT, @"Eyes_Turn_Left"},
                {PXCMFaceData.ExpressionsData.FaceExpression.EXPRESSION_EYES_TURN_RIGHT, @"Eyes_Turn_Right"},
                {PXCMFaceData.ExpressionsData.FaceExpression.EXPRESSION_EYES_CLOSED_LEFT, @"Eyes_Closed_Left"},
                {PXCMFaceData.ExpressionsData.FaceExpression.EXPRESSION_EYES_CLOSED_RIGHT, @"Eyes_Closed_Right"},
                {PXCMFaceData.ExpressionsData.FaceExpression.EXPRESSION_BROW_LOWERER_RIGHT, @"Brow_Lowerer_Right"},
                {PXCMFaceData.ExpressionsData.FaceExpression.EXPRESSION_BROW_LOWERER_LEFT, @"Brow_Lowerer_Left"},
                {PXCMFaceData.ExpressionsData.FaceExpression.EXPRESSION_BROW_RAISER_RIGHT, @"Brow_Raiser_Right"},
                {PXCMFaceData.ExpressionsData.FaceExpression.EXPRESSION_BROW_RAISER_LEFT, @"Brow_Raiser_Left"}
            };

        public void DrawLocation(PXCMFaceData.Face face)
        {
            Debug.Assert(face != null);
            if (m_bitmap == null || !Detection.Checked) return;

            PXCMFaceData.DetectionData detection = face.QueryDetection();
            if (detection == null)
                return;

            lock (m_bitmapLock)
            {
                using (Graphics graphics = Graphics.FromImage(m_bitmap))
                using (var pen = new Pen(m_faceTextOrganizer.Colour, 3.0f))
                using (var brush = new SolidBrush(m_faceTextOrganizer.Colour))
                using (var font = new Font(FontFamily.GenericMonospace, m_faceTextOrganizer.FontSize, FontStyle.Bold))
                {
                    graphics.DrawRectangle(pen, m_faceTextOrganizer.RectangleLocation);
                    String faceId = String.Format("Face ID: {0}",
                        face.QueryUserID().ToString(CultureInfo.InvariantCulture));
                    graphics.DrawString(faceId, font, brush, m_faceTextOrganizer.FaceIdLocation);
                }
            }
        }

        public void DrawLandmark(PXCMFaceData.Face face)
        {
            Debug.Assert(face != null);
            PXCMFaceData.LandmarksData landmarks = face.QueryLandmarks();
            if (m_bitmap == null || !Landmarks.Checked || landmarks == null) return;

            lock (m_bitmapLock)
            {
                using (Graphics graphics = Graphics.FromImage(m_bitmap))
                using (var brush = new SolidBrush(Color.White))
                using (var lowConfidenceBrush = new SolidBrush(Color.Red))
                using (var font = new Font(FontFamily.GenericMonospace, m_faceTextOrganizer.FontSize, FontStyle.Bold))
                {
                    PXCMFaceData.LandmarkPoint[] points;
                    bool res = landmarks.QueryPoints(out points);
                    Debug.Assert(res);

                    var point = new PointF();

                    foreach (PXCMFaceData.LandmarkPoint landmark in points)
                    {
                        point.X = landmark.image.x + LandmarkAlignment;
                        point.Y = landmark.image.y + LandmarkAlignment;

                        if (landmark.confidenceImage == 0)
                            graphics.DrawString("x", font, lowConfidenceBrush, point);
                        else
                            graphics.DrawString("•", font, brush, point);
                    }
                }
            }
        }

        public void DrawPose(PXCMFaceData.Face face)
        {
            Debug.Assert(face != null);
            PXCMFaceData.PoseEulerAngles poseAngles;
            PXCMFaceData.PoseData pdata = face.QueryPose();
            if (pdata == null)
            {
                return;
            }
            if (!Pose.Checked || !pdata.QueryPoseAngles(out poseAngles)) return;

            lock (m_bitmapLock)
            {
                using (Graphics graphics = Graphics.FromImage(m_bitmap))
                using (var brush = new SolidBrush(m_faceTextOrganizer.Colour))
                using (var font = new Font(FontFamily.GenericMonospace, m_faceTextOrganizer.FontSize, FontStyle.Bold))
                {
                    string yawText = String.Format("Yaw = {0}",
                        Convert.ToInt32(poseAngles.yaw).ToString(CultureInfo.InvariantCulture));
                    graphics.DrawString(yawText, font, brush, m_faceTextOrganizer.PoseLocation.X,
                        m_faceTextOrganizer.PoseLocation.Y);

                    string pitchText = String.Format("Pitch = {0}",
                        Convert.ToInt32(poseAngles.pitch).ToString(CultureInfo.InvariantCulture));
                    graphics.DrawString(pitchText, font, brush, m_faceTextOrganizer.PoseLocation.X,
                        m_faceTextOrganizer.PoseLocation.Y + m_faceTextOrganizer.FontSize);

                    string rollText = String.Format("Roll = {0}",
                        Convert.ToInt32(poseAngles.roll).ToString(CultureInfo.InvariantCulture));
                    graphics.DrawString(rollText, font, brush, m_faceTextOrganizer.PoseLocation.X,
                        m_faceTextOrganizer.PoseLocation.Y + 2 * m_faceTextOrganizer.FontSize);
                }
            }
        }

        public void DrawExpressions(PXCMFaceData.Face face)
        {
            Debug.Assert(face != null);
            if (m_bitmap == null || !Expressions.Checked) return;

            PXCMFaceData.ExpressionsData expressionsOutput = face.QueryExpressions();

            if (expressionsOutput == null) return;

            lock (m_bitmapLock)
            {
                using (Graphics graphics = Graphics.FromImage(m_bitmap))
                using (var brush = new SolidBrush(m_faceTextOrganizer.Colour))
                {
                    const int imageSizeWidth = 18;
                    const int imageSizeHeight = 18;

                    int positionX = m_faceTextOrganizer.ExpressionsLocation.X;
                    int positionXText = positionX + imageSizeWidth;
                    int positionY = m_faceTextOrganizer.ExpressionsLocation.Y;
                    int positionYText = positionY + imageSizeHeight / 4;

                    foreach (var expressionEntry in m_expressionDictionary)
                    {
                        PXCMFaceData.ExpressionsData.FaceExpression expression = expressionEntry.Key;
                        PXCMFaceData.ExpressionsData.FaceExpressionResult result;
                        bool status = expressionsOutput.QueryExpression(expression, out result);
                        if (!status) continue;

                        Bitmap cachedExpressionBitmap;
                        bool hasCachedExpressionBitmap = m_cachedExpressions.TryGetValue(expression, out cachedExpressionBitmap);
                        if (!hasCachedExpressionBitmap)
                        {
                            cachedExpressionBitmap = (Bitmap)m_resources.GetObject(expressionEntry.Value);
                            m_cachedExpressions.Add(expression, cachedExpressionBitmap);
                        }

                        using (var font = new Font(FontFamily.GenericMonospace, m_faceTextOrganizer.FontSize, FontStyle.Bold))
                        {
                            Debug.Assert(cachedExpressionBitmap != null, "cachedExpressionBitmap != null");
                            graphics.DrawImage(cachedExpressionBitmap, new Rectangle(positionX, positionY, imageSizeWidth, imageSizeHeight));
                            var expressionText = String.Format("= {0}", result.intensity);
                            graphics.DrawString(expressionText, font, brush, positionXText, positionYText);

                            positionY += imageSizeHeight;
                            positionYText += imageSizeHeight;
                        }
                    }
                }
            }
        }

        public void DrawRecognition(PXCMFaceData.Face face)
        {
            Debug.Assert(face != null);
            if (m_bitmap == null || !Recognition.Checked) return;

            PXCMFaceData.RecognitionData qrecognition = face.QueryRecognition();
            if (qrecognition == null)
            {
                throw new Exception(" PXCMFaceData.RecognitionData null");
            }
            var userId = qrecognition.QueryUserID();
            #region 顯示辨識結果
            string recognitionText = null;
            if (userId == -1) {
                recognitionText = "Not Registered";
            } else {
                var mapping = FaceTracking.NameMapping.Where(x => x.DataIds.Contains(userId)).FirstOrDefault();
                if(mapping != null) {
                    recognitionText = $"{mapping.Id} - {mapping.Name}";
                }
            }
            #endregion


            lock (m_bitmapLock)
            {
                using (Graphics graphics = Graphics.FromImage(m_bitmap))
                using (var brush = new SolidBrush(m_faceTextOrganizer.Colour))
                using (var font = new Font(FontFamily.GenericMonospace, m_faceTextOrganizer.FontSize, FontStyle.Bold))
                {
                    graphics.DrawString(recognitionText, font, brush, m_faceTextOrganizer.RecognitionLocation);
                }
            }
        }

        private void DrawPulse(PXCMFaceData.Face face)
        {
            Debug.Assert(face != null);
            if (m_bitmap == null || !Pulse.Checked) return;

            var pulseData = face.QueryPulse();
            if (pulseData == null)
                return;

            lock (m_bitmapLock)
            {
                var pulseString = "Pulse: " + pulseData.QueryHeartRate();

                using (var graphics = Graphics.FromImage(m_bitmap))
                using (var brush = new SolidBrush(m_faceTextOrganizer.Colour))
                using (var font = new Font(FontFamily.GenericMonospace, m_faceTextOrganizer.FontSize, FontStyle.Bold))
                {
                    graphics.DrawString(pulseString, font, brush, m_faceTextOrganizer.PulseLocation);
                }
            }

        }

        #endregion

        private delegate void DoTrackingCompleted();

        private delegate void UpdatePanelDelegate();

        private delegate void UpdateStatusDelegate(string status);

        public bool IsDetectionEnabled()
        {
            return Detection.Checked;
        }

        public bool IsLandmarksEnabled()
        {
            return Landmarks.Checked;
        }

        public bool IsPoseEnabled()
        {
            return Pose.Checked;
        }

        public bool IsExpressionsEnabled()
        {
            return Expressions.Checked;
        }

        public bool IsPulseEnabled()
        {
            return Pulse.Checked;
        }

        private void DisableDetection()
        {
            Detection.Enabled = false;
            Detection.Checked = false;
            NumDetectionText.Enabled = false;
        }

        private void DisableLandmarks()
        {
            Landmarks.Enabled = false;
            Landmarks.Checked = false;
            NumLandmarksText.Enabled = false;
        }

        private void DisablePose()
        {
            Pose.Enabled = false;
            Pose.Checked = false;
            NumPoseText.Enabled = false;
        }

        private void DisableRecognition()
        {
            Recognition.Enabled = false;
            Recognition.Checked = false;
        }

        private void DisableExpressions()
        {
            Expressions.Enabled = false;
            Expressions.Checked = false;
            NumExpressionsText.Enabled = false;
        }

        private void DisablePulse()
        {
            Pulse.Enabled = false;
            Pulse.Checked = false;
            NumPulseText.Enabled = false;
        }

        private void EnableModuleCheckBoxes()
        {
            foreach (CheckBox moduleCheckBox in m_modulesCheckBoxes)
            {
                moduleCheckBox.Enabled = true;
            }
        }

        private void DisableModuleCheckBoxes()
        {
            foreach (CheckBox moduleCheckBox in m_modulesCheckBoxes)
            {
                moduleCheckBox.Enabled = false;
            }
        }

        private void EnableTextBoxes()
        {
            foreach (var textBox in m_modulesTextBoxes)
            {
                textBox.Enabled = true;
            }
        }

        private void DisableTextBoxes()
        {
            foreach (var textBox in m_modulesTextBoxes)
            {
                textBox.Enabled = false;
            }
        }

        private void SetDefaultSettings()
        {
            var deviceName = GetDeviceName();

            if (deviceName == "InvalidCamera")
                SetDefaultInvalidSettings();

            if (deviceName == "DS4")
                SetDefaultDs4Settings(); ;

            if (deviceName == "IVcam")
                SetDefaultIvcamSettings();
        }

        private void SetDefaultInvalidSettings()
        {
            m_moduleSettings["Detection"] = new ModuleSettings { IsEnabled = false, NumberOfFaces = 0 };
            m_moduleSettings["Landmarks"] = new ModuleSettings { IsEnabled = false, NumberOfFaces = 0 };
            m_moduleSettings["Pose"] = new ModuleSettings { IsEnabled = false, NumberOfFaces = 0 };
            m_moduleSettings["Recognition"] = new ModuleSettings { IsEnabled = false, NumberOfFaces = 0 };
            m_moduleSettings["Expressions"] = new ModuleSettings { IsEnabled = false, NumberOfFaces = 0 };
            m_moduleSettings["Pulse"] = new ModuleSettings { IsEnabled = false, NumberOfFaces = 0 };
        }

        private void SetDefaultIvcamSettings()
        {
            m_moduleSettings["Detection"] = new ModuleSettings { IsEnabled = true, NumberOfFaces = 4 };
            m_moduleSettings["Landmarks"] = new ModuleSettings { IsEnabled = true, NumberOfFaces = 4 };
            m_moduleSettings["Pose"] = new ModuleSettings { IsEnabled = true, NumberOfFaces = 4 };
            m_moduleSettings["Recognition"] = new ModuleSettings { IsEnabled = true, NumberOfFaces = 4 };
            m_moduleSettings["Expressions"] = new ModuleSettings { IsEnabled = false, NumberOfFaces = 4 };
            m_moduleSettings["Pulse"] = new ModuleSettings { IsEnabled = false, NumberOfFaces = 4 };
        }

        private void SetDefaultDs4Settings()
        {
            m_moduleSettings["Detection"] = new ModuleSettings { IsEnabled = true, NumberOfFaces = 4 };
            m_moduleSettings["Landmarks"] = new ModuleSettings { IsEnabled = true, NumberOfFaces = 4 };
            m_moduleSettings["Pose"] = new ModuleSettings { IsEnabled = false, NumberOfFaces = 4 };
            m_moduleSettings["Recognition"] = new ModuleSettings { IsEnabled = false, NumberOfFaces = 4 };
            m_moduleSettings["Expressions"] = new ModuleSettings { IsEnabled = false, NumberOfFaces = 4 };
            m_moduleSettings["Pulse"] = new ModuleSettings { IsEnabled = false, NumberOfFaces = 4 };
        }

        private void Panel2_Click(object sender, EventArgs e)
        {

        }
    }
}
