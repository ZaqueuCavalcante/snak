namespace Game;

public static class Distance
{
   public static double Manhattan(Position a, Position b)
   {
      return Math.Abs(a.Row - b.Row) + Math.Abs(a.Column - b.Column);
   }

   public static double Euclidian(Position a, Position b)
   {
      return Math.Pow(a.Row - b.Row, 2) + Math.Pow(a.Column - b.Column, 2);
   }
}
