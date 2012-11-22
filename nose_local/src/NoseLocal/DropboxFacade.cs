using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using DropNet;
using DropNet.Models;
using System.Diagnostics;
using System.IO;
using System.Xml;
using System.Reflection;
using DropNet.Exceptions;

namespace Nose.Core
{
    internal class DropboxFacade : ICloudStorage
    {
        private readonly string API_KEY;
        private readonly string SECRET;
        private readonly string ENCRYPTION_PWD;

        private DropNetClient _dropbox;
        private UserLogin _login;

        public DropboxFacade()
        {
            API_KEY = ConfigurationManager.AppSettings["DBX_APIKEY"].ToString();
            SECRET = ConfigurationManager.AppSettings["DBX_SECRET"].ToString();
            ENCRYPTION_PWD = ConfigurationManager.AppSettings["ENC_PWD"].ToString();

            _dropbox = new DropNetClient(API_KEY, SECRET);
        }

        void ICloudStorage.startAuthentication()
        {
            _login = _dropbox.GetToken();
            Process.Start(_dropbox.BuildAuthorizeUrl());
        }

        void ICloudStorage.resumeAuthentication()
        {
            _login = _dropbox.GetAccessToken();
            _dropbox.UseSandbox = true;
        }

        void ICloudStorage.save(string pFilename, string pNoseInfo, IEncryptor pEncryptor)
        {
            if (pEncryptor != null)
            {
                string encFile = pFilename;
                byte[] encText = Encoding.UTF8.GetBytes(pNoseInfo);
                byte[] payload = pEncryptor.encrypt(encText, ENCRYPTION_PWD);
                _dropbox.UploadFile(@"/", encFile, payload);
            }
            else
                _dropbox.UploadFile(@"/", pFilename, Encoding.UTF8.GetBytes(pNoseInfo));

            Console.WriteLine("===> uploaded file {0}", DateTime.Now);
        }

        string ICloudStorage.read(string pFileName, IEncryptor pEncryptor)
        {
            byte[] encFileBytes = null;
            bool fileFound = false;
            try
            {
                encFileBytes = _dropbox.GetFile(pFileName);
                fileFound = true;
            }
            catch (DropboxException) 
            {
                fileFound = false;
            }

            if (!fileFound)
                return String.Empty;

            byte [] decBytes = pEncryptor.decrypt(encFileBytes, ENCRYPTION_PWD);
            return Encoding.UTF8.GetString(decBytes);
        }
    }
}
