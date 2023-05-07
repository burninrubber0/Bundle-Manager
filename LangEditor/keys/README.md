# Keys
These files store known keys used in Burnout Paradise's Language resources.

## General format
Keys are plaintext except for the ability to use tags enclosed in `<` and `>`. At present, there are two available tags:
- `f` indicates a text file in the keys folder. For example, `<fvehicles>` means `keys/vehicles.txt`. Each line in the file replaces the tag, creating a new key.
- `r` indicates a numeric range formatted as `start-end`. For example, `<r100-150>` denotes a range of 100 to 150. Each number replaces the tag, creating a new key. Both the start and end are **inclusive**.

Currently, only one tag per line is supported, and tags cannot be used in files accessed through `<f*>` (i.e., tagging is not recursive). This may change in the future if there is demand for it.

**DO NOT ABUSE RANGES.** If you attempt to use it to, for example, find gameDB IDs from 0 to 1000000, **there will be false positives**. This feature is most useful for small ranges that are entirely or mostly sequential.

## Adding keys
If you have keys to add, you can submit them via the [issue for them](https://github.com/burninrubber0/Bundle-Manager/issues/35) or create a new pull request.

New keys should be as specific as possible so as to avoid false positives. Keys submitted here must be for the vanilla game—mods should not be included. Keys from development builds are allowed.

To get a specific set of keys, you can either manually copy them from the game's resources or (preferred) programmatically retrieve them. See the below example for an instance of the latter.

## Example dumping application
This is a dumper used to get all challenge IDs from the extracted ChallengeList resource of Burnout Paradise Remastered PC. It is a C++ console application using the Qt library. You can view the output in challenges.txt.

For ChallengeList details, see [Challenge List](https://burnout.wiki/wiki/Challenge_List) on the Burnout Wiki.

```cpp
#include <QDataStream>
#include <QFile>
#include <QTextStream>

int main(int argc, char *argv[])
{
	// Take extracted challenge file as only argument
	QFile challs(argv[1]);
	QDataStream challsStr(&challs);
	challsStr.setByteOrder(QDataStream::LittleEndian);
	challs.open(QIODeviceBase::ReadOnly);

	// Get num challenges, then IDs
	quint32 numEntries, entries;
	QList<quint64> ids;
	challsStr >> numEntries;
	challsStr >> entries;
	challs.seek(entries + 0xC0);
	quint64 id = 0;
	for (int i = 0; i < numEntries; ++i)
	{
		challsStr >> id;
		ids.append(id);
		challs.seek(challs.pos() + 0xD0);
	}
	challs.close();

	// Write challenge IDs to text file
	QFile out(QString(argv[1]) + ".txt");
	QTextStream outStr(&out);
	out.open(QIODeviceBase::WriteOnly);
	for (int i = 0; i < ids.count(); ++i)
		outStr << ids[i] << '\n';
	out.close();

	return 0;
}
```
