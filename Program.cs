using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DisModels4
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var algo = new FordFulkerson();

            algo.start();
        }

        public class FordFulkerson
        {
            private int[,] graph;
            private int[] parent;

            public int[,] Value()
            {
                try
                {
                    int[,] matrix;

                    var reader = new StreamReader("C:/Users/admin/Desktop/DM/files/dm4.txt");
                    var dimension = reader.ReadLine();
                    var numRows = int.Parse(dimension);
                    matrix = new int[numRows, numRows];

                    for (int i = 0; i < numRows; i++)
                    {
                        var line = reader.ReadLine();
                        var values = line.Split(' ');
                        for (int j = 0; j < numRows; j++)
                        {
                            matrix[i, j] = int.Parse(values[j]);
                        }
                    }
                    reader.Close();

                    return matrix;
                }
                catch (Exception e)
                {
                    throw new Exception(e.Message);
                }
            }

            private bool Search(int source, int sink)
            {
                var isChecked = new bool[graph.GetLength(0)];
                var queue = new Queue<int>();

                queue.Enqueue(source);
                isChecked[source] = true;
                parent[source] = -1;

                while (queue.Count != 0)
                {
                    int current = queue.Dequeue();

                    for (int i = 0; i < graph.GetLength(0); i++)
                    {
                        if (!isChecked[i] && graph[current, i] > 0)
                        {
                            queue.Enqueue(i);
                            parent[i] = current;
                            isChecked[i] = true;
                        }
                    }
                }

                return isChecked[sink];
            }

            public int Flow(int origin, int drop)
            {
                parent = new int[graph.GetLength(0)];
                var maxFlow = 0;

                while (Search(origin, drop))
                {
                    var pathFlow = int.MaxValue;

                    for (int i = drop; i != origin; i = parent[i])
                    {
                        int j = parent[i];
                        pathFlow = Math.Min(pathFlow, graph[j, i]);
                    }

                    for (int i = drop; i != origin; i = parent[i])
                    {
                        int j = parent[i];
                        graph[j, i] -= pathFlow;
                        graph[i, j] += pathFlow;
                    }

                    maxFlow += pathFlow;
                }

                return maxFlow;
            }

            public void start()
            {
                graph = Value();

                int maxFlow = Flow(0, graph.GetLength(0) - 1);
                Console.WriteLine("Maximum flow is " + maxFlow);
            }
        }
    }
}