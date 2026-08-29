using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
public class Maze
{
    public int startX;

    public int startY;

    public int[,] maze;

    public Maze(int inputStartX, int inputStartY, int[,] inputMaze)
    {
        startX = inputStartX;
        startY = inputStartY;
        maze = inputMaze;    
    }
}