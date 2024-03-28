using System;

namespace connected_Pods
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var sol = new Solution();
            var connection = new List<List<int>>();
            connection.Add(new List<int>() { 1, 2 });
            connection.Add(new List<int>() { 2, 3 });
            connection.Add(new List<int>() { 1, 4 });
            connection.Add(new List<int>() { 4, 3 });
            connection.Add(new List<int>() { 5, 6 });

            var queries = new List<List<int>>();
            queries.Add(new List<int>() { 2, 3 });
            queries.Add(new List<int>() { 1, 3 });
            queries.Add(new List<int>() { 2, 1 });
            queries.Add(new List<int>() { 1, 1 });

            queries.Add(new List<int>() { 2, 5 });
            queries.Add(new List<int>() { 1, 5 });
            //queries.Add(new List<int>() { 2, 6 });
            queries.Add(new List<int>() { 1, 5 });
            queries.Add(new List<int>() { 1, 6 });
            var ans = sol.Solve(connection, queries);
            Console.WriteLine(string.Join(",", ans));
        }
    }

    public class Node
    {
        public bool Active;
        public int Value;
        public Node(int value, bool active = true) { Active = active; Value = value; }
    }

    public class Solution
    {
        public List<int> Solve(List<List<int>> connection, List<List<int>> queries)
        {
            var ans = new List<int>();
            var adj = new Dictionary<int, List<Node>>();
            foreach (var con in connection)
            {
                var source = con[0];
                var dest = con[1];
                if (!adj.ContainsKey(source)) adj.Add(source, new List<Node>());
                if (!adj.ContainsKey(dest)) adj.Add(dest, new List<Node>());

                adj[source].Add(new Node(dest, true));
                adj[dest].Add(new Node(source, true));
            }

            var vertices = adj.Keys.Count;
            var visited = new HashSet<int>();
            var regions = new List<HashSet<Node>>();
            for (int i = 1; i < vertices; i++)
            {
                if (visited.Contains(i)) continue;
                var set = new HashSet<Node>();
                Dfs(new Node(i, true), set);
                regions.Add(set);
            }

            void Dfs(Node node, HashSet<Node> set)
            {
                if (visited.Contains(node.Value)) return;
                visited.Add(node.Value);
                set.Add(node);
                foreach (var nei in adj[node.Value])
                {
                    if (visited.Contains(nei.Value)) continue;
                    Dfs(nei, set);
                }
            }

            foreach (var query in queries)
            {
                var decision = query[0];
                var decnode = query[1];
                if (decision == 2)
                {
                    foreach (var reg in regions)
                    {
                        foreach (var r in reg)
                        {
                            if (r.Value == decnode)
                            {
                                r.Active = false;
                                break;
                            }
                        }
                    }
                }

                if (decision == 1)
                {
                    var active = false;
                    int index = 0;
                    int foundindex = -1;
                    foreach (var reg in regions)
                    {
                        foreach (var r in reg)
                        {
                            if (r.Value == decnode)
                            {
                                foundindex = index;
                                if (r.Active)
                                {
                                    active = true;
                                    ans.Add(decnode);
                                }
                                break;
                            }
                        }
                        if (active || foundindex >= 0)
                        {
                            break;
                        }
                        index++;
                    }
                    if (foundindex >= regions.Count) continue;
                    var regi = regions[foundindex];
                    if (!active && regi.Any())
                    {
                        var item = regi.Where(r => r.Active).OrderBy(i => i.Value).FirstOrDefault();
                        if (item != null)
                        {
                            ans.Add(item.Value);
                        }
                    }
                }
            }

            return ans;
        }
    }
}
