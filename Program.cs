using System.Text.RegularExpressions;


if(args.Length < 2)
{
    Console.WriteLine("Error: Requires 2 arguments [search] [replace]");
    return 1;
}

if (string.IsNullOrWhiteSpace(args[0]) || string.IsNullOrWhiteSpace(args[1]))
{
    Console.WriteLine("Error: arguments can't be empty");
    return 1;
}

string match = args[0];
string replace = args[1];

string dir = Directory.GetCurrentDirectory();
Regex search = new Regex(match, RegexOptions.Compiled);

var files = Directory.EnumerateFiles(dir);
ReadOnlySpan<char> dirName = Directory.GetCurrentDirectory().AsSpan();

foreach (string file in files)
{
    ReadOnlySpan<char> fileName = Path.GetFileNameWithoutExtension(file.AsSpan());
    ReadOnlySpan<char> fileExt = Path.GetExtension(file.AsSpan());

    if (search.IsMatch(fileName))
    {
        string newFileName = search.Replace(fileName.ToString(), replace);

        string newFile = Path.Join(dirName, newFileName, fileExt);
        File.Move(file, newFile);
    }
}

Console.WriteLine("Press any key to leave");
Console.ReadKey();
return 0;
