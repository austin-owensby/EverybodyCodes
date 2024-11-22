namespace EverybodyCodes.Services
{
    public class Solution2024_12Service : ISolutionQuestService
    {
        // (ctrl/command + click) the link to open the input file
        // file://./../../Inputs/2024/12/1.txt
        public string PartOne(bool example)
        {
            List<string> lines = Utility.GetInputLines(2024, 12, 1, example);
            List<List<char>> grid = lines.ReverseInPlace().Skip(1).Select(l => l.Skip(1).ToList()).ToList();

            int answer = 0;

            foreach (int y in grid.Count)
            {
                foreach (int x in grid[0].Count) {
                    if (grid[y][x] == 'T') {
                        answer += ((x + y) % 3 + 1) * ((x + y) / 3);
                    }
                }
            }

            return answer.ToString();
        }

        // (ctrl/command + click) the link to open the input file
        // file://./../../Inputs/2024/12/2.txt
        public string PartTwo(bool example)
        {
            List<string> lines = Utility.GetInputLines(2024, 12, 2, example);
            List<List<char>> grid = lines.ReverseInPlace().Skip(1).Select(l => l.Skip(1).ToList()).ToList();

            int answer = 0;

            List<char> targets = ['T', 'H'];

            foreach (int y in grid.Count)
            {
                foreach (int x in grid[0].Count) {
                    int mult = targets.IndexOf(grid[y][x]) + 1;
                    answer += mult * ((x + y) % 3 + 1) * ((x + y) / 3);
                }
            }

            return answer.ToString();
        }

        // (ctrl/command + click) the link to open the input file
        // file://./../../Inputs/2024/12/3.txt
        public string PartThree(bool example)
        {
            List<string> lines = Utility.GetInputLines(2024, 12, 3, example);

            int answer = 0;

            foreach (string line in lines)
            {

            }

            return answer.ToString();
        }
    }
}