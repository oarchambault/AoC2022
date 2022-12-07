using System;
using System.Collections.Generic;
using System.Linq;

var terminalOutputs = System.IO.File.ReadAllLines("input.txt");

FileSystem fileSystem = new();

foreach (var output in terminalOutputs)
{
    var split = output.Split(' ');
    if (output == "$ cd /" || output == "$ ls" || output.StartsWith("dir"))
    {
        continue;
    }
    else if (output == "$ cd ..")
    {
        fileSystem.MoveToParent();
    }
    else if (output.StartsWith("$ cd"))
    {
        fileSystem.MoveToDirectory(split[2]);
    }
    else
    {
        fileSystem.AddFileToCurrentDirectory(split[1], int.Parse(split[0]));
    }
}

var smallerDirectories = fileSystem.AllDirectories.Where(d => d.Size < 100000);
Console.WriteLine($"Total size for small directories is {smallerDirectories.Sum(d => d.Size)}");
Console.WriteLine();

Console.WriteLine($"Total space: {FileSystem.TotalSpace:n0}");
Console.WriteLine($"Space required for update: {FileSystem.SpaceRequiredForUpdate:n0}");
Console.WriteLine($"Space used: {fileSystem.TotalUsedSpace:n0}");
Console.WriteLine($"Space unused: {fileSystem.TotalUnusedSpace:n0}");
var spaceToBeFreed = fileSystem.TotalUnusedSpace - FileSystem.SpaceRequiredForUpdate;
Console.WriteLine($"Space to be freed: {spaceToBeFreed:n0}");

var directories = fileSystem.AllDirectories
    .Where(d => d.Size >= spaceToBeFreed)
    .OrderBy(d => d.Size)
    .ToList();

var directoryToBeDeleted = directories.First();

Console.WriteLine($"We can delete directory '{directoryToBeDeleted.Name}' with size {directoryToBeDeleted.Size:n0}");

internal class FileSystem
{
    public const int TotalSpace = 70_000_000;
    public const int SpaceRequiredForUpdate = 30_000_000;

    public FileSystem()
    {
        Root = new Directory("/");
        CurrentDirectory.Push(Root);
    }

    public Directory Root { get; set; }

    public Stack<Directory> CurrentDirectory { get; set; } = new();

    public List<Directory> AllDirectories { get; } = new();

    public int TotalUsedSpace => Root.Size;

    public int TotalUnusedSpace => TotalSpace - TotalUsedSpace;

    internal void MoveToParent()
    {
        var directory = CurrentDirectory.Pop();
        AllDirectories.Add(directory);
    }

    internal void MoveToDirectory(string name)
    {
        var directory = new Directory(name);
        CurrentDirectory.Peek().AddItem(directory);
        CurrentDirectory.Push(directory);
    }

    internal void AddFileToCurrentDirectory(string name, int size)
    {
        CurrentDirectory.Peek().AddItem(new File(name, size));
    }
}

internal interface IFileSystemItem
{
    public string Name { get; }
    public int Size { get; }
}

internal class Directory : IFileSystemItem
{
    private readonly List<IFileSystemItem> content = new();

    private int? savedSize = null;

    public Directory(string name)
    {
        Name = name;
    }

    public string Name { get; }

    public int Size
    {
        get
        {
            if (savedSize == null)
            {
                savedSize = content.Sum(i => i.Size);
            }
            return savedSize.Value;
        }
    }

    public IEnumerable<IFileSystemItem> Content => content;

    public void AddItem(IFileSystemItem item) => content.Add(item);
}

internal record File(string Name, int Size) : IFileSystemItem;

