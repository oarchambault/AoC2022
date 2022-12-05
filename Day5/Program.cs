var lines = File.ReadAllLines("input.txt");

var containerLines = new List<string>();

var lineIndex = 0;
foreach (var line in lines)
{

    lineIndex++;
    if (line.StartsWith(" 1"))
    {
        lineIndex++;
        break;
    }
    containerLines.Add(line);
}

var crateStacks = new CrateStacks(containerLines);
var crateStacksPart2 = new CrateStacks(containerLines);

for (int i = lineIndex; i < lines.Length; i++)
{
    // Apply instructions
    crateStacks.ApplyMoveInstruction(lines[i]);
    crateStacksPart2.ApplyMoveInstructionV2(lines[i]);
}

Console.WriteLine($"Final top crates are {crateStacks.GetTopCrates()} for part 1");
Console.WriteLine($"Final top crates are {crateStacksPart2.GetTopCrates()} for part 2");



class CrateStacks
{
    private readonly Stack<char>[] stacks;

    public CrateStacks(List<string> containerLines)
    {
        var lineLenght = containerLines[0].Length;
        var numberOfStacks = (lineLenght + 1) / 4;

        stacks = new Stack<char>[numberOfStacks];
        for (int i = 0; i < numberOfStacks; i++)
        {
            stacks[i] = new Stack<char>();
        }

        InitStacksContent(containerLines, numberOfStacks);
    }

    private void InitStacksContent(List<string> containerLines, int numberOfStacks)
    {
        foreach (var line in Enumerable.Reverse(containerLines))
        {
            for (int index = 0; index < numberOfStacks; index++)
            {
                var position = index * 4 + 1;
                var crate = line[position];
                if (crate != ' ')
                {
                    stacks[index].Push(crate);
                }
                //Console.Write(crate);
            }
            //Console.WriteLine();
        }
    }

    public void ApplyMoveInstruction(string moveInstruction)
    {
        var (move, from, to) = ParseInstruction(moveInstruction);
        for (int i = 0; i < move; i++)
        {
            var crate = stacks[from].Pop();
            stacks[to].Push(crate);
        }
    }

    internal void ApplyMoveInstructionV2(string moveInstruction)
    {
        var (move, from, to) = ParseInstruction(moveInstruction);

        var crates = new List<char>();
        for (int i = 0; i < move; i++)
        {
            var crate = stacks[from].Pop();
            crates.Add(crate);
        }

        foreach (var crate in Enumerable.Reverse(crates))
        {
            stacks[to].Push(crate);
        }
    }

    private (int move, int from, int to) ParseInstruction(string moveInstructionLine)
    {
        var split = moveInstructionLine.Split(' ');
        var move = int.Parse(split[1]);
        var from = int.Parse(split[3]) - 1;
        var to = int.Parse(split[5]) - 1;
        return (move, from, to);
    }


    public string GetTopCrates()
    {
        var result = "";
        foreach (var stack in stacks)
        {
            result += stack.Peek();
        }
        return result;
    }


}