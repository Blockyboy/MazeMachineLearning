public class Environment
{
    Agent agent;
    int[] actionChoiceX = [1, 0, -1, 0];
    int[] actionChoiceY = [0, 1, 0, -1];

    int generations = 1000000;

    bool showTrailing = true;

    bool showFullPath = true;

    bool noRecursion = false;

    bool stepLimit = true;

    int minimumSteps = 100;

    bool exportPathOnMaze = true;
    MazeCreator mazeCreator = new MazeCreator();

    public Environment(Agent inputAgent)
    {
        agent = inputAgent;
    }

    public void RunLearning()
    {
        mazeCreator.ImportMazes();
        foreach(KeyValuePair<string, Maze> keyMazePair in mazeCreator.mazeDictionary)
        {
            string filePath = keyMazePair.Key;
            int[,] mazeLayout = keyMazePair.Value.maze;
            string[,] displayMaze = InitialiseDisplayMaze(mazeLayout);

            agent.ReInitialiseQTable(agent.GenerateState(mazeLayout.GetLength(1), mazeLayout.GetLength(0)));

            for(int generation = 0; generation < generations; ++generation)
            {
                agent.steps = 0;
                agent.x = keyMazePair.Value.startX;
                agent.y = keyMazePair.Value.startY;
                agent.explored.Clear();

                if(generation == generations - 1)
                {
                    Console.WriteLine("Maze: " + filePath);
                }

                while(mazeLayout[agent.y,agent.x] != -10)
                {
                    int action = agent.GenerateAction();
                    int nextX = agent.x + actionChoiceX[action];
                    int nextY = agent.y + actionChoiceY[action];

                    double reward = 0 - mazeLayout[nextY,nextX] - 1;

                    if(mazeLayout[nextY, nextX] == 1)
                    {
                        nextX = agent.x;
                        nextY = agent.y;
                    }

                    agent.UpdateTable(action, reward, agent.GenerateState(nextX, nextY));

                    if(showFullPath && generation == generations -1)
                    {
                        DisplayPath(agent.x, agent.y, displayMaze, showTrailing);
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
                    else if(stepLimit)
                    {
                        if(agent.steps < minimumSteps)
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

            if(exportPathOnMaze)
            {
                ExportMazeDrawing(agent.ReturnExploredToCoords(), filePath);
            }

        } 

    }

    public void ExportMazeDrawing(List<(int, int)> path, string filePath)
    {
        if(path.Count > 0)
        {    
            mazeCreator.DrawOnMaze(filePath, path);
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