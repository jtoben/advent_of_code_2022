namespace Day12
{
    class Program
    {
        static void Main(string[] args)
        {
            var input = File.ReadAllLines("input.txt");

            Vector2 startPosition = null;
            List<Vector2> startPositions = new ();
            Vector2 endPosition = null;

            var heightMap = new int[input[0].Length, input.Length];
            for (int x = 0; x < input[0].Length; x++) {
                for (int y = 0; y < input.Length; y++) {
                    var charToParse = input[y][x];
                    if (charToParse == 'S') {
                        charToParse = 'a';
                        startPosition = new Vector2(x, y);
                    } else if (charToParse == 'E') {
                        charToParse = 'z';
                        endPosition = new Vector2(x, y);
                    }

                    heightMap[x, y] = charToParse - 96;

                    if (charToParse == 'a') {
                        startPositions.Add(new Vector2(x, y));
                    }
                }
            }

            Console.WriteLine(PartOne(heightMap, startPosition, endPosition));
            Console.WriteLine(PartTwo(heightMap, startPositions, endPosition));
        }

        private static int PartOne(int[,] heightMap, Vector2 startPosition, Vector2 endPosition) {
            return FindLengthOfShortestPath(heightMap, new List<Vector2> { startPosition }, endPosition);
        }

        private static int PartTwo(int[,] heightMap, List<Vector2> startPositions, Vector2 endPosition) {
            return FindLengthOfShortestPath(heightMap, startPositions, endPosition);
        }

        private static int FindLengthOfShortestPath(int[,] heightMap, List<Vector2> startPositions, Vector2 endPosition) {
            PriorityQueue<Vector2, int> openSet = new();
            HashSet<Vector2> closedSet = new();
            Dictionary<Vector2, int> distancesByPositions = new();

            foreach (var startPosition in startPositions) {
                openSet.Enqueue(startPosition, 0);
                distancesByPositions.Add(startPosition, 0);
            }

            while (openSet.Count > 0) {
                var current = openSet.Dequeue();
                var currentDistance = distancesByPositions[current];
                if (current == endPosition) {
                    return currentDistance;
                }

                var neighbours = GetNeighbours(heightMap, current, closedSet);
                foreach (var neighbour in neighbours) {
                    if (distancesByPositions.GetValueOrDefault(neighbour, int.MaxValue) > currentDistance + 1) {
                        distancesByPositions[neighbour] = currentDistance + 1;
                        openSet.Enqueue(neighbour, currentDistance + 1);
                    }
                }

                closedSet.Add(current);
            }

            throw new Exception("undefined");
        }

        private static List<Vector2> GetNeighbours(int[,] heightMap, Vector2 position, HashSet<Vector2> closedSet) {
            List<Vector2> neighbours = new();
            var currentHeight = heightMap[position.X, position.Y];

            var offsets = new List<Vector2> {
                new Vector2(0, -1),
                new Vector2(1, 0),
                new Vector2(0, 1),
                new Vector2(-1, 0),
            };

            foreach (var offset in offsets) {
                var newPosition = new Vector2(position.X + offset.X, position.Y + offset.Y);

                if (!closedSet.Contains(newPosition) 
                    && (newPosition.X >= 0 && newPosition.X < heightMap.GetLength(0))
                    && (newPosition.Y >= 0 && newPosition.Y < heightMap.GetLength(1))) {
                    
                    var newHeight = heightMap[newPosition.X, newPosition.Y];
                    if (newHeight - currentHeight <= 1) {
                        neighbours.Add(newPosition);
                    }
                }
            }

            return neighbours;
        }

        private record Vector2 (int X, int Y) {
            public static Vector2 operator +(Vector2 a, Vector2 b) => new Vector2(a.X + b.X, a.Y + b.Y);
        }
    }
}