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
    public partial class DuplicateUserForm : Form {

        public Dictionary<int, Image> IdFaceMapping { get; set; } = new Dictionary<int, Image>();

        public int? UserId { get; set; }

        public DuplicateUserForm() {
            InitializeComponent();
        }

        private void DuplicateUserForm_Load(object sender, EventArgs e) {
            imageList1.Images.AddRange(IdFaceMapping.Select(x => x.Value).ToArray());

            listView1.Items.AddRange(
                IdFaceMapping.Select(x => x.Key).Select((x,i) => 
                    new ListViewItem(x.ToString()) {
                        ImageIndex = i
                    }
                ).ToArray());
        }

        private void listView1_DoubleClick(object sender, EventArgs e) {
            if (listView1.SelectedIndices.Count == 0) return;
            var selectedIndex = listView1.SelectedIndices[0];
            UserId = int.Parse(listView1.SelectedItems[0].Text);
            DialogResult = DialogResult.OK;
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e) {
            this.Close();
        }
    }
}
