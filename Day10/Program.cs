using System.Text;

var instructions = File.ReadAllLines("input.txt");

var cycle = 1;
var spritePosition = 1;
var valuesForSignalStrength = new List<int>();

var keyCycles = new int[] { 20, 60, 100, 140, 180, 220 };

var crtDrawer = new StringBuilder();

foreach (var instruction in instructions)
{
    DrawCrtPixel(crtDrawer, cycle, spritePosition);
    cycle++;
    valuesForSignalStrength.Add(spritePosition);

    if (instruction.StartsWith("addx"))
    {
        DrawCrtPixel(crtDrawer, cycle, spritePosition);
        cycle++;
        valuesForSignalStrength.Add(spritePosition);
        spritePosition += int.Parse(instruction.Split(' ')[1]);
    }
}

var signalStrength = keyCycles.Select(c => c * valuesForSignalStrength[c - 1]).Sum();

var crt = crtDrawer.ToString();
for (int i = 0; i < 240; i += 40)
{
    Console.WriteLine(crt.Substring(i, 40));
}

Console.WriteLine();

Console.WriteLine($"Signal strenght is {signalStrength}");

static void DrawCrtPixel(StringBuilder crtDrawer, int cycle, int currentRegisterValue)
{
    var adjustedCycle = (cycle-1) % 40;
    var isLit = adjustedCycle >= currentRegisterValue - 1 && adjustedCycle <= currentRegisterValue + 1;
    crtDrawer.Append(isLit ? "#" : ".");
}