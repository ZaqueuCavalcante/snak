namespace Game;

public static class SnakExtensions
{
   public static double Manhattan(Position a, Position b)
   {
      return Math.Abs(a.Row - b.Row) + Math.Abs(a.Column - b.Column);
   }

   public static double AbsoluteAngle(Position a, Position b)
   {
      if (b.Row == a.Row)
      {
         return b.Column >= a.Column ? 0.000 : 0.500;
      }

      if (b.Column == a.Column)
      {
         return b.Row >= a.Row ? 0.250 : 0.750;
      }

      if (b.Row > a.Row && b.Column > a.Column)
      {
         return 0.125;
      }

      if (b.Row > a.Row && b.Column < a.Column)
      {
         return 0.375;
      }

      if (b.Row < a.Row && b.Column < a.Column)
      {
         return 0.625;
      }

      if (b.Row < a.Row && b.Column > a.Column)
      {
         return 0.875;
      }

      return -1000_000;
   }
}

public static class TestData
{
   public static IEnumerable<object[]> Angles()
   {
      foreach (var value in new List<(Position, Position, double)>()
      {
         (new Position(2, 2), new Position(2, 2), 0.000),

         (new Position(2, 2), new Position(2, 3), 0.000),
         (new Position(2, 2), new Position(3, 3), 0.125),
         (new Position(2, 2), new Position(3, 2), 0.250),
         (new Position(2, 2), new Position(3, 1), 0.375),
         (new Position(2, 2), new Position(2, 1), 0.500),

         (new Position(2, 2), new Position(1, 1), 0.625),
         (new Position(2, 2), new Position(1, 2), 0.750),
         (new Position(2, 2), new Position(1, 3), 0.875),
      })
      {
         yield return [value];
      }
   }
}
