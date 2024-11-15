namespace EverybodyCodes.Services
{
    public class Solution2024_04Service : ISolutionQuestService
    {
        // (ctrl/command + click) the link to open the input file
        // file://./../../Inputs/2024/04/1.txt
        public string PartOne(bool example)
        {
            List<string> lines = Utility.GetInputLines(2024, 4, 1, example);
            List<int> nails = lines.ToInts();

            int shortestNail = nails.Min();

            int answer = nails.Sum(nail => nail - shortestNail);

            return answer.ToString();
        }

        // (ctrl/command + click) the link to open the input file
        // file://./../../Inputs/2024/04/2.txt
        public string PartTwo(bool example)
        {
            List<string> lines = Utility.GetInputLines(2024, 4, 2, example);

            List<int> nails = lines.ToInts();

            int shortestNail = nails.Min();

            int answer = nails.Sum(nail => nail - shortestNail);

            return answer.ToString();
        }

        // (ctrl/command + click) the link to open the input file
        // file://./../../Inputs/2024/04/3.txt
        public string PartThree(bool example)
        {
            List<string> lines = Utility.GetInputLines(2024, 4, 3, example);
            
            List<int> nails = lines.ToInts();

            int averageNail = (int)Math.Round(nails.Average());

            int averageValue = nails.Sum(nail => Math.Abs(nail - averageNail));
            int upOne = nails.Sum(nail => Math.Abs(nail - (averageNail + 1)));
            int downOne = nails.Sum(nail => Math.Abs(nail - (averageNail - 1)));

            int answer = averageValue;
            if (averageValue > upOne || averageValue > downOne) {
                bool descend = averageValue > downOne;

                int targetNail = averageNail;

                while(true) {
                    targetNail = descend ? targetNail - 1 : targetNail + 1;

                    int newAnswer = nails.Sum(nail => Math.Abs(nail - targetNail));

                    if (newAnswer < answer) {
                        answer = newAnswer;
                    }
                    else {
                        break;
                    }
                }
            }

            return answer.ToString();
        }
    }
}