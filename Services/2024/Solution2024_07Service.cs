using System.Text;

namespace EverybodyCodes.Services
{
    public class Solution2024_07Service : ISolutionQuestService
    {
        // (ctrl/command + click) the link to open the input file
        // file://./../../Inputs/2024/07/1.txt
        public string PartOne(bool example)
        {
            List<string> lines = Utility.GetInputLines(2024, 7, 1, example);

            Dictionary<string, int> values = [];

            foreach (string line in lines)
            {
                string[] parts = line.Split(":");
                string plan = parts[0];
                List<string> planAction = parts[1].Split(',').ToList();

                int power = 10;
                int score = 0;

                foreach (int i in 10) {
                    string action = planAction[i % planAction.Count];
                    switch (action) {
                        case "+":
                            power++;
                            break;
                        case "-":
                            if (power > 0) {
                                power--;
                            }
                            break;
                    }
                    score += power;
                }

                values[plan] = score;
            }

            string answer = string.Join("", values.Select(kv => new {planName = kv.Key, Value = kv.Value}).OrderByDescending(v => v.Value).Select(v => v.planName));

            return answer;
        }

        // (ctrl/command + click) the link to open the input file
        // file://./../../Inputs/2024/07/2.txt
        public string PartTwo(bool example)
        {
            List<string> lines = Utility.GetInputLines(2024, 7, 2, example);

            string track = example ?
            """
            S+===
            -   +
            =+=-+
            """ :
            """
            S-=++=-==++=++=-=+=-=+=+=--=-=++=-==++=-+=-=+=-=+=+=++=-+==++=++=-=-=--
            -                                                                     -
            =                                                                     =
            +                                                                     +
            =                                                                     +
            +                                                                     =
            =                                                                     =
            -                                                                     -
            --==++++==+=+++-=+=-=+=-+-=+-=+-=+=-=+=--=+++=++=+++==++==--=+=++==+++-
            """;

            List<string> trackParts = track.Split(Environment.NewLine).ToList();
            List<string> trackPartsPivot = trackParts.Pivot().Select(s => new string(s.ToArray())).ToList();

            string firstLeg = new (trackParts.First().Skip(1).ToArray());
            string secondLeg = new (trackPartsPivot.Last().Skip(1).ToArray());
            string thirdLeg = new (trackParts.Last().ReverseInPlace().Skip(1).ToArray());
            string fourthLeg = new (trackPartsPivot.First().ReverseInPlace().Skip(1).ToArray());

            string linearTrack = firstLeg + secondLeg + thirdLeg + fourthLeg;

            Dictionary<string, int> values = [];
            List<char> overwriteSignals = ['+', '-'];


            foreach (string line in lines)
            {
                string[] parts = line.Split(":");
                string plan = parts[0];
                List<string> planActions = parts[1].Split(',').ToList();

                int power = 10;
                int score = 0;

                int i = 0;

                foreach (int loop in 10) {
                    foreach (char trackAction in linearTrack) {
                        string action = overwriteSignals.Contains(trackAction) ? trackAction.ToString() : planActions[i % planActions.Count];
                        switch (action) {
                            case "+":
                                power++;
                                break;
                            case "-":
                                if (power > 0) {
                                    power--;
                                }
                                break;
                        }
                        score += power;
                        i++;
                    }
                }

                values[plan] = score;
            }

            string answer = string.Join("", values.Select(kv => new {planName = kv.Key, Value = kv.Value}).OrderByDescending(v => v.Value).Select(v => v.planName));

            return answer.ToString();
        }

        // (ctrl/command + click) the link to open the input file
        // file://./../../Inputs/2024/07/3.txt
        public string PartThree(bool example)
        {
            List<string> lines = Utility.GetInputLines(2024, 7, 3, example);

            // First, map out the track
            string track =
            """
            S+= +=-== +=++=     =+=+=--=    =-= ++=     +=-  =+=++=-+==+ =++=-=-=--
            - + +   + =   =     =      =   == = - -     - =  =         =-=        -
            = + + +-- =-= ==-==-= --++ +  == == = +     - =  =    ==++=    =++=-=++
            + + + =     +         =  + + == == ++ =     = =  ==   =   = =++=       
            = = + + +== +==     =++ == =+=  =  +  +==-=++ =   =++ --= + =          
            + ==- = + =   = =+= =   =       ++--          +     =   = = =--= ==++==
            =     ==- ==+-- = = = ++= +=--      ==+ ==--= +--+=-= ==- ==   =+=    =
            -               = = = =   +  +  ==+ = = +   =        ++    =          -
            -               = + + =   +  -  = + = = +   =        +     =          -
            --==++++==+=+++-= =-= =-+-=  =+-= =-= =--   +=++=+++==     -=+=++==+++-
            """;

            List<List<char>> trackGrid = track.Split(Environment.NewLine).Select(r => r.ToList()).ToList();

            Point previousPoint = new(0, 0);
            Point currentPoint = new(1, 0);

            StringBuilder sb = new();
            sb.Append(trackGrid[0][1]);

            while (!(currentPoint.X == 0 && currentPoint.Y == 0)) {
                List<Point> neighbors = trackGrid.GetNeighbors(currentPoint.X, currentPoint.Y);

                Point nextPoint = neighbors.Where(p => !(p.X == previousPoint.X && p.Y == previousPoint.Y) && trackGrid[p.Y][p.X] != ' ').First();
                previousPoint = currentPoint;
                currentPoint = nextPoint;

                sb.Append(trackGrid[currentPoint.Y][currentPoint.X]);
            }

            string oneLap = sb.ToString().Replace('S', '=');

            sb = new();

            Utility.ForEach(2024, () => {sb.Append(oneLap);});

            string linearTrack = sb.ToString();

            // Next, calculate the score to beat
            List<char> competingPlan = lines.First().Split(":")[1].Split(",").Select(s => s[0]).ToList();

            List<char> overwriteSignals = ['+', '-'];
            int power = 10;
            long scoreToBeat = 0;
            foreach (int i in linearTrack.Length) {
                char trackAction = linearTrack[i];
                char action = overwriteSignals.Contains(trackAction) ? trackAction : competingPlan[i % competingPlan.Count];
                switch (action) {
                    case '+':
                        power++;
                        break;
                    case '-':
                        if (power > 0) {
                            power--;
                        }
                        break;
                }
                scoreToBeat += power;
            }

            int answer = 0;

            List<List<char>> plans = competingPlan.GetUniquePermutations();

            foreach (List<char> plan in plans)
            {
                power = 10;
                long score = 0;
                foreach (int i in linearTrack.Length) {
                    char trackAction = linearTrack[i];
                    char action = overwriteSignals.Contains(trackAction) ? trackAction : plan[i % plan.Count];
                    switch (action) {
                        case '+':
                            power++;
                            break;
                        case '-':
                            if (power > 0) {
                                power--;
                            }
                            break;
                    }
                    score += power;
                }

                if (score > scoreToBeat) {
                    answer++;
                }
            }

            return answer.ToString();
        }
    }
}