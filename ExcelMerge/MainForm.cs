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

namespace ExcelMerge
{
    public partial class MainForm : Form 
    {
        public MainForm()   
        {
            InitializeComponent();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            var filePath = string.Empty;

            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.InitialDirectory = Path.GetDirectoryName(Application.ExecutablePath);
                //ofd.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
                //ofd.FilterIndex = 2;
                ofd.RestoreDirectory = true;
                ofd.Multiselect = true;
                ofd.Title = Text;

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    lbxSelectedFiles.Items.AddRange(ofd.FileNames);
                }
            }
        }
    }
}