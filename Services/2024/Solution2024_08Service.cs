namespace EverybodyCodes.Services
{
    public class Solution2024_08Service : ISolutionQuestService
    {
        // (ctrl/command + click) the link to open the input file
        // file://./../../Inputs/2024/08/1.txt
        public string PartOne(bool example)
        {
            List<string> lines = Utility.GetInputLines(2024, 8, 1, example);

            int blocks = int.Parse(lines.First());

            int step = (int)Math.Ceiling(Math.Sqrt(blocks));
            int targetWidth = 2 * (step - 1) + 1;
            int targetBlocks = step * step;
            int missingBlocks = targetBlocks - blocks;

            int answer = missingBlocks * targetWidth;
            
            return answer.ToString();
        }

        // (ctrl/command + click) the link to open the input file
        // file://./../../Inputs/2024/08/2.txt
        public string PartTwo(bool example)
        {
            List<string> lines = Utility.GetInputLines(2024, 8, 2, example);

            int priests = int.Parse(lines.First());
            int acolytes = example ? 5 : 1111;
            int supply = example ? 50 : 20240000;

            int usedBlocks = 0;
            int thickness = 1;
            int width = 1;

            while (usedBlocks <= supply) {
                usedBlocks += width * thickness;
                thickness = thickness * priests % acolytes;
                width += 2;
            }

            width -= 2;

            int blocksNeeded = usedBlocks - supply;

            int answer = blocksNeeded * width;

            return answer.ToString();
        }

        // (ctrl/command + click) the link to open the input file
        // file://./../../Inputs/2024/08/3.txt
        public string PartThree(bool example)
        {
            List<string> lines = Utility.GetInputLines(2024, 8, 3, example);

            long priests = int.Parse(lines.First());
            long acolytes = example ? 5 : 10;
            long supply = example ? 160 : 202400000;

            long usedBlocks = 0;
            long thickness = 1;
            long width = 1;

            List<long> thicknessHistory = [thickness];

            while (usedBlocks <= supply) {
                usedBlocks = thickness * (width == 1 ? 1 : 2);

                for (int i = 1; i <= (width - 1) / 2; i++) {
                    long columnHeight = thicknessHistory.TakeLast(i + 1).Sum();
                    long emptySpace = priests * width * columnHeight % acolytes;
                    usedBlocks += (i == (width - 1) / 2 ? 1 : 2) * (columnHeight - emptySpace);
                }

                thickness = thickness * priests % acolytes + acolytes;
                width += 2;
                thicknessHistory.Add(thickness);
            }

            long answer = usedBlocks - supply;

            return answer.ToString();
        }
    }
}