﻿using UnityEngine;

namespace Dungeon
{
    public class RoomNode : Node
    {
        public RoomNode(Vector2Int bottomLeftAreaCorner,Vector2Int topRightAreaCorner,Node parentNode,int index) : base(parentNode)
        {
            this.BottomLeftAreaCorner = bottomLeftAreaCorner;
            this.TopRightAreaCorner = topRightAreaCorner;
            this.BottomRightAreaCorner = new Vector2Int(topRightAreaCorner.x, bottomLeftAreaCorner.y);
            this.TopLeftAreaCorner = new Vector2Int(bottomLeftAreaCorner.x, TopRightAreaCorner.y);
            this.TreeLayerIndex = index;
        }
        public int Width => (int)(TopRightAreaCorner.x - BottomLeftAreaCorner.x);
        public int Length => (int)(TopRightAreaCorner.y - BottomLeftAreaCorner.y);
    }
}