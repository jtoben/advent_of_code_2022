namespace Day14
{
    class Program
    {
        private static readonly Vector2 _sandSpawnLocation = new Vector2(500, 0);

        static void Main(string[] args)
        {
            var rockPaths = File.ReadAllLines("input.txt")
                .Select(line => {
                    var points = line.Split(" -> ");

                    return points.Select(point => {
                        var coordinates = point.Split(',');
                        return new Vector2(int.Parse(coordinates[0]), int.Parse(coordinates[1]));
                    });
                }
                );
            
            Console.WriteLine(PartOne(rockPaths));
            Console.WriteLine(PartTwo(rockPaths));
        }

        private static int PartOne(IEnumerable<IEnumerable<Vector2>> rockPaths) {
            var grid = CreateGrid(rockPaths);
            var startCoordinate = GetStartCoordinate(rockPaths);
            for (int sandCount = 0; sandCount < int.MaxValue; sandCount++) {
                try {
                    var currentCoordinate = startCoordinate;
                    var nextCoordinate = GetNextCoordinate(startCoordinate, grid);

                    while(currentCoordinate != nextCoordinate) {
                        currentCoordinate = nextCoordinate;
                        nextCoordinate = GetNextCoordinate(currentCoordinate, grid);
                    }

                    grid[nextCoordinate.X, nextCoordinate.Y] = 'o';
                } catch (IndexOutOfRangeException) {
                    return sandCount;
                }
            }

            throw new Exception("undefined");
        }

        private static int PartTwo(IEnumerable<IEnumerable<Vector2>> rockPaths) {
            var grid = CreateGrid(rockPaths, addFloor: true);
            var startCoordinate = GetStartCoordinate(rockPaths);
            
            for (int sandCount = 0; sandCount < int.MaxValue; sandCount++) {
                try {
                    var currentCoordinate = startCoordinate;
                    var nextCoordinate = GetNextCoordinate(startCoordinate, grid);

                    if (currentCoordinate == nextCoordinate) {
                        return sandCount + 1;
                    }

                    while(currentCoordinate != nextCoordinate) {
                        currentCoordinate = nextCoordinate;
                        nextCoordinate = GetNextCoordinate(currentCoordinate, grid);
                    }

                    grid[nextCoordinate.X, nextCoordinate.Y] = 'o';
                } catch (IndexOutOfRangeException) {
                    var oldWidth = grid.GetLength(0);
                    grid = DoubleGridHorizontally(grid);
                    var newWidth = grid.GetLength(0);
                    var halfDifference = (newWidth - oldWidth) / 2;
                    startCoordinate = new Vector2(startCoordinate.X + halfDifference, startCoordinate.Y);
                    sandCount--;
                    continue;
                }
            }

            throw new Exception("undefined");
        }

        private static Vector2 GetNextCoordinate(Vector2 currentCoordinate, char[,] grid) {
            if (grid[currentCoordinate.X, currentCoordinate.Y + 1] == '.') {
                return new Vector2(currentCoordinate.X, currentCoordinate.Y + 1);
            }
            if (grid[currentCoordinate.X - 1, currentCoordinate.Y + 1] == '.') {
                return new Vector2(currentCoordinate.X - 1, currentCoordinate.Y + 1);
            }
            if (grid[currentCoordinate.X + 1, currentCoordinate.Y + 1] == '.') {
                return new Vector2(currentCoordinate.X + 1, currentCoordinate.Y + 1);
            }

            return currentCoordinate;
        }

        private static char[,] CreateGrid(IEnumerable<IEnumerable<Vector2>> rockPaths, bool addFloor = false) {
            var edgeCoordinates = GetEdgeCoordinates(rockPaths);
            int lowestXCoordinate = edgeCoordinates[0];
            int lowestYCoordinate = edgeCoordinates[1];
            int highestXCoordinate = edgeCoordinates[2];
            int highestYCoordinate = edgeCoordinates[3];
            
            var grid = new char[highestXCoordinate - lowestXCoordinate + 1, highestYCoordinate - lowestYCoordinate + 1 + (addFloor ? 2 : 0)];
            for (int x = 0; x < grid.GetLength(0); x++) {
                for (int y = 0; y < grid.GetLength(1); y++) {
                    grid[x, y] = '.';

                    if (_sandSpawnLocation.X - lowestXCoordinate == x && _sandSpawnLocation.Y - lowestYCoordinate == y) {
                        grid[x, y] = '+';
                    }

                    if (addFloor) {
                        if (y == grid.GetLength(1) - 1) {
                            grid[x, y] = '#';
                        }
                    }
                }
            }

            foreach (var rockPath in rockPaths) {
                Vector2 previousPoint = null;
                foreach (var point in rockPath) {
                    if (previousPoint != null) {
                        var xDifference = point.X - previousPoint.X;
                        var yDifference = point.Y - previousPoint.Y;

                        if (xDifference != 0) {
                            xDifference = Math.Abs(xDifference);
                            var lowestX = point.X < previousPoint.X ? point.X : previousPoint.X;

                            for (int i = 0; i <= xDifference; i++) {
                                var x = lowestX - lowestXCoordinate + i;
                                var y = point.Y - lowestYCoordinate;
                                grid[x, y] = '#';
                            }
                        } else {
                            yDifference = Math.Abs(yDifference);
                            var lowestY = point.Y < previousPoint.Y ? point.Y : previousPoint.Y;

                            for (int i = 0; i <= yDifference; i++) {
                                var x = point.X - lowestXCoordinate;
                                var y = lowestY - lowestYCoordinate + i;
                                grid[x, y] = '#';
                            }
                        }
                    }
                    previousPoint = point;
                }
            }

            return grid;
        }

        private static char[,] DoubleGridHorizontally(char[,] grid) {
            var newSize = grid.GetLength(0) * 2;
            var extraPerSize = grid.GetLength(0) / 2;

            var newGrid = new char[newSize, grid.GetLength(1)];

            for (int x = 0; x < newGrid.GetLength(0); x++) {
                for (int y = 0; y < newGrid.GetLength(1); y++) {
                    newGrid[x, y] = '.';

                    if (y == newGrid.GetLength(1) - 1) {
                        newGrid[x, y] = '#';
                    }

                    if (x >= extraPerSize && x <= grid.GetLength(0) - 1 + extraPerSize) {
                        newGrid[x, y] = grid[x - extraPerSize, y];
                    }
                }
            }

            return newGrid;
        }

        private static Vector2 GetStartCoordinate(IEnumerable<IEnumerable<Vector2>> rockPaths) {
            var edgeCoordinates = GetEdgeCoordinates(rockPaths);
            int lowestXCoordinate = edgeCoordinates[0];
            int lowestYCoordinate = edgeCoordinates[1];

            return new Vector2(_sandSpawnLocation.X - lowestXCoordinate, _sandSpawnLocation.Y - lowestYCoordinate);
        }

        private static int[] GetEdgeCoordinates(IEnumerable<IEnumerable<Vector2>> rockPaths) {
            int lowestXCoordinate, lowestYCoordinate, highestXCoordinate, highestYCoordinate;

            lowestXCoordinate = rockPaths.SelectMany(path => path).Min(point => point.X);
            lowestYCoordinate = rockPaths.SelectMany(path => path).Min(point => point.Y);
            highestXCoordinate = rockPaths.SelectMany(path => path).Max(point => point.X);
            highestYCoordinate = rockPaths.SelectMany(path => path).Max(point => point.Y);

            if (lowestXCoordinate > _sandSpawnLocation.X) {
                lowestXCoordinate = _sandSpawnLocation.X;
            }
            if (highestXCoordinate < _sandSpawnLocation.X) {
                highestXCoordinate = _sandSpawnLocation.X;
            }
            if (lowestYCoordinate > _sandSpawnLocation.Y) {
                lowestYCoordinate = _sandSpawnLocation.Y;
            }
            if (highestYCoordinate < _sandSpawnLocation.Y) {
                highestYCoordinate = _sandSpawnLocation.Y;
            }

            return new int[] {
                lowestXCoordinate,
                lowestYCoordinate,
                highestXCoordinate,
                highestYCoordinate
            };
        }

        private record Vector2 (int X, int Y) {
            public static Vector2 operator +(Vector2 a, Vector2 b) => new Vector2(a.X + b.X, a.Y + b.Y);
        }
    }
}