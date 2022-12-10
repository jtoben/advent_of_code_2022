namespace Day9
{
    class Program
    {
        static void Main(string[] args)
        {
            var instructions = File.ReadAllLines("input.txt")
                .Select(x => (Direction: x.Split(" ")[0], Amount: int.Parse(x.Split(" ")[1])));
            
            Console.WriteLine(PartOne(instructions));
            Console.WriteLine(PartTwo(instructions));
        }

        private static int PartOne(IEnumerable<(string Direction, int Amount)> instructions) {
            return CalculateVisitedPositionCountForTail(2, instructions);
        }

        private static int PartTwo(IEnumerable<(string Direction, int Amount)> instructions) {
            return CalculateVisitedPositionCountForTail(10, instructions);
        }

        private static int CalculateVisitedPositionCountForTail(int ropeLength, IEnumerable<(string Direction, int Amount)> instructions) {
            var ropeParts = new List<Vector2>();
            for (int i = 0; i < ropeLength; i++) {
                ropeParts.Add(new Vector2 { X = 0, Y = 0 });
            }

            HashSet<Vector2> visitedTailPositions = new();
            foreach (var instruction in instructions) {
                var offset = GetOffsetForDirection(instruction.Direction);

                for (int i = 0; i < instruction.Amount; i++) {
                    ropeParts[0] += offset;
                    for (int j = 1; j < ropeParts.Count; j++) {
                        var tailOffset = GetOffsetForTail(ropeParts[j - 1], ropeParts[j]);
                        ropeParts[j] += tailOffset;

                        if (j == ropeParts.Count - 1) {
                            visitedTailPositions.Add(ropeParts[j]);
                        }
                    }
                }
            }
            
            return visitedTailPositions.Count();
        }

        private static Vector2 GetOffsetForDirection(string direction) {
            return direction switch {
                "U" => new Vector2 { X = 0, Y = -1},
                "R" => new Vector2 { X = 1, Y = 0},
                "D" => new Vector2 { X = 0, Y = 1},
                "L" => new Vector2 { X = -1, Y = 0},
                _ => throw new Exception("undefined")
            };
        }

        private static Vector2 GetOffsetForTail(Vector2 headPosition, Vector2 tailPosition) {
            var xDistance = Math.Abs(headPosition.X - tailPosition.X);
            var yDistance = Math.Abs(headPosition.Y - tailPosition.Y);

            if (xDistance <= 1 && yDistance <= 1) {
                return new Vector2 { X = 0, Y = 0 };
            }

            xDistance = Math.Clamp(headPosition.X - tailPosition.X, -1, 1);
            yDistance = Math.Clamp(headPosition.Y - tailPosition.Y, -1, 1);

            return new Vector2 { X = xDistance, Y = yDistance };
        }


        private record Vector2 {
            public required int X {get; init; }
            public required int Y {get; init; }

            public static Vector2 operator +(Vector2 a, Vector2 b) => new Vector2 {X = a.X + b.X, Y = a.Y + b.Y};
        }
    }
}