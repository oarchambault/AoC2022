using System.Text;

var input = File.ReadAllLines("input.txt");

var rockSegments = input.Select(ParseRockFormationLine).ToList();

var cave = new Cave(rockSegments);
while (!cave.IsFlowingInfinitely)
{
    cave.DropSand();
}
cave.Print();

Console.WriteLine();
Console.WriteLine($"[Part 1] {cave.SettledSandCount} units of sand come to rest before sand starts flowing into the abyss below.");


cave = new Cave(rockSegments, withBottom: true);
while (!cave.IsSandSourceBlocked)
{
    cave.DropSand();
}

Console.WriteLine();
Console.WriteLine($"[Part 2] {cave.SettledSandCount} units of sand come to rest.");


Position[] ParseRockFormationLine(string lineInput)
{
    var coordinates = lineInput.Split(" -> ")
        .Select(c => c.Split(','))
        .Select(s => new Position(int.Parse(s[0]), int.Parse(s[1])))
        .ToArray();
    return coordinates;
}

record Position(int X, int Y)
{
    public static IEnumerable<Position> GetSegmentPositions(Position from, Position to)
    {
        yield return from;

        if (to.X == from.X)
        {
            for (int y = Math.Min(from.Y, to.Y); y < Math.Max(from.Y, to.Y); y++)
            {
                yield return new Position(from.X, y);
            }
        }
        else if (to.Y == from.Y)
        {
            for (int x = Math.Min(from.X, to.X); x < Math.Max(from.X, to.X); x++)
            {
                yield return new Position(x, from.Y);
            }
        }
        else
        {
            throw new ArgumentException("Segment should be a straight line.");
        }

        yield return to;
    }

    public Position Down() => this with { Y = Y + 1 };

    public Position DownLeft() => this with { X = X - 1, Y = Y + 1 };

    public Position DownRight() => this with { X = X + 1, Y = Y + 1 };
}

class Cave
{
    private static readonly Position SandStart = new Position(500, 0);

    public Cave(List<Position[]> rocks, bool withBottom = false)
    {
        (MinX, MaxX, MinY, MaxY) = GetCaveSize(rocks, withBottom);

        if (withBottom)
        {
            rocks.Add(new Position[] { new Position(0, MaxY), new Position(MaxX, MaxY) });
        }

        Width = MaxX + 1;
        Height = MaxY + 1;
        Matrix = new char[Width, Height];

        InitializeMatrix(rocks);
    }

    public char[,] Matrix { get; }

    public int MinX { get; }
    public int MaxX { get; }
    public int MinY { get; }
    public int MaxY { get; }

    public int Width { get; }
    public int Height { get; }
    public bool IsFlowingInfinitely { get; private set; } = false;
    public bool IsSandSourceBlocked { get; private set; } = false;
    public int SettledSandCount { get; private set; } = 0;

    private static (int minX, int maxX, int minY, int maxY) GetCaveSize(IEnumerable<Position[]> rocks, bool withBottom)
    {
        var allRockPositions = rocks.SelectMany(s => s);
        var minX = allRockPositions.Min(s => s.X);
        var maxX = allRockPositions.Max(s => s.X);
        var minY = allRockPositions.Min(s => s.Y);
        var maxY = allRockPositions.Max(s => s.Y);

        if (withBottom)
        {
            maxY += 2;
            maxX *= 2;
        }

        return (minX, maxX, minY, maxY);
    }

    private void InitializeMatrix(IEnumerable<Position[]> rocks)
    {
        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                Matrix[x, y] = '.';
            }
        }

        foreach (var rock in rocks)
        {
            for (int i = 0; i < rock.Length - 1; i++)
            {
                var positions = Position.GetSegmentPositions(rock[i], rock[i + 1]);
                foreach (var position in positions)
                {
                    SetMatrix(position, '#');
                }
            }
        }
    }

    private void SetMatrix(Position position, char value) => Matrix[position.X, position.Y] = value;

    public void Print()
    {

        for (int y = 0; y < Height; y++)
        {
            var sb = new StringBuilder();
            for (int x = MinX; x < Width; x++)
            {
                sb.Append(Matrix[x, y]);
            }
            Console.WriteLine(sb.ToString());
        }
    }

    public void DropSand()
    {
        var fallingSand = new Position(SandStart.X, SandStart.Y);

        var nextPosition = GetNextFallingPosition(fallingSand);
        while (nextPosition != null && nextPosition != fallingSand)
        {
            fallingSand = nextPosition;
            nextPosition = GetNextFallingPosition(fallingSand);
        }

        if (nextPosition == null)
        {
            IsFlowingInfinitely = true;
        }
        else
        {
            SetMatrix(fallingSand, 'o');
            SettledSandCount++;

            if (fallingSand == SandStart)
            {
                IsSandSourceBlocked = true;
            }
        }
    }

    private Position? GetNextFallingPosition(Position current)
    {
        if (IsOutOfBound(current))
        {
            return null;
        }

        var down = current.Down();
        if (IsOutOfBound(down))
        {
            return null;
        }
        if (!IsBlocked(down))
        {
            return down;
        }

        var downLeft = current.DownLeft();
        if (IsOutOfBound(downLeft))
        {
            return null;
        }
        if (!IsBlocked(downLeft))
        {
            return downLeft;
        }

        var downRight = current.DownRight();
        if (IsOutOfBound(downRight))
        {
            return null;
        }
        if (!IsBlocked(downRight))
        {
            return downRight;
        }

        return current;
    }

    private bool IsBlocked(Position position) => Matrix[position.X, position.Y] != '.';

    private bool IsOutOfBound(Position position) => position.X < 0 || position.X > MaxX || position.Y < 0 || position.Y > MaxY;
}