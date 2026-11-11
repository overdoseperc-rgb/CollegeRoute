namespace CollegeRoute
{
    public static class RouteModel
    {
        public static Graph Create(bool busClosed)
        {
            var graph = new Graph();
            foreach (string id in new[] { "H", "BS", "TS", "Cafe", "M", "X", "E1", "E2" })
                graph.AddVertex(id, id == "Cafe" ? 4 : id == "X" ? 2 : 0);
            // Отсутствующее ребро эквивалентно дороге с бесконечным временем.
            if (!busClosed) graph.AddEdge("H", "BS", 6);
            graph.AddEdge("H", "TS", 11);
            graph.AddEdge("H", "Cafe", 14);
            graph.AddEdge("BS", "M", 10);
            graph.AddEdge("BS", "E1", 22);
            graph.AddEdge("TS", "M", 9);
            graph.AddEdge("TS", "X", 16);
            graph.AddEdge("Cafe", "X", 12);
            graph.AddEdge("M", "E1", 8);
            graph.AddEdge("X", "E1", 7);
            graph.AddEdge("M", "E2", 9);
            graph.AddEdge("X", "E2", 10);
            return graph;
        }
    }
}
