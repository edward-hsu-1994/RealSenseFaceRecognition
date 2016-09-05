using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DF_FaceTracking.cs {
    public partial class RegisterForm : Form {
        public RegisterForm() {
            InitializeComponent();
        }

        /// <summary>
        /// 唯一識別號
        /// </summary>
        public string Id { get; private set; }

        /// <summary>
        /// 使用者名稱
        /// </summary>
        public new string Name { get; private set; }

        /// <summary>
        /// 顯示圖片
        /// </summary>
        public Image Picture {
            get {
                return pictureBox1.Image;
            }
            set {
                pictureBox1.Image = value;
            }
        }

        private void button1_Click(object sender, EventArgs e) {
            string id = textBox1.Text.Trim();
            string name = textBox1.Text.Trim();
            if(id.Length == 0 || name.Length == 0) {
                //error
                MessageBox.Show("ID與Name不該為空字串","資料缺漏",MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            this.Id = id;
            this.Name = name;
            DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
