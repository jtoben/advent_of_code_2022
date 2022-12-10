namespace Day8
{
    class Program
    {
        static void Main(string[] args)
        {
            var input = File.ReadAllLines("input.txt");

            var forestMap = new int[input[0].Length, input.Length];
            for (int x = 0; x < input[0].Length; x++) {
                for (int y = 0; y < input.Length; y++) {
                    forestMap[x, y] = int.Parse(input[y][x].ToString());
                }
            }

            Console.WriteLine(PartOne(forestMap));
            Console.WriteLine(PartTwo(forestMap));
        }

        private static int PartOne(int[,] forestMap) {
            int visibleTreeCount = 0;
            for (int x = 1; x < forestMap.GetLength(0) - 1; x++) {
                for (int y = 1; y < forestMap.GetLength(1) - 1; y++) {
                    if (isTreeVisible(forestMap, (x, y))) {
                        visibleTreeCount++;
                    }
                }
            }

            return visibleTreeCount + (forestMap.GetLength(0) * 2) + (forestMap.GetLength(1) * 2) - 4;
        }

        private static int PartTwo(int[,] forestMap) {
            List<(int X, int Y)> visibleTreeLocations = new();
            for (int x = 1; x < forestMap.GetLength(0) - 1; x++) {
                for (int y = 1; y < forestMap.GetLength(1) - 1; y++) {
                    if (isTreeVisible(forestMap, (x, y))) {
                        visibleTreeLocations.Add((x, y));
                    }
                }
            }

            List<int> scenicScores = new();

            foreach (var treeLocation in visibleTreeLocations) {
                scenicScores.Add(GetScenicScoreForLocation(forestMap, treeLocation));
            }

            return scenicScores.Max();
        }

        private static bool isTreeVisible(int[,] forestMap, (int X, int Y) location) {
            foreach (var direction in Enum.GetValues<Direction>()) {
                if (CheckVisibilityForDirection(forestMap, location, direction)) {
                    return true;
                };
            }
            return false;
        }

        private static bool CheckVisibilityForDirection(int[,] forestMap, (int X, int Y) location, Direction direction) {
            var treeHeight = forestMap[location.X, location.Y];
            var trees = GetTreesForDirection(forestMap, location, direction);

            return trees.Count(x => x < treeHeight) == trees.Count;
        }

        private static List<int> GetTreesForDirection(int[,] forestMap, (int X, int Y) location, Direction direction) {
            List<int> trees = new();
            
            var offset = GetOffsetForDirection(direction);
            do {
                location = (location.X + offset.X, location.Y + offset.Y);
                trees.Add(forestMap[location.X, location.Y]);
            } while (location.X > 0 && location.X < forestMap.GetLength(0) - 1 && location.Y > 0 && location.Y < forestMap.GetLength(1) - 1);

            return trees;
        }

        private static (int X, int Y) GetOffsetForDirection(Direction direction) {
            return direction switch {
                Direction.Up => (0, -1),
                Direction.Right => (1, 0),
                Direction.Down => (0, 1),
                Direction.Left => (-1, 0),
                _ => throw new Exception("undefined")
            };
        }

        private static int GetScenicScoreForLocation(int[,] forestMap, (int X, int Y) location) {
             var treeHeight = forestMap[location.X, location.Y];
             
             List<int> treeCounts = new();
             foreach (var direction in Enum.GetValues<Direction>()) {
                var trees = GetTreesForDirection(forestMap, location, direction);

                int count = 0;
                for (int i = 0; i < trees.Count; i++) {
                    count++;

                    if (treeHeight <= trees[i]) {
                        break;
                    }
                }

                treeCounts.Add(count);
            }

            return treeCounts.Aggregate((a, b) => a * b);
        }

        private enum Direction {
            Up,
            Right,
            Down,
            Left
        }
    }
}