using DF_FaceTracking.cs;
using RealSenseSdkExtensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FaceDatabaseExplorer {
    public partial class Main : Form {
        public static List<NameMapping> NameMapping = null;
        public static List<RecognitionFaceData> FaceData = null;
        public Main() {
            InitializeComponent();

            #region 開啟與儲存檔案篩選
            openFileDialog1.Filter = "Zip files (*.zip)|*.zip";
            openFileDialog1.FileName = null;
            openFileDialog1.Multiselect = false;

            saveFileDialog1.Filter = "Zip files (*.zip)|*.zip";
            saveFileDialog1.FileName = null;
            #endregion

            #region 圖片開啟與另存檔案篩選
            openImageFileDialog1.Filter =
                "JPEG files (*.jpg)|*.jpg|" +
                "BMP files (*.bmp)|*.bmp|" +
                "PNG files (*.png)|*.png";
            openImageFileDialog1.FileName = null;
            openImageFileDialog1.Multiselect = true;

            saveImageFileDialog1.Filter = 
                "JPEG files (*.jpg)|*.jpg|"+
                "BMP files (*.bmp)|*.bmp|" + 
                "PNG files (*.png)|*.png";
            saveImageFileDialog1.FileName = null;
            #endregion

            儲存SToolStripMenuItem.Enabled = false;
            SaveOtherStripMenuItem1.Enabled = false;
            OutputStripMenuItem1.Enabled = false;
        }

        private void 關閉XToolStripMenuItem_Click(object sender, EventArgs e) {
            this.Close();
        }

        private void 開啟OToolStripMenuItem_Click(object sender, EventArgs e) {
            if (openFileDialog1.ShowDialog() != DialogResult.OK) return;
            listBox1.Items.Clear();
            listView1.Items.Clear();
            FaceDatabaseFile.Load(
                openFileDialog1.FileName,
                ref FaceData, ref NameMapping);
            LoadDatabaseUser();
            EnableSave();
        }

        private void EnableSave() {
            儲存SToolStripMenuItem.Enabled = true;
            SaveOtherStripMenuItem1.Enabled = true;
            OutputStripMenuItem1.Enabled = true;
        }

        private void LoadDatabaseUser() {
            listBox1.Items.Clear();
            listBox1.Items.AddRange(NameMapping.Select(x => $"{x.Id} - {x.Name}").ToArray());
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
            var filterFaceData = FaceData.Where(x => NameMapping[listBox1.SelectedIndex].DataIds.Contains(x.ForeignKey)).ToArray();
            imageList1.Images.Clear();
            listView1.Items.Clear();
            imageList1.Images
                .AddRange(filterFaceData.Select(x => x.Image).ToArray());
            listView1.Items
                .AddRange(Enumerable.Range(0, filterFaceData.Length)
                .Select(x=> {
                    var result = new ListViewItem();
                    result.ImageIndex = x;
                    result.Text = filterFaceData[x].ForeignKey.ToString();
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
                "您確定從臉部辨識資料庫中移除選擇的圖片嗎?\r\n"+
                "執行此動作後圖像ID將會自動重新修正",
                "刪除圖片",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);
            if (dialogResult != DialogResult.Yes) return;

            var item = (ListViewItem)listView1.SelectedItems[0];
            FaceData.Remove(
                FaceData
                .Where(x => x.ForeignKey == int.Parse(item.Text))
                .FirstOrDefault());

            FaceDatabaseFile.FormatData(FaceData, NameMapping);
            listBox1_SelectedIndexChanged(null, null);
        }

        private void DeleteUserToolStripMenuItem_Click(object sender, EventArgs e) {
            if (listBox1.SelectedIndex < 0) return;
            var dialogResult = MessageBox.Show(
                "您確定從臉部辨識資料庫中移除選擇的使用者所有資料嗎?\r\n" +
                "執行此動作後圖像ID將會自動重新修正",
                "刪除使用者",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);
            if (dialogResult != DialogResult.Yes) return;
            FaceData = FaceData.Where(x => !NameMapping[listBox1.SelectedIndex].DataIds.Contains(x.ForeignKey)).ToList();
            NameMapping.RemoveAt(listBox1.SelectedIndex);
            FaceDatabaseFile.FormatData(FaceData, NameMapping);
            LoadDatabaseUser();
            listView1.Items.Clear();
        }

        private void editUserToolStripMenuItem_Click(object sender, EventArgs e) {
            if (listBox1.SelectedIndex < 0) return;

            EditUser editor = new EditUser() {
                Id = NameMapping[listBox1.SelectedIndex].Id,
                Name = NameMapping[listBox1.SelectedIndex].Name
            };
            if (editor.ShowDialog() != DialogResult.OK) return;

            NameMapping[listBox1.SelectedIndex].Id = editor.Id;
            NameMapping[listBox1.SelectedIndex].Name = editor.Name;
            LoadDatabaseUser();
        }

        private void SaveUserToolStripMenuItem_Click(object sender, EventArgs e) {
            if (listBox1.SelectedIndex < 0) return;            
            if (SaveUserFolderBrowserDialog1.ShowDialog() != DialogResult.OK) return;
            string DirPath = SaveUserFolderBrowserDialog1.SelectedPath +
                    "\\" + NameMapping[listBox1.SelectedIndex].Id;
            if (!Directory.Exists(DirPath)) {
                Directory.CreateDirectory(DirPath);
            }

            FaceData.Where(x => NameMapping[listBox1.SelectedIndex].DataIds.Contains(x.ForeignKey))
            .Select(x => {
                x.Image.Save(DirPath + "\\" + x.ForeignKey + ".jpg");
                return 0;
            }).ToArray();
        }

        private void SaveOtherStripMenuItem1_Click(object sender, EventArgs e) {
            if (saveFileDialog1.ShowDialog() != DialogResult.OK) return;
            FaceDatabaseFile.Save(saveFileDialog1.FileName, FaceData, NameMapping);
        }

        private void ConvertToDirToolStripMenuItem_Click(object sender, EventArgs e) {
            if (SaveUserFolderBrowserDialog1.ShowDialog() != DialogResult.OK) return;

            for (int i = 0; i < listBox1.Items.Count; i++) {
                string DirPath = SaveUserFolderBrowserDialog1.SelectedPath +
                        "\\" + NameMapping[i].Id;
                if (!Directory.Exists(DirPath)) {
                    Directory.CreateDirectory(DirPath);
                }

                FaceData.Where(x => NameMapping[i].DataIds.Contains(x.ForeignKey))
                .Select(x => {
                    x.Image.Save(DirPath + "\\" + x.ForeignKey + ".jpg");
                    return 0;
                }).ToArray();
            }
        }

        private void AddImageToolStripMenuItem1_Click(object sender, EventArgs e) {
            if (listBox1.SelectedIndex < 0) return;
            if(openImageFileDialog1.ShowDialog() != DialogResult.OK) return;
            FaceDatabaseFile.FormatData(FaceData, NameMapping);

            foreach (var fileName in openImageFileDialog1.FileNames) {
                var image = new Bitmap(fileName);
                if (image.Size.Height != 128 || image.Size.Width != 128) {
                    image = new Bitmap(image, 128, 128);
                }
                var faceData = new RecognitionFaceData(null) {
                    Image = image,
                    PrimaryKey = FaceData.Count,
                    ForeignKey = 100 + FaceData.Count
                };
                FaceData.Add(faceData);
                NameMapping[listBox1.SelectedIndex].DataIds.Add(faceData.ForeignKey);
            }
            FaceDatabaseFile.FormatData(FaceData, NameMapping);
            listBox1_SelectedIndexChanged(null, null);
        }

        private void AddUserToolStripMenuItem1_Click(object sender, EventArgs e) {
            var editor = new EditUser() {
                Id = "", Name = ""
            };
            if (editor.ShowDialog() != DialogResult.OK) return;

            NameMapping.Add(
                new NameMapping() {
                    Id = editor.Id,
                    Name = editor.Name
                }
            );
            LoadDatabaseUser();
            EnableSave();
        }
    }
}