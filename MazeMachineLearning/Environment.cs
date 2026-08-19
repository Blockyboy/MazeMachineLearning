public class Environment
{
    Agent agent;
    int[] actionChoiceX = [1, 0, -1, 0];
    int[] actionChoiceY = [0, 1, 0, -1];

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
        
        for(int generation = 0; generation < 1000; ++generation)
        {
            agent.x = startX;
            agent.y = startY;
            while(maze.mazeLayout[agent.y][agent.x] != -10)
            {
                int action = agent.GenerateAction();
                int nextX = agent.x + actionChoiceX[action];
                int nextY = agent.y + actionChoiceY[action];

                double reward = 0 - maze.mazeLayout[nextY][nextX] - 1;

                if(maze.mazeLayout[nextY][nextX] == 1)
                {
                    nextX = agent.x;
                    nextY = agent.y;
                }
                
                agent.UpdateTable(action, reward, agent.GenerateState(nextX, nextY));

                if(generation == 999)
                {
                    Console.WriteLine("Agent X: " + agent.x);
                    Console.WriteLine("Agent Y: " + agent.y);
                    Console.WriteLine("Action: " + action);
                }

                agent.x = nextX;
                agent.y = nextY;

                agent.DecayEpsillon(0.9);
            }
        } 
    }
}