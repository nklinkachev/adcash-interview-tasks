using System;
using System.Collections.Generic;
using Microsoft.VisualBasic.FileIO;

namespace AdCash_Tasks {
    /// <summary>
    /// A naive approach to the task would be to read all values, store them in an array and use a sorting algorithm
    /// to sort them by bid, then read the top bid id and the 2nd best bid value and return those.
    /// This algorithm will work but it's slower and with higher complexity of O(NlogN) at best. (N-number of bids)
    /// 
    /// A better approach would be to read the values one by one and only keep track of the highest bid id and
    /// the two best bid values, updating them if a new highest or second-highest bid is read.
    /// This approach has a lower complexity of O(N) and a lower runtime. The complexity cannot be reduced further as
    /// it is a hard requirement to read each bid once which already has complexity of O(N)
    /// 
    /// The algorithm can be further improved if there are multiple sources of auction bids.
    /// Those can be processed in parallel and combined at the end, which is not possible if using the approach
    /// of sorting all bid values.
    /// 
    /// For this implementation and values coming from a single file parallelization of the processing wouldn't grant
    /// any performance improvement as the expected performance bottleneck part of the algorithm is reading the data
    /// from disk and not the few in-memory operations.
    ///
    /// </summary>
    public class Task2Solver {
        public void Solve(string path) {

            // Using VisualBasic's csv parser, if needed I can submit a solution that manually parses the file.
            using TextFieldParser csvParser = new TextFieldParser(path);
            csvParser.SetDelimiters(",");

            List<int> bestIds = new List<int>();
            
            // Using decimal instead of float or double to guarantee bid amount accuracy over performance and storage space.
            // decimal type can represent precise amounts for all floating point numbers
            // for practical purposes (up to 28 digits) and is typically used for financial data.
            // The algorithm assumes all bids are positive numbers.
            decimal[] bestBids = new decimal[2];
            bestBids[0] = -1;
            bestBids[1] = -2;

            while (!csvParser.EndOfData) {
                string[] row = csvParser.ReadFields();
                
                // Skip rows with empty id.
                int currentId;
                if (!int.TryParse(row?[0], out currentId)) {
                    continue;
                }

                // Skip rows with empty bid
                decimal currentBid;
                if (!decimal.TryParse(row?[1], out currentBid)) {
                    continue;
                }
                
                UpdateBestValues(currentId, currentBid, ref bestIds, ref bestBids);
            }

            if (bestBids[1] == -1) {
                Console.WriteLine("undefined");
            }
            else {
                foreach (var id in bestIds) {
                    Console.WriteLine("{0}, {1}", id, bestBids[1]);
                }
            }
        }
        
        private void UpdateBestValues(int id, decimal bid, ref List<int> bestIds, ref decimal[] bestBids) {
            // if the bid is 
            if (bid > bestBids[0]) {
                bestIds = new List<int>();
                bestIds.Add(id);
                bestBids[1] = bestBids[0];
                bestBids[0] = bid;
            } else if (bid == bestBids[0]) {
                bestIds.Add(id);
            }else if (bid > bestBids[1]) {
                bestBids[1] = bid;
            }
        }

    }
}