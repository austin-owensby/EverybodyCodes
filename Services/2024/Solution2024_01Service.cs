namespace EverybodyCodes.Services
{
    // (ctrl/command + click) the link to open the input file
    // file://./../../Inputs/2024/01.txt
    public class Solution2024_01Service : ISolutionQuestService
    {
        public string PartOne(bool example)
        {
            List<string> lines = Utility.GetInputLines(2024, 1, 1, example);

            int answer = 0;

            foreach (string line in lines)
            {

            }
            
            return answer.ToString();
        }

        public string PartTwo(bool example)
        {
            List<string> lines = Utility.GetInputLines(2024, 1, 2, example);

            int answer = 0;

            foreach (string line in lines)
            {

            }
            
            return answer.ToString();
        }

        public string PartThree(bool example)
        {
            List<string> lines = Utility.GetInputLines(2024, 1, 3, example);

            int answer = 0;

            foreach (string line in lines)
            {

            }

            return answer.ToString();
        }
    }
}