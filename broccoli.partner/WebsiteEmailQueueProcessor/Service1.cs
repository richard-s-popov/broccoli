using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Timers;
using BroccoliTrade.Logics.MSMQ;

namespace WebsiteEmailQueueProcessor
{
    public partial class WebsiteEmailQueueProcessor : ServiceBase
    {
        Timer timer = new Timer();

        public WebsiteEmailQueueProcessor()
        {
            InitializeComponent();
        }

        void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            try
            {
                QueueProcessor.StartProcessing();
            }
            catch (Exception ex)
            {

                Log("Elapsed: " + ex.Message);
            }
        }

        protected override void OnStart(string[] args)
        {
            try
            {
                timer.Interval = 1000;
                timer.Elapsed += new ElapsedEventHandler(timer_Elapsed);
                timer.AutoReset = true;
                timer.Enabled = true;
                timer.Start();
            }
            catch (Exception e)
            {

                Log("OnStart: " + e.Message);
            }
        }

        protected override void OnStop()
        {
            try
            {
                QueueProcessor.StopProcessing();
                timer.AutoReset = false;
                timer.Enabled = false;
            }
            catch (Exception e)
            {
                
               Log("OnStop: " + e.Message);
            }
            
        }

        private void OnElapsedTime(object source, ElapsedEventArgs e)
        {
            QueueProcessor.StartProcessing();
        }

        private void Log(string msg)
        {
            File.AppendAllText(@"C:\servicelog.txt", DateTime.Now + " " + msg + Environment.NewLine);
        }
    }
}
