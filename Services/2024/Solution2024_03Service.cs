namespace EverybodyCodes.Services
{
    public class Solution2024_03Service : ISolutionQuestService
    {
        // (ctrl/command + click) the link to open the input file
        // file://./../../Inputs/2024/03/1.txt
        public string PartOne(bool example)
        {
            List<string> lines = Utility.GetInputLines(2024, 3, 1, example);
            List<List<char>> grid = lines.Select(x => x.ToList()).ToList();

            int height = grid.Count;
            int width = grid.First().Count;

            int answer = 0;

            List<int> layerCounts = [];

            // Loop until all the spots are dug
            while (grid.Any(row => row.Any(cell => cell == '#')))
            {
                List<List<char>> newGrid = grid.Select(r => r.ToList()).ToList();
                int layerCount = 0;
                // Loop over each row
                foreach (int y in height)
                {
                    // Loop over each column
                    foreach (int x in width)
                    {
                        // Check if this cell is approved to dig at
                        if (grid[y][x] == '#') {
                            // Check if there is an adjacent empty neighbor to maintain a safe slope
                            List<Point> neighbors = grid.GetNeighbors(x, y);

                            if (neighbors.Any(n => grid[n.Y][n.X] == '.')) {
                                layerCount++;
                                newGrid[y][x] = '.';
                            }
                        }
                    }
                }
            
                grid = newGrid;
                layerCounts.Add(layerCount);
            }

            foreach (int i in layerCounts.Count) {
                answer += (i + 1) * layerCounts[i];
            }

            return answer.ToString();
        }

        // (ctrl/command + click) the link to open the input file
        // file://./../../Inputs/2024/03/2.txt
        public string PartTwo(bool example)
        {
            List<string> lines = Utility.GetInputLines(2024, 3, 2, example);
            List<List<char>> grid = lines.Select(x => x.ToList()).ToList();

            int height = grid.Count;
            int width = grid.First().Count;

            int answer = 0;

            List<int> layerCounts = [];

            // Loop until all the spots are dug
            while (grid.Any(row => row.Any(cell => cell == '#')))
            {
                List<List<char>> newGrid = grid.Select(r => r.ToList()).ToList();
                int layerCount = 0;
                // Loop over each row
                foreach (int y in height)
                {
                    // Loop over each column
                    foreach (int x in width)
                    {
                        // Check if this cell is approved to dig at
                        if (grid[y][x] == '#') {
                            // Check if there is an adjacent empty neighbor to maintain a safe slope
                            List<Point> neighbors = grid.GetNeighbors(x, y);

                            if (neighbors.Any(n => grid[n.Y][n.X] == '.')) {
                                layerCount++;
                                newGrid[y][x] = '.';
                            }
                        }
                    }
                }
            
                grid = newGrid;
                layerCounts.Add(layerCount);
            }

            foreach (int i in layerCounts.Count) {
                answer += (i + 1) * layerCounts[i];
            }

            return answer.ToString();
        }

        // (ctrl/command + click) the link to open the input file
        // file://./../../Inputs/2024/03/3.txt
        public string PartThree(bool example)
        {
            List<string> lines = Utility.GetInputLines(2024, 3, 3, example);
            List<List<char>> grid = lines.Select(x => x.ToList()).ToList();

            int height = grid.Count;
            int width = grid.First().Count;

            int answer = 0;

            List<int> layerCounts = [];

            // Loop until all the spots are dug
            while (grid.Any(row => row.Any(cell => cell == '#')))
            {
                List<List<char>> newGrid = grid.Select(r => r.ToList()).ToList();
                int layerCount = 0;
                // Loop over each row
                foreach (int y in height)
                {
                    // Loop over each column
                    foreach (int x in width)
                    {
                        // Check if this cell is approved to dig at
                        if (grid[y][x] == '#') {
                            // Check if there is an adjacent empty neighbor to maintain a safe slope
                            List<Point> neighbors = grid.GetNeighbors(x, y, true);

                            // If neighbors is < 8, then we're at a border, assume it's good to dig here
                            if (neighbors.Count < 8 || neighbors.Any(n => grid[n.Y][n.X] == '.')) {
                                layerCount++;
                                newGrid[y][x] = '.';
                            }
                        }
                    }
                }
            
                grid = newGrid;
                layerCounts.Add(layerCount);
            }

            foreach (int i in layerCounts.Count) {
                answer += (i + 1) * layerCounts[i];
            }

            return answer.ToString();
        }
    }
}