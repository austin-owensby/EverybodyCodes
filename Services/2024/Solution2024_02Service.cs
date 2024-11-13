namespace EverybodyCodes.Services
{
    public class Solution2024_02Service : ISolutionQuestService
    {
        // (ctrl/command + click) the link to open the input file
        // file://./../../Inputs/2024/02/1.txt
        public string PartOne(bool example)
        {
            List<string> lines = Utility.GetInputLines(2024, 2, 1, example);
            List<string> runicWords = lines.First().Split(":")[1].Split(",").ToList();
            List<string> sentence = lines.Last().Split(" ").ToList();

            int answer = sentence.Sum(w => runicWords.Count(rw => w.Contains(rw)));

            return answer.ToString();
        }

        // (ctrl/command + click) the link to open the input file
        // file://./../../Inputs/2024/02/2.txt
        public string PartTwo(bool example)
        {
            List<string> lines = Utility.GetInputLines(2024, 2, 2, example);
            List<string> runicWords = lines.First().Split(":")[1].Split(",").ToList();
            List<string> sentences = lines.Skip(2).ToList();

            // Handle backwards words
            List<string> reversedRunicWords = runicWords.Select(w => w.ReverseInPlace()).ToList();
            runicWords = runicWords.Concat(reversedRunicWords).ToList();

            int answer = 0;

            foreach (string sentence in sentences)
            {
                List<int> indexes = [];

                foreach (string runicWord in runicWords)
                {
                    int index = 0;

                    while (index != -1) {
                        int nextIndex = sentence.IndexOf(runicWord, index);

                        if (nextIndex != -1) {
                            indexes.AddRange(Enumerable.Range(nextIndex, runicWord.Length));
                            index = nextIndex + 1;
                        }
                        else {
                            index = -1;
                        }
                    }
                }

                answer += indexes.Distinct().Count();
            }

            return answer.ToString();
        }

        // (ctrl/command + click) the link to open the input file
        // file://./../../Inputs/2024/02/3.txt
        public string PartThree(bool example)
        {
            List<string> lines = Utility.GetInputLines(2024, 2, 3, example);
            List<string> runicWords = lines.First().Split(":")[1].Split(",").ToList();
            List<string> sentences = lines.Skip(2).ToList();

            int height = sentences.Count;
            int width = sentences.First().Length;

            List<Point> points = [];

            // Loop over each row
            foreach (int y in height) {
                // Loop over each column
                foreach (int x in width) {
                    // Test each word
                    foreach (string runicWord in runicWords) {
                        List<List<Point>> wordPoints = [[],[],[],[]];
                        // Record the potential points of each word in all 4 directions
                        foreach (int i in runicWord.Length) {
                            // Wrap around in left/right direction
                            wordPoints[0].Add(new (Utility.Mod(x + i, width), y));
                            wordPoints[1].Add(new (Utility.Mod(x - i, width), y));
                            
                            // Don't wrap around in up/down direction
                            if (y + i >= height) {
                                wordPoints[2] = [];
                            }
                            else {
                                wordPoints[2].Add(new (x, y + i));
                            }

                            if (y - i < 0) {
                                wordPoints[3] = [];
                            }
                            else {
                                wordPoints[3].Add(new (x, y - i));
                            }
                        }

                        // Based on the points, construct strings
                        List<string> matchCandidates = wordPoints.Select(wp => new string(wp.Select(p => sentences[p.Y][p.X]).ToArray())).ToList();

                        // If strings match, record their points
                        List<Point> newPoints = matchCandidates.FindIndexes(m => m == runicWord).SelectMany(i => wordPoints[i]).ToList();

                        if (newPoints.Count > 0) {
                            points.AddRange(newPoints);
                        }
                    }
                }
            }

            // Get the distinct list of points
            int answer = points.DistinctBy(p => new {p.X, p.Y}).Count();

            return answer.ToString();
        }
    }
}