using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace ModelViewer
{
    public interface ICamera
    {
        Matrix4 LookAtMatrix { get; }
        void Update();
    }
}
