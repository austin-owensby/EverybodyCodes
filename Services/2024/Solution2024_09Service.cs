namespace EverybodyCodes.Services
{
    public class Solution2024_09Service : ISolutionQuestService
    {
        // (ctrl/command + click) the link to open the input file
        // file://./../../Inputs/2024/09/1.txt
        public string PartOne(bool example)
        {
            List<string> lines = Utility.GetInputLines(2024, 9, 1, example);
            List<int> brightnesses = lines.ToInts();
            List<int> availableStamps = [1, 3, 5, 10];
            availableStamps = availableStamps.OrderDescending().ToList();

            int answer = 0;
            
            foreach (int brightness in brightnesses)
            {
                int remainder = brightness;
                while (remainder > 0) {
                    foreach (int stamp in availableStamps) {
                        if (stamp <= remainder) {
                            answer++;
                            remainder -= stamp;
                            break;
                        }
                    }
                }
            }

            return answer.ToString();
        }

        // (ctrl/command + click) the link to open the input file
        // file://./../../Inputs/2024/09/2.txt
        public string PartTwo(bool example)
        {
            List<string> lines = Utility.GetInputLines(2024, 9, 2, example);
            List<int> brightnesses = lines.ToInts();
            List<int> availableStamps = [1, 3, 5, 10, 15, 16, 20, 24, 25, 30];

            // Map sums up to length 5
            Dictionary<int, int> bestStampToBrightness = [];
            bestStampToBrightness[0] = 0;

            foreach (int i in availableStamps) {
                bestStampToBrightness[i] = 1;
                foreach (int j in availableStamps) {
                    if (!bestStampToBrightness.ContainsKey(i + j) || bestStampToBrightness[i + j] > 2) {
                        bestStampToBrightness[i + j] = 2;
                    }
                    foreach (int k in availableStamps) {
                        if (!bestStampToBrightness.ContainsKey(i + j + k) || bestStampToBrightness[i + j + k] > 3) {
                            bestStampToBrightness[i + j + k] = 3;
                        }
                        foreach (int l in availableStamps) {
                            if (!bestStampToBrightness.ContainsKey(i + j + k + l) || bestStampToBrightness[i + j + k + l] > 4) {
                                bestStampToBrightness[i + j + k + l] = 4;
                            }
                            foreach (int m in availableStamps) {
                                if (!bestStampToBrightness.ContainsKey(i + j + k + l + m)) {
                                    bestStampToBrightness[i + j + k + l + m] = 5;
                                }
                            }
                        }
                    }
                }
            }

            List<int> results = [];

            foreach (int brightness in brightnesses) {
                int remainder = brightness % availableStamps.Last();
                int quotient = (brightness - remainder) / availableStamps.Last();

                List<int> options = [
                        quotient + bestStampToBrightness[remainder],
                        quotient >= 1 ? quotient - 1 + bestStampToBrightness[remainder + availableStamps.Last()] : int.MaxValue,
                        quotient >= 2 ? quotient - 2 + bestStampToBrightness[remainder + 2 * availableStamps.Last()] : int.MaxValue,
                        quotient >= 3 ? quotient - 3 + bestStampToBrightness[remainder + 3 * availableStamps.Last()] : int.MaxValue
                    ];

                int bestPlan = options.Min();
                results.Add(bestPlan);
            }

            int answer = results.Sum();
            
            return answer.ToString();
        }

        // (ctrl/command + click) the link to open the input file
        // file://./../../Inputs/2024/09/3.txt
        public string PartThree(bool example)
        {
            List<string> lines = Utility.GetInputLines(2024, 9, 3, example);
            List<int> brightnesses = lines.ToInts();
            List<int> availableStamps = [1, 3, 5, 10, 15, 16, 20, 24, 25, 30, 37, 38, 49, 50, 74, 75, 100, 101];

            List<int> results = [];

            foreach (int brightness in brightnesses) {
                int bestPlan = int.MaxValue;

                bool isOdd = brightness % 2 == 1;

                double midPoint = brightness / 2.0;

                foreach (int i in isOdd ? 50 : 51) {
                    int brightnessA = (int)(midPoint + i + (isOdd ? 0.5 : 0));
                    int brightnessB = (int)(midPoint - i - (isOdd ? 0.5 : 0));

                    // Because these are large numbers and our largest stamps are 100 and 101
                    //    Each brightness can be minimally described as a sum of x * 100 + y * 101
                    int remainder = brightnessA % 101;
                    int hundredCount = remainder == 0 ? 0 : (101 - remainder);
                    int hundredOneCount = (brightnessA - 100 * hundredCount) / 101;

                    int bestPlanA = hundredCount + hundredOneCount;

                    remainder = brightnessB % 101;
                    hundredCount = remainder == 0 ? 0 : (101 - remainder);
                    hundredOneCount = (brightnessB - 100 * hundredCount) / 101;

                    int bestPlanB = hundredCount + hundredOneCount;

                    int plan = bestPlanA + bestPlanB;

                    if (plan < bestPlan) {
                        bestPlan = plan;
                    }
                }
                results.Add(bestPlan);
            }

            int answer = results.Sum();

            return answer.ToString();
        }
    }
}