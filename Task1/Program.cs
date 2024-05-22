using System;

namespace AdCash_Tasks {
    public class Program {
        public static void Main(string[] args) {
            long n = long.Parse(args[0]);
            Console.WriteLine(new Task1Solver().Solve(n));
        }
    }
}