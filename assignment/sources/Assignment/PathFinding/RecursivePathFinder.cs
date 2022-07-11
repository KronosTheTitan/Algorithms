using System;
using System.Collections.Generic;
using System.Threading;

class RecursivePathFinder : PathFinder
{
    public RecursivePathFinder(NodeGraph pGraph) : base(pGraph)
    {
        
    }
    
    //the path
    List<Node> path = new List<Node>();

    //the parents
    Dictionary<Node, Node> parents = new Dictionary<Node, Node>();

    protected override List<Node> generate(Node pFrom, Node pTo)
    {
        //turn the control boolean off.
        //otherwise it will only work for one path generation
        pathFound = false;
        path.Clear();
        
        //the todo list
        List<Node> todo = new List<Node>();
        
        //this is the same as in the iterative version
        todo.AddRange(pFrom.connections);
        foreach (Node node in pFrom.connections)
        {
            if(parents.ContainsKey(node)) continue;
            parents.Add(node,pFrom);
            if (node == pTo)
            {
                path.Add(pFrom);
                path.Add(pTo);
                
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Found End!");
                Console.ResetColor();
                
                return path;
            }
        }

        //same purpose as the first while loop in the iterative version but now using recursion
        Explore(todo,pFrom,pTo,0);

        //if it didnt find anything return an empty path
        if (path.Count <= 0) return path;

        //identical to iterative from this point on
        while (path[path.Count-1]!= pFrom)
        {
            path.Add(parents[path[path.Count-1]]);
            Console.WriteLine("added node at " + parents[path[path.Count-1]].location + " to the path");
        }
        Console.WriteLine("Total path length = " + path.Count);
        
        parents.Clear();
        
        path.Reverse();
        
        Console.WriteLine(path.Count);
        
        return path;
    }

        
    bool pathFound = false;
    
    /// <summary>
    /// recursive method
    /// </summary>
    /// <param name="todo"></param>
    /// <param name="pFrom"></param>
    /// <param name="pTo"></param>
    /// <param name="depth"></param>
    /// <returns></returns>
    bool Explore(List<Node> todo, Node pFrom, Node pTo, int depth)
    {
        //this stops the loop once a path has been found.
        if (pathFound)
        {
            return true;
        }

        //create the temporary todo list
        //its used to allow modifiying of the list during the loop.
        List<Node> newTodo = new List<Node>();

        //loop over every node in todo
        foreach (Node node in todo)
        {
            foreach (Node node1 in node.connections)
            {
                if(parents.ContainsKey(node1)) continue;
                parents.Add(node1,node);
                newTodo.Add(node1);
                    
                Console.WriteLine("Explored Node at " + node1.location + "from Node at " + node.location);
                
                if (node1 == pTo)
                {
                    path.Add(node1);

                    //toggle out of the recursive loop.
                    pathFound = true;
                
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Found End!");
                    Console.ResetColor();
                }
            }
        }
        
        //move the temporary list into the main list.
        todo.Clear();
        todo.AddRange(newTodo);
        
        Console.WriteLine(todo.Count);
        depth++;
        
        //go a layer deeper.
        Explore(todo, pFrom, pTo,depth);
        
        return false;
    }
}