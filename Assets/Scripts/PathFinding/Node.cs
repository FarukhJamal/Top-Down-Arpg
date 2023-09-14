using UnityEngine;

namespace PathFinding
{
    public class Node
    {
        public bool Walkable;
        public Vector3 WorldPosition;
        public int GridX;
        public int GridY;
        public int gCost;
        public int hCost;
        public Node Parent;
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

    }
}
