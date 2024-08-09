using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Controls;

namespace Game;

public partial class MainWindow : Window
{
    private readonly int _rows = 20;
    private readonly int _columns = 20;
    private readonly Image[,] _gridImages;

    private GameState _gameState;

    private bool _gameIsRunning;

	public MainWindow()
    {
        InitializeComponent();

        _gridImages = SetupGrid();
        _gameState = new GameState(_rows, _columns);
    }

    private void Draw()
    {
        DrawGrid();
        DrawSnakeHead();

		ScoreText.Text = $"SCORE {_gameState.Score}";
    }

    private async Task GameLoop()
    {
        while (!_gameState.GameOver)
        {
            await Task.Delay(200);
            _gameState.MoveSnake();
            Draw();
        }
    }

	private async Task RunGame()
	{
        Draw();
        await ShowCountDown();
        Overlay.Visibility = Visibility.Hidden;
        await GameLoop();
        await ShowGameOver();
		_gameState = new GameState(_rows, _columns);
	}

	private async void Window_PreviewKeyDown(object sender, KeyEventArgs e)
	{
		if (Overlay.Visibility == Visibility.Visible)
		{
			e.Handled = true;
		}

		if (!_gameIsRunning)
		{
			_gameIsRunning = true;
			await RunGame();
			_gameIsRunning = false;
		}
	}

	private void Window_KeyDown(object sender, KeyEventArgs e)
	{
        if (_gameState.GameOver) return;

        switch (e.Key)
        {
            case Key.Left:
                _gameState.Snake.GoTo(Direction.Left); break;
            case Key.Right:
                _gameState.Snake.GoTo(Direction.Right); break;
            case Key.Up:
                _gameState.Snake.GoTo(Direction.Up); break;
            case Key.Down:
                _gameState.Snake.GoTo(Direction.Down); break;
        }
	}

	private Image[,] SetupGrid()
    {
        var images = new Image[_rows, _columns];
        GameGrid.Rows = _rows;
        GameGrid.Columns = _columns;

        GameGrid.Width = GameGrid.Height * (_columns / (double) _rows);

		for (int row = 0; row < _rows; row++)
		{
			for (int column = 0; column < _columns; column++)
			{
                var image = new Image
                {
                    Source = Images.Empty,
                    RenderTransformOrigin = new Point(0.5, 0.5),
                };
                images[row, column] = image;
                GameGrid.Children.Add(image);
			}
		}

        return images;
	}

	private void DrawGrid()
    {
		for (int row = 0; row < _rows; row++)
		{
			for (int column = 0; column < _columns; column++)
			{
				var cell = _gameState.Grid.Get()[row, column];
                _gridImages[row, column].Source = cell.ToImage();
                _gridImages[row, column].RenderTransform = Transform.Identity;
			}
		}
	}

	private void DrawSnakeHead()
	{
		var headPosition = _gameState.Snake.GetHead();
        var image = _gridImages[headPosition.Row, headPosition.Column];
        image.Source = Images.Head;

        var rotation = _gameState.Snake.HeadDirection.ToRotation();
        image.RenderTransform = new RotateTransform(rotation);
	}

    private async Task DrawDeadSnake()
    {
        var positions = _gameState.Snake.CellsPositions.ToList();

        for (int i = 0; i < positions.Count; i++)
        {
            var position = positions[i];
            var source = (i == 0) ? Images.DeadHead : Images.DeadBody;
            _gridImages[position.Row, position.Column].Source = source;
            await Task.Delay(50);
        }
    }




	private async Task ShowCountDown()
    {
        for (int i = 3; i >= 1; i--)
        {
            OverlayText.Text = i.ToString();
            await Task.Delay(500);
        }
    }

	private async Task ShowGameOver()
	{
        await DrawDeadSnake();
		await Task.Delay(1000);
		Overlay.Visibility = Visibility.Visible;
        OverlayText.Text = "PRESS ANY KEY TO START";
	}
}
