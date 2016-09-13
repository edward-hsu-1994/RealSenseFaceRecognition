using RealSenseSdkExtensions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FaceRecognition {
    public class FaceRecognitionProgram {
        /// <summary>
        /// 主視窗
        /// </summary>
        public MainForm Form { get; private set; }

        /// <summary>
        /// 資源是否已經釋放
        /// </summary>
        public bool IsDispose { get; private set; }

        /*public byte[] Buffer { get; set; }*/

        public delegate void FaceRecognitionEventHandler(object sender, FaceRecognitionEventArgs args);



        public event FaceRecognitionEventHandler OnStart;
        public event FaceRecognitionEventHandler OnStop;
        public event FaceRecognitionEventHandler OnFrame;
        public event FaceRecognitionEventHandler OnFoundFace;
        public event FaceRecognitionEventHandler OnNotFoundFace;
        public FaceRecognitionProgram(MainForm form) {
            this.Form = form;
        }

        public PXCMFaceConfiguration.RecognitionConfiguration recognitionConfig;
        public PXCMFaceConfiguration moduleConfiguration;
        public PXCMSenseManager realSenseManager;
        private PXCMFaceData moduleOutput;
        private bool _Stop = false;
        private bool _Paush = false;
        private CancellationTokenSource _Token;
        private Task _Task;


        #region 動作控制
        public void Start() {
            _Stop = false;
            _Token = new CancellationTokenSource();
            _Task = Task.Run(() => {
                try {
                    FaceTrackingPipeline();
                } catch { }
            }, _Token.Token);
        }
        public void Stop() {
            _Stop = true;
        }

        public void Paush() {
            _Paush = true;
        }

        public void UnPaush() {
            _Paush = false;
        }

        #endregion

        private void FaceTrackingPipeline() {
            IsDispose = false;
            OnStart?.Invoke(this, null);

            #region Manager Init
            realSenseManager = RealSenseObjects.Session.CreateSenseManager();

            if (realSenseManager == null) {
                MessageBox.Show(
                    "PXCMSenseManager初始化失敗。",
                    "初始化失敗",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                OnStop?.Invoke(this, null);
                return;
            }

            PXCMCaptureManager captureManager = realSenseManager.captureManager;
            if (captureManager == null) {
                MessageBox.Show(
                    "PXCMCaptureManager初始化失敗。",
                    "初始化失敗",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                OnStop?.Invoke(this, null);
                return;
            }
            #endregion

            #region 基本設定
            //設定裝置
            captureManager.FilterByDeviceInfo(Form.SelectedDevice);

            //設定串流類型
            captureManager.FilterByStreamProfiles(Form.SelectedDeviceStreamProfile);


            //啟用臉部追蹤模組
            realSenseManager.EnableFace();
            PXCMFaceModule faceModule = realSenseManager.QueryFace();
            if (faceModule == null) {
                MessageBox.Show(
                    "取得PXCMFaceModule失敗。",
                    "初始化失敗",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                OnStop?.Invoke(this, null);
                return;
            }

            //建立臉部追蹤模組設定
            moduleConfiguration = faceModule.CreateActiveConfiguration();
            if (moduleConfiguration == null) {
                MessageBox.Show(
                    "建立PXCMFaceConfiguration失敗。",
                    "初始化失敗",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                OnStop?.Invoke(this, null);
                return;
            }
            //追蹤模式設定
            moduleConfiguration.SetTrackingMode(Form.ModeType);

            moduleConfiguration.strategy = PXCMFaceConfiguration.TrackingStrategyType.STRATEGY_RIGHT_TO_LEFT;
            moduleConfiguration.detection.isEnabled = true;
            moduleConfiguration.detection.maxTrackedFaces = 4;//最大追蹤4個臉
            moduleConfiguration.landmarks.isEnabled = false;
            moduleConfiguration.pose.isEnabled = false;

            recognitionConfig =
                moduleConfiguration.QueryRecognition();

            if (recognitionConfig == null) {
                MessageBox.Show(
                    "建立RecognitionConfiguration失敗。",
                    "初始化失敗",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                OnStop?.Invoke(this, null);
                return;
            }

            recognitionConfig.Enable();
            #endregion

            #region 讀取資料庫數據
            if (Form.FaceData != null) {
                recognitionConfig.SetDatabase(Form.FaceData);
                moduleConfiguration.ApplyChanges();
            }
            #endregion

            #region 預備啟動
            moduleConfiguration.EnableAllAlerts();
            //moduleConfiguration.SubscribeAlert(FaceAlertHandler);

            pxcmStatus applyChangesStatus = moduleConfiguration.ApplyChanges();
            Form.SetStatus("RealSenseManager初始化中");
            if (applyChangesStatus.IsError() || realSenseManager.Init().IsError()) {
                MessageBox.Show(
                    "RealSenseManager初始化失敗，請檢查設定正確。",
                    "初始化失敗",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                OnStop?.Invoke(this, null);
                return;
            }
            #endregion

            using (moduleOutput = faceModule.CreateOutput()) {
                PXCMCapture.Device.StreamProfileSet profiles;
                PXCMCapture.Device device = captureManager.QueryDevice();

                if (device == null) {
                    MessageBox.Show(
                        "取得設備失敗。",
                        "初始化失敗",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    OnStop?.Invoke(this, null);
                    return;
                }

                device.QueryStreamProfileSet(PXCMCapture.StreamType.STREAM_TYPE_DEPTH, 0, out profiles);

                #region Loop
                while (!_Stop) {
                    while (_Paush) {
                        Application.DoEvents();
                    }
                    if (realSenseManager.AcquireFrame(true).IsError()) break;
                    var isConnected = realSenseManager.IsConnected();
                    if (isConnected) {
                        var sample = realSenseManager.QueryFaceSample();
                        if (sample == null) {
                            realSenseManager.ReleaseFrame();
                            continue;
                        }
                        #region 畫面取出
                        PXCMImage image = null;
                        if (Form.ModeType == PXCMFaceConfiguration.TrackingModeType.FACE_MODE_IR) {
                            image = sample.ir;
                        } else {
                            image = sample.color;
                        }
                        #endregion

                        moduleOutput.Update();//更新辨識
                        PXCMFaceConfiguration.RecognitionConfiguration recognition = moduleConfiguration.QueryRecognition();
                        if (recognition == null) {
                            realSenseManager.ReleaseFrame();
                            continue;
                        }


                        #region 繪圖與事件
                        OnFrame?.Invoke(this, new FaceRecognitionEventArgs() {
                            Image = ToBitmap(image)
                        });
                        FindFace(moduleOutput);
                        #endregion
                    }
                    //發布框
                    realSenseManager.ReleaseFrame();
                }
                #endregion

                //更新資料庫緩衝區
                //Buffer = moduleOutput.QueryRecognitionModule().GetDatabaseBuffer();
            }

            #region 釋放資源
            moduleConfiguration.Dispose();            
            realSenseManager.Close();
            realSenseManager.Dispose();
            #endregion

            IsDispose = true;
            OnStop?.Invoke(this, null);
        }

        public delegate void InvokeDelegate();
        private void FindFace(PXCMFaceData moduleOutput) {
            var faces = moduleOutput.QueryFaces();
            Form.Invoke(new InvokeDelegate(() => {
                if (faces.Length == 0) {
                    OnNotFoundFace?.Invoke(this, new FaceRecognitionEventArgs() {
                        Faces = faces,
                        FirstRecognition = null,
                        Output = moduleOutput
                    });
                } else {
                    OnFoundFace?.Invoke(this, new FaceRecognitionEventArgs() {
                        Faces = faces,
                        FirstRecognition = faces.FirstOrDefault()?.QueryRecognition(),
                        Output = moduleOutput
                    });
                }
            }));
        }

        private Bitmap ToBitmap(PXCMImage image) {
            PXCMImage.ImageData data;
            if (image.AcquireAccess(PXCMImage.Access.ACCESS_READ_WRITE, PXCMImage.PixelFormat.PIXEL_FORMAT_RGB32, out data) <
                pxcmStatus.PXCM_STATUS_NO_ERROR) return null;
            var result = data.ToBitmap(0, image.info.width, image.info.height);
            image.ReleaseAccess(data);
            return result;
        }
    }
}
