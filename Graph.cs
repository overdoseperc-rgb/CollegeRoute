using System;
using System.Collections.Generic;

namespace CollegeRoute
{
    public class Edge
    {
        public string To;
        public int Minutes;
        public Edge(string to, int minutes) { To = to; Minutes = minutes; }
    }

    public class Graph
    {
        public readonly Dictionary<string, List<Edge>> Edges = new Dictionary<string, List<Edge>>();
        public readonly Dictionary<string, int> Delays = new Dictionary<string, int>();

        public void AddVertex(string id, int delay)
        {
            if (delay < 0) throw new ArgumentException("Задержка не может быть отрицательной.");
            if (Edges.ContainsKey(id)) throw new ArgumentException("Вершина уже существует.");
            Edges[id] = new List<Edge>();
            Delays[id] = delay;
        }

        public void AddEdge(string from, string to, int minutes)
        {
            if (minutes < 0) throw new ArgumentException("Дейкстра требует неотрицательных весов.");
            if (!Edges.ContainsKey(from) || !Edges.ContainsKey(to))
                throw new ArgumentException("Неизвестная вершина.");
            Edges[from].Add(new Edge(to, minutes));
        }

        public void RemoveEdge(string from, string to)
        {
            Edges[from].RemoveAll(edge => edge.To == to);
        }
    }
}
