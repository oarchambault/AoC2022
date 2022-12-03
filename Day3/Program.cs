var lines = File.ReadAllLines("input.txt");
//var lines = new string[]
//{
//    "vJrwpWtwJgWrhcsFMMfFFhFp",
//    "jqHRNqRjqzjGDLGLrsFMfFZSrLrFZsSL",
//    "PmmdzqPrVvPwwTWBwg",
//    "wMqvLMZHhHMvwLHjbvcjnnSBnvTQFn",
//    "ttgJtRGJQctTZtZT",
//    "CrZsJsPPZsGzwwsLwLmpwMDw"
//};

var totalPrioritiesSum = 0;

foreach (var rucksackLine in lines)
{
    var commonItems = GetRucksackCommonItems(rucksackLine);
    totalPrioritiesSum += GetSummerPriorities(commonItems);
}

Console.WriteLine($"The sum of common items priorities is {totalPrioritiesSum}");

var part2Total = 0;

for (int i = 0; i < lines.Length; i += 3)
{
    char commonItem = GetCommonItem(lines[i], lines[i + 1], lines[i + 2]);
    part2Total += GetItemPriority(commonItem);
}

Console.WriteLine($"The sum for part2 is {part2Total}");

static char[] GetRucksackCommonItems(string rucksack)
{
    var half = rucksack.Length / 2;
    return GetCompartmentCommonItems(rucksack[0..half], rucksack[half..^0]);
}

static char[] GetCompartmentCommonItems(string compartment1, string compartment2) => compartment1.Intersect(compartment2).ToArray();

static char GetCommonItem(string rucksack1, string rucksack2, string rucksack3) =>
    rucksack1.Intersect(rucksack2).Intersect(rucksack3).Distinct().First();

static int GetSummerPriorities(char[] commonItems) => commonItems.Select(GetItemPriority).Sum();

static int GetItemPriority(char item)
{
    //var A = 'A'; //65
    //var Z = 'Z'; //90
    //var a = 'a'; //97
    //var z = 'z'; //122
    var priority = item < 'a'
        ? item - 38
        : item - 96;
    //Console.WriteLine($"{item} {(int)item} {priority}");
    return priority;
}