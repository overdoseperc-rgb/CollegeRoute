using System;
using System.Collections.Generic;

namespace CollegeRoute
{
    public class RouteResult
    {
        public Dictionary<string, long> Distances;
        public Dictionary<string, string> Previous;

        public List<string> GetPath(string target)
        {
            var path = new List<string>();
            if (!Distances.ContainsKey(target) || Distances[target] == long.MaxValue) return path;
            for (string current = target; current != null; current = Previous[current])
                path.Add(current);
            path.Reverse();
            return path;
        }
    }

    public static class Dijkstra
    {
        // Простая реализация: ближайшую непосещённую вершину ищем перебором.
        public static RouteResult Find(Graph graph, string start)
        {
            if (!graph.Edges.ContainsKey(start)) throw new ArgumentException("Неизвестная стартовая вершина.");
            var distances = new Dictionary<string, long>();
            var previous = new Dictionary<string, string>();
            var visited = new HashSet<string>();
            foreach (string id in graph.Edges.Keys)
            {
                distances[id] = long.MaxValue;
                previous[id] = null;
            }
            distances[start] = 0;

            while (true)
            {
                string nearest = null;
                long best = long.MaxValue;
                foreach (string id in graph.Edges.Keys)
                    if (!visited.Contains(id) && distances[id] < best)
                    {
                        best = distances[id];
                        nearest = id;
                    }
                if (nearest == null) break;
                visited.Add(nearest);

                foreach (Edge edge in graph.Edges[nearest])
                {
                    // Задержка вершины учитывается при входе в неё.
                    long candidate = checked(best + edge.Minutes + graph.Delays[edge.To]);
                    if (candidate < distances[edge.To])
                    {
                        distances[edge.To] = candidate;
                        previous[edge.To] = nearest;
                    }
                }
            }
            return new RouteResult { Distances = distances, Previous = previous };
        }
    }
}
