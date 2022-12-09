var motions = File.ReadAllLines("input.txt");

var head = new Head();
var tail = new Knot();

var ropeTail = new Knot[9];
for (int i = 0; i < ropeTail.Length; i++)
{
    ropeTail[i] = new Knot();
}

foreach (var motion in motions)
{
    var split = motion.Split(' ');
    var direction = split[0];
    var stepCount = int.Parse(split[1]);

    for (int step = 0; step < stepCount; step++)
    {
        head.Move(direction);
        tail.Follow(head.Position);

        for (int i = 0; i < ropeTail.Length; i++)
        {
            if (i == 0)
            {
                ropeTail[i].Follow(head.Position);
            }
            else
            {
                ropeTail[i].Follow(ropeTail[i - 1].Position);
            }
        }
    }
}

Console.WriteLine($"Tail visited {tail.VisitedPositions.Count} different positions.");
Console.WriteLine($"Rope's tail visited {ropeTail[8].VisitedPositions.Count} different positions.");


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
    }
}

class Knot
{
    public Knot()
    {
        VisitedPositions.Add(Position);
    }

    public Position Position { get; } = Position.Origin();

    public HashSet<Position> VisitedPositions { get; } = new();

    public void Follow(Position target)
    {
        if (Position.Covers(target) || Position.Touches(target))
        {
            return;
        }

        MovesToward(target);
        VisitedPositions.Add(Position);
    }

    private void MovesToward(Position target)
    {
        if (target.X > Position.X)
        {
            Position.X++;
        }
        else if (target.X < Position.X)
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
    }
}
