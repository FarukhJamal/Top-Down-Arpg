using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Dungeon
{
    public abstract class Node
    {
        public List<Node> ChildrenNodeList { get; }
        public bool Visited { get; set; }
        public Vector2Int BottomLeftAreaCorner { get; set; }
        public Vector2Int BottomRightAreaCorner { get; set; }
        public Vector2Int TopRightAreaCorner { get; set; }
        public Vector2Int TopLeftAreaCorner { get; set; }
        
        public Node Parent { get; set; }
        public int TreeLayerIndex { get; set; }

        public Node(Node parent)
        {
            ChildrenNodeList = new List<Node>();
            this.Parent = parent;
            if (parent != null)
                parent.AddChild(this);
            
        }

        public void AddChild(Node child)
        {
            ChildrenNodeList.Add(child);
        }

        public void RemoveChild(Node child)
        {
            ChildrenNodeList.Remove(child);
        }
    }
}