namespace EverybodyCodes.Services
{
    public class Solution2024_14Service : ISolutionQuestService
    {
        // (ctrl/command + click) the link to open the input file
        // file://./../../Inputs/2024/14/1.txt
        public string PartOne(bool example)
        {
            List<string> lines = Utility.GetInputLines(2024, 14, 1, example);

            List<char> heightChars = ['U','D'];

            List<int> heightChanges = lines[0].Split(',').Where(x => heightChars.Contains(x[0])).Select(x => x.Replace("U","").Replace("D","-")).ToInts();

            int answer = 0;
            int height = 0;

            foreach (int heightChange in heightChanges)
            {
                height += heightChange;

                answer = Math.Max(answer, height);
            }

            return answer.ToString();
        }

        // (ctrl/command + click) the link to open the input file
        // file://./../../Inputs/2024/14/2.txt
        public string PartTwo(bool example)
        {
            List<string> lines = Utility.GetInputLines(2024, 14, 2, example);

            int answer = 0;

            foreach (string line in lines)
            {

            }

            return answer.ToString();
        }

        // (ctrl/command + click) the link to open the input file
        // file://./../../Inputs/2024/14/3.txt
        public string PartThree(bool example)
        {
            List<string> lines = Utility.GetInputLines(2024, 14, 3, example);

            int answer = 0;

            foreach (string line in lines)
            {

            }

            return answer.ToString();
        }
    }
}