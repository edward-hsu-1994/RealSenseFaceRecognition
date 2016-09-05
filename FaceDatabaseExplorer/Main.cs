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

            #region 圖片另存檔案篩選
            saveImageFileDialog1.Filter = 
                "JPEG files (*.jpg)|*.jpg|"+
                "BMP files (*.bmp)|*.bmp";
            saveImageFileDialog1.FileName = null;
            #endregion

            儲存SToolStripMenuItem.Enabled = false;
        }

        private void 關閉XToolStripMenuItem_Click(object sender, EventArgs e) {
            this.Close();
        }

        private void 開啟OToolStripMenuItem_Click(object sender, EventArgs e) {
            if (openFileDialog1.ShowDialog() != DialogResult.OK) return;
            FaceDatabaseFile.Load(
                openFileDialog1.FileName,
                ref FaceData, ref NameMapping);
            listBox1.Items.Clear();

            listBox1.Items.AddRange(NameMapping.Select(x => $"{x.Id} - {x.Name}").ToArray());

            儲存SToolStripMenuItem.Enabled = true;
        }

        private void 儲存SToolStripMenuItem_Click(object sender, EventArgs e) {
            FaceDatabaseFile.Save(
                openFileDialog1.FileName,
                FaceData,
                NameMapping
            );
        }
        private void listBox1_SelectedIndexChanged(object sender, EventArgs e) { 
            if(listBox1.SelectedIndex < 0)return;
            var filterFaceData = FaceData.Where(x => NameMapping[listBox1.SelectedIndex].DataIds.Contains(x.Id)).ToArray();
            imageList1.Images.Clear();
            listView1.Items.Clear();
            imageList1.Images
                .AddRange(FaceData.Select(x => x.Image).ToArray());
            listView1.Items
                .AddRange(Enumerable.Range(0, filterFaceData.Length)
                .Select(x=> {
                    var result = new ListViewItem();
                    result.ImageIndex = x;
                    result.Text = filterFaceData[x].Id.ToString();
                    return result;
                }).ToArray());
        }

        private void SaveImageMenuItem_Click(object sender, EventArgs e) {
            if (listBox1.SelectedIndex < 0) return;
            if (saveImageFileDialog1.ShowDialog() != DialogResult.OK) return;
            var item = (ListViewItem)listView1.SelectedItems[0];
            item.ImageList.Images[item.ImageIndex]
                .Save(saveImageFileDialog1.FileName);
        }

        private void DeleteImageMenuItem_Click(object sender, EventArgs e) {
            if (listBox1.SelectedIndex < 0) return;
            var dialogResult = MessageBox.Show(
                "您確定從臉部辨識資料庫中移除選擇的圖片嗎?",
                "刪除圖片",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);
            if (dialogResult != DialogResult.Yes) return;

            var item = (ListViewItem)listView1.SelectedItems[0];
            FaceData.Remove(
                FaceData
                .Where(x => x.Id == int.Parse(item.Text))
                .FirstOrDefault());

            listBox1_SelectedIndexChanged(null, null);
        }

        
    }
}