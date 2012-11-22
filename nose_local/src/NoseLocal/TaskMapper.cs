using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IronCow;
using System.Diagnostics;
using Microsoft.Win32;
using System.Text.RegularExpressions;
using System.ComponentModel;

namespace Nose.Core
{
    public class TaskMapper
    {
        private ICloudStorage _cloudStorage;
        private ProcessLogger _logger;
        private RTMFacade _rtm;

        public TaskMapper()
        {
            _cloudStorage = new DropboxFacade();

            _logger = new ProcessLogger(_cloudStorage);
            _rtm = new RTMFacade();
        }

        public void startAuthentication()
        {
            _cloudStorage.startAuthentication();
            _rtm.startAuthentication();
        }

        public void resumeAuthentication()
        {
            _cloudStorage.resumeAuthentication();
            _rtm.resumeAuthentication();
            _logger.sync();
        }

        public List<NoseTask> getTasks(string pList)
        {
            TaskList rtmTasks = _rtm.getActiveTasksList(pList);
            var activeTasks = from task in rtmTasks.Tasks
                              where !task.IsCompleted
                              orderby task.Name
                              select task;

            List<NoseTask> tasks = new List<NoseTask>();
            foreach (Task rtmTask in activeTasks)
                tasks.Add(new NoseTask { TaskID = rtmTask.Id, Name = rtmTask.Name });

            return tasks;
        }

        private void searchInRegistryKey(RegistryKey pKey, List<String> pApps)
        {
            string bannedAppNameFragments = ".*(hotfix|update|pack|sdk|toolkit|plugin|driver).*";

            foreach (string skName in pKey.GetSubKeyNames())
            {
                using (RegistryKey sk = pKey.OpenSubKey(skName))
                {
                    try
                    {
                        if (sk.GetValue("DisplayName") != null)
                        {
                            string appName = sk.GetValue("DisplayName").ToString();
                            if (Regex.IsMatch(appName, bannedAppNameFragments, RegexOptions.IgnoreCase))
                                continue;

                            pApps.Add(appName);
                        }
                    }
                    catch (Exception) { }
                }
            }
        }

        private List<String> getFromRegistry()
        {
            List<String> installedApps = new List<string>();

            string swKey_CurrentUser = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall";
            string swKey_LocalMachine32 = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall";
            string swKey_LocalMachine64 = @"SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Uninstall";

            using (RegistryKey rk = Registry.LocalMachine.OpenSubKey(swKey_CurrentUser))
            {
                searchInRegistryKey(rk, installedApps);
            }

            using (RegistryKey rk = Registry.LocalMachine.OpenSubKey(swKey_LocalMachine32))
            {
                searchInRegistryKey(rk, installedApps);
            }

            using (RegistryKey rk = Registry.LocalMachine.OpenSubKey(swKey_LocalMachine64))
            {
                searchInRegistryKey(rk, installedApps);
            }

            List<string> uniqueApps = new List<string>(installedApps.Distinct());
            uniqueApps.Sort();
            return uniqueApps;
        }

        private List<String> getRunningApps()
        {
            Process[] apps = _logger.getAllProcesses();

            List<string> runningApps = new List<string>();
            string appName = String.Empty;

            foreach (Process proc in apps)
                if (proc.MainWindowTitle.Length > 0)
                {
                    try
                    {
                        appName = proc.MainModule.FileVersionInfo.FileDescription;
                    }
                    catch (Win32Exception)
                    {
                        appName = proc.ProcessName;
                    }

                    runningApps.Add(appName);
                }

            return runningApps;
            
            //var appNames = from app in apps
            //               where app.MainWindowTitle.Length > 0
            //               orderby app.ProcessName
            //               select app.ProcessName;

            //return appNames.Distinct().ToList<string>();
        }

        public List<string> getApps()
        {
            List<string> installed = getFromRegistry();
            List<string> running = getRunningApps();
            installed.AddRange(running);

            List<string> unique = new List<string>(installed.Distinct());
            unique.Sort();
            return unique;
        }

        public List<Profile> getProfiles(string pTaskID)
        {
            return _logger.getProfiles(pTaskID);
        }

        public void startLogging()
        {
            _logger.start();
        }

        public void stopLogging()
        {
            _logger.stop();
        }

        public void map(string pTaskID, string pProgramName, string pPattern)
        {
            _logger.map(pTaskID, pProgramName, pPattern);
        }

        public void unmap(string pTaskID, string pProgramName)
        {
            _logger.unmap(pTaskID, pProgramName);
        }
    }

    public struct NoseTask
    {
        public string TaskID { get; set; }
        public string Name { get; set; }
    }
}
