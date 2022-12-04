namespace Day1
{
    class Program
    {
        static void Main(string[] args)
        {
            var caloriesPerElf = File.ReadAllText("input.txt")
                .Split("\n\n")
                .Select(x => x.Split("\n").Select(int.Parse));
            
            Console.WriteLine(PartOne(caloriesPerElf));
            Console.WriteLine(PartTwo(caloriesPerElf));
        }

        private static int PartOne(IEnumerable<IEnumerable<int>> caloriesPerElf) {
            return caloriesPerElf.Select(x => x.Sum()).Max();
        }

        private static int PartTwo(IEnumerable<IEnumerable<int>> caloriesPerElf) {
            return caloriesPerElf.Select(x => x.Sum()).OrderByDescending(x => x).Take(3).Sum();
        }
    }
}