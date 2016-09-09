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

            Application.Run(new MainForm());
            RealSenseObjects.Session.Dispose();//釋放資源
        }
    }
}
