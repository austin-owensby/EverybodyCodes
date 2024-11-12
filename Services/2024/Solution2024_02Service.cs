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

            int answer = 0;

            foreach (string line in lines)
            {

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