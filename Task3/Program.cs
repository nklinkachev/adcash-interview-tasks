using System;
using AdCash_Tasks;

namespace Task3 {
    class Program {
        static void Main(string[] args) {
            var solver = new Task3Solver();
            solver.ParseInput(string.Join(Environment.NewLine,args));
            int path = solver.Solve();
            if (path > 0) {
                Console.WriteLine(path);
            }
            else {
                Console.WriteLine("No Path");
            }
            
        }
    }
}