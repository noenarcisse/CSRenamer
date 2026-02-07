using System.Text.RegularExpressions;

//faut prog.exe --% arg1 arg2 pour forcer powershell sur les "" :<

if (args.Length != 1)
{
    Console.WriteLine("Error: Usage is \"search\" or \"search>replace\"");
    return 1;
}
else
{
    if (args[0].AsSpan().Contains('>'))
    {
        int separator = args[0].AsSpan().IndexOf('>');
        ReadOnlySpan<char> arg1 = args[0].AsSpan().Slice(0, separator);
        ReadOnlySpan<char> arg2 = args[0].AsSpan().Slice(separator + 1);

        Replace(arg1, arg2);
    }
    else
    {
        Search(args[0]);
    }

    Console.WriteLine("Press any key to leave.");
    Console.ReadKey();
    return 0;
}


void Search(string match)
{
    string dir = Directory.GetCurrentDirectory();
    Regex search = new Regex(match, RegexOptions.Compiled);

    var files = Directory.EnumerateFiles(dir);
    ReadOnlySpan<char> dirName = Directory.GetCurrentDirectory().AsSpan();

    int counter = 0;

    foreach (string file in files)
    {
        ReadOnlySpan<char> fileName = Path.GetFileNameWithoutExtension(file.AsSpan());
        ReadOnlySpan<char> fileExt = Path.GetExtension(file.AsSpan());

        if (search.IsMatch(fileName))
        {
            counter++;
        }
    }
    Console.WriteLine(counter + " files found.");
}

void Replace(ReadOnlySpan<char> matchSpan, ReadOnlySpan<char> replaceSpan)
{
    string match = matchSpan.ToString();
    string replace = replaceSpan.ToString();

    string dir = Directory.GetCurrentDirectory();
    Regex search = new Regex(match.ToString(), RegexOptions.Compiled);

    var files = Directory.EnumerateFiles(dir);
    ReadOnlySpan<char> dirName = Directory.GetCurrentDirectory().AsSpan();

    int counter = 0;

    foreach (string file in files)
    {
        ReadOnlySpan<char> fileName = Path.GetFileNameWithoutExtension(file.AsSpan());
        ReadOnlySpan<char> fileExt = Path.GetExtension(file.AsSpan());

        if (search.IsMatch(fileName))
        {
            string newFileName = search.Replace(fileName.ToString(), replace.ToString());
            string newFile = Path.Join(dirName, $"{newFileName}{fileExt}");

            if (File.Exists(newFile))
            {
                newFile = Path.Join(dirName, $"{newFileName}_{counter}{fileExt}");
                File.Move(file, newFile);
            }
            else

            {
                File.Move(file, newFile);
            }

            counter++;
        }
    }
    Console.WriteLine(counter + " files changed.");
}