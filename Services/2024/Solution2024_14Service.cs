namespace EverybodyCodes.Services
{
    public class Solution2024_14Service : ISolutionQuestService
    {
        // (ctrl/command + click) the link to open the input file
        // file://./../../Inputs/2024/14/1.txt
        public string PartOne(bool example)
        {
            List<string> lines = Utility.GetInputLines(2024, 14, 1, example);

            List<char> heightChars = ['U','D'];

            List<int> heightChanges = lines[0].Split(',').Where(x => heightChars.Contains(x[0])).Select(x => x.Replace("U","").Replace("D","-")).ToInts();

            int answer = 0;
            int height = 0;

            foreach (int heightChange in heightChanges)
            {
                height += heightChange;

                answer = Math.Max(answer, height);
            }

            return answer.ToString();
        }

        // (ctrl/command + click) the link to open the input file
        // file://./../../Inputs/2024/14/2.txt
        public string PartTwo(bool example)
        {
            List<string> lines = Utility.GetInputLines(2024, 14, 2, example);

            List<List<(char, int)>> directionSets = lines.Select(line => line.Split(',').Select(x => (x[0], int.Parse(x.Substring(1, x.Length - 1)))).ToList()).ToList();

            List<Point3D> points = [];

            foreach (List<(char, int)> directionSet in directionSets)
            {
                Point3D currentPoint = new(0, 0, 0);

                foreach ((char, int) direction in directionSet)
                {
                    foreach (int i in direction.Item2)
                    {
                        Point3D nextPoint = direction.Item1 switch
                        {
                            'U' => new(currentPoint.X, currentPoint.Y + 1, currentPoint.Z),
                            'D' => new(currentPoint.X, currentPoint.Y - 1, currentPoint.Z),
                            'L' => new(currentPoint.X - 1, currentPoint.Y, currentPoint.Z),
                            'R' => new(currentPoint.X + 1, currentPoint.Y, currentPoint.Z),
                            'F' => new(currentPoint.X, currentPoint.Y, currentPoint.Z + 1),
                            'B' => new(currentPoint.X, currentPoint.Y, currentPoint.Z - 1),
                            _ => throw new NotImplementedException()
                        };

                        currentPoint = nextPoint;

                        if (!points.Any(p => p.X == nextPoint.X && p.Y == nextPoint.Y && p.Z == nextPoint.Z)) {
                            points.Add(nextPoint);
                        }
                    }
                }
            }

            int answer = points.Count;

            return answer.ToString();
        }

        // (ctrl/command + click) the link to open the input file
        // file://./../../Inputs/2024/14/3.txt
        public string PartThree(bool example)
        {
            List<string> lines = Utility.GetInputLines(2024, 14, 3, example);

            List<List<(char, int)>> directionSets = lines.Select(line => line.Split(',').Select(x => (x[0], int.Parse(x.Substring(1, x.Length - 1)))).ToList()).ToList();

            List<Point3D> points = [];
            List<Point3D> leaves = [];

            // Get the list of all points and leaves
            foreach (List<(char, int)> directionSet in directionSets)
            {
                Point3D currentPoint = new(0, 0, 0);

                foreach ((char, int) direction in directionSet)
                {
                    foreach (int i in direction.Item2)
                    {
                        Point3D nextPoint = direction.Item1 switch
                        {
                            'U' => new(currentPoint.X, currentPoint.Y + 1, currentPoint.Z),
                            'D' => new(currentPoint.X, currentPoint.Y - 1, currentPoint.Z),
                            'L' => new(currentPoint.X - 1, currentPoint.Y, currentPoint.Z),
                            'R' => new(currentPoint.X + 1, currentPoint.Y, currentPoint.Z),
                            'F' => new(currentPoint.X, currentPoint.Y, currentPoint.Z + 1),
                            'B' => new(currentPoint.X, currentPoint.Y, currentPoint.Z - 1),
                            _ => throw new NotImplementedException()
                        };

                        currentPoint = nextPoint;

                        if (!points.Any(p => p.X == nextPoint.X && p.Y == nextPoint.Y && p.Z == nextPoint.Z)) {
                            points.Add(nextPoint);
                        }
                    }
                }
            
                leaves.Add(currentPoint);
            }

            Dictionary<string, List<(int, int)>> leafToTrunkBest = [];

            // 1304
            // 1320

            foreach (Point3D leaf in leaves)
            {
                Dictionary<string, int> distances = [];
                int bestDistanceToTrunk = int.MaxValue;

                Queue<Point3D> queue = [];
                queue.Enqueue(leaf);

                while (queue.Count > 0)
                {
                    Point3D point = queue.Dequeue();

                    // Record best distance
                    string key = $"{point.X},{point.Y},{point.Z}";
                    if (distances.TryGetValue(key, out int value)) {
                        if (value > point.Value) {
                            distances[key] = point.Value;
                        }
                        else {
                            // This point is not better, ignore it
                            continue;
                        }
                    }
                    else {
                        distances[key] = point.Value;
                    }

                    // We found a better answer
                    if (point.X == 0 && point.Z == 0) {
                        if (bestDistanceToTrunk > point.Value) {
                            bestDistanceToTrunk = point.Value;
                            leafToTrunkBest[$"{leaf.X},{leaf.Y},{leaf.Z}"] = [(point.Y, point.Value)];
                        }
                        else if (bestDistanceToTrunk == point.Value) {
                            bestDistanceToTrunk = point.Value;
                            leafToTrunkBest[$"{leaf.X},{leaf.Y},{leaf.Z}"].Add((point.Y, point.Value));
                        }
                        continue;
                    }

                    // We're worse than our best, ignore this point
                    if (point.Value > bestDistanceToTrunk)
                    {
                        continue;
                    }

                    // Check neighbors
                    if (points.Any(p => p.X == point.X + 1 && p.Y == point.Y && p.Z == point.Z)) {
                        Point3D newPoint = new(point.X + 1, point.Y, point.Z){
                            Value = point.Value + 1
                        };
                        queue.Enqueue(newPoint);
                    }
                    if (points.Any(p => p.X == point.X - 1 && p.Y == point.Y && p.Z == point.Z)) {
                        Point3D newPoint = new(point.X - 1, point.Y, point.Z){
                            Value = point.Value + 1
                        };
                        queue.Enqueue(newPoint);
                    }
                    if (points.Any(p => p.X == point.X && p.Y == point.Y + 1 && p.Z == point.Z)) {
                        Point3D newPoint = new(point.X, point.Y + 1, point.Z){
                            Value = point.Value + 1
                        };
                        queue.Enqueue(newPoint);
                    }
                    if (points.Any(p => p.X == point.X && p.Y == point.Y - 1 && p.Z == point.Z)) {
                        Point3D newPoint = new(point.X, point.Y - 1, point.Z){
                            Value = point.Value + 1
                        };
                        queue.Enqueue(newPoint);
                    }
                    if (points.Any(p => p.X == point.X && p.Y == point.Y && p.Z == point.Z + 1)) {
                        Point3D newPoint = new(point.X, point.Y, point.Z + 1){
                            Value = point.Value + 1
                        };
                        queue.Enqueue(newPoint);
                    }
                    if (points.Any(p => p.X == point.X && p.Y == point.Y && p.Z == point.Z - 1)) {
                        Point3D newPoint = new(point.X, point.Y, point.Z - 1){
                            Value = point.Value + 1
                        };
                        queue.Enqueue(newPoint);
                    }
                }
            }

            int answer = int.MaxValue;

            List<Point3D> trunk = points.Where(p => p.X == 0 && p.Z == 0).ToList();

            foreach (Point3D trunkPoint in trunk) {
                int murkinessLevel = 0;

                foreach (List<(int, int)> leaf in leafToTrunkBest.Values) {
                    int bestY = leaf.Select(l => Math.Abs(trunkPoint.Y - l.Item1)).Min();
                    murkinessLevel += bestY + leaf[0].Item2;
                }

                if (murkinessLevel < answer) {
                    answer = murkinessLevel;
                }
            }
            
            return answer.ToString();
        }
    }
}