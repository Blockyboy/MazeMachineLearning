using Spectre.Console;

var mainMenuSelection = "";
Dictionary<string, double> AgentSettings = new Dictionary<string, double>
{
    {"Type", 0}, 
    {"Alpha", 0.1},
    {"Gamma", 0.1},
    {"Epsillon", 0.9},
    {"Epsillon Decay", 0.9},
};

Agent agent = new QLearningAgent(4, AgentSettings["Alpha"], AgentSettings["Gamma"], AgentSettings["Epsillon"], AgentSettings["Epsillon Decay"]);

Environment environment;

while(mainMenuSelection != "Exit")
{
    mainMenuSelection = AnsiConsole.Prompt(
    new SelectionPrompt<string>()
        .Title("Maze Machine Learning (Console App)")
        .AddChoices("Run", "Agent Settings", "Environment Settings", "Exit"));

    AnsiConsole.Clear();

    switch(mainMenuSelection)
    {
        case "Run": 
            if(AgentSettings["Type"] == 0)
            {
                agent = new QLearningAgent(4, AgentSettings["Alpha"], AgentSettings["Gamma"], AgentSettings["Epsillon"], AgentSettings["Epsillon Decay"]);
            }
            else if(AgentSettings["Type"] == 1)
            {
                agent = new SARSAAgent(4, AgentSettings["Alpha"], AgentSettings["Gamma"], AgentSettings["Epsillon"], AgentSettings["Epsillon Decay"]);
            }

            environment = new(agent);

            environment.ConsoleLearning();
        break;

        case "Agent Settings":

            var agentSettingSelection = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("What Agent setting would you like to change?")
                .AddChoices(AgentSettings.Keys.ToList()));

                switch(agentSettingSelection)
                {
                    case "Type":
                        var type = AnsiConsole.Ask<double>("What [green]type[/] of agent would you like? (0: Q Learning) (1: SARSAA) ");
                    break;

                    default:
                        var value = AnsiConsole.Ask<double>($"What [green]value[/] of {agentSettingSelection} would you like? 0 to 1");

                        if(!(0 > value && value >= 1))
                        {
                            AnsiConsole.MarkupLine("Value must be between 0 and 1");
                        }
                        else
                        {
                            AgentSettings[agentSettingSelection] = value;
                        }
                    break;
                }

        break;
    }

    AnsiConsole.Clear();
}