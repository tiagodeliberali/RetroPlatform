using System.Collections.Generic;

namespace Assets.Scripts.Maze
{
    public class MazeSlot
    {
        public bool VisitedByBuilder { get; set; }
        public bool VisitedBySolver { get; set; }
        public bool SolutionPath { get; set; }
        public HashSet<WallPosition> DestroyedWalls { get; private set; }
        public int X { get; private set; }
        public int Y { get; private set; }

        public MazeSlot(int x, int y)
        {
            DestroyedWalls = new HashSet<WallPosition>();
            X = x;
            Y = y;
        }

        public void DestroyWall(MazeSlot other)
        {
            WallPosition position = WallPosition.None;

            if (X > other.X) position = WallPosition.Left;
            else if (X < other.X) position = WallPosition.Rigth;
            else if (Y > other.Y) position = WallPosition.Down;
            else if (Y < other.Y) position = WallPosition.Up;

            if (position != WallPosition.None) DestroyedWalls.Add(position);
        }

        public override bool Equals(object obj)
        {
            MazeSlot other = obj as MazeSlot;

            if (other == null) return false;

            return X == other.X && Y == other.Y;
        }
    }
}
