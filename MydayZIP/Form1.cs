using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Text.RegularExpressions;

namespace MydayZIP
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(textBox1.Text))
            {
                MessageBox.Show("請選擇壓檔案儲存路徑!");
                return;
            }
            if (String.IsNullOrEmpty(textBox2.Text))
            {
                MessageBox.Show("請輸入密碼!");
                return;
            }
            if (String.IsNullOrEmpty(textBox3.Text))
            {
                MessageBox.Show("請重新輸入密碼!");
                return;
            }
            if (textBox2.Text.CompareTo(textBox3.Text) != 0)
            {
                MessageBox.Show("密碼不一致!");
                return;
            }
            int passwdScore = PasswordScore(textBox2.Text);
            if (passwdScore < 4)
            {
                MessageBox.Show("密碼最少需有8個字元並包含大小寫英文字母、數字和符號!");
                return;
            }
            if (listView1.Items.Count == 0)
            {
                MessageBox.Show("請選擇要壓縮的檔案!");
                return;
            }
            List<String> filelist = new List<String>();
            for (int i = 0; i < listView1.Items.Count; i++)
            {
                filelist.Add(listView1.Items[i].Text);
            }
            Form2 f2 = new Form2(textBox1.Text, textBox2.Text, filelist);
            f2.ShowDialog();
        }

        private int PasswordScore(string password)
        {
            int score = 0;
            if (password.Length >= 8)
                score++;
            if (Regex.Match(password, @"\d", RegexOptions.ECMAScript).Success)
                score++;
            if (Regex.Match(password, @"[a-z]", RegexOptions.ECMAScript).Success &&
                Regex.Match(password, @"[A-Z]", RegexOptions.ECMAScript).Success)
                score++;
            if (Regex.Match(password, @"\W", RegexOptions.ECMAScript).Success)
                score++;
            return score;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string _filename;
            string _initdir;
            if (String.IsNullOrEmpty(textBox1.Text))
            {
                _filename = "MydayZIP.zip";
                _initdir = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            }
            else
            {
                _filename = Path.GetFileName(textBox1.Text);
                _initdir = Path.GetDirectoryName(textBox1.Text);
            }
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Title = "請選擇檔案";
            saveFileDialog.Filter = "壓縮檔(*.zip)|*.zip";
            saveFileDialog.FilterIndex = 1;
            saveFileDialog.RestoreDirectory = true;
            saveFileDialog.FileName = _filename;
            saveFileDialog.InitialDirectory = _initdir;
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                textBox1.Text = "";
                textBox1.Text = saveFileDialog.FileName.ToString();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            //dialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            dialog.RestoreDirectory = true;
            dialog.Filter = "所有檔案(*.*)|*.*";
            dialog.Multiselect = true;
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                int cnt;
                for (cnt = 0; cnt < dialog.FileNames.Length; cnt++)
                {
                    listView1.Items.Add(dialog.FileNames[cnt]);
                    //MessageBox.Show(dialog.FileNames[cnt]);
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem lvi in listView1.SelectedItems)
            {
                listView1.Items.RemoveAt(lvi.Index);
            }
        }

        private void textBox1_Enter(object sender, EventArgs e)
        {
            
        }

        private void textBox1_Leave(object sender, EventArgs e)
        {
            
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            ToolTip myToolTip = new ToolTip();
            myToolTip.SetToolTip(textBox1, "請選擇壓縮檔儲存路徑");
            myToolTip.SetToolTip(button1, "選擇壓縮檔儲存路徑");
            myToolTip.SetToolTip(textBox2, "密碼最少需有8個字元並包含大小寫英文字母、數字和符號");
            myToolTip.SetToolTip(textBox3, "密碼最少需有8個字元並包含大小寫英文字母、數字和符號");
            myToolTip.SetToolTip(button2, "新增要壓縮的檔案");
            myToolTip.SetToolTip(button3, "刪除選擇的檔案");
            myToolTip.SetToolTip(button4, "開始壓縮");
            myToolTip.SetToolTip(button5, "關閉程式");
            this.listView1.Focus();
        }
    }
}
