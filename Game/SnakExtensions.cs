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
