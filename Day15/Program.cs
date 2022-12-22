var inputLines = File.ReadAllLines("input.txt");
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

// TODO: PART 2
//for (int row = 0; row < 4_000_000; row++)
//{
//    var uncovered = Enumerable.Range(0, 4_000_000).ToList();
//    foreach (var sensorBeacon in sensorBeacons)
//    {
//        foreach (var coveredPosition in sensorBeacon.GetCoveredPositionsAtRow(row))
//        {
//            uncovered.Remove(coveredPosition);
//        }
//    }
//}

//Console.WriteLine();
//Console.WriteLine($"The only possible position for the distress beacon is ({0},{0})  and its tuning frequency is {0}.");

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

        if(y > maxY || y < minY)
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

    private int GetDistance() => Math.Abs(Sensor.X - Beacon.X) + Math.Abs(Sensor.Y - Beacon.Y);
}