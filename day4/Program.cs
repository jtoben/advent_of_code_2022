namespace Day4
{
    class Program
    {
        static void Main(string[] args)
        {
            var patrolAssignments = File.ReadAllLines("input.txt")
                .Select(x => x.Split(","))
                .Select(x => (First: PatrolAssignment.FromString(x[0]), Second: PatrolAssignment.FromString(x[1])));
            
            Console.WriteLine(PartOne(patrolAssignments));
            Console.WriteLine(PartTwo(patrolAssignments));
        }

        private static int PartOne(IEnumerable<(PatrolAssignment First, PatrolAssignment Second)> patrolAssignments) {
            return patrolAssignments.Count(x => 
                (x.First.Min <= x.Second.Min && x.First.Max >= x.Second.Max)
                || 
                (x.First.Min >= x.Second.Min && x.First.Max <= x.Second.Max));
        }

        private static int PartTwo(IEnumerable<(PatrolAssignment First, PatrolAssignment Second)> patrolAssignments) {
            return patrolAssignments.Count(x => {
                var highestMin = Math.Max(x.First.Min, x.Second.Min);
                var lowestMax = Math.Min(x.First.Max, x.Second.Max);

                return highestMin <= lowestMax;
            });
        }

        private record PatrolAssignment() {

            public required int Min {get; init;}
            public required int Max {get;init; }

            public static PatrolAssignment FromString(string assignmentAsString) {
                var numbers = assignmentAsString
                    .Split("-")
                    .Select(int.Parse);

                return new PatrolAssignment {
                    Min = numbers.Min(),
                    Max = numbers.Max()
                };
            }
        }
    }
}