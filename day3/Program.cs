namespace Day3
{
    class Program
    {
        static void Main(string[] args)
        {
            var rucksacks = File.ReadAllLines("input.txt");

            Console.WriteLine(PartOne(rucksacks));
            Console.WriteLine(PartTwo(rucksacks));
        }

        private static int PartOne(string[] rucksacks) {
            return FindDuplicatesAndSumTheirPriorities(rucksacks.Select(x => new string[] {
                x.Substring(0, x.Length / 2),
                x.Substring(x.Length / 2, x.Length / 2)
            }));
        }

        private static int PartTwo(string[] rucksacks) {
            return FindDuplicatesAndSumTheirPriorities(rucksacks.Chunk(3));
        }

        private static int FindDuplicatesAndSumTheirPriorities(IEnumerable<string[]> rucksacks) {
            return rucksacks
                .Select(x => GetDuplicate(x))
                .Select(x => GetPriority(x))
                .Sum();
        }

        private static char GetDuplicate(string[] strings) {
            IEnumerable<char> duplicateChars = strings[0].ToList();

            for (int i = 1; i < strings.Length; i++) {
                duplicateChars = duplicateChars.Intersect(strings[i]);
            }

            return duplicateChars.First();
        }

        private static int GetPriority(char c) {
            return char.IsLower(c) ? c - 96 : c - 38; // a-z maps to 1-26, A-Z maps to 27-52.
        }
    }
}