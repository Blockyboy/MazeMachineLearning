using System.Drawing;

public class Environment
{
    Agent agent;
    int[] actionChoiceX = [1, 0, -1, 0];
    int[] actionChoiceY = [0, 1, 0, -1];

    int generations = 4500;

    bool showTrailing = false;

    bool showFullPath = false;

    bool noRecursion = false;

    bool stepLimit = true;

    int maximumSteps = 100;

    bool exportPathOnMaze = true;

    int speed = 10;
    public MazeCreator mazeCreator = new MazeCreator();

    public Environment(Agent inputAgent)
    {
        agent = inputAgent;
    }

    public void ConsoleLearning()
    {
        mazeCreator.ImportMazes();
        foreach(KeyValuePair<string, Maze> keyMazePair in mazeCreator.mazeDictionary)
        {
            string filePath = keyMazePair.Key;
            int[,] mazeLayout = keyMazePair.Value.maze;
            string[,] displayMaze = InitialiseDisplayMaze(mazeLayout);
            int StartX = keyMazePair.Value.startX;
            int StartY = keyMazePair.Value.startY;

            agent.ReInitialiseQTable(agent.GenerateState(mazeLayout.GetLength(1), mazeLayout.GetLength(0)));
            RunLearning(filePath, mazeLayout, displayMaze, StartX, StartY);
            if(exportPathOnMaze)
            {
                ExportMazeDrawing(agent.ReturnExploredToCoords(), filePath);
            }
        }
    }

    public Bitmap APILearning(Bitmap image)
    {
        Maze workingMaze = mazeCreator.ImportMazeFromImage(image);
        int[,] mazeLayout = workingMaze.maze;
        int StartX = workingMaze.startX;
        int StartY = workingMaze.startY;

        agent.ReInitialiseQTable(agent.GenerateState(mazeLayout.GetLength(1), mazeLayout.GetLength(0)));

        RunLearning(null, mazeLayout, null, StartX, StartY);
        return mazeCreator.DrawOnMaze(image, agent.ReturnExploredToCoords());
    }
    public HashSet<int> RunLearning(string? filePath, int[,] mazeLayout, string[,]? displayMaze, int StartX, int StartY)
    {
        
        for(int generation = 0; generation < generations; ++generation)
        {
            agent.steps = 0;
            agent.x = StartX;
            agent.y = StartY;
            agent.explored.Clear();

            if(showFullPath)
            {
                displayMaze = InitialiseDisplayMaze(mazeLayout);
            }

            while(mazeLayout[agent.y,agent.x] != -10)
            {
                int action = agent.GenerateAction(agent.x, agent.y);
                int nextX = agent.x + actionChoiceX[action];
                int nextY = agent.y + actionChoiceY[action];

                double reward = 0 - mazeLayout[nextY,nextX] - 1;

                if(mazeLayout[nextY, nextX] == 1)
                {
                    nextX = agent.x;
                    nextY = agent.y;
                }

                agent.UpdateTable(action, reward, agent.GenerateState(nextX, nextY));

                if(showFullPath)
                {
                    Console.WriteLine("Generation" + generation);
                    Console.WriteLine("Maze: " + filePath);
                    Console.WriteLine("");
                    DisplayPath(agent.x, agent.y, displayMaze, showTrailing);
                    Thread.Sleep(speed);
                    Console.Clear();  
                }

                if(agent.explored.Add(agent.GenerateState(nextX, nextY)))
                {
                    agent.x = nextX;
                    agent.y = nextY;                        
                }
                else if(noRecursion)
                {
                    break;
                }
                if(stepLimit)
                {
                    if(agent.steps < maximumSteps)
                    {
                        ++agent.steps;
                    }
                    else
                    {
                        break;
                    }
                }

            }
            agent.DecayEpsillon();
        }
        return agent.explored;
    }

    public void ExportMazeDrawing(List<(int, int)> path, string filePath)
    {
        if(path.Count > 0)
        {    
            mazeCreator.OutputDrawnMaze(mazeCreator.DrawOnMaze(filePath, path), filePath);
        }
    }

    public string[,] InitialiseDisplayMaze(int[,] mazeLayout)
    {
        string[,] displayMaze = new string[mazeLayout.GetLength(0), mazeLayout.GetLength(1)];
        for(int i = 0; i < mazeLayout.GetLength(0); ++i)
        {
            for(int j = 0; j < mazeLayout.GetLength(1); ++j)
            {
                if(mazeLayout[i,j] == 1)
                {
                    displayMaze[i, j] = "▮";
                }
                if(mazeLayout[i,j] == 0)
                {
                    displayMaze[i, j] = " ";
                }
                if(mazeLayout[i,j] == -10)
                {
                    displayMaze[i, j] = "E";
                }
            }
        }

        return displayMaze;
    }

    public void DisplayPath(int agentX, int agentY, string[,] displayMaze, bool showTrail)
    {
        string characterTaken = displayMaze[agentY, agentX];
        displayMaze[agentY, agentX] = "A";
        for(int i = 0; i < displayMaze.GetLength(0); ++i)
        {
            for(int j = 0; j < displayMaze.GetLength(1); ++j)
            {
                Console.Write(displayMaze[i, j]);
            }
            Console.WriteLine();
        }
        Console.WriteLine();

        if(showTrail)
        {
            displayMaze[agentY, agentX] = "*"; 
        }
        else
        {
            displayMaze[agentY, agentX] = characterTaken;            
        }
    }
}