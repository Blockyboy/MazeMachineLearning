public class Agent
{
    private int actionAmount;
    private double[][] qTable;
    private double alpha = 0.1;
    private double gamma = 0.9;
    private double epsillon = 0.9;
    public int x;

    public int y;

    public HashSet<int> explored = new HashSet<int>();

    public Random randomAction = new Random();

    public Agent(int stateAmountInput, int actionAmountInput)
    {
        actionAmount = actionAmountInput;
        qTable  = new double[stateAmountInput][];
        for(int i = 0; i < stateAmountInput; ++i)
        {
            qTable[i] = new double[actionAmount];
        }
    }

    public void DecayEpsillon(double decayValue)
    {
        epsillon = epsillon * decayValue;
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