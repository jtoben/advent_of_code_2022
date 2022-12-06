namespace Day5
{
    class Program
    {
        static void Main(string[] args)
        {
            var input = File.ReadAllText("input.txt")
                .Split(new string[] { Environment.NewLine + Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            
            var stacks = GetStacksFromInput(input[0]);
            var instructions = input[1]
                .Split(Environment.NewLine)
                .Select(Instruction.FromString);

            Console.WriteLine(PartOne(CreateCopy(stacks), instructions));
            Console.WriteLine(PartTwo(CreateCopy(stacks), instructions));
        }

        private static string PartOne(List<Stack<char>> stacks, IEnumerable<Instruction> instructions) {
            foreach (var instruction in instructions) {
                for (int i = 0; i < instruction.NumberToMove; i++) {
                    var charToMove = stacks[instruction.From].Pop();

                    stacks[instruction.To].Push(charToMove);
                }
            }

            return new string(stacks.Select(x => x.Pop()).ToArray());
        }

        private static string PartTwo(List<Stack<char>> stacks, IEnumerable<Instruction> instructions) {
            foreach (var instruction in instructions) {
                var stackToMaintainOrder = new Stack<char>();
                for (int i = 0; i < instruction.NumberToMove; i++) {
                    var charToMove = stacks[instruction.From].Pop();

                    stackToMaintainOrder.Push(charToMove);
                }
                for (int i = 0; i < instruction.NumberToMove; i++) {
                    stacks[instruction.To].Push(stackToMaintainOrder.Pop());
                }
            }

            return new string(stacks.Select(x => x.Pop()).ToArray());
        }

        private static List<Stack<char>> GetStacksFromInput(string input) {
            var stackInputPart = input
                .Split(Environment.NewLine);

            var numberOfStacks = int.Parse(stackInputPart
                .Last()
                .Split(" ", StringSplitOptions.RemoveEmptyEntries)
                .Last());
            
            var stackInput = stackInputPart
                .Take(stackInputPart.Count() - 1)
                .ToList();


            List<Stack<char>> stacks = new();
            for (int i = 0; i < numberOfStacks; i++) {
                stacks.Add(new Stack<char>());

                var index = 1 + (4 * i); // 1, 5, 9 etc.
                for (int j = stackInput.Count - 1; j >= 0; j--) {
                    var charToPush = stackInput[j][index];
                    if (charToPush != ' ') {
                        stacks[i].Push(stackInput[j][index]);
                    }
                }
            }

            return stacks;
        }

        private static List<Stack<char>> CreateCopy(List<Stack<char>> original) {
            var copy = new List<Stack<char>>();
            for (int i = 0; i < original.Count; i++) {
                copy.Add(new Stack<char>(original[i].Reverse()));
            }

            return copy;
        }

        private record Instruction {
            public required int NumberToMove { get; init; }
            public required int From { get; init; }
            public required int To { get; init; }

            public static Instruction FromString(string instructionAsString) {
                var parts = instructionAsString.Split(" ");

                return new Instruction {
                    NumberToMove = int.Parse(parts[1]),
                    From = int.Parse(parts[3]) - 1,
                    To = int.Parse(parts[5]) - 1,
                };
            }
        }
    }
}