namespace Day11
{
    class Program
    {
        static void Main(string[] args)
        {
            var input = File.ReadAllText("input.txt")
                .Split(new string[] { Environment.NewLine + Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries)
                .Select(x => x.Split(Environment.NewLine));
            
            Console.WriteLine(PartOne(input.Select(Monkey.FromString).ToList()));
            Console.WriteLine(PartTwo(input.Select(Monkey.FromString).ToList()));
        }

        private static long PartOne(List<Monkey> monkeys) {
            return CalculateMonkeyBusiness(monkeys, 20, (x) => x / 3L);
        }

        private static long PartTwo(List<Monkey> monkeys) {
            var totalDivisionNumber = monkeys.Aggregate(1L, (a, b) => a * b.DivisionNumber);
            return CalculateMonkeyBusiness(monkeys, 10_000, (x) => x % totalDivisionNumber);
        }

        private static long CalculateMonkeyBusiness(List<Monkey> monkeys, int numberOfRounds, Func<long, long> worryReductionFunction) {
            Dictionary<Monkey, int> itemsInspectedByMonkey = new();
            foreach (var monkey in monkeys) {
                itemsInspectedByMonkey[monkey] = 0;
            }

            for (int _ = 0; _ < numberOfRounds; _++) {
                foreach (var monkey in monkeys) {
                    foreach (var item in monkey.Items) {
                        itemsInspectedByMonkey[monkey]++;

                        var newScore = HandleOperator(item, monkey.Operator, monkey.GetOperationNumber(item));
                        newScore = worryReductionFunction.Invoke(newScore);

                        int newMonkey = newScore % monkey.DivisionNumber == 0 ? monkey.TrueMonkey : monkey.FalseMonkey;
                        monkeys[newMonkey].Items.Add(newScore);
                    }

                    monkey.Items.Clear();
                }
            }

            return itemsInspectedByMonkey
                .Select(x => x.Value)
                .OrderByDescending(x => x)
                .Take(2)
                .Select(x => (long)x)
                .Aggregate((a, b) => a * b);
        }
        
        private static long HandleOperator(long oldNumber, int @operator, long operationNumber) {
            return @operator switch {
                '+' => oldNumber + operationNumber,
                '-' => oldNumber - operationNumber,
                '/' => oldNumber / operationNumber,
                '*' => oldNumber * operationNumber,
                _ => throw new Exception("undefined")
            };
        }

        private record Monkey(List<long> Items, char Operator, string LastOperationPart, int DivisionNumber, int TrueMonkey, int FalseMonkey) {
            public long GetOperationNumber(long oldNumber) {
                if (long.TryParse(LastOperationPart, out long number)) {
                    return number;
                }

                return oldNumber;
            }

            public static Monkey FromString(string[] monkeyAsString) {
                var items = monkeyAsString[1]
                    .Split(": ")[1]
                    .Split(", ")
                    .Select(long.Parse).ToList();

                var operation = monkeyAsString[2]
                    .Split(" = ")[1];
                var @operator = operation.Split(" ")[1];
                var lastOperationPart = operation.Split(" ").Last();

                var divisionNumber = int.Parse(monkeyAsString[3]
                    .Split(" ")
                    .Last());
                var trueMonkey = int.Parse(monkeyAsString[4]
                    .Split(" ")
                    .Last());
                var falseMonkey = int.Parse(monkeyAsString[5]
                    .Split(" ")
                    .Last());

                return new Monkey(items, @operator[0], lastOperationPart, divisionNumber, trueMonkey, falseMonkey);
            }
        }
    }
}