var lines = File.ReadAllLines("input.txt");

var calories = new SortedList<int, int>();
var current = 0;

foreach (var line in lines)
{
    if(line != string.Empty)
    {
        current += int.Parse(line);
    }
    else
    {
        calories.Add(current, current);
        current = 0;
    }
}

Console.WriteLine($"The maximum is {calories.Last().Value}");

Console.WriteLine($"The top 3 count is {calories.TakeLast(3).Sum(p => p.Value)}");

