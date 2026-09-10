public class QLearningAgent : Agent
{
    public QLearningAgent(int actionAmountInput, double alphaInput, double gammaInput, double epsillonInput, double epsillonDecayInput) : base(actionAmountInput, alphaInput, gammaInput, epsillonInput, epsillonDecayInput)
    {
        
    }

    public override void UpdateTable(int action, double reward, int nextState) //Updates table according to Q Learning algorithm
    {
        double currentQ = qTable[GenerateState(x, y)][action];
        double maxFutureQ = qTable[nextState].Max();
        qTable[GenerateState(x, y)][action] = currentQ + alpha * (reward + (gamma * maxFutureQ) - currentQ);  
    }
}