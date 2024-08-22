namespace Game;

public static class Distance
{
   public static double Manhattan(Position a, Position b)
   {
      return Math.Abs(a.Row - b.Row) + Math.Abs(a.Column - b.Column);
   }
}
