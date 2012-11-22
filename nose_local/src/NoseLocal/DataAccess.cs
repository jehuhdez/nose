using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Configuration;
using System.Reflection;
using System.IO;
using System.Timers;

namespace Nose.Core
{
    internal class DataAccess
    {
        private ICloudStorage _storage;
        private IEncryptor _encryptor;

        private XmlLogPersistor _xmlLog;
        private XmlActivitiesPersistor _xmlAct;

        private DateTime _lastCloudSync;

        public DataAccess(ICloudStorage pStorage)
        {
            _storage = pStorage;
            _encryptor = new AESEncryptor();
        }

        private string getLogFilename()
        {
            return DateTime.Now.ToString("yyyyMMdd") + ".log";
        }

        private string getActivitiesFilename()
        {
            return "activities";
        }

        private string getFullFilename(string pFileName)
        {
            string currDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            return String.Format(@"{0}/{1}", currDir, pFileName);
        }

        public void localSync()
        {
            _xmlLog.save(getFullFilename(getLogFilename()));
            _xmlAct.save(getFullFilename(getActivitiesFilename()));

            Console.WriteLine("==> local");
        }

        private void cloudSyncActivities()
        {
            _storage.save(getActivitiesFilename(), _xmlAct.getDocument(), _encryptor);
            _storage.save(getActivitiesFilename() + ".xml", _xmlAct.getDocument(), null);
        }

        private void cloudSyncLog()
        {
            _storage.save(getLogFilename(), _xmlLog.getDocument(), _encryptor);
            _storage.save(getLogFilename() + ".xml", _xmlLog.getDocument(), null);
        }

        public void cloudSync()
        {
            if (_xmlAct == null)
            {
                string xmlRead = _storage.read(getActivitiesFilename(), _encryptor);
                if (!String.IsNullOrEmpty(xmlRead))
                    _xmlAct = XmlActivitiesPersistor.parse(xmlRead);
                else
                    _xmlAct = new XmlActivitiesPersistor(Environment.UserName);
            }

            if(DateTime.Now.Day != _lastCloudSync.Day)
            {
                if (_xmlLog != null)
                {
                    _xmlLog.closeLog("DayEnded");
                    cloudSyncLog();
                }

                _xmlLog = null;
            }

            if (_xmlLog == null)
            {
                string xmlRead = _storage.read(getLogFilename(), _encryptor);
                if (!String.IsNullOrEmpty(xmlRead))
                    _xmlLog = XmlLogPersistor.parse(xmlRead);
                else
                    _xmlLog = new XmlLogPersistor(Environment.UserName, Environment.MachineName);
            }

            cloudSyncActivities();
            cloudSyncLog();

            _lastCloudSync = DateTime.Now;

            Console.WriteLine("==> cloud");
        }

        public void map(string pTaskID, string pProgramName, string pPattern)
        {
            _xmlAct.map(pTaskID, pProgramName, pPattern);
        }

        public void unmap(string pTaskID, string pProgramName)
        {
            _xmlAct.unmap(pTaskID, pProgramName);
        }

        public void log(string pProcessName, string pWindowTitle)
        {
            _xmlLog.logEntry(pProcessName, pWindowTitle);
            Console.WriteLine("{0} ->\t{1}\t{2}", DateTime.Now.ToString(), pProcessName, pWindowTitle);
        }

        public List<Profile> getTaskProfiles(string pTaskID)
        {
            return _xmlAct.getTaskProfiles(pTaskID);
        }

        internal void closeLog(string pEvent="Unknown")
        {
            _xmlLog.closeLog(pEvent);
        }
    }
}
