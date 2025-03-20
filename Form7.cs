using Newtonsoft.Json.Linq;
using OBSWebsocketDotNet;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace neta
{
    public partial class Form7 : Form
    {
        public Form7()
        {
            InitializeComponent();
        }

        private void Form7_Load(object sender, EventArgs e)
        {
            start.Text = Properties.Settings.Default.st;
            end.Text = Properties.Settings.Default.en;
            title.Text = Properties.Settings.Default.ibe;
            ip.Text = Properties.Settings.Default.ipadress;
            script.Text = Properties.Settings.Default.script_name;
            port.Text = Properties.Settings.Default.port;
            source.Text = Properties.Settings.Default.source_name;
            scene.Text = Properties.Settings.Default.scene_name;
            req.Text = Properties.Settings.Default.request;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.script_name = script.Text;
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

            Properties.Settings.Default.source_name = source.Text;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.scene_name = scene.Text;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string ipad = ip.Text;
            string portn = port.Text;
            string reqe = req.Text;
            string obsAddress = "ws://" + ipad + ":" + portn; // OBS WebSocketのアドレス
            string password = null; // 設定したパスワード（空でも可）
            OBSWebsocket obs = new OBSWebsocket();
            try
            {
                obs.ConnectAsync(obsAddress, password);
                System.Threading.Thread.Sleep(5000);

                // 接続が確立されたか確認
                if (obs.IsConnected)
                {
                    Console.WriteLine("OBS に接続成功！");
                    //var response = obs.GetVersion();
                    object settings = new
                    {
                        scene_name = scene.Text,
                        source_name = source.Text,
                        script_name = script.Text,
                        start = start.Text,
                        end = end.Text,
                        title = title.Text
                    };
                    JObject parameters = new JObject
                    {
                    { "settings", JObject.FromObject(settings) }
                    };

                    SetJsonSettings(obs, parameters, reqe);
                    System.Threading.Thread.Sleep(5000);

                    MessageBox.Show("送信成功:request:"+reqe+",obs:"+obsAddress+"," + parameters.ToString());
                }
                else
                {
                    Console.WriteLine("OBS に接続できませんでした。");
                }

                obs.Disconnect();
            }
            catch (AuthFailureException)
            {
                Console.WriteLine("認証に失敗しました！");
                obs.Disconnect();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"OBS に接続できませんでした: {ex.Message}");
                obs.Disconnect();
            }
        }

        static void SetJsonSettings(OBSWebsocket obs, JObject parameters, string req)
        {
            obs.SendRequest(req, parameters);
        }

        private void ip_TextChanged(object sender, EventArgs e)
        {

            Properties.Settings.Default.ipadress = ip.Text;
        }

        private void port_TextChanged(object sender, EventArgs e)
        {

            Properties.Settings.Default.port = port.Text;
        }


        private void req_SelectedIndexChanged(object sender, EventArgs e)
        {

            Properties.Settings.Default.request = req.Text;
        }
    }
}
