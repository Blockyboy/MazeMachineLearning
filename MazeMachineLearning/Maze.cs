using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

public class Maze
{
    public Dictionary<int, int[,]> mazeDictionary = new();

    new Dictionary<int, int> colourMappings = new Dictionary<int, int>
    {
        { Color.Black.ToArgb(), 1 },
        { Color.White.ToArgb(), 0 },
        { Color.Blue.ToArgb(), -10 }
    };

    private string directoryPath = Path.Combine(Directory.GetCurrentDirectory(), "mazeImages");

    public void ImportMazes()
    {
        int count = 0;
        try
        {
            foreach(string filePath in Directory.EnumerateFiles(directoryPath))
            {
                mazeDictionary.Add(count, ImportMazeFromImage(filePath));
                ++count;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }

    public int[,] ImportMazeFromImage(string filePath)
    {
        using(Bitmap image = new(filePath))
        {
            int mazeWidth = image.Width;
            int mazeHeight = image.Height;
            int[,] outputMaze = new int[mazeHeight, mazeWidth];

            for(int y = 0; y < mazeWidth; ++y)
            {
                for(int x = 0; x < mazeWidth; ++x)
                {
                    Color pixel = image.GetPixel(x, y);
                    outputMaze[y, x] = colourMappings[pixel.ToArgb()];
                }
            }
            
            return outputMaze;
        }

    }
}