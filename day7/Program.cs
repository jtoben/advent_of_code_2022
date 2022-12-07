namespace Day7
{
    class Program
    {
        static void Main(string[] args)
        {
            var terminalHistory = System.IO.File.ReadAllLines("input.txt");
            
            Console.WriteLine(PartOne(terminalHistory));
            Console.WriteLine(PartTwo(terminalHistory));
        }

        private static int PartOne(string[] terminalHistory) {
            var rootDirectory = BuildDirectoryFromTerminalHistory(terminalHistory);
            var allDirectories = GetAllDirectories(rootDirectory);

            return allDirectories.Where(x => x.Size <= 100_000).Sum(x => x.Size);
        }

        private static int PartTwo(string[] terminalHistory) {
            var rootDirectory = BuildDirectoryFromTerminalHistory(terminalHistory);
            var allDirectories = GetAllDirectories(rootDirectory);

            var unusedSpace = 70_000_000 - rootDirectory.Size;
            var sizeToDelete = 30_000_000 - unusedSpace;
            
            return allDirectories.OrderBy(x => x.Size).First(x => x.Size >= sizeToDelete).Size;
        }

        private static Directory BuildDirectoryFromTerminalHistory(string[] terminalHistory) {
            Directory currentDirectory = Directory.FromString("dir ", parent: null);

            for (int i = 1; i < terminalHistory.Length; i++) {
                var line = terminalHistory[i];

                var command = line.Split(" ", count: 2)[1];
                if (command.StartsWith("cd")) {
                    if (command.Contains("..")) {
                        currentDirectory = currentDirectory.Parent ?? currentDirectory;
                    } else {
                        currentDirectory = currentDirectory.Directories.Single(x => x.Name == command.Split(" ")[1]);
                    }
                } else {
                    var listedFilesAndDirectories = terminalHistory.Skip(i + 1).TakeWhile(x => !x.StartsWith('$'));
                    foreach (var fileOrDirectory in listedFilesAndDirectories) {
                        if (fileOrDirectory.StartsWith("dir")) {
                            var newDirectory = Directory.FromString(fileOrDirectory, currentDirectory);
                            if (currentDirectory.Directories.Count(x => x.Name == newDirectory.Name) == 0) {
                                currentDirectory.Directories.Add(newDirectory);
                            }
                        } else {
                            var newFile = File.FromString(fileOrDirectory);
                            if (currentDirectory.Files.Count(x => x.Name == newFile.Name) == 0) {
                                currentDirectory.Files.Add(newFile);
                            }
                        }
                    }

                    i += listedFilesAndDirectories.Count();
                }
            }

            var rootDirectory = currentDirectory;
            while (rootDirectory.Parent != null) {
                rootDirectory = rootDirectory.Parent;
            }

            return rootDirectory;
        }

        private static List<Directory> GetAllDirectories(Directory rootDirectory) {
            var directories = new List<Directory>();
            directories.Add(rootDirectory);

            foreach (var directory in rootDirectory.Directories) {
                directories.AddRange(GetAllDirectories(directory));
            }

            return directories;
        }

        private record Directory {
            public List<Directory> Directories { get; init; } = new();
            public List<File> Files { get; init; } = new();
            public required string Name { get; init; }
            public required Directory Parent {get; init; }
            public int Size => Directories.Sum(x => x.Size) + Files.Sum(x => x.Size);

            public static Directory FromString(string directoryAsString, Directory parent) {
                var parts = directoryAsString.Split(" ");

                return new Directory {
                    Name = parts[1],
                    Parent = parent
                };
            }
        }

        private record File {
            public required int Size { get; init; }
            public required string Name { get; init; }

            public static File FromString(string fileAsString) {
                var parts = fileAsString.Split(" ");

                return new File {
                    Size = int.Parse(parts[0]),
                    Name = parts[1]
                };
            }
        }
    }
}