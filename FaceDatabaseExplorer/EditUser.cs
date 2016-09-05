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
    public partial class EditUser : Form {
        private string _id, _name;
        public string Id {
            get {
                return _id;
            }
            set {
                this.textBox1.Text = value;
            }
        }
        public new string Name {
            get {
                return _name;
            }
            set {
                this.textBox2.Text = value;
            }
        }
        public EditUser() {
            InitializeComponent();
        }

        private void EditUser_Load(object sender, EventArgs e) {
            textBox1.Focus();
        }

        private void button1_Click(object sender, EventArgs e) {
            string id = textBox1.Text.Trim();
            string name = textBox2.Text.Trim();
            if (id.Length == 0 || name.Length == 0) {
                //error
                MessageBox.Show("ID與Name不該為空字串", "資料缺漏", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            this._id = id;
            this._name = name;
            DialogResult = DialogResult.OK;
            this.Close();
        }
        private void button2_Click(object sender, EventArgs e) {
            DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
