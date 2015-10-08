using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Ionic.Zip;
using System.Threading;
using System.IO;

namespace MydayZIP
{
    public partial class Form2 : Form
    {
        ZipFile zip;
        string zippath;
        string zippasswd;
        List<String> filelist;
        BackgroundWorker bgwWorker = new BackgroundWorker();
        public Form2(string strText,string strPasswd, List<String> files)
        {
            InitializeComponent();
            this.ControlBox = false;
            this.Text = strText;
            zippath = strText;
            zippasswd = strPasswd;
            filelist = new List<String>(files);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //this.Close();
            bgwWorker.CancelAsync();
            label1.Text = "取消中...";
            button1.Enabled = false;
        }

        private void Form2_Shown(object sender, EventArgs e)
        {
            bgwWorker.DoWork += new DoWorkEventHandler(bgwWorker_DoWork);
            bgwWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bgwWorker_RunWorkerCompleted);
            bgwWorker.ProgressChanged += new ProgressChangedEventHandler(bgwWorker_ProgressChanged);
            bgwWorker.WorkerReportsProgress = true;
            bgwWorker.WorkerSupportsCancellation = true;
            DeleteZIPFile();
            bgwWorker.RunWorkerAsync();
        }
        private void bgwWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            int i;
            for (i = 0; i < filelist.Count; i++)
            {
                int _p = Convert.ToInt32((float)(i / filelist.Count) * 100);
                bgwWorker.ReportProgress(_p,filelist[i]);
                if (bgwWorker.CancellationPending)
                {
                    e.Cancel = true;
                    return;
                }
                zip = new ZipFile(zippath, System.Text.Encoding.Default);
                zip.Password = zippasswd;
                zip.AddFile(filelist[i], "");
                zip.Save();
            }
        }
        private void bgwWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBar1.Value = e.ProgressPercentage;
            label1.Text = e.UserState.ToString();

        }
        private void bgwWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled)
            {
                DeleteZIPFile();
                MessageBox.Show("取消檔案壓縮!");
                this.Close();
            }
            else if (e.Error != null)
            {
                //DeleteZIPFile();
                MessageBox.Show("檔案壓縮錯誤: " + e.Error.Message);
                this.Close();
            }
            else
            {
                MessageBox.Show("檔案壓縮完成!");
                this.Close();
            }
        }

        private void Form2_Load(object sender, EventArgs e)
        {
        }
        private void DeleteZIPFile()
        {
            if (System.IO.File.Exists(zippath))
            {
                System.IO.File.Delete(zippath);
            }
        }
    }
}
