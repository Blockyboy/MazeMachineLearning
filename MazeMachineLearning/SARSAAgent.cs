public class SARSAAgent : Agent
{
    public SARSAAgent(int stateAmountInput, int actionAmountInput) : base(stateAmountInput, actionAmountInput)
    {
        
    }

    public override void UpdateTable(int action, double reward, int nextState)
    {
        double currentQ = qTable[GenerateState(x, y)][action];
        double nextQ = qTable[nextState][GenerateAction(ReturnCoords(nextState).Item1, ReturnCoords(nextState).Item2)];
        qTable[GenerateState(x, y)][action] = currentQ + alpha * (reward + (gamma * nextQ) - currentQ);  
    }
}