using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FaceRecognition {
    public static class Program {
        
        /// <summary>
        /// 應用程式的主要進入點。
        /// </summary>
        [STAThread]
        static void Main() {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            #region RealSense SDK Init
            RealSenseObjects.Session = PXCMSession.CreateInstance();
            if(RealSenseObjects.Session == null) {
                MessageBox.Show(
                    "RealSense Session初始化失敗，請確認您有安裝SDK。",
                    "初始化失敗",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return;
            }
            #endregion

            MainForm form = new MainForm();
            Application.Run(form);
            if (!form.IsDisposed) {//尚未釋放資源就關閉
                form.realSenseProgram.moduleConfiguration.Dispose();
                form.realSenseProgram.realSenseManager.Close();
                form.realSenseProgram.realSenseManager.Dispose();
            }
            RealSenseObjects.Session.Dispose();//釋放資源
        }
    }
}
