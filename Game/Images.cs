using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Game;

public static class Images
{
    public readonly static ImageSource Empty = LoadPng(nameof(Empty));
    public readonly static ImageSource Body = LoadPng(nameof(Body));
    public readonly static ImageSource Head = LoadPng(nameof(Head));
    public readonly static ImageSource Food = LoadPng(nameof(Food));
    public readonly static ImageSource DeadBody = LoadPng(nameof(DeadBody));
    public readonly static ImageSource DeadHead = LoadPng(nameof(DeadHead));

	private static BitmapImage LoadPng(string fileName)
	{
		return new BitmapImage(new Uri($"Assets/{fileName}.png", UriKind.Relative));
	}
}
