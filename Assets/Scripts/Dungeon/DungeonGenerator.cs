using System.Collections.Generic;
using Helpers;

namespace Dungeon
{
    public class DungeonGenerator
    {
        private int dungeonWidth;
        private int dungeonLength;
        
        private List<RoomNode> allSpaceNodes = new List<RoomNode>();
        public DungeonGenerator(int _dungeonWidth, int _dungeonLength)
        {
            this.dungeonWidth = _dungeonWidth;
            this.dungeonLength = _dungeonLength;
        }
        public List<Node> CalculateRooms(int maxIterations, int minRoomWidth, int minRoomLength)
        {
            BinarySpacePartitioner bsp = new BinarySpacePartitioner(dungeonWidth, dungeonLength);
            allSpaceNodes = bsp.PrepareNodesCollection(maxIterations, minRoomWidth, minRoomLength);
            List<Node> roomSpaces = StructureHelper.TraverseGraphToExtractLowestLeaf(bsp.RootNode);
            RoomGenerator roomGenerator = new RoomGenerator(maxIterations, minRoomLength, minRoomWidth);
            List<RoomNode> roomList = roomGenerator.GenerateRoomsInGivenSpaces(roomSpaces);
            return new List<Node>(roomList);
        }
    }
}