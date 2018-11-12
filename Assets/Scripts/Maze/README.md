# Maze builder

![image](https://user-images.githubusercontent.com/180231/48312637-f7cd0b80-e598-11e8-9071-a1b1043f9754.png)

This is a small project that helps building Mazes. The Maze generation is based on Depth-first search, applied over an array
of MazeSlots with random decisions to choose next node.

## Using on Unity for 2D games

Here we have a implementation of the maze to Unity, inside Maze Controller. 

First, it creates a Maze with a defined number of horizontal cells and vertical cells:

```c#
    private MazeSlot[,] BuildMaze(int x, int y)
    {
        MazeBuilder builder = new MazeBuilder(
          x, // number of horizontal cells
          y  // number of vertical cells
        );
        builder.BuildMaze(builder.Maze[0, 0]);
        return builder.Maze;
    }
```

Than, we have a MazeDrawerGameObject that receives or maze and some other parameters that will be explained here:

```c#
void Awake()
    {
        MazeSlot[,] maze = BuildMaze(6, 3);

        drawer = new MazeDrawerGameObject(
          5,            // Number of prefabs that will be used to draw the floor on each cell
          4,            // Number of prefabs that will be used to draw the wall on each cell
          0.95f,        // Position scale factor. By default, each prefab is set on absolute positions. This will adjust the position
          Wall,         // Prefab of the maze wall. Should be square by now.
          Floor,        // Prefab of the maze floor. Should be square by now.
          gameObject,   // Parent object, used to set the maze position on screen
          (prefab) => (GameObject)Instantiate(prefab));
        drawer.DrawMaze(maze);
    }
```

Nice things to understand:
* The wall of a cell overlaps the wall of the neighboring cell
* The floor of a cell overlaps the roof of the neighboring cell
* The **first column of maze cells are with the wrong number of prefabs for the floor/roof** (It is missing one! Its a bug!!)
* The space between blocks is controlled by `Position scale factor`. Change it to reduce the space and make it looks ok

Here is a maze cell details:

![image](https://user-images.githubusercontent.com/180231/48312786-0a484480-e59b-11e8-87be-d39368faec59.png)

## Maze builder original project

If you wish, check my repository with a console version of the maze builder, that includes a maze solver as well.
https://github.com/tiagodeliberali/MazeCreator
