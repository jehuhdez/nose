using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using TM = System.Timers;

using Nose.Core;
using IronCow;
using Microsoft.Win32;


namespace LocalMapper
{
    public partial class Form1 : Form
    {
        private bool _logging = false;
        private TaskMapper _mapper;

        private Timer _timer;
        private List<String> _currApps = new List<string>();

        public Form1()
        {
            InitializeComponent();
            _mapper = new TaskMapper();
            _mapper.startAuthentication();

            SystemEvents.SessionSwitch += new SessionSwitchEventHandler(handleSessionSwitch);
        }

        private void setUpAppSyncTimer()
        {
            _timer = new Timer();
            _timer.Tick += new EventHandler(_timerTick);
            _timer.Interval = 3000;
        }

        void _timerTick(object sender, EventArgs e)
        {
            List<string> newApps = _mapper.getApps();

            var diff = newApps.Except(_currApps.Select(ca => ca)).ToList();
            if (diff.Count == 0)
                return;

            lstApps.DataSource = newApps;
            _currApps = newApps;
        }

        private void handleSessionSwitch(object sender, SessionSwitchEventArgs e)
        {
            switch (e.Reason)
            {
                case SessionSwitchReason.SessionLock:
                    {
                        if (_logging)
                            _mapper.stopLogging();
                    }
                    break;

                case SessionSwitchReason.SessionUnlock:
                    {
                        if (_logging)
                            _mapper.startLogging();
                    }
                    break;
            }
        }

        private void btnLog_Click(object sender, EventArgs e)
        {
            if (_logging)
            {
                _mapper.stopLogging();
                btnLog.Text = "registra";
                _logging = false;
            }
            else
            {
                _mapper.startLogging();
                btnLog.Text = "detente";
                _logging = true;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            button1.Enabled = false;
            _mapper.resumeAuthentication();

            lstApps.DataSource = _mapper.getApps();
            //setUpAppSyncTimer();
            //_timer.Start();

            lstTasks.DataSource = _mapper.getTasks("Inbox");
            lstTasks.ValueMember = "TaskID";
            lstTasks.DisplayMember = "Name";

            button1.Visible = false;
            btnLog.Visible = true;
            btnAddActProf.Visible = true;
            btnRemActProf.Visible = true;
        }

        private void btnAddActProf_Click(object sender, EventArgs e)
        {
            string taskID = lstTasks.SelectedValue.ToString();
            string appName = lstApps.SelectedValue.ToString();
            string pattern = txtKeyword.Text;

            _mapper.map(taskID, appName, pattern);

            lstTasks_SelectedIndexChanged(null, null);

            txtKeyword.Text = String.Empty;
        }

        private void lstTasks_SelectedIndexChanged(object sender, EventArgs e)
        {
            string taskID = lstTasks.SelectedValue.ToString();

            lstMappedApps.DisplayMember = "AsBasicString";
            lstMappedApps.ValueMember = "ApplicationName";
            lstMappedApps.DataSource = _mapper.getProfiles(taskID);
        }

        private void btnRemActProf_Click(object sender, EventArgs e)
        {
            string taskID = lstTasks.SelectedValue.ToString();
            string appName = lstMappedApps.SelectedValue.ToString();

            _mapper.unmap(taskID, appName);

            lstTasks_SelectedIndexChanged(null, null);
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_logging)
                _mapper.stopLogging();

            //_timer.Stop();
        }

        private void txtKeyword_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                btnAddActProf_Click(null, null);
        }

        private void lstMappedApps_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
                btnRemActProf_Click(null, null);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

        private void lstApps_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

    }
}
