var input = File.ReadAllText("input.txt");

Console.WriteLine("Part 1");
GetMarkerPosition("mjqjpqmgbljsphdztnvjfqwrcgsmlb", 4);
GetMarkerPosition("bvwbjplbgvbhsrlpgdmjqwftvncz", 4);
GetMarkerPosition("nppdvjthqldpwncqszvftbrmjlhg", 4);
GetMarkerPosition("nznrnfrfntjfmvfwmzdfjlvtqnbhcprsg", 4);
GetMarkerPosition("zcfzfwzzqfrljwzlrfnpqdbhtmscgvjw", 4);
GetMarkerPosition(input, 4);

Console.WriteLine();
Console.WriteLine("Part 2");
GetMarkerPosition("mjqjpqmgbljsphdztnvjfqwrcgsmlb", 14);
GetMarkerPosition("bvwbjplbgvbhsrlpgdmjqwftvncz", 14);
GetMarkerPosition("nppdvjthqldpwncqszvftbrmjlhg", 14);
GetMarkerPosition("nznrnfrfntjfmvfwmzdfjlvtqnbhcprsg", 14);
GetMarkerPosition("zcfzfwzzqfrljwzlrfnpqdbhtmscgvjw", 14);
GetMarkerPosition(input, 14);


static int GetMarkerPosition(string input, int markerLenght)
{
    for (int index = 0; index < input.Length - (markerLenght + 1); index++)
    {
        var substring = input.Substring(index, markerLenght);
        if (CharsAreDistinct(substring))
        {
            Console.WriteLine($"Found {substring} at {index + markerLenght}");
            return index;
        }
    }

    return -1;
}

static bool CharsAreDistinct(string value) => value.Distinct().Count() == value.Length;