Maze maze = new();
Agent agent = new(((7 + 7) * (7+7+1)) / 2 + 7, 4);
Environment environment = new(agent, 3, 3);
environment.RunLearning();