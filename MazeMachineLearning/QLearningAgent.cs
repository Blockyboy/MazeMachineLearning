public class QLearningAgent : Agent
{
    public QLearningAgent(int stateAmountInput, int actionAmountInput) : base(stateAmountInput, actionAmountInput)
    {
        
    }

    public override void UpdateTable(int action, double reward, int nextState)
    {
        double currentQ = qTable[GenerateState(x, y)][action];
        double maxFutureQ = qTable[nextState].Max();
        qTable[GenerateState(x, y)][action] = currentQ + alpha * (reward + (gamma * maxFutureQ) - currentQ);  
    }
}