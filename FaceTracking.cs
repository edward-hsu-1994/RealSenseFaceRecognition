using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Remoting.Channels;
using System.Windows.Forms;
using RealSenseSdkExtensions;
using System.IO;
using System.Collections.Generic;

namespace DF_FaceTracking.cs
{
    internal class FaceTracking
    {
        private readonly MainForm m_form;
        private FPSTimer m_timer;
        private bool m_wasConnected;

        public static string DatabasePath = "database.zip";
        private static bool DatabaseChanged = false;
        public static List<NameMapping> NameMapping = new List<NameMapping>();
        public static RecognitionFaceData[] FaceData = new RecognitionFaceData[0];
        public FaceTracking(MainForm form)
        {
            m_form = form;
        }

        private void DisplayDeviceConnection(bool isConnected)
        {
            if (isConnected && !m_wasConnected) m_form.UpdateStatus("Device Reconnected", MainForm.Label.StatusLabel);
            else if (!isConnected && m_wasConnected)
                m_form.UpdateStatus("Device Disconnected", MainForm.Label.StatusLabel);
            m_wasConnected = isConnected;
        }

        private void DisplayPicture(PXCMImage image)
        {
            PXCMImage.ImageData data;
            if (image.AcquireAccess(PXCMImage.Access.ACCESS_READ, PXCMImage.PixelFormat.PIXEL_FORMAT_RGB32, out data) <
                pxcmStatus.PXCM_STATUS_NO_ERROR) return;
            m_form.DrawBitmap(data.ToBitmap(0, image.info.width, image.info.height));
            m_timer.Tick("");
            image.ReleaseAccess(data);
        }

        private void CheckForDepthStream(PXCMCapture.Device.StreamProfileSet profiles, PXCMFaceModule faceModule)
        {
            PXCMFaceConfiguration faceConfiguration = faceModule.CreateActiveConfiguration();
            if (faceConfiguration == null)
            {
                Debug.Assert(faceConfiguration != null);
                return;
            }

            PXCMFaceConfiguration.TrackingModeType trackingMode = faceConfiguration.GetTrackingMode();
            faceConfiguration.Dispose();

            if (trackingMode != PXCMFaceConfiguration.TrackingModeType.FACE_MODE_COLOR_PLUS_DEPTH
                || trackingMode != PXCMFaceConfiguration.TrackingModeType.FACE_MODE_IR)
                return;
            if (profiles.depth.imageInfo.format == PXCMImage.PixelFormat.PIXEL_FORMAT_DEPTH) return;
            PXCMCapture.DeviceInfo dinfo;
            m_form.Devices.TryGetValue(m_form.GetCheckedDevice(), out dinfo);

            if (dinfo != null)
                MessageBox.Show(
                    String.Format("Depth stream is not supported for device: {0}. \nUsing 2D tracking", dinfo.name),
                    @"Face Tracking",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void FaceAlertHandler(PXCMFaceData.AlertData alert)
        {
            m_form.UpdateStatus(alert.label.ToString(), MainForm.Label.StatusLabel);
        }


        PXCMFaceConfiguration.RecognitionConfiguration qrecognition=null;
        public void SimplePipeline()
        {
            PXCMSenseManager pp = m_form.Session.CreateSenseManager();

            if (pp == null)
            {
                throw new Exception("PXCMSenseManager null");
            }

            PXCMCaptureManager captureMgr = pp.captureManager;
            if (captureMgr == null)
            {
                throw new Exception("PXCMCaptureManager null");
            }

            var selectedRes = m_form.GetCheckedColorResolution();
            if (selectedRes != null && !m_form.IsInPlaybackState())  
            {
                // Set active camera
                PXCMCapture.DeviceInfo deviceInfo;
                m_form.Devices.TryGetValue(m_form.GetCheckedDevice(), out deviceInfo);
                captureMgr.FilterByDeviceInfo(m_form.GetCheckedDeviceInfo());

                // activate filter only live/record mode , no need in playback mode
                var set = new PXCMCapture.Device.StreamProfileSet
                {
                    color =
                    {
                        frameRate = selectedRes.Item2,
                        imageInfo =
                        {
                            format = selectedRes.Item1.format,
                            height = selectedRes.Item1.height,
                            width = selectedRes.Item1.width
                        }
                    }
                };

                if (m_form.IsPulseEnabled() && (set.color.imageInfo.width < 1280 || set.color.imageInfo.height < 720))
                {
                    captureMgr.FilterByStreamProfiles(PXCMCapture.StreamType.STREAM_TYPE_COLOR, 1280, 720, 0);
                }
                else
                {
                    captureMgr.FilterByStreamProfiles(set);
                }
            }

            // Set Source & Landmark Profile Index 
            if (m_form.IsInPlaybackState())
            {
                //pp.captureManager.FilterByStreamProfiles(null);
                captureMgr.SetFileName(m_form.GetFileName(), false);
                captureMgr.SetRealtime(false);
            }
            else  if (m_form.GetRecordState())
            {
                captureMgr.SetFileName(m_form.GetFileName(), true);
            }
            
            // Set Module            
            pp.EnableFace();
            PXCMFaceModule faceModule = pp.QueryFace();
            if (faceModule == null)
            {
                Debug.Assert(faceModule != null);
                return;
            }
            
            PXCMFaceConfiguration moduleConfiguration = faceModule.CreateActiveConfiguration();
            if (moduleConfiguration == null)
            {
                Debug.Assert(moduleConfiguration != null);
                return;
            }

            var checkedProfile = m_form.GetCheckedProfile();
            var mode = m_form.FaceModesMap.First(x => x.Value == checkedProfile).Key;
            
            moduleConfiguration.SetTrackingMode(mode);

            moduleConfiguration.strategy = PXCMFaceConfiguration.TrackingStrategyType.STRATEGY_RIGHT_TO_LEFT;

            moduleConfiguration.detection.maxTrackedFaces = m_form.NumDetection;
            moduleConfiguration.landmarks.maxTrackedFaces = m_form.NumLandmarks;
            moduleConfiguration.pose.maxTrackedFaces = m_form.NumPose;
            
            PXCMFaceConfiguration.ExpressionsConfiguration econfiguration = moduleConfiguration.QueryExpressions();
            if (econfiguration == null)
            {
                throw new Exception("ExpressionsConfiguration null");
            }
            econfiguration.properties.maxTrackedFaces = m_form.NumExpressions;

            econfiguration.EnableAllExpressions();
            moduleConfiguration.detection.isEnabled = m_form.IsDetectionEnabled();
            moduleConfiguration.landmarks.isEnabled = m_form.IsLandmarksEnabled();
            moduleConfiguration.pose.isEnabled = m_form.IsPoseEnabled();
            if (m_form.IsExpressionsEnabled())
            { 
                econfiguration.Enable();
            }

            PXCMFaceConfiguration.PulseConfiguration pulseConfiguration = moduleConfiguration.QueryPulse();
            if (pulseConfiguration == null)
            {
                throw new Exception("pulseConfiguration null");
            }
			
            pulseConfiguration.properties.maxTrackedFaces = m_form.NumPulse;
            if (m_form.IsPulseEnabled())
            {
                pulseConfiguration.Enable();
            }

            qrecognition = moduleConfiguration.QueryRecognition();
            if (qrecognition == null)
            {
                throw new Exception("PXCMFaceConfiguration.RecognitionConfiguration null");
            }
            if (m_form.IsRecognitionChecked())
            {
                qrecognition.Enable();


                #region 臉部辨識資料庫讀取
                if (File.Exists(DatabasePath)) {
                    m_form.UpdateStatus("正在讀取資料庫", MainForm.Label.StatusLabel);
                    List<RecognitionFaceData> faceData = null;
                    FaceDatabaseFile.Load(DatabasePath, ref faceData, ref NameMapping);
                    FaceData = faceData.ToArray();
                    qrecognition.SetDatabase(FaceData);
                }
                #endregion
            }



            moduleConfiguration.EnableAllAlerts();
            moduleConfiguration.SubscribeAlert(FaceAlertHandler);

            pxcmStatus applyChangesStatus = moduleConfiguration.ApplyChanges();

            m_form.UpdateStatus("Init Started", MainForm.Label.StatusLabel);

            if (applyChangesStatus < pxcmStatus.PXCM_STATUS_NO_ERROR || pp.Init() < pxcmStatus.PXCM_STATUS_NO_ERROR)
            {
                m_form.UpdateStatus("Init Failed", MainForm.Label.StatusLabel);
            }
            else
            {
                using (PXCMFaceData moduleOutput = faceModule.CreateOutput())
                {
                    Debug.Assert(moduleOutput != null);
                    PXCMCapture.Device.StreamProfileSet profiles;
                    PXCMCapture.Device device  =  captureMgr.QueryDevice();

                    if (device == null)
                    {
                        throw new Exception("device null");
                    }
                    
                    device.QueryStreamProfileSet(PXCMCapture.StreamType.STREAM_TYPE_DEPTH, 0, out profiles);
                    CheckForDepthStream(profiles, faceModule);
                    
                    m_form.UpdateStatus("Streaming", MainForm.Label.StatusLabel);
                    m_timer = new FPSTimer(m_form);

                    #region loop
                    while (!m_form.Stopped)
                    {
                        if (pp.AcquireFrame(true) < pxcmStatus.PXCM_STATUS_NO_ERROR) break;
                        var isConnected = pp.IsConnected();
                        DisplayDeviceConnection(isConnected);
                        if (isConnected)
                        {
                            var sample = pp.QueryFaceSample();
                            if (sample == null)
                            {
                                pp.ReleaseFrame();
                                continue;
                            }
                            switch (mode)
                            {
                                case PXCMFaceConfiguration.TrackingModeType.FACE_MODE_IR:
                                    if (sample.ir != null)
                                    DisplayPicture(sample.ir);
                                    break;
                                default:
                                    DisplayPicture(sample.color);
                                    break;
                            }

                            moduleOutput.Update();
                            PXCMFaceConfiguration.RecognitionConfiguration recognition = moduleConfiguration.QueryRecognition();
                            if (recognition == null)
                            {
                                pp.ReleaseFrame();
                                continue;
                            }

                            if (recognition.properties.isEnabled)
                            {
                                UpdateRecognition(moduleOutput);
                            }

                            m_form.DrawGraphics(moduleOutput);
                            m_form.UpdatePanel();
                        }
                        pp.ReleaseFrame();
                    }
                    #endregion
                }

   //             moduleConfiguration.UnsubscribeAlert(FaceAlertHandler);
   //             moduleConfiguration.ApplyChanges();
                m_form.UpdateStatus("Stopped", MainForm.Label.StatusLabel);
            }

            #region 儲存臉部辨識資訊檔案
            if (DatabaseChanged) {
                FaceDatabaseFile.Save(DatabasePath, FaceData.ToList(), NameMapping);
            }
            #endregion

            var dbm = new FaceDatabaseManager(pp);
            

            moduleConfiguration.Dispose();
            pp.Close();
            pp.Dispose();
        }

        private void UpdateRecognition(PXCMFaceData faceOutput)
        {
            //TODO: add null checks
            if (m_form.Register) RegisterUser(faceOutput);
            if (m_form.Unregister) UnregisterUser(faceOutput);
            FaceData = faceOutput.QueryRecognitionModule().GetDatabase();
            DatabaseChanged = true;
        }

        private void RegisterUser(PXCMFaceData faceOutput)
        {
            m_form.Register = false;
            if (faceOutput.QueryNumberOfDetectedFaces() <= 0)
                return;

            PXCMFaceData.Face qface = faceOutput.QueryFaceByIndex(0);
            if (qface == null)
            {
                throw new Exception("PXCMFaceData.Face null");
            }
            PXCMFaceData.RecognitionData rdata = qface.QueryRecognition();
            if (rdata == null)
            {
                throw new Exception(" PXCMFaceData.RecognitionData null");
            }
            #region 註冊視窗
            bool isRegistered = rdata.IsRegistered();//已註冊?
            int realSenseId = rdata.RegisterUser();
            var collection = faceOutput.QueryRecognitionModule()
                .GetDatabase()
                .Where(x => x.ForeignKey == realSenseId);
            var dbItem = collection.LastOrDefault();
            if (realSenseId == -1) return;
            if (isRegistered) {
                //已註冊的畫面卻又重新註冊，可能是辨識錯誤的更正
                List<RecognitionFaceData> faceData = 
                    faceOutput.QueryRecognitionModule()
                    .GetDatabase().ToList();

                dbItem = faceData.Last();
                dbItem.ForeignKey = dbItem.PrimaryKey + 100;//校正Id
                faceData[faceData.Count - 1] = dbItem;

                //整理資料庫的錯誤
                FaceDatabaseFile.FormatData(faceData, NameMapping);
                qrecognition.SetDatabase(faceData.ToArray());
            }
            
            var registerForm = new RegisterForm() {
                Picture = dbItem.Image
            };

            if(registerForm.ShowDialog() == DialogResult.OK) {
                var mapping = NameMapping.Where(x => x.Id == registerForm.Id).FirstOrDefault();
                if(mapping == null) {
                    mapping = new NameMapping() {
                        Id = registerForm.Id,
                        Name = registerForm.Name
                    };
                    NameMapping.Add(mapping);
                }
                if(registerForm.Name.Length > 0) {
                    mapping.Name = registerForm.Name;
                }
                mapping.DataIds.Add(dbItem.ForeignKey);
            }
            #endregion
        }

        private void UnregisterUser(PXCMFaceData faceOutput)
        {
            m_form.Unregister = false;
            if (faceOutput.QueryNumberOfDetectedFaces() <= 0)
            {
                return;
            }

            var qface = faceOutput.QueryFaceByIndex(0);
            if (qface == null)
            {
                throw new Exception("PXCMFaceData.Face null");
            }

            PXCMFaceData.RecognitionData rdata = qface.QueryRecognition();
            if (rdata == null)
            {
                throw new Exception(" PXCMFaceData.RecognitionData null");
            }

            if (!rdata.IsRegistered())
            {
                return;
            }
            #region 解除註冊
            var realSenseId = rdata.QueryUserID();
            var mapping = NameMapping.Where(x => x.DataIds.Contains(realSenseId)).FirstOrDefault();
            if (mapping != null) {
                mapping.DataIds.Remove(realSenseId);
            }
            #endregion
            rdata.UnregisterUser();
        }
    }
}