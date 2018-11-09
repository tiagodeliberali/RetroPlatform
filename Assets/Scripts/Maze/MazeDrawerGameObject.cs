using System;
using UnityEngine;

namespace Assets.Scripts.Maze
{
    public class MazeDrawerGameObject : MazeDrawerSingleSide<GameObject>
    {
        GameObject MazeGameObject;
        Func<GameObject, GameObject> InstantiateAction;
        float SpacingScale;

        public MazeDrawerGameObject(int width, int height, float spacingScale, GameObject wall, GameObject floor, GameObject parent, Func<GameObject, GameObject> instantiateAction)
            : base(width, height, wall, floor)
        {
            MazeGameObject = parent;
            InstantiateAction = instantiateAction;
            SpacingScale = spacingScale;
        }

        protected override void DrawScreen()
        {
            for (int y = screen.GetLength(1) - 1; y >= 0; y--)
            {
                for (int x = 0; x < screen.GetLength(0); x++)
                {
                    if (screen[x, y] != null)
                    {
                        GameObject mazeObject = InstantiateAction(screen[x, y]);
                        mazeObject.transform.parent = MazeGameObject.transform;
                        mazeObject.transform.localPosition = new Vector3(x * SpacingScale, y * SpacingScale);
                    }
                }
            }
        }
    }
}
