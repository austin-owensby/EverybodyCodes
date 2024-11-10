using EverybodyCodes.Gateways;

namespace EverybodyCodes.PuzzleHelper
{
    public class PuzzleHelperService(EverybodyCodesGateway everybodyCodesGateway)
    {
        private readonly EverybodyCodesGateway everybodyCodesGateway = everybodyCodesGateway;

        /// <summary>
        /// Generates solution files.
        /// </summary>
        /// <returns></returns>
        public async Task<string> GenerateMissingSolutionServiceFiles()
        {
            string output = string.Empty;

            bool update = false;

            // Create a folder for each year that is missing one
            DateTime now = DateTime.UtcNow.AddHours(Globals.SERVER_UTC_OFFSET);
            for (int year = Globals.START_YEAR; year <= now.Year; year++)
            {
                string yearFolderPath = Path.Combine(Environment.CurrentDirectory, $"Services/{year}");

                if (!Directory.Exists(yearFolderPath))
                {
                    Directory.CreateDirectory(yearFolderPath);
                    Console.WriteLine($"Created folder for {year}.");
                    output += $"Created folder for {year}.\n";
                    update = true;
                }

                // Create/update files for each quest that is missing one
                for (int quest = 1; quest <= Globals.LAST_PUZZLE; quest++)
                {
                    string questFilePath = Path.Combine(yearFolderPath, $"Solution{year}_{quest:D2}Service.cs");

                    if (!File.Exists(questFilePath))
                    {
                        // Initialize the new service file
                        using StreamWriter serviceFile = new(questFilePath);

                        await serviceFile.WriteAsync($$"""
            namespace EverybodyCodes.Services
            {
                // (ctrl/command + click) the link to open the input file
                // file://./../../Inputs/{{year}}/{{quest:D2}}.txt
                public class Solution{{year}}_{{quest:D2}}Service : ISolutionQuestService
                {
                    public string PartOne(bool example)
                    {
                        List<string> lines = Utility.GetInputLines({{year}}, {{quest}}, 1, example);

                        int answer = 0;

                        foreach (string line in lines)
                        {

                        }

                        return answer.ToString();
                    }

                    public string PartTwo(bool example)
                    {
                        List<string> lines = Utility.GetInputLines({{year}}, {{quest}}, 2, example);

                        int answer = 0;

                        foreach (string line in lines)
                        {

                        }

                        return answer.ToString();
                    }

                    public string PartThree(bool example)
                    {
                        List<string> lines = Utility.GetInputLines({{year}}, {{quest}}, 3, example);

                        int answer = 0;

                        foreach (string line in lines)
                        {

                        }

                        return answer.ToString();
                    }
                }
            }
            """);

                        Console.WriteLine($"Created solution file for Year: {year}, Quest: {quest}.");
                        output += $"Created solution file for Year: {year}, Quest: {quest}.\n";
                        update = true;
                    }
                }
            }

            if (!update)
            {
                Console.WriteLine("No updates applied.");
                output += "No updates applied.\n";
            }

            return output;
        }

        /// <summary>
        /// Imports the quest's input file.
        /// </summary>
        /// <param name="year"></param>
        /// <param name="quest"></param>
        /// <returns></returns>
        public async Task<string> ImportInputFile(int year, int quest)
        {
            string output = string.Empty;

            Tuple<int, int> latestResults = GetLatestYearAndDate();
            int latestPuzzleYear = latestResults.Item1;
            int latestPuzzleQuest = latestResults.Item2;

            if (latestPuzzleYear < year || (latestPuzzleYear == year && latestPuzzleQuest < quest))
            {
                Console.WriteLine("No updates applied.");
                output += "No updates applied.\n";
            }
            else
            {
                bool update = await WriteInputFile(year, quest);

                if (update)
                {
                    output = $"Created input file for Year: {year}, Quest: {quest}.";
                }
                else
                {
                    Console.WriteLine("No updates applied.");
                    output += "No updates applied.\n ";
                }
            }

            return output;
        }

        /// <summary>
        /// Fetch and write the input file if it doesn't exist
        /// </summary>
        /// <param name="year"></param>
        /// <param name="quest"></param>
        /// <returns></returns>
        private async Task<bool> WriteInputFile(int year, int quest)
        {
            bool update = false;

            string yearFolderPath = Path.Combine(Environment.CurrentDirectory, $"Inputs/{year}");

            if (!Directory.Exists(yearFolderPath))
            {
                Directory.CreateDirectory(yearFolderPath);
            }

            string inputFilePath = Path.Combine(Environment.CurrentDirectory, $"Inputs/{year}/{quest:D2}.txt");

            if (!File.Exists(inputFilePath))
            {
                string response;
                try
                {
                    response = await everybodyCodesGateway.ImportInput(year, quest);
                }
                catch (Exception)
                {
                    Console.WriteLine("An error occurred while getting the puzzle input from Everybody Codes");
                    throw;
                }

                using StreamWriter inputFile = new(inputFilePath);
                await inputFile.WriteAsync(response);

                Console.WriteLine($"Created input file for Year: {year}, Quest: {quest}.");
                update = true;
            }

            return update;
        }

        /// <summary>
        /// Based on today's date, calculate the latest Everybody Codes year and quest available
        /// </summary>
        /// <returns></returns>
        private static Tuple<int, int> GetLatestYearAndDate()
        {
            DateTime now = DateTime.UtcNow.AddHours(Globals.SERVER_UTC_OFFSET);
            int latestPuzzleYear, latestPuzzleQuest;

            // If we're in November, then the latest available puzzle is today
            if (now.Month == Globals.EVENT_MONTH)
            {
                latestPuzzleYear = now.Year;

                int todaysQuest = 0;
        
                // Iterate over all days of November
                for (int day = 1; day <= 30; day++)
                {
                    DateTime date = new DateTime(latestPuzzleYear, Globals.EVENT_MONTH, day);

                    if (date.Day > now.Day) {
                        break;
                    }

                    // Check if it's a weekday (Monday to Friday)
                    if (date.DayOfWeek != DayOfWeek.Saturday && date.DayOfWeek != DayOfWeek.Sunday)
                    {
                        todaysQuest++;

                        if (todaysQuest == Globals.LAST_PUZZLE) {
                            break;
                        }
                    }
                }

                if (todaysQuest == 0) {
                    // Otherwise the latest puzzle is from the end of the previous event
                    latestPuzzleYear = now.Year - 1;
                    latestPuzzleQuest = Globals.LAST_PUZZLE;
                }
                latestPuzzleQuest = todaysQuest;
            }
            else
            {
                // Otherwise the latest puzzle is from the end of the previous event
                latestPuzzleYear = now.Year - 1;
                latestPuzzleQuest = Globals.LAST_PUZZLE;
            }

            return Tuple.Create(latestPuzzleYear, latestPuzzleQuest);
        }
    }
}