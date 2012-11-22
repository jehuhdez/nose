using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace Nose.Core
{
    internal class XmlActivitiesPersistor
    {
        private const string TASK_PATH = "//activity[@taskID='{0}']";
        private const string APP_PATH = "profile[@applicationName='{0}']";

        private XmlDocument _doc;
        private XmlNode _actNode;

        private XmlActivitiesPersistor(XmlDocument pDoc)
        {
            _doc = pDoc;
            _actNode = _doc.SelectSingleNode("//activities");
        }

        public XmlActivitiesPersistor(string pUser)
        {
            _doc = setUpXmlDoc(pUser);
            _actNode = _doc.SelectSingleNode("//activities");
        }

        private XmlNode getActivityNode(string pTaskID)
        {
            string taskPath = String.Format(TASK_PATH, pTaskID);
            XmlNode taskNode = _doc.SelectSingleNode(taskPath);
            
            if (taskNode == null)
            {
                XmlElement taskElement = _doc.CreateElement("activity");
                taskElement.SetAttribute("taskID", pTaskID);
                taskElement.SetAttribute("creationDate", DateTime.Now.ToString());

                _actNode.AppendChild(taskElement);
                taskNode = taskElement;
            }

            return taskNode;
        }

        private XmlNode getProfileNode(XmlNode pTaskNode, string pAppName)
        {
            string appPath = String.Format(APP_PATH, pAppName);
            XmlNode appNode = pTaskNode.SelectSingleNode(appPath);

            if (appNode == null)
            {
                XmlElement appElement = _doc.CreateElement("profile");
                appElement.SetAttribute("applicationName", pAppName);
                appElement.SetAttribute("pattern", String.Empty);
                appElement.SetAttribute("creationDate", DateTime.Now.ToString());

                pTaskNode.AppendChild(appElement);
                appNode = appElement;
            }

            return appNode;
        }

        private XmlDocument setUpXmlDoc(string pUser)
        {
            XmlDocument xmlDoc = new XmlDocument();
            XmlDeclaration dec = xmlDoc.CreateXmlDeclaration("1.0", null, null);
            xmlDoc.AppendChild(dec);

            XmlElement root = xmlDoc.CreateElement("nose");
            root.SetAttribute("version", "0.1");
            xmlDoc.AppendChild(root);

            XmlElement info = xmlDoc.CreateElement("info");
            info.SetAttribute("user", pUser);

            XmlElement appLog = xmlDoc.CreateElement("activities");

            root.AppendChild(info);
            root.AppendChild(appLog);

            return xmlDoc;
        }

        public void map(string pTaskID, string pProgramName, string pPattern)
        {
            XmlNode taskNode = getActivityNode(pTaskID);
            XmlNode appNode = getProfileNode(taskNode, pProgramName);
            appNode.Attributes["pattern"].Value = pPattern;
        }

        public void unmap(string pTaskID, string pProgramName)
        {
            XmlNode taskNode = getActivityNode(pTaskID);
            XmlNode appNode = getProfileNode(taskNode, pProgramName);
            taskNode.RemoveChild(appNode);
        }

        public void save(string pFileName)
        {
            _doc.Save(pFileName);
        }

        public string getDocument()
        {
            return _doc.OuterXml;
        }

        public List<Profile> getTaskProfiles(string pTaskID)
        {
            List<Profile> profiles = new List<Profile>();

            string profilePath = String.Format(TASK_PATH + "/profile", pTaskID);
            XmlNodeList profileNodes = _doc.SelectNodes(profilePath);
            foreach(XmlNode node in profileNodes)
            {
                string appName = node.Attributes["applicationName"].Value;
                string pattern = node.Attributes["pattern"].Value;
                profiles.Add(new Profile { ApplicationName = appName, Pattern = pattern });
            }
                
            return profiles;
        }

        public static XmlActivitiesPersistor parse(string pXML)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(pXML);

            return new XmlActivitiesPersistor(doc);
        }
    }

    public struct Profile
    {
        public string ApplicationName { get; set; }
        public string Pattern { get; set; }

        public string AsBasicString
        {
            get { return String.Format("{0}\t=>\t{1}", ApplicationName, Pattern); }
        }
    }
}
