var elevationMap = File.ReadAllLines("input.txt").Select(l => l.ToArray()).ToArray();

var (start, end) = FindStartAndEndPositions(elevationMap);

var shortestPath = FindBestPath(start);

Console.WriteLine($"The shortest path has a lenght of {shortestPath.Count}");

var part2Starts = FindPart2Starts(elevationMap);
IList<(int row, int col)> shortestPathPart2 = Array.Empty<(int row, int col)>();
foreach (var part2Start in part2Starts)
{
    var path = FindBestPath(part2Start);
    if(path.Count != 0 && (shortestPathPart2.Count == 0 || shortestPathPart2.Count > path.Count))
    {
        shortestPathPart2 = path;
    }
}

Console.WriteLine($"The shortest path for part 2 has a lenght of {shortestPathPart2.Count}");

IList<(int row, int col)> FindBestPath((int row, int col) startPosition)
{
    var parentMap = new Dictionary<(int row, int col), (int row, int col)>();
    var queue = new Queue<(int row, int col)>();
    var visited = new HashSet<(int row, int col)>() { startPosition };

    queue.Enqueue(startPosition);

    while (queue.Count > 0)
    {
        var current = queue.Dequeue();
        if (current == end)
        {
            return ResolveParentMap(current, parentMap);
        }

        foreach (var nextDirection in GetNextDirections(current))
        {
            if (!visited.Contains(nextDirection))
            {
                visited.Add(nextDirection);
                parentMap[nextDirection] = current;
                queue.Enqueue(nextDirection);
            }
        }
    }

    return Array.Empty<(int row, int col)>();
}

IList<(int row, int col)> ResolveParentMap((int row, int col) last, Dictionary<(int row, int col), (int row, int col)> parentMap)
{
    var path = new List<(int row, int col)>();
    var current = last;
    while (parentMap.ContainsKey(current))
    {
        path.Add(current);
        current = parentMap[current];
    }
    return Enumerable.Reverse(path).ToList();
}

IEnumerable<(int row, int col)> GetNextDirections((int row, int col) current)
{
    if (CanMoveTo(current, (current.row - 1, current.col))) yield return (current.row - 1, current.col); // Up
    if (CanMoveTo(current, (current.row, current.col + 1))) yield return (current.row, current.col + 1); // Right
    if (CanMoveTo(current, (current.row + 1, current.col))) yield return (current.row + 1, current.col); // Down
    if (CanMoveTo(current, (current.row, current.col - 1))) yield return (current.row, current.col - 1); // Left
}

bool CanMoveTo((int row, int col) from, (int row, int col) to)
{
    var isInsideBound = to.row >= 0 && to.row < elevationMap.Length && to.col >= 0 && to.col < elevationMap[0].Length;
    var isNotTooHigh = isInsideBound && elevationMap[to.row][to.col] <= elevationMap[from.row][from.col] + 1;
    return isInsideBound && isNotTooHigh;
}

static ((int row, int col) start, (int row, int col) end) FindStartAndEndPositions(char[][] elevationMap)
{
    var start = (-1, -1);
    var end = (-1, -1);
    for (int row = 0; row < elevationMap.Length; row++)
    {
        for (int col = 0; col < elevationMap[row].Length; col++)
        {
            if (elevationMap[row][col] == 'S')
            {
                start = (row, col);
                elevationMap[row][col] = 'a';
            }

            if (elevationMap[row][col] == 'E')
            {
                end = (row, col);
                elevationMap[row][col] = 'z';
            }
        }
    }
    return (start, end);
}

static IEnumerable<(int row, int col)> FindPart2Starts(char[][] elevationMap)
{
    var starts = new List<(int row, int col)>();
    for (int row = 0; row < elevationMap.Length; row++)
    {
        for (int col = 0; col < elevationMap[row].Length; col++)
        {
            if (elevationMap[row][col] == 'a')
            {
                starts.Add((row, col));
            }
        }
    }
    return starts;
}
