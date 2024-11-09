using System.IO;
using System.Text.Json;

namespace Game;

public static class SnakExtensions
{
   public static double Multiply(double[] a, double[] b)
   {
      var value = 0d;
      for (int i = 0; i < a.Length; i++)
      {
         value += a[i] * b[i];
      }
      return value;
   }

   public static double VelX(this Direction direction)
   {
      if (direction == Direction.Right) return 1.00;
      if (direction == Direction.Left) return -1.00;
      return 0.00;
   }

   public static double VelY(this Direction direction)
   {
      if (direction == Direction.Down) return 1.00;
      if (direction == Direction.Up) return -1.00;
      return 0.00;
   }

   public static double Absolute(this Direction direction)
   {
      if (direction == Direction.Right) return 0.50;
      if (direction == Direction.Down) return 1.00;
      if (direction == Direction.Left) return -0.50;
      return -1.00;
   }

   public static double AbsoluteDeltaRow(Position a, Position b, int rows)
   {
      return (b.Row - a.Row) / (double)rows;
   }

   public static double AbsoluteDeltaColumn(Position a, Position b, int columns)
   {
      return (b.Column - a.Column) / (double)columns;
   }

   public static List<T> TakePercent<T>(this List<T> source, double percent)
   {
      int count = (int)(source.Count * percent / 100);
      return source.Take(count).ToList();
   }

   public static double[] Merge(double[] a, double[] b)
   {
      var random = new Random();
      var value = new double[a.Length];
      for (int i = 0; i < a.Length; i++)
      {
         value[i] = random.NextDouble() > 0.5 ? a[i] : b[i];
      }
      return value;
   }

   public static NeuralNetwork Merge(NeuralNetwork a, NeuralNetwork b)
   {
      var result = NeuralNetwork.NewRandom();

      for (int neuron = 0; neuron <= a.IntermediateNeurons.GetUpperBound(0); neuron++)
      {
         result.IntermediateNeurons[neuron] = Merge(a.IntermediateNeurons[neuron], b.IntermediateNeurons[neuron]);
      }

      for (int neuron = 0; neuron <= a.OutputNeurons.GetUpperBound(0); neuron++)
      {
         result.OutputNeurons[neuron] = Merge(a.OutputNeurons[neuron], b.OutputNeurons[neuron]);
      }

      return result;
   }

   private static JsonSerializerOptions _options = new JsonSerializerOptions() 
   { 
      WriteIndented = true
   };
   public static void SaveJson(object obj, string fileName)
   {
      var jsonString = JsonSerializer.Serialize(obj, _options);
      File.WriteAllText(fileName, jsonString);
   }
}

public static class TestData
{
   public static IEnumerable<object[]> Directions()
   {
      foreach (var value in new List<(Direction, double)>()
      {
         (Direction.Right, 0.50),
         (Direction.Down, 1.00),
         (Direction.Left, -0.50),
         (Direction.Up, -1.00),
      })
      {
         yield return [value];
      }
   }

   public static IEnumerable<object[]> VelXDirections()
   {
      foreach (var value in new List<(Direction, double)>()
      {
         (Direction.Right, 1.00),
         (Direction.Down, 0.00),
         (Direction.Left, -1.00),
         (Direction.Up, 0.00),
      })
      {
         yield return [value];
      }
   }

   public static IEnumerable<object[]> VelYDirections()
   {
      foreach (var value in new List<(Direction, double)>()
      {
         (Direction.Right, 0.00),
         (Direction.Down, 1.00),
         (Direction.Left, 0.00),
         (Direction.Up, -1.00),
      })
      {
         yield return [value];
      }
   }

   public static IEnumerable<object[]> DeltaRows()
   {
      foreach (var value in new List<(Position, Position, double)>()
      {
         (new Position(2, 2), new Position(2, 2), 0.000),

         (new Position(2, 2), new Position(2, 3), 0.000),
         (new Position(2, 2), new Position(3, 3), 0.100),
         (new Position(2, 2), new Position(3, 2), 0.100),
         (new Position(2, 2), new Position(3, 1), 0.100),
         (new Position(2, 2), new Position(2, 1), 0.000),

         (new Position(2, 2), new Position(1, 1), -0.100),
         (new Position(2, 2), new Position(1, 2), -0.100),
         (new Position(2, 2), new Position(1, 3), -0.100),

         (new Position(0, 0), new Position(9, 9), 0.900),
         (new Position(0, 9), new Position(9, 0), 0.900),

         (new Position(9, 9), new Position(0, 0), -0.900),
         (new Position(9, 0), new Position(0, 9), -0.900),
      })
      {
         yield return [value];
      }
   }

   public static IEnumerable<object[]> DeltaColumns()
   {
      foreach (var value in new List<(Position, Position, double)>()
      {
         (new Position(2, 2), new Position(2, 2), 0.000),

         (new Position(2, 2), new Position(2, 3), 0.100),
         (new Position(2, 2), new Position(3, 3), 0.100),
         (new Position(2, 2), new Position(3, 2), 0.000),
         (new Position(2, 2), new Position(3, 1), -0.100),
         (new Position(2, 2), new Position(2, 1), -0.100),

         (new Position(2, 2), new Position(1, 1), -0.100),
         (new Position(2, 2), new Position(1, 2), 0.000),
         (new Position(2, 2), new Position(1, 3), 0.100),

         (new Position(0, 0), new Position(9, 9), 0.900),
         (new Position(0, 9), new Position(9, 0), -0.900),

         (new Position(9, 9), new Position(0, 0), -0.900),
         (new Position(9, 0), new Position(0, 9), 0.900),
      })
      {
         yield return [value];
      }
   }
}
