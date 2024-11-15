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
			    await Task.Delay(150);
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

double[][] intermediateNeurons =
[
	[859.1268723518754, -919.9865283568706, -512.8579947426972, -202.17221655211983],
	[621.7589998942597, -577.3211358825223, 604.4696178782842, -718.7457389367084],
	[-553.0929904257694, -572.5654579367088, 576.8314167352114, 684.6634737891045],
	[434.3717383996109, 499.61409296696115, 324.89751888471255, -367.70161956529296],
	[824.4303538965598, -258.44979689873514, -705.1929382677883, 593.020946037881],
	[166.03661791311902, -974.6661210890281, -837.5880375809417, -419.6246994268995],
	[375.9135151807566, -874.6915800986923, 708.047549993955, -240.5105770569404],
	[706.6429159522722, -918.2582320958128, -876.8105651788065, 822.4916432540545]
];

double[][] outputNeurons =
[
	[-3.523382950742871, 759.1405368511896, -475.6660792576091, 683.1874528276924, 547.0082422341277, -406.11165667691137, -430.4557443631745, -833.0755493947884],
	[435.03810543182135, 975.11650349296, 263.7498901416002, -320.98925252144636, 395.3669666599078, 983.6723244740651, 347.4207929376896, 206.25450653881762],
	[-653.3281249312059, -768.7482159395145, -955.9595795956919, -245.4959292784813, -964.0291282360724, 521.7501708008253, -358.6880800482686, 758.2860675431941],
	[76.74266592071126, -511.91312020892224, 358.7914320349994, -862.032043884926, -54.72379806013225, -995.7336837079396, 349.0946938447303, 428.80784757049923]
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
