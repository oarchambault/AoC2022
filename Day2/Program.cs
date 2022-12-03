var lines = File.ReadAllLines("input.txt");

var totalScore = 0;
var totalScore2 = 0;
foreach (var line in lines)
{
    totalScore += ParseLine(line);
    totalScore2 += ParseLinePart2(line);
}

Console.WriteLine($"Total score is {totalScore}");
Console.WriteLine($"Total score is for part 2 is {totalScore2}");

int ParseLine(string line)
{
    return line switch
    {
        "A X" => 3 + 1,
        "A Y" => 6 + 2,
        "A Z" => 0 + 3,
        "B X" => 0 + 1,
        "B Y" => 3 + 2,
        "B Z" => 6 + 3,
        "C X" => 6 + 1,
        "C Y" => 0 + 2,
        "C Z" => 3 + 3,
        _ => throw new Exception("Unexpected line value: " + line)
    };
}

int ParseLinePart2(string line)
{
    //'X' => Result.Lose,
    //'Y' => Result.Draw,
    //'Z' => Result.Win,

    var opponentPlay = line[0];
    var myPlay = line switch
    {
        "A X" => "Z",
        "A Y" => "X",
        "A Z" => "Y",
        "B X" => "X",
        "B Y" => "Y",
        "B Z" => "Z",
        "C X" => "Y",
        "C Y" => "Z",
        "C Z" => "X",
        _ => throw new Exception("Unexpected line value: " + line)
    };

    return ParseLine(opponentPlay + " " + myPlay);
}

