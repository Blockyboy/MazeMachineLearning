public class Agent
{
    private int actionAmount; //Amount of actions that can be done
    private double[][] qTable; //Qtable for storing states and actions
    private double alpha = 0.9; //Alpha of the Bellman equation, the learning rate (how much new information overrides old information)
    private double gamma = 0.9; //Gamma of the Bellman equation, which accounts for the discount of future reward in the equation
    private double epsillon = 0.9; //Epsillon, an additional aspect that determines how much the agent wants to explore.

    private double epsillonDecay = 0.9;
    public int x;

    public int y;

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

    public int GenerateAction()
    {
        if(randomAction.NextDouble() < epsillon)
        {
            return randomAction.Next(actionAmount);
        }

        return Array.IndexOf(qTable[GenerateState(x, y)], qTable[GenerateState(x, y)].Max());
    }

    public void UpdateTable(int action, double reward, int nextState)
    {
        double currentQ = qTable[GenerateState(x, y)][action];
        double maxFutureQ = qTable[nextState].Max();

        qTable[GenerateState(x, y)][action] = currentQ + alpha * (reward + (gamma * maxFutureQ) - currentQ);
    }
}