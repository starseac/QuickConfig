using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace QuickConfig
{
    public partial class frmLoading : Form
    {
         static Thread WaitingThread;

        static frmLoading _Self;

        public static Form SelfForm;

        static frmLoading()
        {
            
        }
        public frmLoading(Point Location)
        {
            InitializeComponent();
            this.Location = new Point(Location.X - this.Width / 2, Location.Y - this.Height / 2);
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (this.pictureBox1 != null && this.pictureBox1.Image != null)
            {
                this.pictureBox1.Image.Dispose();
            }
            if (this.BackgroundImage != null)
            {
                this.BackgroundImage.Dispose();
            }
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        /// <summary>
        /// 显示等待窗锁
        /// </summary>
        static object lockObj = new object();

        public static void ShowLoading()
        {
            lock (lockObj)
            {
                if (frmLoading.WaitingThread == null)
                {
                    frmLoading.WaitingThread = new Thread(new ThreadStart(() =>
                    {
                        
                        frmLoading._Self = new frmLoading(new Point(Screen.PrimaryScreen.WorkingArea.Width/2,Screen.PrimaryScreen.WorkingArea.Height/2));
                        frmLoading._Self.TopMost = true;

                        if (frmLoading._Self.ShowDialog() != DialogResult.OK)
                        {
                            frmLoading._Self = null;
                            //if (SelfForm == null)
                            //    return;
                            //Form formActive = null;
                            //formActive = Application.OpenForms["MainForm"];
                            //var prop = formActive.GetType().GetProperty("CallSelectInvoke");
                            //formActive.BeginInvoke(Delegate.CreateDelegate(prop.PropertyType, formActive, "Activate"));
                            //formActive.BeginInvoke(Delegate.CreateDelegate(prop.PropertyType, formActive, "Select"));

                            frmLoading.WaitingThread = null;
                        }
                    }));
                    frmLoading.WaitingThread.Start();
                }
            }
        }

        public static void ShowLoading(Point Location)
        {
            lock (lockObj)
            {
                if (frmLoading.WaitingThread == null)
                {
                    frmLoading.WaitingThread = new Thread(new ThreadStart(() =>
                    {
                        frmLoading._Self = new frmLoading(Location);
                        frmLoading._Self.TopMost = true;
                        
                        if (frmLoading._Self.ShowDialog() != DialogResult.OK)
                        {
                            frmLoading._Self = null;
                            //if (SelfForm == null)
                            //    return;
                            //Form formActive = null;
                            //formActive = Application.OpenForms["MainForm"];
                            //var prop = formActive.GetType().GetProperty("CallSelectInvoke");
                            //formActive.BeginInvoke(Delegate.CreateDelegate(prop.PropertyType, formActive, "Activate"));
                            //formActive.BeginInvoke(Delegate.CreateDelegate(prop.PropertyType, formActive, "Select"));

                            frmLoading.WaitingThread = null;
                        }
                    }));
                    frmLoading.WaitingThread.Start();
                }
            }
        }

        delegate void CloseSelfDialog();

        public static void HideLoading()
        {
            Thread.Sleep(1000);
            if (frmLoading.WaitingThread != null)
            {
                if (frmLoading._Self != null)
                {
                    frmLoading._Self.BeginInvoke(new CloseSelfDialog(() =>
                    {
                        frmLoading._Self.Close();
                    }));
                }
                else
                {
                    frmLoading.WaitingThread.Abort();
                    frmLoading.WaitingThread = null;
                    frmLoading._Self = null;

                    //Form formActive = null;
                    //formActive = Application.OpenForms["MainForm"];
                    //formActive.Activate();
                    //formActive.Select();
                }
            }
        }
    }
}
