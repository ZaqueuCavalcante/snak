namespace Game;

public class GameState
{
    public int Rows { get; }
    public int Columns { get; }
    public Grid Grid { get; }
    public Snake Snake { get; }

    public bool IsRunning { get; set; }
    public int Score { get; private set; }
    public int Steps { get; private set; }
    public bool GameOver { get; private set; }
    public bool Zerou { get; private set; }

    public NeuralNetwork NeuralNetwork { get; private set; }

	public GameState(int rows, int columns)
    {
        Rows = rows;
        Columns = columns;
        Grid = new Grid(rows, columns);
        Snake = new Snake();

        AddSnake();
        Grid.AddFood();

        // Pegar os pesos de um arquivo?
        double[][] intermediateNeurons =
        [
            [-297.0830254774801, -694.6333103675022, -914.7228022272527],
            [319.0434706151102, 138.12778608651502, 845.2635421496104],
            [775.823777547776, 777.1921359611354, 93.09790028447651],
            [762.0135445719052, 464.67303635762846, 356.1230114112318],
            [-568.3439613556116, 619.0517535048862, -759.9628284481839],
        ];
        double[][] outputNeurons =
        [
            [-451.76115780693135, 45.440905099145766, 705.0478368701818, -888.4022816894761, -870.593761998947],
            [-884.1789508019677, -872.1655248425088, 730.6145387228539, 385.90745137822796, 499.60566719590815],
            [659.4054386758462, 355.328029300337, 927.8301937302774, -487.0529442847711, 999.5042412898849],
            [993.783224198075, 144.92184660718294, -15.87803568691038, -131.0726111249063, -26.14016853971134],
        ];
        NeuralNetwork = new NeuralNetwork(intermediateNeurons, outputNeurons);
        // NeuralNetwork = NeuralNetwork.NewRandom();
    }

    private void AddSnake()
    {
        int middleRow = Rows / 2;
        for (int column = 2; column >= 1; column--)
        {
            Grid.PutSnake(middleRow, column);
            Snake.Append(middleRow, column);
        }
    }

    private void MoveSnakeHead(Position position)
    {
        Grid.PutSnake(position);
        Snake.MoveTo(position);
    }

    private void RemoveSnakeTail()
    {
        Grid.PutEmpty(Snake.GetTailPosition());
        Snake.DropTail();
    }

    public void DummyDecision()
    {
        var headPosition = Snake.GetHeadPosition();
        var foodPosition = Grid.GetFoodPosition();

        if (headPosition.Row < foodPosition.Row)
        {
            var direction = Direction.Down;
            var targetCell = WillHit(headPosition.MoveTo(direction));
            if (targetCell != CellType.Snake)
            {
                Snake.GoTo(direction); return;
            }

            direction = Direction.Left;
            targetCell = WillHit(headPosition.MoveTo(direction));
            if (targetCell != CellType.Snake && targetCell != CellType.Outside)
            {
                Snake.GoTo(direction); return;
            }

            direction = Direction.Up;
            targetCell = WillHit(headPosition.MoveTo(direction));
            if (targetCell != CellType.Snake && targetCell != CellType.Outside)
            {
                Snake.GoTo(direction); return;
            }

            direction = Direction.Right;
            targetCell = WillHit(headPosition.MoveTo(direction));
            if (targetCell != CellType.Snake && targetCell != CellType.Outside)
            {
                Snake.GoTo(direction); return;
            }
        }
        else if (headPosition.Row > foodPosition.Row)
        {
            var direction = Direction.Up;
            var targetCell = WillHit(headPosition.MoveTo(direction));
            if (targetCell != CellType.Snake)
            {
                Snake.GoTo(direction); return;
            }

            direction = Direction.Right;
            targetCell = WillHit(headPosition.MoveTo(direction));
            if (targetCell != CellType.Snake && targetCell != CellType.Outside)
            {
                Snake.GoTo(direction); return;
            }

            direction = Direction.Down;
            targetCell = WillHit(headPosition.MoveTo(direction));
            if (targetCell != CellType.Snake && targetCell != CellType.Outside)
            {
                Snake.GoTo(direction); return;
            }

            direction = Direction.Left;
            targetCell = WillHit(headPosition.MoveTo(direction));
            if (targetCell != CellType.Snake && targetCell != CellType.Outside)
            {
                Snake.GoTo(direction); return;
            }
        }
        else if (headPosition.Column < foodPosition.Column)
        {
            var direction = Direction.Right;
            var targetCell = WillHit(headPosition.MoveTo(direction));
            if (targetCell != CellType.Snake)
            {
                Snake.GoTo(direction); return;
            }

            direction = Direction.Down;
            targetCell = WillHit(headPosition.MoveTo(direction));
            if (targetCell != CellType.Snake && targetCell != CellType.Outside)
            {
                Snake.GoTo(direction); return;
            }

            direction = Direction.Left;
            targetCell = WillHit(headPosition.MoveTo(direction));
            if (targetCell != CellType.Snake && targetCell != CellType.Outside)
            {
                Snake.GoTo(direction); return;
            }

            direction = Direction.Up;
            targetCell = WillHit(headPosition.MoveTo(direction));
            if (targetCell != CellType.Snake && targetCell != CellType.Outside)
            {
                Snake.GoTo(direction); return;
            }
        }
        else if (headPosition.Column > foodPosition.Column)
        {
            var direction = Direction.Left;
            var targetCell = WillHit(headPosition.MoveTo(direction));
            if (targetCell != CellType.Snake)
            {
                Snake.GoTo(direction); return;
            }

            direction = Direction.Up;
            targetCell = WillHit(headPosition.MoveTo(direction));
            if (targetCell != CellType.Snake && targetCell != CellType.Outside)
            {
                Snake.GoTo(direction); return;
            }

            direction = Direction.Right;
            targetCell = WillHit(headPosition.MoveTo(direction));
            if (targetCell != CellType.Snake && targetCell != CellType.Outside)
            {
                Snake.GoTo(direction); return;
            }

            direction = Direction.Down;
            targetCell = WillHit(headPosition.MoveTo(direction));
            if (targetCell != CellType.Snake && targetCell != CellType.Outside)
            {
                Snake.GoTo(direction); return;
            }
        }
    }

    public void SmartDecision()
    {
        var headPosition = Snake.GetHeadPosition();
        var directions = headPosition.GetSmartDirection(Rows, Columns);
        if (directions.Count == 1)
        {
            Snake.GoTo(directions[0]); return;
        }

        var firstPosition = headPosition.MoveTo(directions[0]);
        var secondPosition = headPosition.MoveTo(directions[1]);

        var firstTargetCell = WillHit(firstPosition);
        if (firstTargetCell == CellType.Snake)
        {
            Snake.GoTo(directions[1]); return;
        }
        var secondTargetCell = WillHit(secondPosition);
        if (secondTargetCell == CellType.Snake)
        {
            Snake.GoTo(directions[0]); return;
        }

        var foodPosition = Grid.GetFoodPosition();
        var firstDistance = Distance.Euclidian(firstPosition, foodPosition);
        var secondDistance = Distance.Euclidian(secondPosition, foodPosition);

        if (firstDistance < secondDistance)
        {
            Snake.GoTo(directions[0]); return;
        }
        if (secondDistance < firstDistance)
        {
            Snake.GoTo(directions[1]); return;
        }

        var random = new Random();
        Snake.GoTo(directions[random.Next(2)]);
    }



    public void NeuralNetworkDecision()
    {
        var headPosition = Snake.GetHeadPosition();
        var foodPosition = Grid.GetFoodPosition();

        var absoluteDirection = Snake.GetHeadDirection().Absolute();
        var absoluteDeltaRow = SnakExtensions.AbsoluteDeltaRow(headPosition, foodPosition, Rows);
        var absoluteDeltaColumn = SnakExtensions.AbsoluteDeltaColumn(headPosition, foodPosition, Columns);
        
        double[] inputs = [absoluteDirection, absoluteDeltaRow, absoluteDeltaColumn];

        var direction = NeuralNetwork.Calculate(inputs);
        
        Snake.GoTo(direction);
    }



    public void MoveSnake()
    {
        Snake.ChangeDirection();
        Steps++;

        var newHeadPosition = Snake.NextHeadPosition();
        var targetCell = WillHit(newHeadPosition);

        if (targetCell is CellType.Outside or CellType.Snake)
        {
            GameOver = true;
            return;
        }

        if (targetCell == CellType.Empty)
        {
            RemoveSnakeTail();
            MoveSnakeHead(newHeadPosition);
            return;
        }

        if (targetCell == CellType.Food)
        {
            MoveSnakeHead(newHeadPosition);
            Score++;
            Zerou = Grid.AddFood() == 0;
        }
    }

    private CellType WillHit(Position newHeadPosition)
    {
        if (Grid.IsOutside(newHeadPosition)) return CellType.Outside;

        if (newHeadPosition == Snake.GetTailPosition()) return CellType.Empty;

        return Grid.GetCellAt(newHeadPosition);
    }
}
