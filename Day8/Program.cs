var treeInput = File.ReadAllLines("input.txt");

var maxRowIndex = treeInput.Length;
var maxColIndex = treeInput[0].Length;

var visibleTrees = new HashSet<Tree>();
var maxScenicScore = -1;
var maxScenicRow = -1;
var maxScenicCol = -1;

var trees = treeInput
    .Select((row, rowIndex) =>
        row.Select((treeHeight, colIndex) => new Tree(treeHeight, rowIndex, colIndex)).ToArray())
    .ToArray();

for (int rowIndex = 0; rowIndex < maxRowIndex; rowIndex++)
{
    var treeRow = GetTreeRow(trees, rowIndex, 0, maxColIndex);
    visibleTrees.UnionWith(GetVisibleTrees(treeRow));
    visibleTrees.UnionWith(GetVisibleTrees(treeRow.Reverse()));

    for (int colIndex = 0; colIndex < maxColIndex; colIndex++)
    {
        if (rowIndex == 0)
        {
            var treeColumn = GetTreeColumn(trees, colIndex, 0, maxRowIndex);
            visibleTrees.UnionWith(GetVisibleTrees(treeColumn));
            visibleTrees.UnionWith(GetVisibleTrees(treeColumn.Reverse()));
        }

        var treeColumnUp = GetTreeColumn(trees, colIndex, 0, rowIndex + 1).Reverse();
        var treeColumnDown = GetTreeColumn(trees, colIndex, rowIndex, maxRowIndex);
        var treeRowLeft = GetTreeRow(trees, rowIndex, 0, colIndex + 1).Reverse();
        var treeRowRight = GetTreeRow(trees, rowIndex, colIndex, maxColIndex);

        var visibleTreesUp = GetNumberOfVisibleTreesFromFirst(treeColumnUp);
        var visibleTreesDown = GetNumberOfVisibleTreesFromFirst(treeColumnDown);
        var visibleTreesLeft = GetNumberOfVisibleTreesFromFirst(treeRowLeft);
        var visibleTreesRight = GetNumberOfVisibleTreesFromFirst(treeRowRight);

        var scenicScore = visibleTreesUp * visibleTreesDown * visibleTreesLeft * visibleTreesRight;
        if(scenicScore > maxScenicScore)
        {
            maxScenicScore = scenicScore;
            maxScenicRow = rowIndex;
            maxScenicCol = colIndex;
        }
    }

}

Console.WriteLine($"We found {visibleTrees.Count} visible trees.");
Console.WriteLine();

Console.WriteLine($"The max scenic score is {maxScenicScore} at ({maxScenicRow}, {maxScenicCol}).");
Console.WriteLine();

static IEnumerable<Tree> GetTreeRow(Tree[][] trees, int rowIndex, int startColIndex, int endColIndex)
{
    return trees[rowIndex][startColIndex..endColIndex];
}

static IEnumerable<Tree> GetTreeColumn(Tree[][] trees, int colIndex, int startRowIndex, int endRowIndex)
{
    return Enumerable
        .Range(startRowIndex, Math.Abs(startRowIndex - endRowIndex))
        .Select(rowIndex => trees[rowIndex][colIndex]);
}

IEnumerable<Tree> GetVisibleTrees(IEnumerable<Tree> treeLine)
{
    var maxHeight = -1;
    foreach (Tree tree in treeLine)
    {
        if (tree.Height > maxHeight)
        {
            maxHeight = tree.Height;
            yield return tree;
        }
    }
}

int GetNumberOfVisibleTreesFromFirst(IEnumerable<Tree> treeLine)
{
    var height = treeLine.First().Height;
    return treeLine.Skip(1).TakeWhile(t => t.Height < height).Count() + 1;
}

record Tree(int Height, int Row, int Column);