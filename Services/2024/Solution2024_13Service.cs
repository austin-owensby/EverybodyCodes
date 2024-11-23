namespace EverybodyCodes.Services
{
    public class Solution2024_13Service : ISolutionQuestService
    {
        // (ctrl/command + click) the link to open the input file
        // file://./../../Inputs/2024/13/1.txt
        public string PartOne(bool example)
        {
            List<string> lines = Utility.GetInputLines(2024, 13, 1, example);
            List<List<char>> grid = lines.Select(l => l.ToList()).ToList();

            int startY = grid.FindIndex(y => y.Contains('S'));
            int startX = grid[startY].FindIndex(x => x == 'S');

            Dictionary<string, int> shortestPath = [];
            shortestPath[$"{startX},{startY}"] = 0;

            Queue<Point> queue = new();
            queue.Enqueue(new Point(startX, startY){Value = 0});

            while (queue.Count > 0) {
                Point currentPoint = queue.Dequeue();

                char currentValue = grid[currentPoint.Y][currentPoint.X];
       
                if (currentValue == 'S' || currentValue == 'E') {
                    currentValue = '0';
                }

                int currentLevel = currentValue.ToInt();

                List<Point> points = grid.GetNeighbors(currentPoint.X, currentPoint.Y).Where(p => grid[p.Y][p.X] != '#').ToList();

                foreach (Point point in points) {
                    char value = grid[point.Y][point.X];

                    if (value == 'S' || value == 'E') {
                        value = '0';
                    }

                    int level = value.ToInt();

                    int cost = level > currentLevel ? Math.Min(level - currentLevel, currentLevel - level + 10) : Math.Min(currentLevel - level, level - currentLevel + 10);

                    point.Value = cost + currentPoint.Value + 1;

                    if (shortestPath.TryGetValue($"{point.X},{point.Y}", out int previousCost)) {
                        if (point.Value < previousCost) {
                            shortestPath[$"{point.X},{point.Y}"] = point.Value;
                            queue.Enqueue(point);
                        }
                    }
                    else {
                        shortestPath[$"{point.X},{point.Y}"] = point.Value;
                        queue.Enqueue(point);
                    }
                }
            }

            int endY = grid.FindIndex(y => y.Contains('E'));
            int endX = grid[endY].FindIndex(x => x == 'E');

            int answer = shortestPath[$"{endX},{endY}"];

            return answer.ToString();
        }

        // (ctrl/command + click) the link to open the input file
        // file://./../../Inputs/2024/13/2.txt
        public string PartTwo(bool example)
        {
            List<string> lines = Utility.GetInputLines(2024, 13, 2, example);
            List<List<char>> grid = lines.Select(l => l.ToList()).ToList();

            int startY = grid.FindIndex(y => y.Contains('S'));
            int startX = grid[startY].FindIndex(x => x == 'S');

            Dictionary<string, int> shortestPath = [];
            shortestPath[$"{startX},{startY}"] = 0;

            Queue<Point> queue = new();
            queue.Enqueue(new Point(startX, startY){Value = 0});

            while (queue.Count > 0) {
                Point currentPoint = queue.Dequeue();

                char currentValue = grid[currentPoint.Y][currentPoint.X];
       
                if (currentValue == 'S' || currentValue == 'E') {
                    currentValue = '0';
                }

                int currentLevel = currentValue.ToInt();

                List<Point> points = grid.GetNeighbors(currentPoint.X, currentPoint.Y).Where(p => grid[p.Y][p.X] != '#').ToList();

                foreach (Point point in points) {
                    char value = grid[point.Y][point.X];

                    if (value == 'S' || value == 'E') {
                        value = '0';
                    }

                    int level = value.ToInt();

                    int cost = level > currentLevel ? Math.Min(level - currentLevel, currentLevel - level + 10) : Math.Min(currentLevel - level, level - currentLevel + 10);

                    point.Value = cost + currentPoint.Value + 1;

                    if (shortestPath.TryGetValue($"{point.X},{point.Y}", out int previousCost)) {
                        if (point.Value < previousCost) {
                            shortestPath[$"{point.X},{point.Y}"] = point.Value;
                            queue.Enqueue(point);
                        }
                    }
                    else {
                        shortestPath[$"{point.X},{point.Y}"] = point.Value;
                        queue.Enqueue(point);
                    }
                }
            }

            int endY = grid.FindIndex(y => y.Contains('E'));
            int endX = grid[endY].FindIndex(x => x == 'E');

            int answer = shortestPath[$"{endX},{endY}"];

            return answer.ToString();
        }

        // (ctrl/command + click) the link to open the input file
        // file://./../../Inputs/2024/13/3.txt
        public string PartThree(bool example)
        {
            List<string> lines = Utility.GetInputLines(2024, 13, 3, example);
            List<List<char>> grid = lines.Select(l => l.ToList()).ToList();

            List<Point> startingPoints = [];

            int endY = 0;
            int endX = 0;

            foreach (int y in grid.Count) {
                foreach (int x in grid[y].Count) {
                    if (grid[y][x] == 'S') {
                        startingPoints.Add(new(x, y){Value = 0});
                    }
                    else if (grid[y][x] == 'E')
                    {
                        endX = x;
                        endY = y;
                    }
                }
            }

            int answer = int.MaxValue;
            
            foreach (Point startingPoint in startingPoints) {
                Dictionary<string, int> shortestPath = [];
                shortestPath[$"{startingPoint.X},{startingPoint.Y}"] = 0;

                Queue<Point> queue = new();
                queue.Enqueue(startingPoint);

                while (queue.Count > 0) {
                    Point currentPoint = queue.Dequeue();

                    char currentValue = grid[currentPoint.Y][currentPoint.X];
        
                    if (currentValue == 'S' || currentValue == 'E') {
                        currentValue = '0';
                    }

                    int currentLevel = currentValue.ToInt();

                    List<Point> points = grid.GetNeighbors(currentPoint.X, currentPoint.Y).Where(p => grid[p.Y][p.X] != '#').ToList();

                    foreach (Point point in points) {
                        char value = grid[point.Y][point.X];

                        if (value == 'S' || value == 'E') {
                            value = '0';
                        }

                        int level = value.ToInt();

                        int cost = level > currentLevel ? Math.Min(level - currentLevel, currentLevel - level + 10) : Math.Min(currentLevel - level, level - currentLevel + 10);

                        point.Value = cost + currentPoint.Value + 1;

                        if (answer > point.Value)
                        {
                            if (shortestPath.TryGetValue($"{point.X},{point.Y}", out int previousCost)) {
                                if (point.Value < previousCost) {
                                    shortestPath[$"{point.X},{point.Y}"] = point.Value;
                                    queue.Enqueue(point);
                                }
                            }
                            else {
                                shortestPath[$"{point.X},{point.Y}"] = point.Value;
                                queue.Enqueue(point);
                            }
                        }
                    }
                }

                if (shortestPath.TryGetValue($"{endX},{endY}", out int pointAnswer)) {
                    pointAnswer = shortestPath[$"{endX},{endY}"];
                    answer = Math.Min(answer, pointAnswer);
                }
            }

            return answer.ToString();
        }
    }
}