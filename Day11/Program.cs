Logger.LogEnabled = false;

var monkeysNotes = File.ReadAllText("input.txt");

var monkeys = monkeysNotes.Split("\r\n\r\n").Select(n => Monkey.FromNotes(n)).ToArray();

const int numberOfRounds = 10000; // or 20

var superModulo = monkeys.Select(m => m.TestDivisibleby).Aggregate(1, (acc, next) => acc * next);

for (int round = 0; round < numberOfRounds; round++)
{
    foreach (var monkey in monkeys)
    {
        //monkey.ProceedRoundPart1(monkeys);
        monkey.ProceedRoundPart2(monkeys, superModulo);
    }

    if (numberOfRounds > 20 && round % 1000 == 0)
    {
        Console.WriteLine();
        Console.WriteLine($"After round {round + 1}, the monkeys are holding items with these worry levels:");
        foreach (var monkey in monkeys)
        {
            Console.WriteLine($"{monkey.Name}: {string.Join(", ", monkey.WorryItems)}");
        }
        Console.WriteLine();
    }
}

Console.WriteLine();
foreach (var monkey in monkeys)
{
    Console.WriteLine($"{monkey.Name} inspected items {monkey.InspectionCount} times.");
}

var mostActiveMonkeys = monkeys.OrderByDescending(m => m.InspectionCount).Take(2).ToList();
Console.WriteLine($"Most active monkeys are {mostActiveMonkeys[0].Name} and {mostActiveMonkeys[1].Name}");
Console.WriteLine($"The monkey business level is {mostActiveMonkeys[0].InspectionCount * mostActiveMonkeys[1].InspectionCount}");



class Monkey
{
    public string Name { get; private set; } = string.Empty;
    public IList<long> WorryItems { get; private set; } = Array.Empty<long>();
    public Func<long, long> WorryOperation { get; private set; } = (old) => old;
    public int TestDivisibleby { get; private set; }
    public int TestTrueThrowTo { get; private set; }
    public int TestFalseThrowTo { get; private set; }

    public long InspectionCount { get; set; }

    private Monkey() { }

    public static Monkey FromNotes(string monkeyNotes)
    {
        var lines = monkeyNotes.Split("\r\n");

        var monkey = new Monkey();

        monkey.Name = lines[0].TrimEnd(':');
        monkey.WorryItems = lines[1].Split(':')[1].Split(", ").Select(long.Parse).ToList();

        var operationNote = lines[2].Split("= old ")[1].Split(' ');
        monkey.WorryOperation = operationNote switch
        {
            ["*", "old"] => (long old) => old * old,
            ["*", string value] => (long old) => old * long.Parse(value),
            ["+", string value] => (long old) => old + long.Parse(value),
            _ => throw new ArgumentException(nameof(operationNote)),
        };

        monkey.TestDivisibleby = int.Parse(lines[3].Split(' ').Last());
        monkey.TestTrueThrowTo = int.Parse(lines[4].Split(' ').Last());
        monkey.TestFalseThrowTo = int.Parse(lines[5].Split(' ').Last());

        return monkey;
    }

    public void ProceedRoundPart1(Monkey[] allMonkeys)
    {
        Logger.Log($"{Name}:");

        for (int i = 0; i < WorryItems.Count; i++)
        {
            InspectionCount++;
            Logger.Log($"  Monkey inspects an item with a worry level of {WorryItems[i]}.");
            WorryItems[i] = WorryOperation(WorryItems[i]);
            Logger.Log($"    Worry level increased to {WorryItems[i]}.");
            WorryItems[i] = WorryItems[i] / 3;
            Logger.Log($"    Monkey gets bored with item. Worry level is divided by 3 to {WorryItems[i]}.");

            var testResult = WorryItems[i] % TestDivisibleby == 0;
            var newMonkey = testResult ? TestTrueThrowTo : TestFalseThrowTo;
            Logger.Log($"    Current worry level is {(testResult ? "" : "not")} divisible by {TestDivisibleby}.");
            Logger.Log($"    Item with worry level {WorryItems[i]} is thrown to monkey {newMonkey}.");
            allMonkeys[newMonkey].WorryItems.Add(WorryItems[i]);
        }

        WorryItems.Clear();
    }

    public void ProceedRoundPart2(Monkey[] allMonkeys, long superModulo)
    {
        for (int i = 0; i < WorryItems.Count; i++)
        {
            InspectionCount++;
            WorryItems[i] = WorryOperation(WorryItems[i]) % superModulo;

            var testResult = WorryItems[i] % TestDivisibleby == 0;
            var newMonkey = testResult ? TestTrueThrowTo : TestFalseThrowTo;
            allMonkeys[newMonkey].WorryItems.Add(WorryItems[i]);
        }

        WorryItems.Clear();
    }
}

static class Logger
{
    public static bool LogEnabled = true;

    public static void Log(string message = "")
    {
        if (LogEnabled)
        {
            Console.WriteLine(message);
        }
    }
}

