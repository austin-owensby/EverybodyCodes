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

            int answer = 0;

            foreach (string line in lines)
            {

            }

            return answer.ToString();
        }
    }
}