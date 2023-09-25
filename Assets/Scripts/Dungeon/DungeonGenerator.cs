using System.Collections.Generic;
using Helpers;

namespace Dungeon
{
    public class DungeonGenerator
    {
        private int dungeonWidth;
        private int dungeonLength;
        
        private List<RoomNode> allNodesCollection = new List<RoomNode>();
        public DungeonGenerator(int _dungeonWidth, int _dungeonLength)
        {
            this.dungeonWidth = _dungeonWidth;
            this.dungeonLength = _dungeonLength;
        }
        public List<Node> CalculateDungeon(int maxIterations, int minRoomWidth, int minRoomLength,float roomBottomCornerModifier,float roomTopCornerModifier,int roomOffset,int corridorWidth)
        {
            BinarySpacePartitioner bsp = new BinarySpacePartitioner(dungeonWidth, dungeonLength);
            allNodesCollection = bsp.PrepareNodesCollection(maxIterations, minRoomWidth, minRoomLength);
            List<Node> roomSpaces = StructureHelper.TraverseGraphToExtractLowestLeaf(bsp.RootNode);
            RoomGenerator roomGenerator = new RoomGenerator(maxIterations, minRoomLength, minRoomWidth);
            List<RoomNode> roomList = roomGenerator.GenerateRoomsInGivenSpaces(roomSpaces,roomBottomCornerModifier,roomTopCornerModifier,roomOffset);

            CorridorGenerator corridorGenerator = new CorridorGenerator();
            var corridorList = corridorGenerator.CreateCorridors(allNodesCollection, corridorWidth);
            
            return new List<Node>(roomList);
        }
    }
}