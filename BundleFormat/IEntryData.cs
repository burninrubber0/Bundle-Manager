using BundleUtilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BundleFormat
{
	public interface IEntryData
	{
		bool Read(BundleEntry entry, ILoader loader = null);

		bool Write(BundleEntry entry);

		EntryType GetEntryType(BundleEntry entry);

		IEntryEditor GetEditor(BundleEntry entry);
	}
}
