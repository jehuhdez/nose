using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nose.Core
{
    interface ICloudStorage
    {
        void startAuthentication();
        void resumeAuthentication();
        void save(string pFilename, string pNoseData, IEncryptor pEncryptor);
        string read(string pFilename, IEncryptor pEncryptor);
    }
}
