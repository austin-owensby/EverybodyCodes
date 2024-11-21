namespace EverybodyCodes.Services
{
    public class Solution2024_10Service : ISolutionQuestService
    {
        // (ctrl/command + click) the link to open the input file
        // file://./../../Inputs/2024/10/1.txt
        public string PartOne(bool example)
        {
            List<string> lines = Utility.GetInputLines(2024, 10, 1, example);
            List<List<char>> grid = lines.Select(l => l.ToList()).ToList();

            string answer = "";

            for (int y = 2; y <= 5; y++)
            {
                for (int x = 2; x <= 5; x++)
                {
                    List<char> columnLetters = grid[y].Where(c => c != '.').ToList();
                    List<char> rowLetters = grid.Select(c => c[x]).Where(c => c != '.').ToList();

                    char letter = columnLetters.Intersect(rowLetters).FirstOrDefault();
                    answer += letter;
                }
            }

            return answer;
        }

        // (ctrl/command + click) the link to open the input file
        // file://./../../Inputs/2024/10/2.txt
        public string PartTwo(bool example)
        {
            List<string> lines = Utility.GetInputLines(2024, 10, 2, example);

            List<List<List<char>>> grids = lines
                        .ChunkByExclusive(string.IsNullOrWhiteSpace)
                        .Select(x =>
                            x
                            .Pivot()
                            .Select(x => new string(x.ToArray()))
                            .ChunkByExclusive(string.IsNullOrWhiteSpace)
                            .Select(x => x.Pivot())
                            .ToList()
                        )
                        .SelectMany(x => x)
                        .ToList();

            int answer = 0;

            foreach (List<List<char>> grid in grids)
            {
                int i = 1;
                for (int y = 2; y <= 5; y++)
                {
                    for (int x = 2; x <= 5; x++)
                    {
                        List<char> columnLetters = grid[y].Where(c => c != '.').ToList();
                        List<char> rowLetters = grid.Select(c => c[x]).Where(c => c != '.').ToList();

                        char letter = columnLetters.Intersect(rowLetters).FirstOrDefault();
                        answer += i * Utility.GetCharValue(char.ToLower(letter));
                        i++;
                    }
                }
            }

            return answer.ToString();
        }

        // (ctrl/command + click) the link to open the input file
        // file://./../../Inputs/2024/10/3.txt
        public string PartThree(bool example)
        {
            List<string> lines = Utility.GetInputLines(2024, 10, 3, example);

            List<List<char>> grids = lines.Select(l => l.ToList()).ToList();

            bool hasUpdate = true;

            while (hasUpdate)
            {
                hasUpdate = false;
                for (int yOffset = 0; yOffset < grids.Count - 2; yOffset += 6)
                {
                    for (int xOffset = 0; xOffset < grids.First().Count - 2; xOffset += 6)
                    {
                        for (int y = yOffset + 2; y < 6 + yOffset; y++)
                        {
                            for (int x = xOffset + 2; x < 6 + xOffset; x++)
                            {
                                if (grids[y][x] == '.') {
                                    List<char> columnLetters = [grids[y][xOffset], grids[y][xOffset + 1], grids[y][xOffset + 6], grids[y][xOffset + 7]];
                                    List<char> rowLetters = [grids[yOffset][x], grids[yOffset + 1][x], grids[yOffset + 6][x], grids[yOffset + 7][x]];

                                    List<char> intersection = columnLetters.Intersect(rowLetters).ToList();

                                    if (intersection.Count == 1) {
                                        grids[y][x] = intersection[0];
                                        hasUpdate = true;
                                    }
                                    else {
                                        List<char> otherColumnLetters = [];
                                        List<char> otherRowLetters = [];
                                        
                                        foreach (int i in 4) {
                                            if (xOffset + i + 2 != x) {
                                                otherColumnLetters.Add(grids[y][xOffset + i + 2]);
                                            }
                                            if (yOffset + i + 2 != y) {
                                                otherRowLetters.Add(grids[yOffset + i + 2][x]);
                                            }
                                        }

                                        if (!(otherColumnLetters.Any(c => c == '.') || otherRowLetters.Any(c => c == '.'))) {
                                            char colChar = columnLetters.Except(otherColumnLetters).First();
                                            char rowChar = rowLetters.Except(otherRowLetters).First();

                                            if (colChar == '?' && rowChar != '?') {
                                                grids[y][x] = rowChar;
                                                hasUpdate = true;


                                                if (grids[y][xOffset] == '?') {
                                                    grids[y][xOffset] = rowChar;
                                                }
                                                else if (grids[y][xOffset + 1] == '?') {
                                                    grids[y][xOffset + 1] = rowChar;
                                                }
                                                else if (grids[y][xOffset + 6] == '?') {
                                                    grids[y][xOffset + 6] = rowChar;
                                                }
                                                else if (grids[y][xOffset + 7] == '?') {
                                                    grids[y][xOffset + 7] = rowChar;
                                                }
                                            }
                                            else if (rowChar == '?' && colChar != '?') {
                                                grids[y][x] = colChar;
                                                hasUpdate = true;

                                                if (grids[yOffset][x] == '?') {
                                                    grids[yOffset][x] = colChar;
                                                }
                                                else if (grids[yOffset + 1][x] == '?') {
                                                    grids[yOffset + 1][x] = colChar;
                                                }
                                                else if (grids[yOffset + 6][x] == '?') {
                                                    grids[yOffset + 6][x] = colChar;
                                                }
                                                else if (grids[yOffset + 7][x] == '?') {
                                                    grids[yOffset + 7][x] = colChar;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }  
            }

            int answer = 0;

            for (int yOffset = 0; yOffset < grids.Count - 2; yOffset += 6)
            {
                for (int xOffset = 0; xOffset < grids.First().Count - 2; xOffset += 6)
                {
                    int gridScore = 0;
                    bool validRuneWord = true;
                    int i = 1;
                    for (int y = yOffset + 2; y < 6 + yOffset; y++)
                    {
                        if (!validRuneWord) {
                            break;
                        }
                        for (int x = xOffset + 2; x < 6 + xOffset; x++)
                        {
                            char letter = grids[y][x];
                            if (letter == '.') {
                                validRuneWord = false;
                                gridScore = 0;
                                break;
                            }
                            gridScore += i * Utility.GetCharValue(char.ToLower(letter));
                            i++;
                        }
                    }
                    answer += gridScore;
                }
            }

            return answer.ToString();
        }
    }
}