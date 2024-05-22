using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace AdCash_Tasks {
    /// <summary>
    /// The final implemented solution for this task is a simple greedy algorithm with
    /// runtime complexity if O(logN) and memory complexity of O(1) (for an upper bound on N of 64 bit integer
    /// as discussed in an email).
    /// The proof of why this is the optimal solution is a bit involved and is given below.
    /// The greedy algorithm is:
    ///     1. If the number is 1 the algorithm is done.
    ///     2. If the number is 3 subtract 1. This is a corner case where the greedy heuristic fails.
    ///        This is due to 2 being a power of 2 but not divisible by 4. for larger numbers this is never the case.
    ///     3. If the number is even, divide it by two.
    ///     4. if the number is odd, it is between two even numbers, one of which is divisible by 4.
    ///        Add or subtract one to make the number divisible by 4.
    ///
    /// With the current implementation the algorithm will work for all numbers taht fit in a standard signed 64-bit integer
    /// except for the two (positive and negative) max values as an intermediate step in the algorithm overflows
    /// I've left the implementation as is to preserve the simplicity of the greedy algorithm. It can be reworked slightly
    /// to prevent the overflow, or a BigInteger value type can be used to support arbitrary large numbers of N.
    ///  
    /// We'll look the provided numeber and the operations in binary. We want the most significant bit of the number
    /// to be at position 0 or the rightmost position.
    /// There's a few observations about this problem.
    ///     1. For numbers that are not powers of 2 only the division operation shifts the most significant bit by one.
    ///        For powers of 2 both substraction and division shift the most significant bit by one.
    ///        That gives a lower bound of the required operations of logN.
    ///     2. It is not optimal to add 1 and then subtract 1 from a number if we haven't divided the number by 2
    ///        between those two steps as they cancel out.
    ///     3. Adding or subtracting multiple times from a number and then dividing by 2 is not optimal.
    ///        There's 4 cases adding/subtracting for an even number and adding/subtracting for odd number.
    ///        I'll provide explanation for one case as all 4 are very similar in nature.
    ///        If we have an even number, adding 2 to it and then dividing by 2 is equivalent to diving by 2 and adding 1.
    ///        In general adding 2k to the number and then dividing by 2 is equivalent to dividign by 2 and adding k.
    ///        In the case of multiple additions before division more operations were used to arrive at the same number.
    ///        Thus, it's more optimal to divide first if the number is even and only then add or subtract.
    ///        If we have an odd number, adding or subtracting one puts us at the case for even number.
    ///  
    ///     
    /// With these observations my first solution was a recursive algorithm:
    /// If we have a function SolveRecursive(N) that returns the optimal number of steps for a number N, then it would look something like:
    ///     SolveRecursive(1) = 0
    ///     SolveRecursive(N) = SolveRecursive(N/2) + 1 when n is even
    ///     SolveRecursive(N) = Min( Solve(N-1), SolveRecursive(N+1) ) + 1
    ///
    /// With a cache/memoization HashTable the algorithm has runtime complexity of O(logN) and memory complexity of O(logN)
    /// 
    /// It is not obvious at first glance that this recursive algorithm will ever complete as the branch
    /// SolveRecursive(N+1) increases the starting number. However, it is guaranteed to divide the number by 2 in the very
    /// next step which will always make the number lower than the starting N (for numbers > 1, and for 1 this branch is
    /// not applied)
    ///
    /// Runtime and memory complexity of O(logN) is acceptable for integers up to 64 bits.
    /// Since there was no upper bound specified on the integer in the task definition I looked to find a solution that
    /// uses O(1) memory. That way the algorithm could process an arbitrary large number if it can be stored in memory.
    /// A greedy algorithm aproach would fit the requirement.
    /// My initial (and wrong) conjecture was that we can divide by 2 when the number is even, and when it is odd we can add
    /// or subtract 1 towards the nearest power of 2. Comparing the results of this algorithm to the recursive one it
    /// quickly became obvious it is wrong. A counterexample would be the numebr 13.
    ///
    /// Gettign back to the observations, it is only the division operation that shifts the most significant bit down by 1 place.
    /// We need to shift the most significant bit logN (rounded up) times. That fact combined with the fact that it's always
    /// optimal to divide when the number is even means for any number N the optimal solution includes exactly logN (rounded up)
    /// division operations. The greedy algorithm should optimize the number of additions and subtractions
    ///
    /// Looking at the end of the number, if it ends in a group of M 1s: 
    ///     If M=1, The most efficient way to "remove" the 1 would be to subtract one and then divide by 2.
    ///         The division operations are a fixed number for the whole number, and this process only uses 1 subtraction
    ///         or division.
    ///     If M>1, the group of 1s can be "removed" by adding 1 to the number, then dividing M times, and subtracting one.
    ///         In case the group of 1s is only preceeded by a single 0, the subtraction is not needed as the resulting
    ///         new 1 will be bundled with the precedeing 1s (...1101111 would turn to ...1110000).
    ///         In this case the required number of additions and subtractions is 1.
    ///     
    /// The greedy algorithm is based on this heuristic which holds for each number >3. 3 is a special case where
    /// subtracting 1 to get to the number 2 is better than adding 1 to make the number divisible by 4.
    /// Thus, the greedy algorithm should produce optimal solution.
    ///
    /// As a sanity check I ran both the recursive algorithm and the greedy algorithm up to N=1000000
    /// They both agree for each value of N.
    /// </summary>
    public class Task1Solver {
        public int Solve(long n) {
            int steps = 0;
            if (n < 0) {
                steps += 2;
                n *= -1;
            }
            while (n != 1 && n!=3) {
                // using bitwise operations as they are generally faster than modulus and division.
                if ((n & 0b1) == 0) {
                    // if divisible by 2 divide by 2
                    n >>= 1;
                }
                // if N is not divisible by 2 we make the number divisible by 4 by adding or subtracting 1
                else if ((n & 0b11) == 1) {
                    // num%4 == 1
                    n -= 1;
                }
                else {
                    // num%4==3
                    n += 1;
                }

                steps += 1;
            }

            if (n == 3) {
                steps += 2;
            }
            return steps;
        }

        public int SolveRecursive(int n, ref Dictionary<int,int> mem) {
            if (n == 1) {
                return 0;
            }

            if (!mem.ContainsKey(n)) {
                if (n % 2 == 0) {
                    mem[n] = SolveRecursive(n / 2, ref mem) + 1;
                }
                else {
                    mem[n]= Math.Min(SolveRecursive(n - 1, ref mem), SolveRecursive(n + 1, ref mem)) + 1;
                }
            }

            return mem[n];
        }
    }
}