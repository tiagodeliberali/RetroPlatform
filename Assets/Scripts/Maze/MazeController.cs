using Assets.Scripts.Maze;
using UnityEngine;

public class MazeController : MonoBehaviour {

    public GameObject Wall;
    public GameObject Floor;

    MazeDrawerGameObject drawer;

    void Awake()
    {
        MazeSlot[,] maze = BuildMaze(6, 3);

        drawer = new MazeDrawerGameObject(5, 4, 0.95f, Wall, Floor, gameObject, (prefab) => (GameObject)Instantiate(prefab));
        drawer.DrawMaze(maze);
    }

    private MazeSlot[,] BuildMaze(int x, int y)
    {
        MazeBuilder builder = new MazeBuilder(x, y);
        builder.BuildMaze(builder.Maze[0, 0]);
        return builder.Maze;
    }
}
