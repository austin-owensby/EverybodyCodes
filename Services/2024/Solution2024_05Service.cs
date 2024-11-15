namespace EverybodyCodes.Services
{
    public class Solution2024_05Service : ISolutionQuestService
    {
        // (ctrl/command + click) the link to open the input file
        // file://./../../Inputs/2024/05/1.txt
        public string PartOne(bool example)
        {
            List<string> lines = Utility.GetInputLines(2024, 5, 1, example);
            List<List<int>> columns = lines.Select(line => line.Split(' ').ToInts()).Pivot();

            int answer = 0;

            for (int round = 1; round <= 10; round++) {
                int clapperIndex = (round - 1) % 4;
                int clapper = columns[clapperIndex].Shift();

                int columnIndex = round % 4;
                List<int> column = columns[columnIndex];

                if (clapper <= column.Count) {
                    column.Insert(clapper - 1, clapper);
                }
                else {
                    column.Insert(column.Count * 2 + 1 - clapper, clapper);
                }

                answer = int.Parse(string.Join("", columns.Select(c => c.First())));
            }

            return answer.ToString();
        }

        // (ctrl/command + click) the link to open the input file
        // file://./../../Inputs/2024/05/2.txt
        public string PartTwo(bool example)
        {
            List<string> lines = Utility.GetInputLines(2024, 5, 2, example);
            List<List<int>> columns = lines.Select(line => line.Split(' ').ToInts()).Pivot();

            long answer = 0;

            Dictionary<string, int> states = [];
            int round = 1;

            while (answer == 0) {
                int clapperIndex = (round - 1) % 4;
                int clapper = columns[clapperIndex].Shift();

                int columnIndex = round % 4;
                List<int> column = columns[columnIndex];

                int clapperMod = (clapper - 1) % (column.Count * 2) + 1;

                if (clapperMod <= column.Count) {
                    column.Insert(clapperMod - 1, clapper);
                }
                else {
                    column.Insert(column.Count * 2 + 1 - clapperMod, clapper);
                }

                string state = string.Join("", columns.Select(c => c.First()));
                states[state] = states.GetValueOrDefault(state) + 1;

                if (states[state] == 2024) {
                    answer = round * long.Parse(state);
                }

                round++;
            }

            return answer.ToString();
        }

        // (ctrl/command + click) the link to open the input file
        // file://./../../Inputs/2024/05/3.txt
        public string PartThree(bool example)
        {
            List<string> lines = Utility.GetInputLines(2024, 5, 3, example);
            List<List<int>> columns = lines.Select(line => line.Split(' ').ToInts()).Pivot();

            long answer = 0;

            Dictionary<string, int> states = [];
            int round = 1;

            int newKeyCountdown = 1000;

            while (answer == 0) {
                int clapperIndex = (round - 1) % 4;
                int clapper = columns[clapperIndex].Shift();

                int columnIndex = round % 4;
                List<int> column = columns[columnIndex];

                int clapperMod = (clapper - 1) % (column.Count * 2) + 1;

                if (clapperMod <= column.Count) {
                    column.Insert(clapperMod - 1, clapper);
                }
                else {
                    column.Insert(column.Count * 2 + 1 - clapperMod, clapper);
                }

                string state = string.Join("", columns.Select(c => c.First()));

                if (states.ContainsKey(state)) {
                    newKeyCountdown--;

                    // We've gone 1000 iterations without a new key, assume we're in a loop and no new values will be shouted
                    if (newKeyCountdown == 0) {
                        answer = states.Max(s => long.Parse(s.Key));
                    }
                }
                else {
                    newKeyCountdown = 1000;
                }

                states[state] = states.GetValueOrDefault(state) + 1;

                round++;
            }

            return answer.ToString();
        }
    }
}