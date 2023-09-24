using System.Collections.Generic;

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
            return new List<Node>(allSpaceNodes);
        }
    }
}