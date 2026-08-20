using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

public class MazeCreator
{
        public Dictionary<string, Maze> mazeDictionary = new();

    new Dictionary<int, int> colourMappings = new Dictionary<int, int>
    {
        { Color.Black.ToArgb(), 1 },
        { Color.White.ToArgb(), 0 },
        { Color.Green.ToArgb(), 0 },
        { Color.Blue.ToArgb(), -10 }
    };

    private string directoryPath = Path.Combine(Directory.GetCurrentDirectory(), "mazeImages");
    private string directoryOutput = Path.Combine(Directory.GetCurrentDirectory(), "exportedMazes");

    public void ImportMazes()
    {
        try
        {
            foreach(string filePath in Directory.EnumerateFiles(directoryPath))
            {
                mazeDictionary.Add(filePath, ImportMazeFromImage(filePath));
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }

    public Maze ImportMazeFromImage(string filePath)
    {
        using(Bitmap image = new(filePath))
        {
            int startX = 0;
            int startY = 0;
            int mazeWidth = image.Width;
            int mazeHeight = image.Height;
            int[,] outputMazeArray = new int[mazeHeight, mazeWidth];

            for(int y = 0; y < mazeHeight; ++y)
            {
                for(int x = 0; x < mazeWidth; ++x)
                {
                    Color pixel = image.GetPixel(x, y);
                    if(pixel.R == 0 && pixel.G == 128 && pixel.B == 0)
                    {
                        startX = x;
                        startY = y;
                    }
                    outputMazeArray[y, x] = colourMappings[pixel.ToArgb()];
                }
            }

            Maze outputMaze = new(startX, startY, outputMazeArray);
            
            return outputMaze;
        }
    }

    public void DrawOnMaze(string filePath, List<(int, int)> path)
    {
        using(Bitmap image = new(filePath))
        {
            using (Bitmap editableImage = new Bitmap(image.Width, image.Height, PixelFormat.Format24bppRgb))
            {
                using (Graphics g = Graphics.FromImage(editableImage))
                {
                    // Set PageUnit to prevent automatic DPI stretching
                    g.PageUnit = GraphicsUnit.Pixel;
                    g.DrawImageUnscaled(image, 0, 0);
                }
                
                foreach((int, int) point in path)
                {
                    editableImage.SetPixel(point.Item1, point.Item2, Color.Red);
                }

                string finalPath = Path.Combine(directoryOutput, "solution" + Path.GetFileName(filePath));

                editableImage.Save(finalPath);
            }
        }
    }
}