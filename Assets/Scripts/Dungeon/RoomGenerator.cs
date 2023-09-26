﻿using System.Collections.Generic;
using Helpers;
using UnityEngine;

namespace Dungeon
{
    public class RoomGenerator
    {
        private int roomWidth;
        private int roomLength;
        private int iterations;
        public RoomGenerator(int maxIterations,int minRoomLength,int minRoomWidth)
        {
            this.iterations = maxIterations;
            this.roomLength = minRoomLength;
            this.roomWidth = minRoomWidth;
        }

        public List<RoomNode> GenerateRoomsInGivenSpaces(List<Node> roomSpaces,float roomBottomCornerModifier,float roomTopCornerModifier,int roomOffset)
        {
            List<RoomNode> listToReturn = new List<RoomNode>();
            foreach (var space in roomSpaces)
            {
                Vector2Int newBottomLeftPoint =
                    StructureHelper.GenerateBottomLeftCornerBetween(space.BottomLeftAreaCorner,
                        space.TopRightAreaCorner, roomBottomCornerModifier, roomOffset);
                Vector2Int newTopRightPoint=StructureHelper.GenerateTopRightCornerBetween(space.BottomLeftAreaCorner,
                    space.TopRightAreaCorner, roomTopCornerModifier, roomOffset);
                
                space.BottomLeftAreaCorner = newBottomLeftPoint;
                space.TopRightAreaCorner = newTopRightPoint;
                space.BottomRightAreaCorner = new Vector2Int(newTopRightPoint.x, newBottomLeftPoint.y);
                space.TopLeftAreaCorner = new Vector2Int(newBottomLeftPoint.x, newTopRightPoint.y);
                listToReturn.Add((RoomNode)space);
            }

            return listToReturn;
        }
    }
}