using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace Nose.Core
{
    internal class XmlLogPersistor
    {
        private XmlDocument _doc;
        private XmlNode _appLogNode;
        private XmlNode _lastEntry;

        private DateTime _lastLog;

        private XmlLogPersistor(XmlDocument pXmlDoc)
        {
            _doc = pXmlDoc;
            _appLogNode = _doc.SelectSingleNode("//applicationLogs");
        }

        public XmlLogPersistor(string pUser, string pDevice)
        {
            _doc = setupXmlDoc(pUser, pDevice);
            _appLogNode = _doc.SelectSingleNode("//applicationLogs");
        }

        private XmlDocument setupXmlDoc(string pUser, string pDevice)
        {
            XmlDocument xmlDoc = new XmlDocument();
            XmlDeclaration dec = xmlDoc.CreateXmlDeclaration("1.0", null, null);
            xmlDoc.AppendChild(dec);

            XmlElement root = xmlDoc.CreateElement("nose");
            root.SetAttribute("version", "0.1");
            xmlDoc.AppendChild(root);

            XmlElement info = xmlDoc.CreateElement("info");
            info.SetAttribute("user", pUser);
            info.SetAttribute("device", pDevice);

            XmlElement appLog = xmlDoc.CreateElement("applicationLogs");

            root.AppendChild(info);
            root.AppendChild(appLog);

            return xmlDoc;
        }

        private void setLastDuration(DateTime pNow)
        {
            if (_lastEntry != null)
            {
                TimeSpan delta = pNow - _lastLog;
                _lastEntry.Attributes["durationInSecs"].Value = delta.TotalSeconds.ToString("0.00");
            }
        }

        public void logEntry(string pProcessName, string pWindowTitle)
        {
            DateTime now = DateTime.Now;
            setLastDuration(now);

            XmlElement log = _doc.CreateElement("application");
            log.SetAttribute("start", now.ToString());
            log.SetAttribute("durationInSecs", "0.00");
            log.SetAttribute("name", pProcessName);
            log.SetAttribute("description", pWindowTitle);

            _appLogNode.AppendChild(log);

            _lastEntry = log;
            _lastLog = now;
        }

        public void reset()
        {
            _appLogNode.RemoveAll();
        }

        public void save(string pFileName)
        {
            _doc.Save(pFileName);
        }

        public string getDocument()
        {
            return _doc.OuterXml;
        }

        public static XmlLogPersistor parse(string pXML)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(pXML);

            return new XmlLogPersistor(doc);
        }

        internal void closeLog()
        {
            setLastDuration(DateTime.Now);
        }
    }
}
