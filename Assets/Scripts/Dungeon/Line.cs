using UnityEngine;

namespace Dungeon
{
    public class Line
    {
        private Orientation orientation;
        private Vector2Int coordinates;

        public Line(Orientation orientation, Vector2Int coordinates)
        {
            this.orientation = orientation;
            this.coordinates = coordinates;
        }

        public Vector2Int Coordinates
        {
            get => coordinates;
            set => coordinates = value;
        }

        public Orientation Orientation
        {
            get => orientation;
            set => orientation = value;
        }
    }

    public enum Orientation
    {
        Horizental=0,
        Vertical=1
    }
}