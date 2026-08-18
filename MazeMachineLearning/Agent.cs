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

    public Agent(int stateAmountInput, int actionAmountInput, int startX, int startY)
    {
        actionAmount = actionAmountInput;
        x = startX;
        y = startY;
        qTable  = new double[stateAmountInput][];
        for(int i = 0; i < stateAmountInput; ++i)
        {
            qTable[i] = new double[actionAmount];
        }
    }

    private int GenerateState()
    {
        return ((x+y) * (x+y+1)) / 2 + y;
    }

    private int GenerateAction()
    {
        if(randomAction.NextDouble() < epsillon)
        {
            return randomAction.Next(actionAmount);
        }

        return Array.IndexOf(qTable[GenerateState()], qTable[GenerateState()].Max());
    }

    private void UpdateTable(int action, double reward, int nextState)
    {
        double currentQ = qTable[GenerateState()][action];
        double maxFutureQ = qTable[]
    }
    
}