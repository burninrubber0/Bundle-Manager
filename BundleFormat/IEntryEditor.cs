using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BundleFormat
{
	public interface IEntryEditor
	{
		DialogResult ShowDialog(IWin32Window owner);
	}
}
