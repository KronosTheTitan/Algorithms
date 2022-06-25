using System;
using System.Collections.Generic;
using System.Threading;

class IterativePathFinder : PathFinder
{
    public IterativePathFinder(NodeGraph pGraph) : base(pGraph)
    {
        
    }

    protected override List<Node> generate(Node pFrom, Node pTo)
    {
        List<Node> path = new List<Node>();
        List<Node> todo = new List<Node>();

        Dictionary<Node, Node> parents = new Dictionary<Node, Node>();

        bool pathFound = false;
        
        todo.AddRange(pFrom.connections);
        foreach (Node node in pFrom.connections)
        {
            parents.Add(node,pFrom);
            if (node == pTo)
            {
                path.Add(pFrom);
                path.Add(pTo);
                return path;
            }
        }

        int iterations = 0;
        
        while (!pathFound)
        {
            Console.WriteLine(todo.Count);
            if (path.Count != 0)
            {
                pathFound = true;
                continue;
            }

            List<Node> newTodo = new List<Node>();

            while (todo.Count>0)
            {
                foreach (Node node1 in todo[0].connections)
                {
                    if(parents.ContainsKey(node1)) continue;
                    parents.Add(node1,todo[0]);
                    todo.Add(node1);
                    if (node1 == pTo)
                    {
                        path.Add(node1);
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Found End!");
                        Console.ResetColor();
                    }
                }

                todo.Remove(todo[0]);
            }
        }

        while (path[path.Count-1]!= pFrom)
        {
            path.Add(parents[path[path.Count-1]]);
        }
        
        path.Reverse();
        return path;
    }
}