namespace EverybodyCodes.Services
{
    public class Solution2024_06Service : ISolutionQuestService
    {
        private class Node {
            public string Name { get; set; } = string.Empty;
            public string Path { get; set; } = string.Empty;
            public int Length { get; set; }
        }

        // (ctrl/command + click) the link to open the input file
        // file://./../../Inputs/2024/06/1.txt
        public string PartOne(bool example)
        {
            List<string> lines = Utility.GetInputLines(2024, 6, 1, example);
            Dictionary<string, List<string>> map = lines.ToDictionary(line => line.Split(":")[0], line => line.Split(":")[1].Split(",").ToList());

            List<Node> visitedNodes = [];

            Queue<Node> queue = [];
            queue.Enqueue(new Node(){Name = "RR", Path="RR", Length=1});

            while(queue.Count > 0) {
                Node node = queue.Dequeue();
                visitedNodes.Add(node);

                if (map.ContainsKey(node.Name)) {
                    List<string> newNodeNames = map[node.Name];

                    foreach (string nodeName in newNodeNames) {
                        queue.Enqueue(new Node(){Name = nodeName, Path=node.Path + nodeName, Length=node.Length + 1});
                    }
                }
            }

            List<Node> endNodes = visitedNodes.Where(node => node.Name == "@").ToList();

            Node uniqueNode = endNodes.First(node => endNodes.Count(vn => vn.Length == node.Length) == 1);

            string answer = uniqueNode.Path;

            return answer.ToString();
        }

        // (ctrl/command + click) the link to open the input file
        // file://./../../Inputs/2024/06/2.txt
        public string PartTwo(bool example)
        {
            List<string> lines = Utility.GetInputLines(2024, 6, 2, example);
            Dictionary<string, List<string>> map = lines.ToDictionary(line => line.Split(":")[0], line => line.Split(":")[1].Split(",").ToList());

            List<Node> visitedNodes = [];

            Queue<Node> queue = [];
            queue.Enqueue(new Node(){Name = "RR", Path="R", Length=1});

            while(queue.Count > 0) {
                Node node = queue.Dequeue();
                visitedNodes.Add(node);

                if (map.ContainsKey(node.Name)) {
                    List<string> newNodeNames = map[node.Name];

                    foreach (string nodeName in newNodeNames) {
                        queue.Enqueue(new Node(){Name = nodeName, Path=node.Path + nodeName[0], Length=node.Length + 1});
                    }
                }
            }

            List<Node> endNodes = visitedNodes.Where(node => node.Name == "@").ToList();

            Node uniqueNode = endNodes.First(node => endNodes.Count(vn => vn.Length == node.Length) == 1);

            string answer = uniqueNode.Path;

            return answer.ToString();
        }

        // (ctrl/command + click) the link to open the input file
        // file://./../../Inputs/2024/06/3.txt
        public string PartThree(bool example)
        {
            List<string> lines = Utility.GetInputLines(2024, 6, 3, example);
            Dictionary<string, List<string>> map = lines.ToDictionary(line => line.Split(":")[0], line => line.Split(":")[1].Split(",").Where(n => n != "BUG" && n != "ANT").ToList());

            List<Node> visitedNodes = [];

            Queue<Node> queue = [];
            queue.Enqueue(new Node(){Name = "RR", Path="R", Length=1});

            while(queue.Count > 0) {
                Node node = queue.Dequeue();
                visitedNodes.Add(node);

                if (map.ContainsKey(node.Name)) {
                    List<string> newNodeNames = map[node.Name];

                    foreach (string nodeName in newNodeNames) {
                        queue.Enqueue(new Node(){Name = nodeName, Path=node.Path + nodeName[0], Length=node.Length + 1});
                    }
                }
            }

            List<Node> endNodes = visitedNodes.Where(node => node.Name == "@").ToList();

            Node uniqueNode = endNodes.First(node => endNodes.Count(vn => vn.Length == node.Length) == 1);

            string answer = uniqueNode.Path;

            return answer.ToString();
        }
    }
}