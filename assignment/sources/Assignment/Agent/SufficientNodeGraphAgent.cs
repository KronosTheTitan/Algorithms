using System.Collections.Generic;
using GXPEngine;

class SufficientNodeGraphAgent : NodeGraphAgent
{
    private Node _target = null;

    public Node _current = null;

    private List<Node> _nodes = new List<Node>();

    public SufficientNodeGraphAgent(NodeGraph pNodeGraph) : base(pNodeGraph)
    {
        SetOrigin(width / 2, height / 2);

        //position ourselves on a random node
        if (pNodeGraph.nodes.Count > 0)
        {
            _current = pNodeGraph.nodes[Utils.Random(0, pNodeGraph.nodes.Count)];
            jumpToNode(_current);
        }

        //listen to nodeclicks
        pNodeGraph.OnNodeLeftClicked += onNodeClickHandler;
    }
    protected virtual void onNodeClickHandler(Node pNode)
    {
        //_nodes.Add(pNode);
        if (_nodes.Count == 0)
        {            
            if (_current.connections.Contains(pNode)) _nodes.Add(pNode);
        }
        else
        {
            if (_nodes[_nodes.Count-1].connections.Contains(pNode)) _nodes.Add(pNode);
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