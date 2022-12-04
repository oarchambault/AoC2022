var lines = File.ReadAllLines("input.txt");

//var lines = new string[]
//{
//    "2-4,6-8",
//    "2-3,4-5",
//    "5-7,7-9",
//    "2-8,3-7",
//    "6-6,4-6",
//    "2-6,4-8"
//};

var totalFullOverlap = 0;
var totalPartialOverlap = 0;

foreach (var line in lines)
{
    var ranges = line.Split(',');
    var firstRange = new SectionRange(ranges[0]);
    var secondRange = new SectionRange(ranges[1]);
    
    if (SectionRange.RangesFullyOverlap(firstRange, secondRange))
    {
        totalFullOverlap++;
    }

    if(SectionRange.RangesPartiallyOverlap(firstRange, secondRange))
    {
        totalPartialOverlap++;
    }
}

Console.WriteLine($"There are {totalFullOverlap} fully overlapping assignment pairs.");
Console.WriteLine($"There are {totalPartialOverlap} partially overlapping assignment pairs.");



class SectionRange
{
    public int From { get; }
    public int To { get; }

    public SectionRange(string value)
    {
        var split = value.Split('-');
        From = int.Parse(split[0]);
        To = int.Parse(split[1]);
    }

    public bool ContainsRange(SectionRange otherRange) => otherRange.From >= From && otherRange.To <= To;

    public bool Overlaps(SectionRange otherRange) => 
        From <= otherRange.To && From >= otherRange.From || To >= otherRange.To && To <= otherRange.From;


    public static bool RangesFullyOverlap(SectionRange firstRange, SectionRange secondRange) =>
        firstRange.ContainsRange(secondRange) || secondRange.ContainsRange(firstRange);

    public static bool RangesPartiallyOverlap(SectionRange firstRange, SectionRange secondRange) =>
        firstRange.Overlaps(secondRange) || secondRange.Overlaps(firstRange);
}