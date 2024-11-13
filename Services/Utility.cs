using System.Text.RegularExpressions;
using System.Numerics;

namespace EverybodyCodes.Services
{
    #region Useful Classes
    public class Point
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Value { get; set; }

        public Point() { }

        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }
    }
    #endregion

    // Reminders of useful LINQ functions
    //    * Chunk(n) splits list into list of lists of length n
    //    * Except returns the list of values from the first not in the second
    //    * Intersect returns the list of values in both lists
    //    * Union returns a list of distinct elements between 2 lists
    //    * Zip combine each element of 2 lists 
    //    * Enumerable.Range(a, b) starting at a, increments with each element b times
    //    * Enumerable.Repeat(a, b) repeats a, b times

    /// <summary>
    /// This class holds a handful of custom helper functions to make solving these problems easier
    /// </summary>
    public static class Utility
    {
        #region Array Operations
        /// <summary>
        /// Chunks a list based on a predicate. The element that matches the predicate is not included in a chunk 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="predicate"></param>
        /// <remarks>Ex. ["A", "B", "1", "C", "D", "E"].ChunkByExclusive(x => x == "1") would return a list with [["A", "B"], ["C", "D", "E"]]</remarks>
        /// <returns></returns>
        public static List<List<T>> ChunkByExclusive<T>(this IEnumerable<T> list, Func<T, bool> predicate)
        {
            return list.ChunkBy(predicate, false);
        }

        /// <summary>
        /// Chunks a list based on a predicate. The element that matches the predicate is included in the current chunk
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="predicate"></param>
        /// <remarks>Ex. ["A", "B", "1", "C", "D", "E"].ChunkByInclusive(x => x == "1") would return a list with [["A", "B", "1"], ["C", "D", "E"]]</remarks>
        /// <returns></returns>
        public static List<List<T>> ChunkByInclusive<T>(this IEnumerable<T> list, Func<T, bool> predicate)
        {
            return list.ChunkBy(predicate, true);
        }

        private static List<List<T>> ChunkBy<T>(this IEnumerable<T> list, Func<T, bool> predicate, bool inclusive)
        {
            List<List<T>> resultList = [];
            List<T> result = [];

            foreach (T item in list)
            {
                if (predicate(item))
                {
                    if (inclusive)
                    {
                        result.Add(item);
                    }

                    resultList.Add(result);
                    result = [];
                }
                else
                {
                    result.Add(item);
                }
            }

            // Add the last chunk if needed
            if (result.Count != 0)
            {
                resultList.Add(result);
            }

            return resultList.Where(x => x.Count != 0).ToList();
        }

        /// <summary>
        /// Pivot's a 2D list
        /// </summary>
        /// <param name="list"></param>
        /// <typeparam name="T"></typeparam>
        /// <remarks>Ex. [[1,2],[3,4],[5,6]].Pivot() returns [[1, 3, 5], [2, 4, 6]]</remarks>
        /// <returns></returns>
        public static List<List<T>> Pivot<T>(this IEnumerable<IEnumerable<T>> list)
        {
            return list
                .SelectMany(inner => inner.Select((item, index) => new { item, index }))
                .GroupBy(i => i.index, i => i.item)
                .Select(g => g.ToList())
                .ToList();
        }

        /// <summary>
        /// Reverses a list in place
        /// </summary>
        /// <param name="list"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static List<T> ReverseInPlace<T>(this List<T> list)
        {
            list.Reverse();

            return list.ToList();
        }

        /// <summary>
        /// Reverses a string
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ReverseInPlace(this string str) {
            return new string(str.ToCharArray().Reverse().ToArray());
        }
        
        /// <summary>
        /// Get all permutations for the list
        /// </summary>
        /// <param name="list"></param>
        /// <typeparam name="T"></typeparam>
        /// <remarks>Ex. [1, 2, 3].GetPermutations() returns  [[1, 2, 3], [1, 3, 2], [2, 1, 3], [2, 3, 1], [3, 1, 2], [3, 2, 1]]</remarks>
        /// <returns></returns>
        public static IEnumerable<IEnumerable<T>> GetPermutations<T>(this IEnumerable<T> list)
        {
            return GetPermutations(list, list.Count());
        }

        /// <summary>
        /// Get all permutations for the list with a certain length
        /// </summary>
        /// <param name="list"></param>
        /// <param name="length"></param>
        /// <remarks>Ex. [1, 2, 3].GetPermutations(2) returns  [[1, 2], [1, 3], [2, 1], [2, 3], [3, 1], [3, 2]]</remarks>
        /// <returns></returns>
        public static IEnumerable<IEnumerable<T>> GetPermutations<T>(this IEnumerable<T> list, int length)
        {
            return length == 1
                ? list.Select(t => new T[] { t })
                : list.GetPermutations(length - 1)
                .SelectMany(t => list.Where(e => !t.Contains(e)),
                    (t1, t2) => t1.Concat(new T[] { t2 }));
        }

        /// <summary>
        /// Get all combinations for the list
        /// </summary>
        /// <param name="list"></param>
        /// <param name="length"></param>
        /// <typeparam name="T"></typeparam>
        /// <remarks>Ex. [1, 2, 3].GetCombinations(2) returns [[1, 2], [1, 3], [2, 3]]</remarks>
        /// <returns></returns>
        public static IEnumerable<IEnumerable<T>> GetCombinations<T>(this IEnumerable<T> list, int length) where T : IComparable
        {
            return length == 1
                ? list.Select(t => new T[] { t })
                : list.GetCombinations(length - 1)
                .SelectMany(t => list.Where(o => o.CompareTo(t.Last()) > 0),
                    (t1, t2) => t1.Concat(new T[] { t2 }));
        }

        /// <summary>
        /// Returns a list of indexes in a list based on a condition
        /// </summary>
        /// <param name="list"></param>
        /// <param name="predicate"></param>
        /// <typeparam name="T"></typeparam>
        /// <remarks>Ex. [1, 2, 3, 4, 6].FindIndexes(x => x % 2 == 0) returns [1, 3, 4]</remarks>
        /// <returns></returns>
        public static List<int> FindIndexes<T>(this IEnumerable<T> list, Func<T, bool> predicate)
        {
            List<int> indexes = [];

            foreach (int i in list.Count())
            {
                T item = list.ElementAt(i);

                if (predicate(item))
                {
                    indexes.Add(i);
                }
            }

            return indexes;
        }

        /// <summary>
        /// Removes and returns the last element of the list
        /// </summary>
        /// <param name="list"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T Pop<T>(this List<T> list)
        {
            T last = list.Last();

            list.Remove(last);

            return last;
        }

        /// <summary>
        /// Removes and returns the first element 
        /// </summary>
        /// <param name="list"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T Shift<T>(this List<T> list)
        {
            T first = list.First();

            list.Remove(first);

            return first;
        }
        
        /// <summary>
        /// Returns a list of points of available neighbors based on an initial coordinate.
        /// Assumes that each row has a consistent length
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="includeDiagonal"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static List<Point> GetNeighbors<T>(this List<List<T>> grid, int x, int y, bool includeDiagonal = false) {
            List<Point> neighbors = [];
            
            bool yNotMin = y > 0;
            bool xNotMin = x > 0;
            bool yNotMax = y < grid.Count - 1;
            bool xNotMax = x < grid.First().Count - 1;

            if (xNotMin) {
                neighbors.Add(new Point(x - 1, y));

                if (includeDiagonal) {
                    if (yNotMin) {
                        neighbors.Add(new Point(x - 1, y - 1));
                    }

                    if (yNotMax) {
                        neighbors.Add(new Point(x - 1, y + 1));
                    }
                }
            }

            if (xNotMax) {
                neighbors.Add(new Point(x + 1, y));

                if (includeDiagonal) {
                    if (yNotMin) {
                        neighbors.Add(new Point(x + 1, y - 1));
                    }

                    if (yNotMax) {
                        neighbors.Add(new Point(x + 1, y + 1));
                    }
                }
            }

            if (yNotMin) {
                neighbors.Add(new Point(x, y - 1));
            }

            if (yNotMax) {
                neighbors.Add(new Point(x, y + 1));
            }

            return neighbors;
        }
        #endregion

        #region String Operations
        /// <summary>
        /// Splits a string by a given substring
        /// </summary>
        /// <param name="input"></param>
        /// <param name="split"></param>
        /// <remarks>Ex. SplitSubstring("ABC, DEF, GHI", ", ") would return a list with ["ABC", "DEF", "GHI"]</remarks>
        /// <returns></returns>
        public static List<string> SplitSubstring(this string input, string split)
        {
            return input.Split(split).Where(l => l != split).ToList();
        }

        /// <summary>
        /// A quick version of regex that assumes we're receiving 1 line and only want the first match
        /// </summary>
        /// <param name="input"></param>
        /// <param name="regex"></param>
        /// <remarks>Ex. "ABC12DEF".QuickRegex("([A-Za-z]+)\d+([A-Za-z]+)") returns ["ABC", "DEF"]</remarks>
        /// <returns></returns>
        public static List<string> QuickRegex(this string input, string regex)
        {
            List<string> response = [];

            Regex rx = new(regex);

            MatchCollection match = rx.Matches(input);

            if (match.Count != 0)
            {
                GroupCollection groups = match.First().Groups;

                // Not starting at i = 0 because that returns the whole match, we only care about the captures
                for (int i = 1; i < groups.Count; i++)
                {
                    Group group = groups[i];

                    response.Add(group.Value);
                }
            }

            return response;
        }

        /// <summary>
        /// A quick version of regex that assumes that each element of the list is 1 line and only want the first match
        /// </summary>
        /// <param name="input"></param>
        /// <param name="regex"></param>
        /// <remarks>Ex. ["ABC12DEF", "G63HIJK"].QuickRegex("([A-Za-z]+)\d+([A-Za-z]+)") returns [["ABC", "DEF"],["G", "HIJK"]]</remarks>
        /// <returns></returns>
        public static List<List<string>> QuickRegex(this IEnumerable<string> input, string regex)
        {
            return input.Select(i => i.QuickRegex(regex)).ToList();
        }
        #endregion

        #region Math
        /// <summary>
        /// Mathematical mod (As opposed to C#'s remainder operator '%')
        /// </summary>
        /// <param name="num"></param>
        /// <param name="mod"></param>
        /// <remarks>This is only needed if you'll be dealing with negative numbers. Ex. Mod(-5, 4) returns 3 while -5 % 4 returns -1</remarks>
        /// <returns></returns>
        public static int Mod(int num, int mod)
        {
            if (num >= 0) {
                return num % mod;
            }
            else {
                return num + (int)Math.Ceiling((double)Math.Abs(num) / mod) * mod;
            }
        }

        /// <summary>
        /// Mathematical mod (As opposed to C#'s remainder operator '%')
        /// </summary>
        /// <param name="num"></param>
        /// <param name="mod"></param>
        /// <remarks>This is only needed if you'll be dealing with negative numbers. Ex. Mod(-5, 4) returns 3 while -5 % 4 returns -1</remarks>
        /// <returns></returns>
        public static long Mod(long num, long mod)
        {
            if (num >= 0)
            {
                return num % mod;
            }
            else
            {
                return num + (long)Math.Ceiling((double)Math.Abs(num) / mod) * mod;
            }
        }

        /// <summary>
        /// Given 2 numbers, find the Least Common Multiple between them
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        /// <remarks>Ex. Utility.LCM(24, 36) returns 72</remarks>
        public static T LCM<T>(T a, T b) where T: notnull, INumber<T>
        {
            return a / GCF(a, b) * b;
        }

        /// <summary>
        /// Given 2 numbers, find the Least Common Multiple between them
        /// </summary>
        /// <param name="values"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        /// <remarks>Ex. Utility.LCM([24, 36]) returns 72</remarks>
        public static T LCM<T>(List<T> values) where T: notnull, INumber<T>
        {
            if (values.Count == 0) {
                return default!;
            }
            else if (values.Count == 1) {
                return values.First();
            }
            else {
                T value = LCM(values[0], values[1]);

                for (int i = 2; i < values.Count; i++) {
                    value = LCM(value, values[i]);
                }

                return value;
            }
        }

        /// <summary>
        /// Given 2 numbers, find the Greatest Common Factor between them
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        /// <remarks>Ex. Utility.GCF(24, 36) returns 12</remarks>
        public static T GCF<T>(T a, T b) where T: notnull, INumber<T>
        {
            while (b != default)
            {
                T temp = b;
                b = a % b;
                a = temp;
            }
            return a;
        }

        /// <summary>
        /// Given 2 numbers, find the Greatest Common Factor between them
        /// </summary>
        /// <param name="values"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        /// <remarks>Ex. Utility.GCF([24, 36]) returns 12</remarks>
        public static T GCF<T>(List<T> values) where T: notnull, INumber<T>
        {
            if (values.Count == 0) {
                return default!;
            }
            else if (values.Count == 1) {
                return values.First();
            }
            else {
                T value = LCM(values[0], values[1]);

                for (int i = 2; i < values.Count; i++) {
                    value = LCM(value, values[i]);
                }

                return value;
            }
        }

        /// <summary>
        /// Given a matrix, calculate it's determinate
        /// </summary>
        /// <param name="matrix"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        /// <remarks>Ex. Utility.MatrixDeterminate([[4, 1], [0, 2]]) returns 8</remarks>
        public static T MatrixDeterminate<T>(List<List<T>> matrix) where T : notnull, INumber<T> {
            if (matrix.Count == 0) {
                return default!;
            }

            if (!matrix.All(row => row.Count == matrix.Count)) {
                throw new Exception("Recieved a non-square matrix");
            }

            if (matrix.Count == 2) {
                T value = matrix[0][0] * matrix[1][1] - matrix[0][1] * matrix[1][0];
                return value;
            }
            else {
                T answer = default!;
                foreach (int i in matrix.Count) {
                    T value = matrix[0][i];

                    // Don't bother doing the calcs if it won't change the results
                    if (value != default) {
                        List<List<T>> newMatrix = matrix.Skip(1).Select(row => row.Where((x, index) => index != i).ToList()).ToList();
                        value *= MatrixDeterminate(newMatrix);
                        
                        answer += i % 2 == 0 ? value : -value;
                    }
                }
                return answer;
            }
        }

        #endregion

        #region Conversion
        /// <summary>
        /// Converts 'a' to 1 and 'A' to 27
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static int GetCharValue(this char value)
        {
            int asciiValue = value;

            int offset = char.IsLower(value) ? 'a' - 1 : 'A' - 27;

            return asciiValue - offset;
        }

        /// <summary>
        /// Parses a list of strings into a list of ints
        /// </summary>
        /// <param name="strings"></param>
        /// <returns></returns>
        public static List<int> ToInts(this IEnumerable<string> strings)
        {
            return strings.Select(int.Parse).ToList();
        }

        /// <summary>
        /// Parses a list of list of strings into a list of list of ints
        /// </summary>
        /// <param name="strings"></param>
        /// <returns></returns>
        public static List<List<int>> ToInts(this IEnumerable<IEnumerable<string>> strings)
        {
            return strings.Select(ToInts).ToList();
        }

        /// <summary>
        /// Parses a string to a list of ints where each char becomes an int
        /// </summary>
        /// <param name="input"></param>
        /// <remarks>Ex. "15234".ToInts() returns [1, 5, 2, 3, 4]</remarks>
        /// <returns></returns>
        public static List<int> ToInts(this string input)
        {
            return input.Select(i => i.ToString()).ToInts();
        }

        /// <summary>
        /// Parses a list of strings into a list of longs
        /// </summary>
        /// <param name="strings"></param>
        /// <returns></returns>
        public static List<long> ToLongs(this IEnumerable<string> strings)
        {
            return strings.Select(long.Parse).ToList();
        }

        /// <summary>
        /// Parses a list of ints into a list of longs
        /// </summary>
        /// <param name="ints"></param>
        /// <returns></returns>
        public static List<long> ToLongs(this IEnumerable<int> ints)
        {
            return ints.Select(i => (long)i).ToList();
        }

        /// <summary>
        /// Simplified ToString for char array
        /// </summary>
        /// <param name="chars"></param>
        /// <remarks>Ex. ['A', 'B', 'C'].CharsToString() returns "ABC"</remarks>
        /// <returns></returns>
        public static string CharsToString(this char[] chars)
        {
            return new string(chars);
        }

        /// <summary>
        /// Simplified ToString for char list
        /// </summary>
        /// <param name="chars"></param>
        /// <remarks>Ex. ['A', 'B', 'C'].CharsToString() returns "ABC"</remarks>
        /// <returns></returns>
        public static string CharsToString(this IEnumerable<char> chars)
        {
            return new string(chars.ToArray());
        }

        /// <summary>
        /// Converts a char to an int
        /// </summary>
        /// <param name="value"></param>
        /// <remarks> Ex. '1'.ToInt() returns 1</remarks>
        /// <returns></returns>
        public static int ToInt(this char value)
        {
            return int.Parse(value.ToString());
        }

        #endregion

        #region Other
        /// <summary>
        /// Repeats a function a certain amount of times
        /// </summary>
        /// <param name="count"></param>
        /// <param name="action"></param>
        /// <remarks>Ex. 3.ForEach(() => {i++;}) would increase i by 3</remarks>
        public static void ForEach<T>(this T count, Action action) where T : notnull, INumber<T>
        {
            foreach (T i in count) {
                action.Invoke();
            }
        }

        /// <summary>
        /// Repeats a function a certain amount of times where the first parameter is the index
        /// </summary>
        /// <param name="count"></param>
        /// <param name="action"></param>
        /// <remarks>Ex. 3.ForEach((i) => Console.Write(i)) would print 012</remarks>
        public static void ForEach<T>(this T count, Action<T> action) where T : notnull, INumber<T>
        {
            foreach (T i in count) {
                action.Invoke(i);
            }
        }
        
        /// <summary>
        /// An extension to non-null numbers to allow use of simpler foreach syntax
        /// </summary>
        /// <param name="count"></param>
        /// <typeparam name="T"></typeparam>
        /// <remarks>Ex. foreach(int i in 10) { Console.Write(i) } would print 012</remarks>
        /// <returns></returns>
        public static IEnumerator<T> GetEnumerator<T>(this T count) where T : notnull, INumber<T>
        {
            for (T i = default!; i < count; i++) {
                yield return i;
            }
        }
        #endregion

        # region ASCII Parsing
        /// <summary>
        /// Given a string and desired height, print the ASCII and map it to a readable string
        /// </summary>
        /// <param name="characters"></param>
        /// <param name="height"></param>
        /// <param name="emptyChar"></param>
        /// <param name="textChar"></param>
        /// <remarks>Currently this only parses for heights 6 and 10 and these heights are not complete for the full alphabet</remarks>
        /// <returns></returns>
        public static string ParseASCIILetters(IEnumerable<char> characters, int height, char emptyChar = '.', char textChar = '#')
        {
            string text = string.Join(string.Empty, characters);
            return ParseASCIILetters(text, height, emptyChar, textChar);
        }

        /// <summary>
        /// Given a string and desired height, print the ASCII and map it to a readable string
        /// </summary>
        /// <param name="characters"></param>
        /// <param name="height"></param>
        /// <param name="emptyChar"></param>
        /// <param name="textChar"></param>
        /// <remarks>Currently this only parses for heights 6 and 10 and these heights are not complete for the full alphabet</remarks>
        /// <returns></returns>
        public static string ParseASCIILetters(string characters, int height, char emptyChar = '.', char textChar = '#')
        {
            string formattedOutput = characters.Replace(emptyChar, '.').Replace(textChar, '#');
            IEnumerable<char[]> outputRows = formattedOutput.Chunk(characters.Length / height);

            foreach (char[] outputLine in outputRows)
            {
                string value = new(outputLine);
                Console.WriteLine(value.Replace('#', '█').Replace('.', ' '));
            }

            IEnumerable<string> pivotedOutput = outputRows.Pivot().Select(r => r.CharsToString());
            List<List<string>> rotatedLetters = pivotedOutput.ChunkByExclusive(x => x == new string('.', height));
            List<List<string>> letters = rotatedLetters.Select(x => x.Pivot().Select(y => y.CharsToString()).ToList()).ToList();

            Dictionary<string, char> mapping = height switch
            {
                6 => ASCIIMap6,
                10 => ASCIIMap10,
                _ => throw new Exception($"There is no mapping for height: {height}, only 6 and 10."),
            };

            string parsedOutput = "";
            foreach (List<string> letter in letters)
            {
                string key = string.Join(string.Empty, letter);

                if (mapping.TryGetValue(key, out char value))
                {
                    parsedOutput += value;
                }
                else
                {
                    parsedOutput += '?';
                }
            }

            return parsedOutput;
        }

        // ABCEFGHIJKLOPRSUYZ
        private static readonly Dictionary<string, char> ASCIIMap6 = new(){
            {
                """
                .##.
                #..#
                #..#
                ####
                #..#
                #..#
                """.Replace("\r", string.Empty).Replace("\n", string.Empty)
                ,'A'
            },
            {
                """
                ###.
                #..#
                ###.
                #..#
                #..#
                ###.
                """.Replace("\r", string.Empty).Replace("\n", string.Empty)
                ,'B'
            },
            {
                """
                .##.
                #..#
                #...
                #...
                #..#
                .##.
                """.Replace("\r", string.Empty).Replace("\n", string.Empty)
                ,'C'
            },
            {
                """
                ####
                #...
                ###.
                #...
                #...
                ####
                """.Replace("\r", string.Empty).Replace("\n", string.Empty)
                ,'E'
            },
            {
                """
                ####
                #...
                ###.
                #...
                #...
                #...
                """.Replace("\r", string.Empty).Replace("\n", string.Empty)
                ,'F'
            },
            {
                """
                .##.
                #..#
                #...
                #.##
                #..#
                .###
                """.Replace("\r", string.Empty).Replace("\n", string.Empty)
                ,'G'
            },
            {
                """
                #..#
                #..#
                ####
                #..#
                #..#
                #..#
                """.Replace("\r", string.Empty).Replace("\n", string.Empty)
                ,'H'
            },
            {
                """
                ###
                .#.
                .#.
                .#.
                .#.
                ###
                """.Replace("\r", string.Empty).Replace("\n", string.Empty)
                ,'I'
            },
            {
                """
                ..##
                ...#
                ...#
                ...#
                #..#
                .##.
                """.Replace("\r", string.Empty).Replace("\n", string.Empty)
                ,'J'
            },
            {
                """
                #..#
                #.#.
                ##..
                #.#.
                #.#.
                #..#
                """.Replace("\r", string.Empty).Replace("\n", string.Empty)
                ,'K'
            },
            {
                """
                #...
                #...
                #...
                #...
                #...
                ####
                """.Replace("\r", string.Empty).Replace("\n", string.Empty)
                ,'L'
            },
            {
                """
                .##.
                #..#
                #..#
                #..#
                #..#
                .##.
                """.Replace("\r", string.Empty).Replace("\n", string.Empty)
                ,'O'
            },
            {
                """
                ###.
                #..#
                #..#
                ###.
                #...
                #...
                """.Replace("\r", string.Empty).Replace("\n", string.Empty)
                ,'P'
            },
            {
                """
                ###.
                #..#
                #..#
                ###.
                #.#.
                #..#
                """.Replace("\r", string.Empty).Replace("\n", string.Empty)
                ,'R'
            },
            {
                """
                .###
                #...
                #...
                .##.
                ...#
                ###.
                """.Replace("\r", string.Empty).Replace("\n", string.Empty)
                ,'S'
            },
            {
                """
                #..#
                #..#
                #..#
                #..#
                #..#
                .##.
                """.Replace("\r", string.Empty).Replace("\n", string.Empty)
                ,'U'
            },
            {
                """
                #...#
                #...#
                .#.#.
                ..#..
                ..#..
                ..#..
                """.Replace("\r", string.Empty).Replace("\n", string.Empty)
                ,'Y'
            },
            {
                """
                ####
                ...#
                ..#.
                .#..
                #...
                ####
                """.Replace("\r", string.Empty).Replace("\n", string.Empty)
                ,'Z'
            }
        };

        // ABCEFGHJKLNPRXZ
        private static readonly Dictionary<string, char> ASCIIMap10 = new(){
            {
                """
                ..##..
                .#..#.
                #....#
                #....#
                #....#
                ######
                #....#
                #....#
                #....#
                #....#
                """.Replace("\r", string.Empty).Replace("\n", string.Empty)
                ,'A'
            },
            {
                """
                #####.
                #....#
                #....#
                #....#
                #####.
                #....#
                #....#
                #....#
                #....#
                #####.
                """.Replace("\r", string.Empty).Replace("\n", string.Empty)
                ,'B'
            },
            {
                """
                .####.
                #....#
                #.....
                #.....
                #.....
                #.....
                #.....
                #.....
                #....#
                .####.
                """.Replace("\r", string.Empty).Replace("\n", string.Empty)
                ,'C'
            },
            {
                """
                ######
                #.....
                #.....
                #.....
                #####.
                #.....
                #.....
                #.....
                #.....
                ######
                """.Replace("\r", string.Empty).Replace("\n", string.Empty)
                ,'E'
            },
            {
                """
                ######
                #.....
                #.....
                #.....
                #####.
                #.....
                #.....
                #.....
                #.....
                #.....
                """.Replace("\r", string.Empty).Replace("\n", string.Empty)
                ,'F'
            },
            {
                """
                .####.
                #....#
                #.....
                #.....
                #.....
                #..###
                #....#
                #....#
                #...##
                .###.#
                """.Replace("\r", string.Empty).Replace("\n", string.Empty)
                ,'G'
            },
            {
                """
                #....#
                #....#
                #....#
                #....#
                ######
                #....#
                #....#
                #....#
                #....#
                #....#
                """.Replace("\r", string.Empty).Replace("\n", string.Empty)
                ,'H'
            },
            {
                """
                ...###
                ....#.
                ....#.
                ....#.
                ....#.
                ....#.
                ....#.
                #...#.
                #...#.
                .###..
                """.Replace("\r", string.Empty).Replace("\n", string.Empty)
                ,'J'
            },
            {
                """
                #....#
                #...#.
                #..#..
                #.#...
                ##....
                ##....
                #.#...
                #..#..
                #...#.
                #....#
                """.Replace("\r", string.Empty).Replace("\n", string.Empty)
                ,'K'
            },
            {
                """
                #.....
                #.....
                #.....
                #.....
                #.....
                #.....
                #.....
                #.....
                #.....
                ######
                """.Replace("\r", string.Empty).Replace("\n", string.Empty)
                ,'L'
            },
            {
                """
                #....#
                ##...#
                ##...#
                #.#..#
                #.#..#
                #..#.#
                #..#.#
                #...##
                #...##
                #....#
                """.Replace("\r", string.Empty).Replace("\n", string.Empty)
                ,'N'
            },
            {
                """
                #####.
                #....#
                #....#
                #....#
                #####.
                #.....
                #.....
                #.....
                #.....
                #.....
                """.Replace("\r", string.Empty).Replace("\n", string.Empty)
                ,'P'
            },
            {
                """
                #####.
                #....#
                #....#
                #....#
                #####.
                #..#..
                #...#.
                #...#.
                #....#
                #....#
                """.Replace("\r", string.Empty).Replace("\n", string.Empty)
                ,'R'
            },
            {
                """
                #....#
                #....#
                .#..#.
                .#..#.
                ..##..
                ..##..
                .#..#.
                .#..#.
                #....#
                #....#
                """.Replace("\r", string.Empty).Replace("\n", string.Empty)
                ,'X'
            },
            {
                """
                ######
                .....#
                .....#
                ....#.
                ...#..
                ..#...
                .#....
                #.....
                #.....
                ######
                """.Replace("\r", string.Empty).Replace("\n", string.Empty)
                ,'Z'
            }
        };
        #endregion

        # region File Processing
        /// <summary>
        /// Get the input file line by line
        /// </summary>
        /// <param name="year"></param>
        /// <param name="quest"></param>
        /// <param name="part"></param>
        /// <param name="example">Defaults to false, if true will pull an example file you've manually added</param>
        /// <remarks>Ex. GetInputLines(2024, 2, 3) reads the data from "/Inputs/2024/02/3.txt". GetInputLines(2019, 5, 2, true) reads the data from "/Inputs/2024/05/2_example.txt"</remarks>
        /// <returns></returns>
        public static List<string> GetInputLines(int year, int quest, int part, bool example = false)
        {
            return File.ReadAllLines(Path.Combine(Environment.CurrentDirectory, "Inputs", year.ToString(), $"{quest:D2}", $"{part}{(example ? "_example" : string.Empty)}.txt")).ToList();
        }
        #endregion
    }
}