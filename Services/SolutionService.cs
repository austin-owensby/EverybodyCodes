using EverybodyCodes.Gateways;

namespace EverybodyCodes.Services
{
    public class SolutionService(IServiceProvider serviceProvider, EverybodyCodesGateway everybodyCodesGateway)
    {
        private readonly IServiceProvider serviceProvider = serviceProvider;
        private readonly EverybodyCodesGateway everybodyCodesGateway = everybodyCodesGateway;

        /// <summary>
        /// Execute the specific solution based on the passed in parameters
        /// </summary>
        /// <param name="year"></param>
        /// <param name="quest"></param>
        /// <param name="part"></param>
        /// <param name="send"></param>
        /// <param name="example"></param>
        /// <returns></returns>
        /// <exception cref="SolutionNotFoundException"></exception>
        public async Task<string> GetSolution(int year, int quest, int part, bool send, bool example)
        {
            ISolutionQuestService service = FindSolutionService(year, quest);

            // Run the specific solution
            string answer = part switch {
                1 => service.PartOne(example),
                2 => service.PartTwo(example),
                3 => service.PartThree(example),
                _ => throw new Exception($"Unknown part: {part}, expected 1, 2, or 3.")
            };

            // Optionally submit the answer to Everybody Codes
            if (send)
            {
                try
                {
                    string response = await everybodyCodesGateway.SubmitAnswer(year, quest, part, answer);
                    answer = $"Submitted answer: {answer}.\nEverybody Codes response: {response}";
                }
                catch (Exception e)
                {
                    Console.WriteLine("An error occurred while submitting the answer to Everybody Codes");
                    answer = $"Submitted answer: {answer}.\nEverybody Codes response: {e.Message}";
                }
            }

            return answer;
        }

        /// <summary>
        /// Fetch the specific service for the specified year and quest
        /// </summary>
        /// <param name="year"></param>
        /// <param name="quest"></param>
        /// <returns></returns>
        private ISolutionQuestService FindSolutionService(int year, int quest)
        {
            IEnumerable<ISolutionQuestService> services = serviceProvider.GetServices<ISolutionQuestService>();

            // Use ':D2' to front pad 0s to single digit quests to match the formatting
            string serviceName = $"EverybodyCodes.Services.Solution{year}_{quest:D2}Service";
            ISolutionQuestService? service = services.FirstOrDefault(s => s.GetType().ToString() == serviceName);

            // If the service was not found, throw an exception
            if (service == null)
            {
                throw new SolutionNotFoundException($"No solutions found for quest {quest}/{year}.");
            }

            return service;
        }
    }
}