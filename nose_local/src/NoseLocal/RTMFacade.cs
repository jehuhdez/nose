using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Configuration;
using IronCow;

namespace Nose.Core
{
    class RTMFacade
    {
        private readonly string API_KEY;
        private readonly string SECRET;

        private Rtm _rtm;
        private string _rtmAuthFrob;

        public bool Authenticated
        {
            get { return !String.IsNullOrEmpty(_rtm.AuthToken); }
        }

        public RTMFacade()
        {
            API_KEY = ConfigurationManager.AppSettings["RTM_APIKEY"].ToString();
            SECRET = ConfigurationManager.AppSettings["RTM_SECRET"].ToString();

            _rtm = new Rtm(API_KEY, SECRET);
        }

        public void startAuthentication()
        {
            _rtmAuthFrob = _rtm.GetFrob();
            Process.Start(_rtm.GetAuthenticationUrl(_rtmAuthFrob, AuthenticationPermissions.Write));
        }

        public void resumeAuthentication()
        {
            _rtm.AuthToken = _rtm.GetToken(_rtmAuthFrob);
        }

        public TaskList getActiveTasksList(string pListName)
        {
            foreach (TaskList list in _rtm.TaskLists)
                if (String.Equals(pListName, list.Name, StringComparison.CurrentCultureIgnoreCase))
                    return list;

            return null;
        }
    }
}
