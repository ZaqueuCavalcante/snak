using System.Windows;
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
				[890.8540553648327, -463.4474113577445, -160.80040257347753, -206.33353751978325],
				[629.2396160577878, 577.7605114476341, -522.4836821188098, -859.7382403207157],
				[-533.3475093882323, 943.6111169481321, 14.08582666234804, 895.8125584692605],
				[-863.4519264428952, -985.828218471201, 135.38635483538815, -749.9317096118807],
				[115.8469242177714, -400.192156296721, -860.5666794926278, -261.7318798627242],
				[-845.9514216524933, -821.5238479407096, 66.65981803403065, 65.62406519184174],
				[-777.2943796914309, -47.83247895054069, 689.952720638021, 221.2486835293862],
				[-18.24738597935243, 926.1880003581284, -48.71571128442963, 710.5212586120078]
			];
			double[][] outputNeurons =
			[
				[-578.0080327202388, 462.03930937317887, 899.9940829549121, -184.4619592654742, -73.6634601482682, 750.676005433043, 490.43710186790145, -698.9348116923895],
				[-339.44637836161996, 89.65459240145697, 468.52254006045496, -52.96150250534288, -760.0025522013987, -955.789313322917, 556.3753079375817, 209.02638947299852],
				[346.8449008291559, 281.6855268865122, -977.9231083747104, 682.9935778767087, -920.6669846358055, 68.49625454161037, 157.2408741895572, -438.7719211424021],
				[73.14723613417664, 868.2104465809239, 166.8413698697973, 96.99823532114169, -866.4657275096062, -399.1450792172559, 449.0070719990367, 495.29761263869136]
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
