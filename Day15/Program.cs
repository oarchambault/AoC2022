var inputLines = File.ReadAllLines("input.txt");

// PART 1
//var line = 10;
var line = 2_000_000;

var sensorBeacons = ParseInput(inputLines).ToArray();

var coveredPositions = new HashSet<int>();
foreach (var sensorBeacon in sensorBeacons)
{
    foreach (var coveredPosition in sensorBeacon.GetCoveredPositionsAtRow(line))
    {
        coveredPositions.Add(coveredPosition);
    }
}

var allSensors = sensorBeacons.Select(s => s.Sensor).Where(s => s.Y == line);
var allBeacons = sensorBeacons.Select(b => b.Beacon).Where(b => b.Y == line);

foreach (var item in allSensors.Union(allBeacons))
{
    coveredPositions.Remove(item.X);
}

Console.WriteLine();
Console.WriteLine($"In the row where y={line}, there are {coveredPositions.Count} positions where a beacon cannot be present.");

// PART 2
//var max = 20;
var max = 4_000_000;

var borders = new Dictionary<Position, int>();
foreach (var sensorBeacon in sensorBeacons)
{
    var borderingPositions = sensorBeacon.GetSensorBorderingPositions()
        .Where(p => p.X >= 0 && p.Y >= 0 && p.X <= max && p.Y <= max)
        .ToArray();

    foreach (var borderingPosition in borderingPositions)
    {
        if (!borders.ContainsKey(borderingPosition))
        {
            borders[borderingPosition] = 1;
        }
        else
        {
            borders[borderingPosition]++;
        }
    }
}

var possiblePositions = borders.Where(p => p.Value >= 4).Select(p => p.Key).ToList();

//foreach (var sensorBeacon in sensorBeacons)
//{
//    for (int i = possiblePositions.Count - 1; i >= 0; i--)
//    {
//        if (sensorBeacon.SensorCoversPosition(possiblePositions[i]))
//        {
//            possiblePositions.RemoveAt(i);
//        }
//    }
//}

var position = possiblePositions.Single();
var frequency = position.X * 4_000_000L + position.Y;

Console.WriteLine();
Console.WriteLine($"The only possible position for the distress beacon is {position} and its tuning frequency is {frequency}.");


static IEnumerable<SensorAndBeacon> ParseInput(string[] inputLines)
{
    foreach (var inputLine in inputLines)
    {
        var splitCoords = inputLine
            .Replace("Sensor at x=", "")
            .Replace(" closest beacon is at x=", "")
            .Replace(" y=", "")
            .Split(' ', ':', ',')
            .Select(int.Parse)
            .ToArray();

        yield return new SensorAndBeacon(splitCoords[0], splitCoords[1], splitCoords[2], splitCoords[3]);
    }
}

record Position(int X, int Y)
{
    public override string ToString() => $"({X},{Y})";
}

class SensorAndBeacon
{
    public SensorAndBeacon(int sensorX, int sensorY, int beaconX, int beaconY)
    {
        Sensor = new Position(sensorX, sensorY);
        Beacon = new Position(beaconX, beaconY);

        Distance = GetDistance();
    }

    public Position Sensor { get; } = default!;
    public Position Beacon { get; } = default!;
    public int Distance { get; }


    public IEnumerable<int> GetCoveredPositionsAtRow(int y)
    {
        var minY = Sensor.Y - Distance;
        var maxY = Sensor.Y + Distance;

        if (y > maxY || y < minY)
        {
            //Console.WriteLine($"Row {y} is not crossing sensor {Sensor} (D={Distance}) range.");
            return Enumerable.Empty<int>();
        }

        var positions = new List<int>();

        var distanceFromLine = Math.Abs(y - Sensor.Y);

        //Console.WriteLine($"Row {y} is crossing sensor {Sensor} (D={Distance}) range. Distance to line is {distanceFromLine}.");

        var minX = Sensor.X - Distance + distanceFromLine;
        var maxX = Sensor.X + Distance - distanceFromLine;

        for (int x = minX; x <= maxX; x++)
        {
            positions.Add(x);
        }

        return positions;
    }

    public IEnumerable<Position> GetSensorBorderingPositions()
    {
        var borderingDistance = Distance + 1;

        for (int i = 0; i < borderingDistance; i++)
        {
            yield return new Position(Sensor.X + i, Sensor.Y - borderingDistance + i);
            yield return new Position(Sensor.X + borderingDistance - i, Sensor.Y + i);
            yield return new Position(Sensor.X - i, Sensor.Y + borderingDistance - i);
            yield return new Position(Sensor.X - borderingDistance + i, Sensor.Y - i);
        }
    }

    public bool SensorCoversPosition(Position position)
    {
        var distance = GetDistance(Sensor, position);
        return distance <= Distance;
    }

    private int GetDistance() => GetDistance(Sensor, Beacon);

    private static int GetDistance(Position p1, Position p2) => Math.Abs(p1.X - p2.X) + Math.Abs(p1.Y - p2.Y);
}