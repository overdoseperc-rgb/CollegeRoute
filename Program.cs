using System;
using System.Collections.Generic;
using System.Text;

namespace CollegeRoute
{
    class Program
    {
        static void ShowRoute(Graph graph)
        {
            RouteResult result = Dijkstra.Find(graph, "H");
            var path = result.GetPath("E1");
            if (path.Count == 0) { Console.WriteLine("Путь до E1 не найден."); return; }
            Console.WriteLine("Маршрут: " + string.Join(" -> ", path));
            Console.WriteLine("Время до центрального входа E1: " + result.Distances["E1"] + " мин.");
            Console.WriteLine("Расчёт по этапам:");
            for (int i = 1; i < path.Count; i++)
                foreach (Edge edge in graph.Edges[path[i - 1]])
                    if (edge.To == path[i])
                        Console.WriteLine("  " + path[i - 1] + " -> " + path[i] + ": " + edge.Minutes +
                            " мин + задержка " + graph.Delays[path[i]] + " мин");
        }

        static void ShowRoads(SortedDictionary<int, KeyValuePair<string, Edge>> roads)
        {
            foreach (var road in roads)
                Console.WriteLine(road.Key + ". " + road.Value.Key + " -> " + road.Value.Value.To +
                    ": " + road.Value.Value.Minutes + " мин");
        }

        static void Main()
        {
            Console.OutputEncoding = Encoding.UTF8;
            Graph graph = RouteModel.Create(false);
            var roads = new SortedDictionary<int, KeyValuePair<string, Edge>>();
            int roadNumber = 1;
            foreach (var vertex in graph.Edges)
                foreach (Edge edge in vertex.Value)
                    roads.Add(roadNumber++, new KeyValuePair<string, Edge>(vertex.Key, edge));
            Console.WriteLine("=== ПУТЬ ДО КОЛЛЕДЖА | АЛГОРИТМ ДЕЙКСТРЫ ===");
            Console.WriteLine("H — дом; BS — автобус; TS — трамвай; Cafe — кофейня;");
            Console.WriteLine("M — метро; X — переход; E1 — центральный вход; E2 — технический вход.");
            ShowRoute(graph);
            while (true)
            {
                Console.WriteLine("\n1. Рассчитать маршрут H -> E1");
                Console.WriteLine("2. Сравнить маршрут до и после закрытия дороги");
                Console.WriteLine("3. Удалить дорогу по номеру");
                Console.WriteLine("4. Показать расстояния от H до всех вершин");
                Console.WriteLine("5. Показать рёбра текущего графа");
                Console.WriteLine("0. Выход");
                Console.Write("Ваш выбор: ");
                string choice = Console.ReadLine();
                if (choice == null || choice == "0") return;
                if (choice == "1") ShowRoute(graph);
                else if (choice == "2")
                {
                    Graph beforeGraph = RouteModel.Create(false);
                    Graph afterGraph = RouteModel.Create(true);
                    Console.WriteLine("\nВсе дороги открыты");
                    ShowRoute(beforeGraph);
                    Console.WriteLine("\nДорога H -> BS закрыта");
                    ShowRoute(afterGraph);
                    long before = Dijkstra.Find(beforeGraph, "H").Distances["E1"];
                    long after = Dijkstra.Find(afterGraph, "H").Distances["E1"];
                    Console.WriteLine("Изменение времени: +" + (after - before) + " мин.");
                }
                else if (choice == "3")
                {
                    if (roads.Count == 0)
                    {
                        Console.WriteLine("Все дороги уже удалены.");
                        continue;
                    }

                    ShowRoads(roads);
                    Console.Write("Введите номер дороги (0 — отмена): ");
                    string input = Console.ReadLine();
                    if (input == null) return;
                    if (!int.TryParse(input, out int number))
                    {
                        Console.WriteLine("Введите целый номер дороги.");
                        continue;
                    }
                    if (number == 0) continue;
                    if (!roads.TryGetValue(number, out var road))
                    {
                        Console.WriteLine("Дороги с таким номером нет в текущем графе.");
                        continue;
                    }

                    graph.Edges[road.Key].Remove(road.Value);
                    roads.Remove(number);
                    Console.WriteLine("Удалена дорога " + road.Key + " -> " + road.Value.To + ".");
                    ShowRoute(graph);
                }
                else if (choice == "4")
                {
                    RouteResult result = Dijkstra.Find(graph, "H");
                    foreach (var item in result.Distances)
                        Console.WriteLine(item.Key + ": " + (item.Value == long.MaxValue ? "недостижима" : item.Value + " мин"));
                    if (result.Distances["E2"] != long.MaxValue)
                        Console.WriteLine("Через E2 с дополнительными 3 минутами до аудитории: " +
                            (result.Distances["E2"] + 3) + " мин относительно нулевого общего времени после E1.");
                }
                else if (choice == "5")
                {
                    if (roads.Count == 0) Console.WriteLine("В графе нет дорог.");
                    ShowRoads(roads);
                    Console.WriteLine("Дополнительно при входе: Cafe +4 мин, X +2 мин.");
                }
                else Console.WriteLine("Введите номер от 0 до 5.");
            }
        }
    }
}
