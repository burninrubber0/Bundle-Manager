using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BundleManager
{
    public interface ILoader
    {
        void SetStatus(string status);
        void SetProgress(int progress);
    }
}
