using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nose.Core
{
    interface IEncryptor
    {
        byte[] encrypt(byte [] pText, string pKey);
        byte[] decrypt(byte [] pText, string pKey);
    }
}
