using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using TM = System.Timers;
using System.Configuration;
using System.ComponentModel;

namespace Nose.Core
{
    class ProcessLogger
    {
        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();

        private bool _isLogging = false;
        private DataAccess _logger;
        private CancellationTokenSource _cancelTokenSrc;

        private TM.Timer _cloudSyncTimer;
        private TM.Timer _localSyncTimer;

        private readonly double AUTO_SAVE_MINS;
        private readonly double CLOUD_SYNC_MINS;

        public ProcessLogger(ICloudStorage pStorage)
        {
            _logger = new DataAccess(pStorage);

            string cfgAutoSaveMins = ConfigurationManager.AppSettings["AUTO_SAVE_MINS"].ToString();
            string cfgCloudSyncMins = ConfigurationManager.AppSettings["DBX_SAVE_MINS"].ToString();

            AUTO_SAVE_MINS = double.Parse(cfgAutoSaveMins);
            CLOUD_SYNC_MINS = double.Parse(cfgCloudSyncMins);

            setUpLocalSyncTimer();
            setUpCloudSyncTimer();
        }

        internal void sync()
        {
            // first retrieve any activities already stored in the cloud
            _logger.cloudSync();
            _logger.localSync();
        }

        private void setUpCloudSyncTimer()
        {
            double cloudSyncMillisecs = CLOUD_SYNC_MINS * 60000.0;
            NoseTimer timer = new NoseTimer(cloudSyncMillisecs);
            timer.DataAccess = _logger;

            _cloudSyncTimer = timer;
            _cloudSyncTimer.Elapsed += cloudSyncCallback;
            _cloudSyncTimer.AutoReset = true;
        }

        private void setUpLocalSyncTimer()
        {
            double localSyncMillisecs = AUTO_SAVE_MINS * 60000.0;
            NoseTimer timer = new NoseTimer(localSyncMillisecs);
            timer.DataAccess = _logger;

            _localSyncTimer = timer;
            _localSyncTimer.Elapsed += localSyncCallback;
            _localSyncTimer.AutoReset = true;
        }

        private static void cloudSyncCallback(object pSender, EventArgs pEventInfo)
        {
            NoseTimer timer = pSender as NoseTimer;
            timer.DataAccess.cloudSync();
        }

        private static void localSyncCallback(object pSender, EventArgs pEventInfo)
        {
            NoseTimer timer = pSender as NoseTimer;
            timer.DataAccess.localSync();
        }

        public void start()
        {
            if (_isLogging)
                return;

            _cancelTokenSrc = new CancellationTokenSource();
            new Thread(() => log(_logger, _cancelTokenSrc.Token)).Start();

            _localSyncTimer.Start();
            _cloudSyncTimer.Start();

            _isLogging = true;
        }

        public void stop()
        {
            _isLogging = false;

            _localSyncTimer.Stop();
            _cloudSyncTimer.Stop();

            _logger.closeLog();
            sync();

            _cancelTokenSrc.Cancel(false);
        }

        private void log(DataAccess pLogData, CancellationToken pCancelToken)
        {
            string lastTitle = String.Empty;
            try
            {
                while (true)
                {
                    IntPtr activeWindowHandle = GetForegroundWindow();
                    Process activeProcess = getActiveProcess(activeWindowHandle);
                    if ((activeProcess != null) && (lastTitle != activeProcess.MainWindowTitle))
                    {
                        string appName = String.Empty;

                        //pLogData.log(activeProcess.ProcessName, activeProcess.MainWindowTitle);
                        try
                        {
                            appName = activeProcess.MainModule.FileVersionInfo.ProductName;
                        }
                        catch (Win32Exception)
                        {
                            appName = activeProcess.ProcessName;
                        }
                        
                        string title = activeProcess.MainWindowTitle;
                        pLogData.log(appName, title);

                        lastTitle = title;
                    }

                    pCancelToken.ThrowIfCancellationRequested();
                    Thread.Sleep(500);
                }
            }
            catch (OperationCanceledException)
            {
            }
        }

        public void map(string pTaskID, string pProgramName, string pPattern)
        {
            _logger.map(pTaskID, pProgramName, pPattern);
        }

        public void unmap(string pTaskID, string pProgramName)
        {
            _logger.unmap(pTaskID, pProgramName);
        }

        public List<Profile> getProfiles(string pTaskID)
        {
            return _logger.getTaskProfiles(pTaskID);
        }

        public Process[] getAllProcesses()
        {
            return Process.GetProcesses();
        }

        private static Process getActiveProcess(IntPtr pActiveHandle)
        {
            Process[] processes = Process.GetProcesses();
            foreach (Process p in processes)
            {
                if (p.MainWindowHandle == pActiveHandle)
                    return p;
            }

            return null;
        }
    }

    internal class NoseTimer : TM.Timer
    {
        public NoseTimer(double pInterval)
            : base(pInterval)
        {
        }

        public DataAccess DataAccess { get; set; }
    }
}
