
using System.Numerics;

/* Purely static class that deals with arithmetic operations or parsing. */
public static class BoardHelper
{

    public static byte ParseName(string tileName)
    {
        return byte.Parse(tileName);
    }

    public static Vector2 GetColRowFromTileId(byte tileId)
    {
        int row = tileId / BoardManager.Instance.GridSize;
        int col = tileId - (row * BoardManager.Instance.GridSize);
        return new Vector2(row, col);
    }
}
