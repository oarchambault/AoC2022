var input = File.ReadAllText("input.txt");

var pairs = input.Split("\r\n\r\n").Select(p => ParsePair(p)).ToArray(); ;

var correctOrderIndexSum = 0;
for (int i = 0; i < pairs.Length; i++)
{
    var pairIndex = i + 1;
    var pair = pairs[i];
    if (pair.CompareOrder())
    {
        Console.WriteLine($"Pair {pairIndex} is in the right order!");
        correctOrderIndexSum += i + 1;
    }
    else
    {
        Console.WriteLine($"Pair {pairIndex} is not in the right order! :(");
    }
}

Console.WriteLine();
Console.WriteLine($"[Part 1] The sum of correct pair indexes is {correctOrderIndexSum}");

var firstDivider = new Packet("[[2]]");
var secondDivider = new Packet("[[6]]");

var allPackets = pairs.SelectMany(p => new[] { p.Left, p.Right }).ToList();

allPackets.Add(firstDivider);
allPackets.Add(secondDivider);

var orderedPackets = allPackets.Order().ToList();

var firstDividerIndex = orderedPackets.IndexOf(firstDivider) + 1;
var secondDividerIndex = orderedPackets.IndexOf(secondDivider) + 1;
var decoderKey = firstDividerIndex * secondDividerIndex;

Console.WriteLine();
Console.WriteLine($"[Part 2] The divider packets are {firstDividerIndex}th and {secondDividerIndex}th, so the decoder key is {decoderKey}");


static Pair ParsePair(string pair)
{
    var split = pair.Split("\r\n");
    return new Pair(new Packet(split[0]), new Packet(split[1]));
}

enum CompareResult
{
    OK,
    NOK,
    UNKNOWN
}

record Pair(Packet Left, Packet Right)
{
    public bool CompareOrder()
    {
        return PacketItem.CompareOrderRec(Left.Value, Right.Value) switch
        {
            CompareResult.OK => true,
            CompareResult.NOK => false,
            CompareResult.UNKNOWN => throw new ArgumentException(),
            _ => throw new NotImplementedException(),
        };
    }
}

class Packet : IComparable
{
    public Packet(string stringValue)
    {
        StringValue = stringValue;
        Value = PacketItem.ParsePacket(stringValue);
    }

    public string StringValue { get; set; }
    public PacketList Value { get; set; }

    public int CompareTo(object? obj)
    {
        var other = obj as Packet;
        if(other == null)
        {
            return 1;
        }

        return PacketItem.CompareOrderRec(Value, other.Value) switch
        {
            CompareResult.OK => -1,
            CompareResult.NOK => 1,
            CompareResult.UNKNOWN => throw new ArgumentException(),
            _ => throw new NotImplementedException(),
        };

    }
}

abstract class PacketItem
{
    public static PacketList ParsePacket(string value)
    {
        return (Parse(value) as PacketList) ?? throw new ArgumentException("Expected list.", nameof(value));
    }
    private static PacketItem Parse(string value)
    {
        string currentInt = "";
        var lists = new Stack<PacketList>();

        for (int i = 0; i < value.Length; i++)
        {
            var c = value[i];
            if (c == '[')
            {
                var newList = new PacketList();
                if (lists.TryPeek(out var previousList))
                {
                    previousList.Items.Add(newList);
                }
                lists.Push(newList);
            }
            else if (c == ',')
            {
                if (currentInt != "")
                {
                    lists.Peek().Items.Add(new PacketInteger(int.Parse(currentInt)));
                    currentInt = "";
                }
                continue;
            }
            else if (c == ']')
            {
                if (currentInt != "")
                {
                    lists.Peek().Items.Add(new PacketInteger(int.Parse(currentInt)));
                    currentInt = "";
                }

                var list = lists.Pop();
                if (lists.TryPeek(out var previousList))
                {
                    previousList.Items.Add(list);
                }
                else
                {
                    return list;
                }
            }
            else if (char.IsDigit(c))
            {
                currentInt += c;
                continue;
            }
        }

        throw new ArgumentException("Invalid input", nameof(value));
    }

    public static CompareResult CompareOrderRec(PacketItem leftItem, PacketItem rightItem)
    {
        if (leftItem is PacketList leftList && rightItem is PacketList rightList)
        {
            for (int i = 0; i < leftList.Items.Count; i++)
            {
                if (i >= rightList.Items.Count)
                {
                    return CompareResult.NOK;
                }

                var result = CompareOrderRec(leftList.Items[i], rightList.Items[i]);
                if (result != CompareResult.UNKNOWN)
                {
                    return result;
                }
            }

            if (leftList.Items.Count < rightList.Items.Count)
            {
                return CompareResult.OK;
            }
        }
        else if (leftItem is PacketInteger leftInt && rightItem is PacketInteger rightInt)
        {
            return Math.Sign(leftInt.Value - rightInt.Value) switch
            {
                -1 => CompareResult.OK,
                0 => CompareResult.UNKNOWN,
                1 => CompareResult.NOK,
                _ => throw new ArgumentException(),
            };
        }
        else if (leftItem is PacketInteger leftInt2 && rightItem is PacketList)
        {
            return CompareOrderRec(new PacketList(leftInt2), rightItem);
        }
        else if (leftItem is PacketList && rightItem is PacketInteger rightInt2)
        {
            return CompareOrderRec(leftItem, new PacketList(rightInt2));
        }

        return CompareResult.UNKNOWN;
    }
}

class PacketList : PacketItem
{
    public PacketList()
    {
        Items = new List<PacketItem>();
    }

    public PacketList(PacketInteger packetInteger)
    {
        Items = new List<PacketItem> { packetInteger };
    }

    public IList<PacketItem> Items { get; }
}

class PacketInteger : PacketItem
{
    public PacketInteger(int value)
    {
        Value = value;
    }

    public int Value { get; }
}

