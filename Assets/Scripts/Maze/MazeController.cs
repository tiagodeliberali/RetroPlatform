using Assets.Scripts.Maze;
using UnityEngine;

public class MazeController : MonoBehaviour {

    public GameObject Wall;
    public GameObject Floor;

    MazeDrawerGameObject drawer;
    MazeBuilder builder;

    void Awake()
    {
        builder = new MazeBuilder(6, 3);
        drawer = new MazeDrawerGameObject(5, 4, 0.95f, Wall, Floor, gameObject, (prefab) => (GameObject)Instantiate(prefab));

        drawer.DrawMaze(builder.Build());
    }
}
