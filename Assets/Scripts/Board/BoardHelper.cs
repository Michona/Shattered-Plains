
/* Purely static class that deals with arithmetic operations or parsing. */

using UnityEngine;

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

    public static int GetDistanceBetweenTiles(byte fromTileId, byte toTileId) {
        return GetDistanceBetweenGridPositions(GetColRowFromTileId(fromTileId), GetColRowFromTileId(toTileId));
    }
    public static int GetDistanceBetweenGridPositions(GridPosition from, GridPosition to) {
        return (Mathf.Abs(from.Col - to.Col) + Mathf.Abs(from.Row - to.Row));
    }
}
