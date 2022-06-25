using System;
using System.Collections.Generic;
using System.Threading;

class RecursivePathFinder : PathFinder
{
    public RecursivePathFinder(NodeGraph pGraph) : base(pGraph)
    {
        
    }
    
    List<Node> path = new List<Node>();

    Dictionary<Node, Node> parents = new Dictionary<Node, Node>();

    protected override List<Node> generate(Node pFrom, Node pTo)
    {
        pathFound = false;
        path.Clear();
        List<Node> todo = new List<Node>();
        
        todo.AddRange(pFrom.connections);
        foreach (Node node in pFrom.connections)
        {
            if(parents.ContainsKey(node)) continue;
            parents.Add(node,pFrom);
            if (node == pTo)
            {
                path.Add(pFrom);
                path.Add(pTo);
                return path;
            }
        }

        Explore(todo,pFrom,pTo,0);

        if (path.Count <= 0) return path;

        while (path[path.Count-1]!= pFrom)
        {
            path.Add(parents[path[path.Count-1]]);
        }
        
        parents.Clear();
        
        path.Reverse();
        
        Console.WriteLine(path.Count);
        
        return path;
    }

        
    bool pathFound = false;
    bool Explore(List<Node> todo, Node pFrom, Node pTo, int depth)
    {
        
        if (pathFound)
        {
            return true;
        }

        List<Node> newTodo = new List<Node>();

        foreach (Node node in todo)
        {
            foreach (Node node1 in node.connections)
            {
                if(parents.ContainsKey(node1)) continue;
                parents.Add(node1,node);
                newTodo.Add(node1);
                if (node1 == pTo)
                {
                    path.Add(node1);

                    pathFound = true;
                
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Found End!");
                    Console.ResetColor();
                }
            }
        }
        
        todo.Clear();
        todo.AddRange(newTodo);
        
        Console.WriteLine(todo.Count);
        depth++;

        Explore(todo, pFrom, pTo,depth);
        
        return false;
    }
}