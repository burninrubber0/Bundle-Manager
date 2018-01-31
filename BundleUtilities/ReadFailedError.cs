using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BundleUtilities
{
    public class ReadFailedError : Exception
    {
        public ReadFailedError(string msg) : base(msg)
        {

        }

        public ReadFailedError(string msg, Exception innerException) : base(msg, innerException)
        {

        }
    }
}
