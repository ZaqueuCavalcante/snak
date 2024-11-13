using System.Text.Json;

namespace Game;

public static class SnakTrainer
{
   public static void Run()
   {
      const int epochs = 100;
      const int populationSize = 100_000;

      var games = new List<GameState>();
      for (int i = 0; i < populationSize; i++)
      {
         games.Add(new GameState(10, 10));
      }

      for (int epoch = 0; epoch < epochs; epoch++)
      {
         Parallel.ForEach(games, game =>
         {
            while (!game.GameOver & !game.Zerou & game.Steps < 2500)
            {
               game.NeuralNetworkDecision();
               game.MoveSnake();
            }
            game.NeuralNetwork.Score = game.Score;
         });

         // Ordenar redes segundo o score de cada uma
         var bests = games.OrderByDescending(x => x.Score).Select(x => x.NeuralNetwork).ToList();
         var slice1 = bests.TakePercent(20);
         var slice2 = bests.Skip(slice1.Count).ToList().TakePercent(40);

         Console.WriteLine($"{epoch} - {bests.First().Score}");

         // Montar nova lista de redes
         var newNetworks = new List<NeuralNetwork>();

         // - As 20% melhores passam pra nova
         newNetworks.AddRange(slice1);

         // - 40% das melhores restantes são cruzadas com as 20% melhores
         for (int i = 0; i < slice1.Count; i++)
         {
            var a = slice1.OrderBy(x => Guid.NewGuid()).First();
            var b = slice2.OrderBy(x => Guid.NewGuid()).First();
            newNetworks.Add(SnakExtensions.Merge(a, b));
         }

         // - A lista é completada novamente com redes random
         var missing = populationSize - newNetworks.Count;
         for (int i = 0; i < missing; i++)
         {
            newNetworks.Add(NeuralNetwork.NewRandom());
         }

         games = [];
         foreach (var net in newNetworks)
         {
            games.Add(new GameState(10, 10) { NeuralNetwork = net });
         }
      }

      var best = games.Select(x => x.NeuralNetwork).OrderByDescending(x => x.Score).First();
      string jsonString = JsonSerializer.Serialize(best);
      Console.WriteLine(jsonString);
   }
}
