namespace EverybodyCodes.Services
{
    public class Solution2024_11Service : ISolutionQuestService
    {
        // (ctrl/command + click) the link to open the input file
        // file://./../../Inputs/2024/11/1.txt
        public string PartOne(bool example)
        {
            List<string> lines = Utility.GetInputLines(2024, 11, 1, example);
            Dictionary<string, List<string>> lifesycle = lines.Select(l => l.Split(":")).ToDictionary(l => l[0], l => l[1].Split(",").ToList());

            List<string> termites = ["A"];

            foreach (int generation in 4)
            {
                termites = termites.Select(x => lifesycle[x]).SelectMany(x => x).ToList();
            }

            int answer = termites.Count;

            return answer.ToString();
        }

        // (ctrl/command + click) the link to open the input file
        // file://./../../Inputs/2024/11/2.txt
        public string PartTwo(bool example)
        {
            List<string> lines = Utility.GetInputLines(2024, 11, 2, example);
            Dictionary<string, List<string>> lifesycle = lines.Select(l => l.Split(":")).ToDictionary(l => l[0], l => l[1].Split(",").ToList());

            List<string> termites = ["Z"];

            foreach (int generation in 10)
            {
                termites = termites.Select(x => lifesycle[x]).SelectMany(x => x).ToList();
            }

            int answer = termites.Count;

            return answer.ToString();
        }

        // (ctrl/command + click) the link to open the input file
        // file://./../../Inputs/2024/11/3.txt
        public string PartThree(bool example)
        {
            List<string> lines = Utility.GetInputLines(2024, 11, 3, example);
            Dictionary<string, List<string>> lifesycle = lines.Select(l => l.Split(":")).ToDictionary(l => l[0], l => l[1].Split(",").ToList());

            List<int> results = [];

            foreach (string startingTermite in lifesycle.Keys) {
                List<string> termites = [startingTermite];

                foreach (int generation in 20)
                {
                    termites = termites.Select(x => lifesycle[x]).SelectMany(x => x).ToList();
                }

                results.Add(termites.Count);
            }

            int answer = results.Max() - results.Min();

            return answer.ToString();
        }
    }
}