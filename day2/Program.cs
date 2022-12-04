namespace Day2
{
    class Program
    {
        static void Main(string[] args)
        {
            var input = File.ReadAllLines("input.txt")
                .Select(x => x.Split(" "))
                .Select(x =>(Opponent: DescriptionToMove(x[0]), MineAsString: x[1]));

            
            Console.WriteLine(PartOne(input));
            Console.WriteLine(PartTwo(input));

        }

        private static int PartOne(IEnumerable<(Move Opponent, string MineAsString)> input) {
            var moves = input.Select(x => (Opponent: x.Opponent, Mine: DescriptionToMove(x.MineAsString)));

            return moves.Select(x => PointsForMove(x) + (int)x.Mine).Sum();
        }

        private static int PartTwo(IEnumerable<(Move Opponent, string MineAsString)> input) {
            var moves = input.Select(x => (Opponent: x.Opponent, Mine: InstructionToMove(x.Opponent, x.MineAsString)));

            return moves.Select(x => PointsForMove(x) + (int)x.Mine).Sum();
        }

        private static int PointsForMove((Move Opponent, Move Mine) moves) {
            if (moves.Opponent == moves.Mine) {
                return 3;
            }

            switch (moves.Opponent) {
                case Move.Rock:
                    return moves.Mine == Move.Paper ? 6 : 0;
                case Move.Paper:
                    return moves.Mine == Move.Scissor ? 6 : 0;
                case Move.Scissor:
                    return moves.Mine == Move.Rock ? 6 : 0;
            }

            return 0;
        }

        private static Move DescriptionToMove(string description) {
            return description switch {
                "A" or "X" => Move.Rock,
                "B" or "Y" => Move.Paper,
                "C" or "Z" => Move.Scissor,
                _ => throw new Exception("undefined")
            };
        }

        private static Move InstructionToMove(Move opponent, string instruction) {
            return instruction switch {
                "X" => GetLosingMove(opponent),
                "Y" => opponent,
                "Z" => GetWinningMove(opponent),
                _ => throw new Exception("undefined")
            };
        }

        private static Move GetWinningMove(Move opponent) {
            return opponent switch {
                Move.Rock => Move.Paper,
                Move.Paper => Move.Scissor,
                Move.Scissor => Move.Rock,
                _ => throw new Exception("undefined")
            };
        }
        
        private static Move GetLosingMove(Move opponent) {
            return opponent switch {
                Move.Rock => Move.Scissor,
                Move.Paper => Move.Rock,
                Move.Scissor => Move.Paper,
                _ => throw new Exception("undefined")
            };
        }

        private enum Move {
            Rock = 1,

            Paper = 2,

            Scissor = 3
        }
    }
}