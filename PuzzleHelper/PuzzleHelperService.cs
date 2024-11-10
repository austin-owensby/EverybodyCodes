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
                public class Solution{{year}}_{{quest:D2}}Service : ISolutionQuestService
                {
                    // (ctrl/command + click) the link to open the input file
                    // file://./../../Inputs/{{year}}/{{quest:D2}}_part1.txt
                    public string PartOne(bool example)
                    {
                        List<string> lines = Utility.GetInputLines({{year}}, {{quest}}, 1, example);

                        int answer = 0;

                        foreach (string line in lines)
                        {

                        }

                        return answer.ToString();
                    }

                    // (ctrl/command + click) the link to open the input file
                    // file://./../../Inputs/{{year}}/{{quest:D2}}_part2.txt
                    public string PartTwo(bool example)
                    {
                        List<string> lines = Utility.GetInputLines({{year}}, {{quest}}, 2, example);

                        int answer = 0;

                        foreach (string line in lines)
                        {

                        }

                        return answer.ToString();
                    }

                    // (ctrl/command + click) the link to open the input file
                    // file://./../../Inputs/{{year}}/{{quest:D2}}_part3.txt
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
                bool firstFullWeek = false;
        
                // Iterate over all days of November
                // TODO, what is the pattern for start dates?
                //  Right now we just have 1 year of data, 2024, so we can't know the intended pattern
                //  If we assume that it's the first 4 full weeks of November, then years with November starting on a Tuesday or Wednesday won't have 4 weeks that have 5 weekdays in November
                //  Maybe in these cases we'll have puzzles on December 1st and 2nd?
                //  Regardless, this won't be a problem until 2028
                //  For now move forward with our assumption that we'll move into December
                for (int day = 1; day <= 30; day++)
                {
                    DateTime date = new DateTime(latestPuzzleYear, Globals.EVENT_MONTH, day);

                    if (date.Day > now.Day) {
                        break;
                    }

                    // Check if it's a weekday (Monday to Friday)
                    if (date.DayOfWeek != DayOfWeek.Saturday && date.DayOfWeek != DayOfWeek.Sunday)
                    {
                        if (date.DayOfWeek == DayOfWeek.Monday && !firstFullWeek) {
                            firstFullWeek = true;
                        }
                        else if (!firstFullWeek) {
                            continue;
                        }

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
                else {
                    latestPuzzleQuest = todaysQuest;
                }
            }
            else
            {
                // This logic assumes that if November starts on a Tuesday or Wednesday then the final quests will be in December
                DateTime novemberFirst = new DateTime(now.Year, Globals.EVENT_MONTH, 1);
                bool tuesdayIssue = now.Month == Globals.EVENT_MONTH + 1 && now.Day == 1 && novemberFirst.DayOfWeek == DayOfWeek.Tuesday;
                bool wednesdayIssue = now.Month == Globals.EVENT_MONTH + 1 && now.Day <= 2 && novemberFirst.DayOfWeek == DayOfWeek.Wednesday;

                if (tuesdayIssue) {
                    // If November starts on a Tuesday, quest 20 is on December 1st
                    latestPuzzleYear = now.Year;
                    latestPuzzleQuest = Globals.LAST_PUZZLE;
                }
                else if (wednesdayIssue) {
                    // If November starts on a Wednesday, quest 19 is on December 1st and 20 on December 2nd
                    latestPuzzleYear = now.Year;
                    latestPuzzleQuest = Globals.LAST_PUZZLE - (now.Day == 1 ? 1 : 0);
                }
                else {
                    // Otherwise the latest puzzle is from the end of the previous event
                    latestPuzzleYear = now.Year - 1;
                    latestPuzzleQuest = Globals.LAST_PUZZLE;
                }
            }

            return Tuple.Create(latestPuzzleYear, latestPuzzleQuest);
        }
    }
}