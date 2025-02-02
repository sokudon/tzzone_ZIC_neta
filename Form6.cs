using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.DataFormats;

namespace neta
{
    public partial class Form6 : Form
    {

        private NETA_TIMER parentForm; // Form1のインスタンスを保持する

        public Form6(NETA_TIMER form1)
        {
            InitializeComponent();
            parentForm = form1; // 渡されたForm1のインスタンスを保存
        }

        public class DisplayItem
        {
            public string Name { get; set; }
            public bool IsChecked { get; set; }
        }

        private void Form6_Load(object sender, EventArgs e)
        {
            comboBox1.Text = Properties.Settings.Default.custom_ambigous;
            now.Text = Properties.Settings.Default.custom_curr;
            elapsed.Text = Properties.Settings.Default.custom_elapsed;
            end.Text = Properties.Settings.Default.custom_end;
            comboBox2.Text = Properties.Settings.Default.custom_finished;
            left.Text = Properties.Settings.Default.custom_left;
            comboBox3.Text = Properties.Settings.Default.custom_not_start;
            span.Text = Properties.Settings.Default.custom_span;
            start.Text = Properties.Settings.Default.custom_start;

            // checkedListBox1.SetItemChecked(i, checkedStates[i]);
            string[] item = Properties.Settings.Default.display_order.Split(",");
            int ITEMMAX = 6;

            for (var i = 0; i < ITEMMAX; i++)
            {
                checkedListBox1.Items.Add(item[i]);
          
                string s = item[i].ToString();

                if (s == "現在時刻") { checkedListBox1.SetItemChecked(i, Properties.Settings.Default.display_curr); }
                if (s == "経過時間") { checkedListBox1.SetItemChecked(i, Properties.Settings.Default.display_elapsed); }
                if (s == "残り時間") { checkedListBox1.SetItemChecked(i, Properties.Settings.Default.display_left); }
                if (s == "イベ期間") { checkedListBox1.SetItemChecked(i, Properties.Settings.Default.display_span); }
                if (s == "開始時間") { checkedListBox1.SetItemChecked(i, Properties.Settings.Default.display_start); }
                if (s == "終了時間") { checkedListBox1.SetItemChecked(i, Properties.Settings.Default.display_end); }

            }

        }


        private void Form6_FormClosing(object sender, FormClosingEventArgs e)
        {
            string allValues = "";
            foreach (var item in checkedListBox1.Items)
            {
                allValues += item.ToString() + ",";
            }
            Properties.Settings.Default.display_order = allValues;
        }


        private void now_SelectedIndexChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.custom_curr = now.Text;
        }

        private void elapsted_SelectedIndexChanged(object sender, EventArgs e)
        {

            Properties.Settings.Default.custom_elapsed = elapsed.Text;
        }

        private void left_SelectedIndexChanged(object sender, EventArgs e)
        {

            Properties.Settings.Default.custom_left = left.Text;
        }

        private void span_SelectedIndexChanged(object sender, EventArgs e)
        {

            Properties.Settings.Default.custom_span = span.Text;
        }

        private void start_SelectedIndexChanged(object sender, EventArgs e)
        {

            Properties.Settings.Default.custom_start = start.Text;
        }



        private void end_SelectedIndexChanged(object sender, EventArgs e)
        {

            Properties.Settings.Default.custom_end = end.Text;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

            Properties.Settings.Default.custom_ambigous = comboBox1.Text;
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

            Properties.Settings.Default.custom_finished = comboBox2.Text;
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {

            Properties.Settings.Default.custom_not_start = comboBox3.Text;
        }


        private void checkedListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                int selectedIndex = checkedListBox1.SelectedIndex;


                if (selectedIndex < 0)
                {
                    return;
                }

                // Get the checked state of the item
                bool isChecked = checkedListBox1.GetItemChecked(selectedIndex);
                var selected = checkedListBox1.SelectedItems[0];
                string s = selected.ToString();

                if (s == "現在時刻") { Properties.Settings.Default.display_curr = isChecked; }
                if (s == "経過時間") { Properties.Settings.Default.display_elapsed = isChecked; }
                if (s == "残り時間") { Properties.Settings.Default.display_left = isChecked; }
                if (s == "イベ期間") { Properties.Settings.Default.display_span = isChecked; }
                if (s == "開始時間") { Properties.Settings.Default.display_start = isChecked; }
                if (s == "終了時間") { Properties.Settings.Default.display_end = isChecked; }


                string allValues = "";
                foreach (var item in checkedListBox1.Items)
                {
                    allValues += item.ToString() + ",";
                }
                Properties.Settings.Default.display_order = allValues;

                bool fif = Properties.Settings.Default.font_margn;
                if (parentForm != null)
                {
                    if (fif)
                    {
                        parentForm.menu_align(0, true);
                    }
                    else
                    {
                        parentForm.menu_align(50, false);
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(sender.ToString() + ex.Message.ToString());
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void checkedListBox1_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            checkedListBox1_SelectedIndexChanged(sender, e);
        }

        //https://g.co/gemini/share/579eec4097d6
        private void buttonUp_Click(object sender, EventArgs e)
        {
            int selectedIndex = checkedListBox1.SelectedIndex;
            bool isChecked = checkedListBox1.GetItemChecked(selectedIndex);

            if (selectedIndex < 0)
            {
                return;
            }

            // 一番上でない場合
            if (selectedIndex > 0)
            {
                // 選択された項目と一つ上の項目を入れ替える
                object selectedItem = checkedListBox1.SelectedItem;

                // Get the checked state of the item
                checkedListBox1.Items.RemoveAt(selectedIndex);
                checkedListBox1.Items.Insert(selectedIndex - 1, selectedItem);

                checkedListBox1.SetItemChecked(selectedIndex - 1, isChecked);
                checkedListBox1.SelectedIndex = selectedIndex - 1;

            }
            // 一番上の場合、一番下に移動
            else if (selectedIndex == 0 && checkedListBox1.Items.Count > 1)
            {
                object selectedItem = checkedListBox1.SelectedItem;
                checkedListBox1.Items.RemoveAt(selectedIndex);
                checkedListBox1.Items.Add(selectedItem);

                checkedListBox1.SetItemChecked(checkedListBox1.Items.Count - 1, isChecked);
                checkedListBox1.SelectedIndex = checkedListBox1.Items.Count - 1;

            }
            checkedListBox1.SetItemChecked(selectedIndex, isChecked);
        }

        private void buttonDown_Click(object sender, EventArgs e)
        {
            int selectedIndex = checkedListBox1.SelectedIndex;
            bool isChecked = checkedListBox1.GetItemChecked(selectedIndex);

            if (selectedIndex < 0)
            {
                return;
            }

            // 一番下でない場合
            if (selectedIndex < checkedListBox1.Items.Count - 1)
            {
                // 選択された項目と一つ下の項目を入れ替える
                object selectedItem = checkedListBox1.SelectedItem;
                checkedListBox1.Items.RemoveAt(selectedIndex);
                checkedListBox1.Items.Insert(selectedIndex + 1, selectedItem);

                checkedListBox1.SetItemChecked(selectedIndex + 1, isChecked);
                checkedListBox1.SelectedIndex = selectedIndex + 1;
            }
            // 一番下の場合、一番上に移動
            else if (selectedIndex == checkedListBox1.Items.Count - 1 && checkedListBox1.Items.Count > 1)
            {
                object selectedItem = checkedListBox1.SelectedItem;
                checkedListBox1.Items.RemoveAt(selectedIndex);
                checkedListBox1.Items.Insert(0, selectedItem);

                checkedListBox1.SetItemChecked(0, isChecked);
                checkedListBox1.SelectedIndex = 0;
            }
        }

    }
}
