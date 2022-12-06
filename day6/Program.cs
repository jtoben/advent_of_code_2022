namespace Day6
{
    class Program
    {
        static void Main(string[] args)
        {
            var dataStream = File.ReadAllText("input.txt");
            
            Console.WriteLine(PartOne(dataStream));
            Console.WriteLine(PartTwo(dataStream));
        }

        private static int PartOne(string dataStream) {
            return GetStartOfMarker(dataStream, 4);
        }

        private static int PartTwo(string dataStream) {
            return GetStartOfMarker(dataStream, 14);
        }

        private static int GetStartOfMarker(string dataStream, int numberOfCharactersToCheck) {

            for (int i = numberOfCharactersToCheck - 1; i < dataStream.Length; i++) {
                var buffer = dataStream.Substring(i - numberOfCharactersToCheck + 1, numberOfCharactersToCheck);

                bool hasDuplicates = buffer.GroupBy(x => x).Count() < numberOfCharactersToCheck;
                if (!hasDuplicates) {
                    return i + 1;
                }
            }

            throw new Exception("undefined");
        }
    }
}