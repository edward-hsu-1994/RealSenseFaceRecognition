using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FaceRecognition {
    public partial class RegisterForm : Form {
        public Image Picture {
            get {
                return pictureBox1.Image;
            }
            set {
                pictureBox1.Image = value;
            }
        }

        public string UserName {
            get {
                return textBox1.Text;
            }
            set {
                textBox1.Text = value;
            }
        }
        
        public RegisterForm() {
            InitializeComponent();
        }

        private void RegisterForm_Load(object sender, EventArgs e) {
            textBox1.Focus();
        }

        private void button1_Click(object sender, EventArgs e) {
            if(UserName.Length == 0) {
                MessageBox.Show("姓名字串長度不該為0", "姓名格式錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            DialogResult = DialogResult.OK;
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e) {
            this.Close();
        }
    }
}
