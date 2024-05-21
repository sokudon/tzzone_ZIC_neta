using System;
using System.Windows.Forms;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text;
using System.Net;
using System.Text.RegularExpressions;

namespace neta
{
    public partial class Form4 : Form
    {
        

        public Form4()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
          

            WebClient wc = new WebClient();
            wc.Encoding = Encoding.UTF8;
            DateTime dt = DateTime.Now;

            string url = Properties.Settings.Default.api.ToString().Replace("TODAY()", dt.ToString("yyyy-MM-dd"));
            string parseop = Properties.Settings.Default.parse;
            string text = "";
            try
            {
                text = wc.DownloadString(url);
                File.WriteAllText(@"tmp.js", text);
            }
            catch (WebException exc)
            {
                MessageBox.Show(exc.Message);
                return;
            }


            {
                using (var reader = new StreamReader("tmp.js"))
                using (var jsonReader = new JsonTextReader(reader))
                {
                    jsonReader.DateParseHandling = DateParseHandling.None;
                    var root = JToken.Load(jsonReader);
                    DisplayTreeView(root, url);
                }
            }
        }


        private void DisplayTreeView(JToken root, string rootName)
        {
            treeView1.BeginUpdate();
            try
            {
                treeView1.Nodes.Clear();
                var tNode = treeView1.Nodes[treeView1.Nodes.Add(new TreeNode(rootName))];
                tNode.Tag = root;

                AddNode(root, tNode);

                treeView1.ExpandAll();
            }
            finally
            {
                treeView1.EndUpdate();
            }
        }

        private void AddNode(JToken token, TreeNode inTreeNode)
        {
            if (token == null)
                return;
            if (token is JValue)
            {
                var childNode = inTreeNode.Nodes[inTreeNode.Nodes.Add(new TreeNode(token.ToString()))];
                childNode.Tag = token;
            }
            else if (token is JObject)
            {
                var obj = (JObject)token;
                foreach (var property in obj.Properties())
                {
                    var childNode = inTreeNode.Nodes[inTreeNode.Nodes.Add(new TreeNode(property.Name))];
                    childNode.Tag = property;
                    AddNode(property.Value, childNode);
                }
            }
            else if (token is JArray)
            {
                var array = (JArray)token;
                for (int i = 0; i < array.Count; i++)
                {
                    var childNode = inTreeNode.Nodes[inTreeNode.Nodes.Add(new TreeNode(i.ToString()))];
                    childNode.Tag = array[i];
                    AddNode(array[i], childNode);
                }
            }
            else
            {
                System.Diagnostics.Debug.WriteLine(string.Format("{0} not implemented", token.Type)); // JConstructor, JRaw
            }
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            var n = treeView1.SelectedNode;
            var info = n.Tag;
            var type = info.GetType().Name;
            textBox1.Text = n.Text + "\r\n"
            +n.Name + "\r\n"
            + type+ "\r\n"
            + ((Newtonsoft.Json.Linq.JToken)info).Path;

        }

        private void 名前に登録ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var n = treeView1.SelectedNode;
            var info = n.Tag;
            var type = info.GetType().Name;
            if (type == "JValue") {
                var tmp = ((Newtonsoft.Json.Linq.JToken)info).Path;
                tmp = Regex.Replace(tmp, "\\[\\d+\\]", "");
                tmp = Regex.Replace(tmp, "^\\.", "");
                tmp = Regex.Replace(tmp, "\\.", "/");
                textBox2.Text = "/"+tmp;
            }
        }

        private void 開始時間に登録ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var n = treeView1.SelectedNode;
            var info = n.Tag;
            var type = info.GetType().Name;
            if (type == "JValue")
            {
                var tmp = ((Newtonsoft.Json.Linq.JToken)info).Path;
                tmp = Regex.Replace(tmp, "\\[\\d+\\]", "");
                tmp = Regex.Replace(tmp, "^\\.", "");
                tmp = Regex.Replace(tmp, "\\.", "/");
                textBox3.Text = "/" + tmp;
            }
        }

        private void 終了時間に登録ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var n = treeView1.SelectedNode;
            var info = n.Tag;
            var type = info.GetType().Name;
            if (type == "JValue")
            {
                var tmp = ((Newtonsoft.Json.Linq.JToken)info).Path;
                tmp = Regex.Replace(tmp, "\\[\\d+\\]", "");
                tmp = Regex.Replace(tmp, "^\\.", "");
                tmp = Regex.Replace(tmp, "\\.", "/");
                textBox4.Text = "/" + tmp;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var m = Regex.Match(textBox2.Text, "^\\/.+$");
            var m1 = Regex.Match(textBox3.Text, "^\\/.+$");
            var m2 = Regex.Match(textBox4.Text, "^\\/.+$");
            if (m.Success && m1.Success && m2.Success)
            {
              
                    Properties.Settings.Default.parse = textBox2.Text + "," + textBox3.Text + "," + textBox4.Text;
              
            }
        }

        private void Form4_Load(object sender, EventArgs e)
        {
            var op = Properties.Settings.Default.parse.Split(',');
            if (op.Length == 3) {
                textBox2.Text = op[0];
                textBox3.Text = op[1];
                textBox4.Text = op[2];
            }
        }
    }
}
