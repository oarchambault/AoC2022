var motions = File.ReadAllLines("input.txt");

var head = new Head();
var tail = new Tail();

tail.Follow(head);

foreach (var motion in motions)
{
    var split = motion.Split(' ');
    var direction = split[0];
    var stepCount = int.Parse(split[1]);

    for (int step = 0; step < stepCount; step++)
    {
        head.Move(direction);
        tail.Follow(head);
    }

    Console.WriteLine($"Tail visited {tail.VisitedPositions.Count} different positions.");
}

record Position
{
    public Position(int x, int y)
    {
        X = x;
        Y = y;
    }
    public int X { get; set; }

    public int Y { get; set; }

    public static Position Origin() => new Position(0, 0);

    public bool Covers(Position another) => X == another.X && Y == another.Y;

    public bool Touches(Position another)
    {
        return another.X >= X - 1 && another.X <= X + 1 && another.Y >= Y - 1 && another.Y <= Y + 1;
    }

    public override string ToString() => $"({X},{Y})";
}

class Head
{
    public Position Position { get; } = Position.Origin();

    public void Move(string direction)
    {
        switch (direction)
        {
            case "U":
                Position.Y++; break;
            case "R":
                Position.X++; break;
            case "D":
                Position.Y--; break;
            case "L":
                Position.X--; break;
            default: throw new ArgumentException(direction, nameof(direction));
        };

        Console.WriteLine($"Head moved to {Position}");
    }
}

class Tail
{
    public Tail()
    {
        VisitedPositions.Add(Position);
    }

    public Position Position { get; } = Position.Origin();

    public HashSet<Position> VisitedPositions { get; } = new();

    public void Follow(Head head)
    {
        if (Position.Covers(head.Position))
        {
            Console.WriteLine($"Tail covers head.");
        }
        else if (Position.Touches(head.Position))
        {
            Console.WriteLine($"Tail {Position} touches head.");
        }
        else
        {
            Console.WriteLine($"Tail {Position} must follow head.");
            MovesToward(head.Position);
            Console.WriteLine($"Tail moved to {Position}.");
            VisitedPositions.Add(Position);
        }
    }

    private void MovesToward(Position target)
    {
        if(target.X > Position.X)
        {
            Position.X++;
        }
        else if(target.X < Position.X)
        {
            Position.X--;
        }

        if (target.Y > Position.Y)
        {
            Position.Y++;
        }
        else if (target.Y < Position.Y)
        {
            Position.Y--;
        }

        //if (target.X == Position.X)
        //{
        //    if (target.Y < Position.Y)
        //    {
        //        Position.Y++;
        //    }
        //    else if(target.Y > Position.Y)
        //    {
        //        Position.Y++;
        //    }
        //}
        //else if (target.Y == Position.Y)
        //{
        //    if (target.X < Position.X)
        //    {
        //        Position.X++;
        //    }
        //    else if (target.X > Position.X)
        //    {
        //        Position.X++;
        //    }
        //}
        //else if(target.X < Position.X && target.Y < Position.Y)
        //{
        //    Position.X--;
        //    Position.Y--;
        //}
        //else if (target.X < Position.X && target.Y > Position.Y)
        //{
        //    Position.X--;
        //    Position.Y;
        //}
        //else if (target.X < Position.X && target.Y < Position.Y)
        //{
        //    Position.X--;
        //    Position.Y--;
        //}
        //else if (target.X < Position.X && target.Y < Position.Y)
        //{
        //    Position.X--;
        //    Position.Y--;
        //}
    }
}
