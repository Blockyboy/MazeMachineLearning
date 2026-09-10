public abstract class Agent //Base class for RL agent
{
    protected int actionAmount; //Amount of actions that can be done
    protected double[][] qTable; //Qtable for storing states and actions
    protected double alpha = 0.1; //Alpha of the Bellman equation, the learning rate (how much new information overrides old information)
    protected double gamma = 0.9; //Gamma of the Bellman equation, which accounts for the discount of future reward in the equation
    protected double epsillon = 0.9; //Epsillon, an additional aspect that determines how much the agent wants to explore.

    protected double epsillonDecay = 0.9; //Rate at which epsillon decays
    public int x; //Current x coordinate in maze

    public int y; //Current y coordinate in maze

    public int steps = 0; //Hoe many steps have been taken

    public HashSet<int> explored = new HashSet<int>(); //Explored states

    public Random randomAction = new Random(); //Random for epsillon greedy movements

    public Agent(int actionAmountInput, double alphaInput, double gammaInput, double epsillonInput, double epsillonDecayInput)
    {

        alpha = alphaInput;
        gamma = gammaInput;
        epsillon = epsillonInput;
        epsillonDecay = epsillonDecayInput;
        actionAmount = actionAmountInput;
    }

    public void ReInitialiseQTable(int stateAmountInput) //Creates fresh Q table
    {
        ClearQTable();
        qTable  = new double[stateAmountInput][];
        for(int i = 0; i < stateAmountInput; ++i)
        {
            qTable[i] = new double[actionAmount];
        }
    }

    public (int, int) ReturnCoords(double state) //Returns the coordinates of the state
    {
        int wHelper = (int)Math.Floor((Math.Sqrt(8 * state + 1) - 1) / 2);
        int tHelper = ((wHelper * wHelper) + wHelper) / 2;

        int y = (int)state - tHelper;
        int x = wHelper - y;

        return (x,y);
    }

    public List<(int, int)> ReturnExploredToCoords() //Returns the hashset of explored coordinates as a list of coordinates
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

    public void ClearQTable() //Clears Q Table completely 
    {
        if(qTable != null)
        {
            for (int i = 0; i < qTable.Length; i++)
            {
                Array.Clear(qTable[i], 0, qTable[i].Length);
            }
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

    public int GenerateAction(int agentX, int agentY) //Generates epsillon greedy action
    {
        if(randomAction.NextDouble() < epsillon)
        {
            return randomAction.Next(actionAmount);
        }

        return Array.IndexOf(qTable[GenerateState(agentX, agentY)], qTable[GenerateState(agentX, agentY)].Max());
    }

    public abstract void UpdateTable(int action, double reward, int nextState);
}