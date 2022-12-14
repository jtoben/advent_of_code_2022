namespace Day13
{
    class Program
    {
        static void Main(string[] args)
        {
            var packetPairs = File.ReadAllText("input.txt")
                .Split(new string[] { Environment.NewLine + Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries)
                .Select(x => x.Split(Environment.NewLine));

            var allPackets = File.ReadAllText("input.txt")
                .Replace(Environment.NewLine + Environment.NewLine, Environment.NewLine)
                .Split(Environment.NewLine)
                .ToList();
            
            Console.WriteLine(PartOne(packetPairs));
            Console.WriteLine(PartTwo(allPackets));
        }

        private static int PartOne(IEnumerable<string[]> packetPairs) {
            List<int> results = new();
            foreach (var packetPair in packetPairs) {
                var result = Compare(packetPair[0], packetPair[1]);

                results.Add(result);
            }

            return results.Select((r, i) => r == 1 ? i + 1 : 0).Sum();
        }

        

        private static int PartTwo(List<string> packets) {
            string dividerPackageA = "[[2]]";
            string dividerPackageB = "[[6]]";

            packets.Add(dividerPackageA);
            packets.Add(dividerPackageB);

            List<string> sortedPackets = new() { packets[0] };
            for (int i = 1; i < packets.Count; i++) {
                for (int j = 0; j < sortedPackets.Count; j++) {
                    var packageToCompare = packets[i];
                    var sortedPackage = sortedPackets[j];

                    var result = Compare(packageToCompare, sortedPackage);

                    if (result == -1) {
                        sortedPackets.Insert(j, packageToCompare);
                        break;
                    } else if (j == sortedPackets.Count - 1) {
                        sortedPackets.Add(packageToCompare);
                        break;
                    }
                }
            }

            sortedPackets.Reverse();

            var indexA = sortedPackets.IndexOf(dividerPackageA) + 1;
            var indexB = sortedPackets.IndexOf(dividerPackageB) + 1;

            return indexA * indexB;
        }

        private static int Compare(string partA, string partB) {
            if (partA == "" && partB == "") {
                return 0;
            } else if (partA == "") {
                return 1;
            } else if (partB == "") {
                return -1;
            }

            var nextItemA = GetNextItem(partA);
            var nextItemB = GetNextItem(partB);

            var aIsInteger = nextItemA.Number != -1;
            var bIsInteger = nextItemB.Number != -1;

            if (aIsInteger && bIsInteger) {
                if (nextItemA.Number < nextItemB.Number) {
                    return 1;
                }
                if (nextItemA.Number > nextItemB.Number) {
                    return -1;
                }
            } else if (!aIsInteger && !bIsInteger) {

                var result =  Compare(nextItemA.Range, nextItemB.Range);
                if (result != 0) {
                    return result;
                }
            } else if (aIsInteger) {
                string newPartA = $"{nextItemA.Number}";

                var result = Compare(newPartA, nextItemB.Range);
                if (result != 0) {
                    return result;
                }
            } else {
                string newPartB = $"{nextItemB.Number}";

                var result = Compare(nextItemA.Range, newPartB);
                if (result != 0) {
                    return result;
                }
            }

            var noNextA = partA.Length < nextItemA.Length + 1;
            var noNextB = partB.Length < nextItemB.Length + 1;

            if (noNextA && noNextB) {
                return 0;
            } else if (noNextA) {
                return 1;
            } else if (noNextB) {
                return -1;
            }

            var newA = partA[(nextItemA.Length + 1)..];
            var newB = partB[(nextItemB.Length + 1)..];

            return Compare(newA, newB);
        }

        private static (int Number, string Range, int Length) GetNextItem(string part) {
            if (part[0] == '[') {
                var range = GetOuterRange(part);
                return (-1, part[range], part[range].Length + 2);
            } else {
                for (int i = 1; i < part.Length; i++) {
                    if (part[i] == ',' || part[i] == ']') {
                        return (int.Parse(part.Substring(0, i)), "", part.Substring(0, i).Length);
                    }
                }
            }
            return (int.Parse(part), "", part.Length);

            throw new Exception("undefined");
        }

        private static Range GetOuterRange(string packet) {
            int start = -1, end = -1;

            int bracketCounter = 0;
            for (int i = 0; i < packet.Length; i++) {
                if (packet[i] == '[') {
                    if (bracketCounter == 0) {
                        start = i + 1;
                    }
                    bracketCounter++;
                } else if (packet[i] == ']') {
                    bracketCounter--;
                    if (bracketCounter == 0) {
                        end = i;
                        break;
                    }
                }
            }

            return new Range(start, end);
        }
    }
}