public abstract class Agent
{
    protected int actionAmount; //Amount of actions that can be done
    protected double[][] qTable; //Qtable for storing states and actions
    protected double alpha = 0.1; //Alpha of the Bellman equation, the learning rate (how much new information overrides old information)
    protected double gamma = 0.9; //Gamma of the Bellman equation, which accounts for the discount of future reward in the equation
    protected double epsillon = 0.9; //Epsillon, an additional aspect that determines how much the agent wants to explore.

    protected double epsillonDecay = 0.9;
    public int x;

    public int y;

    public int steps = 0;

    public HashSet<int> explored = new HashSet<int>();

    public Random randomAction = new Random();

    public Agent(int stateAmountInput, int actionAmountInput)
    {
        actionAmount = actionAmountInput;
        qTable  = new double[stateAmountInput][]; //Initialising Q Table
        for(int i = 0; i < stateAmountInput; ++i)
        {
            qTable[i] = new double[actionAmount];
        }
    }

    public void ReInitialiseQTable(int stateAmountInput)
    {
        ClearQTable();
        qTable  = new double[stateAmountInput][]; //Initialising Q Table
        for(int i = 0; i < stateAmountInput; ++i)
        {
            qTable[i] = new double[actionAmount];
        }
    }

    public (int, int) ReturnCoords(double state)
    {
        int wHelper = (int)Math.Floor((Math.Sqrt(8 * state + 1) - 1) / 2);
        int tHelper = ((wHelper * wHelper) + wHelper) / 2;

        int y = (int)state - tHelper;
        int x = wHelper - y;

        return (x,y);
    }

    public List<(int, int)> ReturnExploredToCoords()
    {
        List<(int, int)> path = new List<(int, int)>();
        while(explored.Count > 0)
        {
            int nextItem = explored.First();

            path.Add(ReturnCoords(nextItem));

            explored.Remove(nextItem);
        }

        return path;
    }

    public void ClearQTable()
    {
        for (int i = 0; i < qTable.Length; i++)
        {
            Array.Clear(qTable[i], 0, qTable[i].Length);
        }
    }

    public void DecayEpsillon()
    {
        epsillon = epsillon * epsillonDecay;
    }

    public int GenerateState(int inputX, int inputY)
    {
        return ((inputX + inputY) * (inputX+inputY+1)) / 2 + inputY;
    }

    public int GenerateAction(int agentX, int agentY)
    {
        if(randomAction.NextDouble() < epsillon)
        {
            return randomAction.Next(actionAmount);
        }

        return Array.IndexOf(qTable[GenerateState(agentX, agentY)], qTable[GenerateState(agentX, agentY)].Max());
    }

    public abstract void UpdateTable(int action, double reward, int nextState);
}