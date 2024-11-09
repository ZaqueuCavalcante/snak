namespace Game;

public static class SnakTrainer
{
   public static void Run()
   {
      var games = new List<GameState>();
      for (int i = 0; i < 1_000_000; i++)
      {
         games.Add(new GameState(10, 10));
      }

      foreach (var game in games)
      {
         while (!game.GameOver & !game.Zerou & game.Steps < 1_000)
         {
            game.NeuralNetworkDecision();
            game.MoveSnake();
         }
      }

      var best = games.OrderByDescending(x => x.Score).First();

      var nn = best.NeuralNetwork;
   }
}
