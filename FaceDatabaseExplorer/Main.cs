using DF_FaceTracking.cs;
using RealSenseSdkExtensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FaceDatabaseExplorer {
    public partial class Main : Form {
        public List<NameMapping> NameMapping = null;
        public List<RecognitionFaceData> FaceData = null;
        public Main() {
            InitializeComponent();

            #region 開啟檔案篩選
            openFileDialog1.Filter = "Zip files (*.zip)|*.zip";
            openFileDialog1.FileName = null;
            openFileDialog1.Multiselect = false;
            #endregion
        }

        private void 關閉XToolStripMenuItem_Click(object sender, EventArgs e) {
            this.Close();
        }

        private void 開啟OToolStripMenuItem_Click(object sender, EventArgs e) {
            if (openFileDialog1.ShowDialog() != DialogResult.OK) return;
            FaceDatabaseFile.Load(
                openFileDialog1.FileName,
                ref FaceData, ref NameMapping);
        }
    }
}