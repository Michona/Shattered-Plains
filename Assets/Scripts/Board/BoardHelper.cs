
/* Purely static class that deals with arithmetic operations or parsing. */
public static class BoardHelper
{

    public static byte ParseName(string tileName)
    {
        return byte.Parse(tileName);
    }

    public static GridPosition GetColRowFromTileId(byte tileId)
    {
        int row = tileId / Consts.GRID_SIZE;
        int col = tileId - (row * Consts.GRID_SIZE);
        return new GridPosition(col, row);
    }

    public static byte GetTileIdFromColRow(GridPosition pos)
    {
        return (byte)(pos.Row * Consts.GRID_SIZE + pos.Col);
    }

}
