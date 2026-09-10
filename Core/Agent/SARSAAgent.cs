public class SARSAAgent : Agent
{
    public SARSAAgent(int actionAmountInput, double alphaInput, double gammaInput, double epsillonInput, double epsillonDecayInput) : base(actionAmountInput, alphaInput, gammaInput, epsillonInput, epsillonDecayInput)
    {
        
    }

    public override void UpdateTable(int action, double reward, int nextState) //Updates table according to SARSA algorithm
    {
        double currentQ = qTable[GenerateState(x, y)][action];
        double nextQ = qTable[nextState][GenerateAction(ReturnCoords(nextState).Item1, ReturnCoords(nextState).Item2)];
        qTable[GenerateState(x, y)][action] = currentQ + alpha * (reward + (gamma * nextQ) - currentQ);  
    }
}