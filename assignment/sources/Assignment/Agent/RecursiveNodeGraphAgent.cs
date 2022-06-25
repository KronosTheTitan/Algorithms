using System.Collections.Generic;
using GXPEngine;

class RecursiveNodeGraphAgent : NodeGraphAgent
{
    private Node _target = null;

    public Node _current = null;

    private List<Node> _nodes = new List<Node>();

    private NodeGraph _nodeGraph;

    public RecursiveNodeGraphAgent(NodeGraph pNodeGraph) : base(pNodeGraph)
    {
        SetOrigin(width / 2, height / 2);

        _nodeGraph = pNodeGraph;
        _pathFinder = new IterativePathFinder(_nodeGraph);
        //_pathFinder = new RecursivePathFinder(_nodeGraph);

        //position ourselves on a random node
        if (pNodeGraph.nodes.Count > 0)
        {
            _current = pNodeGraph.nodes[Utils.Random(0, pNodeGraph.nodes.Count)];
            jumpToNode(_current);
        }

        //listen to nodeclicks
        pNodeGraph.OnNodeLeftClicked += onNodeClickHandler;
    }

    private PathFinder _pathFinder;
    private Node start = null;
    private Node end = null;
    protected virtual void onNodeClickHandler(Node pNode)
    {
        if (pNode == start)
        {
            start = null;
            return;
        }
        else
        {
            if (pNode == end)
            {
                end = null;
                return;
            }
        }

        if (start != null)
        {
            end = pNode;
            jumpToNode(start);
            _nodes = _pathFinder.Generate(start, end); 
            start = null;
            end = null;
            return;
        }
        else
        {
            start = pNode;
        }
    }

    protected override void Update()
    {
        //no target? Don't walk
        if (_target == null)
        {
            if (_nodes.Count > 0) _target = _nodes[0];
            else
            {
                return;
            }
        }

        //Move towards the target node, if we reached it, clear the target
        if (moveTowardsNode(_target))
        {
            _nodes.Remove(_target);
            _current = _target;
            _target = null;
        }
    }
}