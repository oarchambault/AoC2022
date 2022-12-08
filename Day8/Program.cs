var treeInput = File.ReadAllLines("input.txt");

var maxRowIndex = treeInput.Length;
var maxColIndex = treeInput[0].Length;

var visibleTrees = new HashSet<Tree>();

var trees = treeInput
    .Select((row, rowIndex) =>
        row.Select((treeHeight, colIndex) => new Tree(int.Parse(treeHeight.ToString()), rowIndex, colIndex)).ToArray())
    .ToArray();

for (int rowIndex = 0; rowIndex < maxRowIndex; rowIndex++)
{
    var treeRow = GetTreeRow(trees, rowIndex, 0, maxColIndex);
    visibleTrees.UnionWith(GetVisibleTrees(treeRow));
    visibleTrees.UnionWith(GetVisibleTrees(treeRow.Reverse()));
}

for (int colIndex = 0; colIndex < maxColIndex; colIndex++)
{
    var treeColumn = GetTreeColumn(trees, colIndex, 0, maxRowIndex);
    visibleTrees.UnionWith(GetVisibleTrees(treeColumn));
    visibleTrees.UnionWith(GetVisibleTrees(treeColumn.Reverse()));
}

Console.WriteLine($"We found {visibleTrees.Count} visible trees.");
Console.WriteLine();

static IEnumerable<Tree> GetTreeRow(Tree[][] trees, int rowIndex, int startColIndex, int endColIndex)
{
    return trees[rowIndex][startColIndex..endColIndex];
}

static IEnumerable<Tree> GetTreeColumn(Tree[][] trees, int colIndex, int startRowIndex, int endRowIndex)
{
    return Enumerable.Range(startRowIndex, endRowIndex).Select(rowIndex => trees[rowIndex][colIndex]);
}

IEnumerable<Tree> GetVisibleTrees(IEnumerable<Tree> treeLine)
{
    var maxHeight = -1;
    foreach (Tree tree in treeLine)
    {
        if (tree.height > maxHeight)
        {
            maxHeight = tree.height;
            yield return tree;
        }
    }
}

record Tree(int height, int row, int column);