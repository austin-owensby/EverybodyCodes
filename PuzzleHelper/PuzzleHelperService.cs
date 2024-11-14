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
                    // file://./../../Inputs/{{year}}/{{quest:D2}}/1.txt
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
                    // file://./../../Inputs/{{year}}/{{quest:D2}}/2.txt
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
                    // file://./../../Inputs/{{year}}/{{quest:D2}}/3.txt
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
        /// Imports the quest's part's input file.
        /// </summary>
        /// <param name="year"></param>
        /// <param name="quest"></param>
        /// <param name="part"></param>
        /// <returns></returns>
        public async Task<string> ImportInputFile(int year, int quest, int part)
        {
            string output = string.Empty;

            Tuple<int, int> latestResults = GetLatestYearAndDate();
            int latestPuzzleYear = latestResults.Item1;
            int latestPuzzleQuest = latestResults.Item2;

            if (latestPuzzleYear < year || (latestPuzzleYear == year && latestPuzzleQuest < quest))
            {
                Console.WriteLine("The file is not available yet.");
                output += "The file is not available yet.\n";
            }
            else
            {
                bool update = await WriteInputFile(year, quest, part);

                if (update)
                {
                    output = $"Created input file for Year: {year}, Quest: {quest}, Part: {part}.";
                }
                else
                {
                    Console.WriteLine("The file already exists.");
                    output += "The file already exists.\n ";
                }
            }

            return output;
        }

        /// <summary>
        /// Fetch and write the input file if it doesn't exist
        /// </summary>
        /// <param name="year"></param>
        /// <param name="quest"></param>
        /// <param name="part"></param>
        /// <returns></returns>
        private async Task<bool> WriteInputFile(int year, int quest, int part)
        {
            bool update = false;

            string yearFolderPath = Path.Combine(Environment.CurrentDirectory, $"Inputs/{year}");

            if (!Directory.Exists(yearFolderPath))
            {
                Directory.CreateDirectory(yearFolderPath);
            }

            string dayFolderPath = Path.Combine(Environment.CurrentDirectory, $"Inputs/{year}/{quest:D2}");

            if (!Directory.Exists(dayFolderPath))
            {
                Directory.CreateDirectory(dayFolderPath);
            }

            string inputFilePath = Path.Combine(Environment.CurrentDirectory, $"Inputs/{year}/{quest:D2}/{part}.txt");

            if (!File.Exists(inputFilePath))
            {
                string response;
                try
                {
                    response = await everybodyCodesGateway.ImportInput(year, quest, part);
                }
                catch (Exception)
                {
                    Console.WriteLine("An error occurred while getting the puzzle input from Everybody Codes");
                    throw;
                }

                using StreamWriter inputFile = new(inputFilePath);
                await inputFile.WriteAsync(response);

                Console.WriteLine($"Created input file for Year: {year}, Quest: {quest}, Part: {part}.");
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

            // This calculation is left as an exercise to the reader
            List<DateTime> dates = Enumerable.Range(1, 4 * 7).Where(d => (d - 1) % 7 < 5).Select(d => new DateTime(now.Year, Globals.EVENT_MONTH, d).AddDays((10 - (int)new DateTime(now.Year, Globals.EVENT_MONTH, 1).DayOfWeek) % 7 - 2)).ToList();

            if (now < dates[0]) {
                // The event has not started yet
                latestPuzzleYear = now.Year - 1;
                latestPuzzleQuest = Globals.LAST_PUZZLE;
            }
            else {
                latestPuzzleYear = now.Year;
                latestPuzzleQuest = dates.Count(d => d < now);
            }

            return Tuple.Create(latestPuzzleYear, latestPuzzleQuest);
        }
    }
}