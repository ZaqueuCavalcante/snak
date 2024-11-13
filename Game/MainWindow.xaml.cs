﻿using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Controls;

namespace Game;

public partial class MainWindow
{
    private readonly int _rows = 10;
    private readonly int _columns = 10;
    private readonly Image[,] _gridImages;
    private GameState _game;
    private PlayerMode _playerMode = PlayerMode.Human;

	public MainWindow()
    {
        InitializeComponent();
        _gridImages = SetupGridImages();
        _game = new GameState(_rows, _columns);
    }

	private Image[,] SetupGridImages()
    {
        GameGrid.Rows = _rows;
        GameGrid.Columns = _columns;
        GameGrid.Width = GameGrid.Height * (_columns / (double) _rows);

        var images = new Image[_rows, _columns];
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

	private async Task RunGame()
	{
        Draw();
        await DrawCountDown();
        Overlay.Visibility = Visibility.Hidden;

        await GameLoop();

        if (_game.GameOver)
        {
			await DrawGameOver();
        }
        if (_game.Zerou)
        {
	        await DrawZerou();
        }
		_game = new GameState(_rows, _columns);
	}

    private async Task GameLoop()
    {
	    if (_playerMode == PlayerMode.Human)
	    {
	        while (!_game.GameOver & !_game.Zerou)
	        {
	            await Task.Delay(1000);
	            _game.MoveSnake();
	            Draw();
	        }
	    }

	    if (_playerMode == PlayerMode.DummyIfElse)
	    {
		    while (!_game.GameOver & !_game.Zerou)
		    {
			    await Task.Delay(50);
                _game.DummyDecision();
			    _game.MoveSnake();
			    Draw();
		    }
	    }

	    if (_playerMode == PlayerMode.SmartIfElse)
	    {
		    while (!_game.GameOver & !_game.Zerou)
		    {
			    await Task.Delay(40);
                _game.SmartDecision();
			    _game.MoveSnake();
			    Draw();
		    }
	    }

	    if (_playerMode == PlayerMode.NeuralNetwork)
	    {
			// TODO: Load from file.json
double[][] intermediateNeurons =
[
	[-86.3, -586.09, 880.63, 686.35],
	[637.55, -277.61, -302.11, -243.72],
	[-991.37, -947.74, -30.2, -186.53],
	[-421.65, -459.67, -600.34, -741.77],
	[-728.57, -945.43, -763.04, 519.97],
	[-933.91, -38.84, -505.99, 955.08],
	[-116.15, 725.0, 31.04, 223.9],
	[-920.32, -362.8, 405.0, 378.45]
];
double[][] outputNeurons =
[
	[-338.05, 73.8, 718.21, 201.39, -722.18, 656.46, -803.71, 585.31],
	[27.0, 21.26, -244.88, 376.91, 653.97, -988.59, -282.85, -41.48],
	[-767.21, 126.09, 814.37, 718.18, -620.35, -567.12, 183.69, 667.27],
	[212.37, -899.51, -555.72, 243.09, -441.16, 354.56, 395.26, -909.6]
];
			_game.NeuralNetwork = new NeuralNetwork(intermediateNeurons, outputNeurons);

		    while (!_game.GameOver & !_game.Zerou)
		    {
			    await Task.Delay(40);
                _game.NeuralNetworkDecision();
			    _game.MoveSnake();
			    Draw();
		    }
	    }
    }

    // EVENTS - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - //

	private async void Window_PreviewKeyDown(object _, KeyEventArgs e)
	{
		if (Overlay.Visibility == Visibility.Visible)
		{
			e.Handled = true;
		}

		if (!_game.IsRunning)
		{
			_playerMode = GetPlayerMode(e);
			
			_game.IsRunning = true;
			await RunGame();
			_game.IsRunning = false;
		}
	}

	private void Window_KeyDown(object _, KeyEventArgs e)
	{
		if (_game.GameOver) return;
		if (_playerMode != PlayerMode.Human) return;

        switch (e.Key)
        {
            case Key.Up:
                _game.Snake.GoTo(Direction.Up); break;
            case Key.Right:
                _game.Snake.GoTo(Direction.Right); break;
            case Key.Down:
                _game.Snake.GoTo(Direction.Down); break;
            case Key.Left:
                _game.Snake.GoTo(Direction.Left); break;
        }
	}

	private static PlayerMode GetPlayerMode(KeyEventArgs e)
	{
		return e.Key switch
		{
			Key.H => PlayerMode.Human,
			Key.D => PlayerMode.DummyIfElse,
			Key.S => PlayerMode.SmartIfElse,
			Key.N => PlayerMode.NeuralNetwork,
			_ => PlayerMode.Human
		};
	}

    // DRAW - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - //

    private void Draw()
    {
        DrawGrid();
        DrawSnakeHead();
		ScoreText.Text = $"SCORE = {_game.Score} | STEPS = {_game.Steps}";
    }

	private void DrawGrid()
    {
		var opct = 1.0;
		var cells = _game.Snake.CellsPositions.ToList().ConvertAll(x =>
		{
			opct -= 0.007;
			return new { x.Row, x.Column, Opacity = opct };
		});

		for (int row = 0; row < _rows; row++)
		{
			for (int column = 0; column < _columns; column++)
			{
				var cell = _game.Grid.GetCellAt(row, column);
                _gridImages[row, column].Source = cell.ToImage();
                _gridImages[row, column].RenderTransform = Transform.Identity;

				var position = cells.FirstOrDefault(x => x.Row == row && x.Column == column);
                _gridImages[row, column].Opacity = position?.Opacity ?? 1.0;
			}
		}
	}

	private void DrawSnakeHead()
	{
		var headPosition = _game.Snake.GetHeadPosition();
        var image = _gridImages[headPosition.Row, headPosition.Column];
        image.Source = Images.Head;

        var rotation = _game.Snake.HeadDirection.ToRotation();
        image.RenderTransform = new RotateTransform(rotation);
	}

	private async Task DrawCountDown()
    {
        for (int i = 3; i >= 1; i--)
        {
            OverlayText.Text = i.ToString();
            await Task.Delay(500);
        }
    }

	private async Task DrawGameOver()
	{
        await DrawDeadSnake();
		await Task.Delay(1000);
		Overlay.Visibility = Visibility.Visible;
        OverlayText.Text = "Human | Dummy | Smart | Neural";
	}

	private async Task DrawZerou()
	{
		await Task.Delay(1000);
		Overlay.Visibility = Visibility.Visible;
		OverlayText.Text = "🐍 ZEROU 🐍";
		await Task.Delay(2000);
		OverlayText.Text = "Human | Dummy | Smart | Neural";
	}

    private async Task DrawDeadSnake()
    {
        var positions = _game.Snake.CellsPositions.ToList();

        for (int i = 0; i < positions.Count; i++)
        {
            var position = positions[i];
            var source = (i == 0) ? Images.DeadHead : Images.DeadBody;
            _gridImages[position.Row, position.Column].Source = source;
            await Task.Delay(50);
        }
    }
}
