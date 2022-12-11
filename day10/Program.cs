namespace Day10
{
    class Program
    {
        static void Main(string[] args)
        {
            var instructions = File.ReadAllLines("input.txt")
                .Select(Instruction.FromString);
            
            Console.WriteLine(PartOne(new Queue<Instruction>(instructions)));
            PartTwo(new Queue<Instruction>(instructions));
        }

        private static int PartOne(Queue<Instruction> instructions) {
            int x = 1;
            var instruction = instructions.Dequeue();

            List<int> importantCycleValues = new();
            
            for (int cycle = 1; cycle <= 220; cycle++) {
                if (cycle % 40 == 20) {
                    importantCycleValues.Add(cycle * x);
                }

                instruction = instruction with { Duration = instruction.Duration - 1 };
                if (instruction.Duration == 0) {
                    x += instruction.Amount;
                    instruction = instructions.Dequeue();
                }
            }

            return importantCycleValues.Sum();
        }

        private static void PartTwo(Queue<Instruction> instructions) {
            int x = 1;
            var instruction = instructions.Dequeue();
            var pixels = new [] {
                new char[40],
                new char[40],
                new char[40],
                new char[40],
                new char[40],
                new char[40]
            };

            for (int cycle = 1; cycle <= 240; cycle++) {
                var row = (cycle - 1) / 40;
                var column = (cycle - 1) % 40;
                pixels[row][column] = Math.Abs(x - column) <= 1 ? '#' : '.';

                if (cycle == 240) {
                    break;
                }

                instruction = instruction with { Duration = instruction.Duration - 1 };
                if (instruction.Duration == 0) {
                    x += instruction.Amount;
                    instruction = instructions.Dequeue();
                }
            }

            for (int i = 0; i < pixels.Length; i++) {
                for (int j = 0; j < pixels[i].Length; j++) {
                    Console.Write(pixels[i][j]);
                }
                Console.WriteLine();
            }
        }

        private record Instruction(int Duration, int Amount) {

            public static Instruction FromString(string instructionAsString) {
                var parts = instructionAsString.Split(" ");

                int duration = parts[0] == "noop" ? 1 : 2;
                int amount = parts[0] == "noop" ? 0 : int.Parse(parts[1]);

                return new Instruction(duration, amount);
            }
        }
    }
}