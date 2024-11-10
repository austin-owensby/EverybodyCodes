namespace EverybodyCodes.Services
{
    // (ctrl/command + click) the link to open the input file
    // file://./../../Inputs/2024/09.txt
    public class Solution2024_09Service : ISolutionQuestService
    {
        public string PartOne(bool example)
        {
            List<string> lines = Utility.GetInputLines(2024, 9, 1, example);

            int answer = 0;

            foreach (string line in lines)
            {

            }

            return answer.ToString();
        }

        public string PartTwo(bool example)
        {
            List<string> lines = Utility.GetInputLines(2024, 9, 2, example);

            int answer = 0;

            foreach (string line in lines)
            {

            }

            return answer.ToString();
        }

        public string PartThree(bool example)
        {
            List<string> lines = Utility.GetInputLines(2024, 9, 3, example);

            int answer = 0;

            foreach (string line in lines)
            {

            }

            return answer.ToString();
        }
    }
}