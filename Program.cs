using System.Text.RegularExpressions;

//faut prog.exe --% arg1 arg2 pour forcer powershell sur les "" :<

if(args.Length < 2)
{
    Console.WriteLine("Error: Requires 2 arguments [search] [replace]");
    return 1;
}

string match = args[0];
string replace = args[1];


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
        string newFileName = search.Replace(fileName.ToString(), replace);
        string newFile = Path.Join(dirName, $"{newFileName}{fileExt}");

        File.Move(file, newFile);

        counter++;
    }
}
Console.WriteLine(counter+" files changed.");
Console.WriteLine("Press any key to leave.");
Console.ReadKey();
return 0;
