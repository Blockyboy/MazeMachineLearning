public class Environment
{
    Agent agent;
    int[] actionChoiceX = [1, 0, -1, 0];
    int[] actionChoiceY = [0, 1, 0, -1];

    bool showStepByStep = false;

    bool showFullPath = true;

    int startX;
    int startY; 
    Maze maze = new Maze();

    public Environment(Agent inputAgent, int inputStartX, int inputStartY)
    {
        agent = inputAgent;
        startX = inputStartX;
        startY = inputStartY;
    }

    public void RunLearning()
    {
        maze.ImportMazes();
        foreach(KeyValuePair<int, int[,]> keyMazePair in maze.mazeDictionary)
        {
            int key = keyMazePair.Key;
            int[,] mazeLayout = keyMazePair.Value;
            string[,] displayMaze = InitialiseDisplayMaze(mazeLayout);
            agent.ReInitialiseQTable(agent.GenerateState(mazeLayout.GetLength(1), mazeLayout.GetLength(0)));

            for(int generation = 0; generation < 10000; ++generation)
            {
                agent.x = startX;
                agent.y = startY;
                agent.explored.Clear();

                if(generation == 999)
                {
                    Console.WriteLine("Maze: " + key);
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

                    if(generation == 9999)
                    {
                        if(showStepByStep)
                        {
                            DisplayStepByStep(agent.x, agent.y, displayMaze);                            
                        }
                        if(showFullPath)
                        {
                            DisplayPath(agent.x, agent.y, displayMaze);
                        }
                    }

                    agent.UpdateTable(action, reward, agent.GenerateState(nextX, nextY));

                    if(agent.explored.Add(agent.GenerateState(nextX, nextY)))
                    {
                        agent.x = nextX;
                        agent.y = nextY;                        
                    }
                    else
                    {
                        break;
                    }

                }
                agent.DecayEpsillon();
            }
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

    public void DisplayStepByStep(int agentX, int agentY, string[,] displayMaze)
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

        displayMaze[agentY, agentX] = characterTaken;
    }
    public void DisplayPath(int agentX, int agentY, string[,] displayMaze)
    {
        displayMaze[agentY, agentX] = "*";
        for(int i = 0; i < displayMaze.GetLength(0); ++i)
        {
            for(int j = 0; j < displayMaze.GetLength(1); ++j)
            {
                Console.Write(displayMaze[i, j]);
            }
            Console.WriteLine();
        }
        Console.WriteLine();
    }
}