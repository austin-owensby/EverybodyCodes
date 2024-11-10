namespace EverybodyCodes.Services
{
    // (ctrl/command + click) the link to open the input file
    // file://./../../Inputs/2024/01.txt
    public class Solution2024_01Service : ISolutionQuestService
    {
        public string PartOne(bool example)
        {
            List<string> lines = Utility.GetInputLines(2024, 1, 1, example);
            string input = lines.First();

            int bCount = input.Count(i => i == 'B');
            int cCount = input.Count(i => i == 'C');

            int answer = bCount + 3 * cCount;

            return answer.ToString();
        }

        public string PartTwo(bool example)
        {
            List<string> lines = Utility.GetInputLines(2024, 1, 2, example);
            string input = lines.First();

            int answer = 0;

            foreach (char[] pair in input.Chunk(2)) {
                answer += pair.Contains('x') ? 0 : 2;

                answer += pair[0] switch {
                    'B' => 1,
                    'C' => 3, 
                    'D' => 5,
                    _ => 0
                };

                answer += pair[1] switch {
                    'B' => 1,
                    'C' => 3, 
                    'D' => 5,
                    _ => 0
                };
            }
            
            return answer.ToString();
        }

        public string PartThree(bool example)
        {
            List<string> lines = Utility.GetInputLines(2024, 1, 3, example);
            string input = lines.First();

            int answer = 0;

            foreach (char[] group in input.Chunk(3)) {
                int xCount = group.Count(c => c == 'x');
                
                answer += xCount switch {
                    0 => 6,
                    1 => 2,
                    _ => 0
                };

                foreach (int i in group.Length) {
                    answer += group[i] switch {
                        'B' => 1,
                        'C' => 3, 
                        'D' => 5,
                        _ => 0
                    };
                }
            }

            return answer.ToString();
        }
    }
}