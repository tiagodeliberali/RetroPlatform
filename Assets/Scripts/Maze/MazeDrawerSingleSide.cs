﻿namespace Assets.Scripts.Maze
{
    public abstract class MazeDrawerSingleSide<T>
        where T: class
    {
        T Wall;
        T Floor;

        protected T[,] screen;

        int xSize;
        int ySize;

        public MazeDrawerSingleSide(int width, int height, T wall, T floor)
        {
            xSize = width - 1;
            ySize = height - 1;
            Wall = wall;
            Floor = floor;
        }

        public void DrawMaze(MazeSlot[,] maze)
        {
            screen = new T[maze.GetLength(0) * xSize + 1, maze.GetLength(1) * ySize + 1];

            for (int y = maze.GetLength(1) - 1; y >= 0; y--)
            {
                for (int x = 0; x < maze.GetLength(0); x++)
                {
                    int screenX = x * xSize + 1;
                    int screenY = y * ySize;

                    if (!maze[x, y].DestroyedWalls.Contains(WallPosition.Rigth))
                    {
                        for (int i = 0; i < ySize; i++)
                        {
                            screen[screenX + xSize - 1, screenY + i] = Wall;
                        }
                    }
                    else
                    {
                        screen[screenX + xSize - 1, screenY] = Floor;
                    }

                    if (!maze[x, y].DestroyedWalls.Contains(WallPosition.Down))
                    {
                        for (int i = 0; i < xSize - 1; i++)
                        {
                            screen[screenX + i, screenY] = Floor;
                        }
                    }
                }
            }

            DrawUpperLeftBorder();
            DrawScreen();
        }

        private void DrawUpperLeftBorder()
        {
            for (int x = 0; x < screen.GetLength(0); x++)
            {
                screen[x, screen.GetLength(1) - 1] = Floor;
            }

            for (int y = ySize; y < screen.GetLength(1) - 1; y++)
            {
                screen[0, y] = Wall;
            }

            screen[0, 0] = Floor;
            screen[screen.GetLength(0) - 1, 0] = Floor;

            for (int y = 1; y < ySize; y++)
            {
                screen[screen.GetLength(0) - 1, y] = null;
            }
        }

        protected abstract void DrawScreen();
    }
}
