using Helpers;
using UnityEngine;

namespace PathFinding
{
    public class Node: IHeapItem<Node>
    {
        public bool Walkable;
        public Vector3 WorldPosition;
        public int GridX;
        public int GridY;
        public int gCost;
        public int hCost;
        public Node Parent;
        private int heapIndex;
        public int fCost
        {
            get
            {
                return gCost + hCost;
            }
        }
        public Node(bool walkable, Vector3 worldPosition,int gridX,int gridY)
        {
            Walkable = walkable;
            WorldPosition = worldPosition;
            GridX = gridX;
            GridY = gridY;
        }

        public int CompareTo(Node nodeToCompare)
        {
            int compare = fCost.CompareTo(nodeToCompare.fCost);
            if (compare == 0)
                compare = hCost.CompareTo(nodeToCompare.hCost);
            return -compare;
        }

        public int HeapIndex
        {
            get
            {
                return heapIndex;
            }
            set
            {
                heapIndex = value;
            }
        }
    }
}
