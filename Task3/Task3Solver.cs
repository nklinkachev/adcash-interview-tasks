using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;

namespace AdCash_Tasks {
    /// <summary>
    /// The solution for this tasks consists of first running a breadth first search from the starting square
    /// and computing the shortest path to each reachable square, including the impassable walls.
    /// The second step is to run the same breadth first search from the end square backwards, which computes
    /// the shortest path for each square that the end square is reachable from, including impassable walls.
    ///
    /// Although the task does not require it, this algorithm should work when one of the entrance or exit squares is blocked
    /// 
    /// Runtime and Memory complexity of the algorithm is O(H*W) where H is the height of the matrix and W is the width
    /// </summary>

    public class Task3Solver {

        private List<string> Map;
        private int[,] ForwardPathMap;
        private int[,] BackwardsPathMap;
        private bool[,] VisitedMap;
        private int W;
        private int H;
        
        // This value is used by the algorithm to mean there is no path to a specific square.
        // Given that the map is at most 20x20, no path will be longer than 400.
        private int InfinitePath { get; set; } = int.MaxValue / 3;
        private Queue<Tuple<int, int>> bfsQueue { get; set; }

        private void Fill2dArray<T>(ref T[,] array, T value) {
            // Because Array.Fill doesn't support multi dimensional arrays.
            for (int i = 0; i < array.GetLength(0); i++) {
                for (int j = 0; j < array.GetLength(1); j++) {
                    array[i, j] = value;
                }
            }
        }
        
        public void ParseInput(string matrix) {
            Map = matrix.Split(Environment.NewLine)
                .Where(x=>!string.IsNullOrEmpty(x))
                .ToList();
            
            W = Map.First().Length;
            H = Map.Count;
            
            ForwardPathMap = new int[H, W];
            BackwardsPathMap = new int[H, W];

            Fill2dArray(ref ForwardPathMap, InfinitePath);
            Fill2dArray(ref BackwardsPathMap, InfinitePath);
            VisitedMap = new Boolean[H, W];
        }

        public int Solve() {
            // Forward pass from the entrance to each reachable node.
            bfsQueue = new Queue<Tuple<int, int>>();
            bfsQueue.Enqueue(new Tuple<int, int>(0,0));
            ForwardPathMap[0, 0] = 1;
            VisitedMap[0, 0] = true;
            
            while (bfsQueue.Any()) {
                var coords = bfsQueue.Dequeue();
                int h = coords.Item1;
                int w = coords.Item2;
                
                if (Map[h][w] == '1') {
                    // impassable wall, cannot continue
                    continue;
                }

                // Try to move to neighbors in the 4 cardinal directions.
                if (h - 1 >= 0 && !VisitedMap[h - 1, w]) {
                    ForwardPathMap[h - 1, w] = ForwardPathMap[h, w] + 1;
                    VisitedMap[h - 1, w] = true;
                    bfsQueue.Enqueue(new Tuple<int, int>(h - 1, w));
                }

                if (w - 1 >= 0 && !VisitedMap[h, w - 1]) {
                    ForwardPathMap[h, w - 1] = ForwardPathMap[h, w] + 1;
                    VisitedMap[h, w - 1] = true;
                    bfsQueue.Enqueue(new Tuple<int, int>(h, w - 1));
                }

                if (h + 1 < H && !VisitedMap[h + 1, w]) {
                    ForwardPathMap[h + 1, w] = ForwardPathMap[h, w] + 1;
                    VisitedMap[h + 1, w] = true;
                    bfsQueue.Enqueue(new Tuple<int, int>(h + 1, w));
                }

                if (w + 1 < W && !VisitedMap[h, w + 1]) {
                    ForwardPathMap[h, w + 1] = ForwardPathMap[h, w] + 1;
                    VisitedMap[h, w + 1] = true;
                    bfsQueue.Enqueue(new Tuple<int, int>(h, w + 1));
                }
            }
            
            // Backwards pass from the exit, using the pathmap from the forward pass to compute shortest total distance
            // bfsQueue is clear.
            int shortestPath = InfinitePath;
            Fill2dArray(ref VisitedMap, false);
            bfsQueue.Enqueue(new Tuple<int, int>(H-1,W-1));
            BackwardsPathMap[H - 1, W - 1] = 1;
            while (bfsQueue.Any()) {
                var coords = bfsQueue.Dequeue();
                int h = coords.Item1;
                int w = coords.Item2;

                int currentPath = ForwardPathMap[h, w] + BackwardsPathMap[h, w] - 1;
                if (shortestPath > currentPath) {
                    shortestPath = currentPath;
                }
                if (Map[h][w] == '1') {
                    // impassable wall, cannot continue
                    continue;
                }

                // Try to move to neighbors in the 4 cardinal directions.
                if (h - 1 >= 0 && !VisitedMap[h - 1, w]) {
                    BackwardsPathMap[h - 1, w] = BackwardsPathMap[h, w] + 1;
                    VisitedMap[h - 1, w] = true;
                    bfsQueue.Enqueue(new Tuple<int, int>(h - 1, w));
                }

                if (w - 1 >= 0 && !VisitedMap[h, w - 1]) {
                    BackwardsPathMap[h, w - 1] = BackwardsPathMap[h, w] + 1;
                    VisitedMap[h, w - 1] = true;
                    bfsQueue.Enqueue(new Tuple<int, int>(h, w - 1));
                }

                if (h + 1 < H && !VisitedMap[h + 1, w]) {
                    BackwardsPathMap[h + 1, w] = BackwardsPathMap[h, w] + 1;
                    VisitedMap[h + 1, w] = true;
                    bfsQueue.Enqueue(new Tuple<int, int>(h + 1, w));
                }

                if (w + 1 < W && !VisitedMap[h, w + 1]) {
                    BackwardsPathMap[h, w + 1] = BackwardsPathMap[h, w] + 1;
                    VisitedMap[h, w + 1] = true;
                    bfsQueue.Enqueue(new Tuple<int, int>(h, w + 1));
                }
            }

            if (shortestPath == InfinitePath) {
                shortestPath = -1;
            }

            return shortestPath;
        }
    }
}