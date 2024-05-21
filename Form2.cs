using System;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Xml.Linq;
using System.Text;
using System.IO;

namespace neta
{
    public partial class VER : Form
    {
        public VER()
        {
            InitializeComponent();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }



        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(linkLabel1.Text);
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(linkLabel2.Text);
        }

        private void VER_Load(object sender, EventArgs e)
        {
            label3.Text = "BUILDDATE:" + Properties.Settings.Default.build;
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string data = "https://raw.githubusercontent.com/sokudon/netataimaC-/master/App.config";

            try
            {
                var list = XDocument.Load(data);

                var names = list.Descendants("setting");

                foreach (var xmls in names)
                {
                    Match m = Regex.Match(xmls.ToString(), "name=\"build\"");//build 
                    if (m.Success)
                    {
                        Match mm = Regex.Match(xmls.ToString(), "<value>.*?<\\/value>");//buid
                        if (mm.Success)
                        {
                            DateTime app;
                            DateTime net;
                            string tmp = Regex.Replace(mm.Value, "<.*?>", "");
                            if (DateTime.TryParse(Properties.Settings.Default.build.Trim(), out app))
                            { }
                            if (DateTime.TryParse(tmp, out net))
                            { }
                            TimeSpan dra = net - app;
                            if (dra.TotalSeconds > 0)
                            {
                                MessageBox.Show("最新版があります BUILDDATE:" + net.ToString());
                            }
                            else {

                                MessageBox.Show("このアプリは最新版です BUIDDATE:" + app.ToString());
                            }
                        }
                    }
                }
            }
            catch (Exception exc)
            {
                label3.Text = exc.Message;
            }
        }
    }
}
