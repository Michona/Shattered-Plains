using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviourPunCallbacks
{
    public static BoardManager Instance;

    [SerializeField]
    private Tile[] Tiles = new Tile[Consts.MAX_TILES];

    #region Pathfinding fields

    /* used for BFS on path finding */
    private bool[,] visited = new bool[Consts.GRID_SIZE, Consts.GRID_SIZE];

    private readonly int[] dx = new int[] {-1, 0, 1, 0}; 
    private readonly int[] dy = new int[] {0, -1, 0, 1};
    private readonly int NO_PARENT = -1;

    private Queue<byte> queue = new Queue<byte>();
    private int[] parent = new int[Consts.MAX_TILES];
    private byte pathSize = 0;

    #endregion

    void Awake()
    {
        Instance = this;
    }

    #region Public Methods

    public Vector3 GetVectorFromTileId(byte tileId)
    {
        return GetTileFromList(tileId).gameObject.transform.position;
    }

    public byte GetTileIdFromVector(Vector3 pos)
    {
        float distance = float.MaxValue;
        byte closestTileId = 0;

        foreach (Tile tile in Tiles) {
            if (Vector3.Distance(tile.gameObject.transform.position, pos) < distance) {
                distance = Vector3.Distance(tile.gameObject.transform.position, pos);
                closestTileId = tile.Id;
            }
        }
        return closestTileId;
    }

    public Tile GetTileFromTileId(byte tileId)
    {
        return GetTileFromList(tileId);
    }

    /* Updates the state of the tile. Usually runs on all clients! */
    public void SetTileState(byte tileId, bool _isOccupied)
    {
        GetTileFromList(tileId).isOccupied = _isOccupied;
    }

    public Vector3[] GetVector3Path(byte startTileId, byte endTileId)
    {
        for (byte i = 0; i < Consts.GRID_SIZE; i ++) {
            for (byte j = 0; j < Consts.GRID_SIZE; j ++) {
                visited[i, j] = false;
            }
        }

        GridPosition startV = BoardHelper.GetColRowFromTileId(startTileId);

        queue.Enqueue(startTileId);
        parent[startTileId] = NO_PARENT;
        pathSize = 0;
        visited[startV.Col, startV.Row] = true;

        while (queue.Count > 0) {

            byte currentTile = queue.Dequeue();

            if (currentTile == endTileId) {
                queue.Clear();
                break;
            }

            int topX = BoardHelper.GetColRowFromTileId(currentTile).Col;
            int topY = BoardHelper.GetColRowFromTileId(currentTile).Row;

            for (byte i = 0; i < 4; i ++) {

                if (topX + dx[i] >= 0 && topX + dx[i] < Consts.GRID_SIZE && topY + dy[i] >= 0 && topY + dy[i] < Consts.GRID_SIZE) {

                    GridPosition adjecentTile = new GridPosition(topX + dx[i], topY + dy[i]);

                    if (!visited[adjecentTile.Col, adjecentTile.Row]
                        && !GetTileFromList(BoardHelper.GetTileIdFromColRow(adjecentTile)).isOccupied) {

                        queue.Enqueue(BoardHelper.GetTileIdFromColRow(adjecentTile));
                        visited[adjecentTile.Col, adjecentTile.Row] = true;
                        parent[BoardHelper.GetTileIdFromColRow(adjecentTile)] = currentTile; 
                    }
                }
            }
        }

        return GetPathList(endTileId).ConvertAll(new Converter<byte, Vector3>(GetVectorFromTileId)).ToArray();
    }

    #endregion

    private List<byte> GetPathList(int curr)
    {
        List<byte> pathList = new List<byte>();

        while (curr != NO_PARENT) {
            pathSize++;
            pathList.Add((byte)curr);
            curr = parent[curr];
        }
        pathList.Reverse();
        pathList.RemoveAt(0);

        return pathList;
    }

    /* The tiles have to be ordered in the prefab! */
    private Tile GetTileFromList(byte tileId)
    {
        return Tiles[tileId];
    }

}

public struct GridPosition
{
    public GridPosition(int col, int row)
    {
        this.Col = col;
        this.Row = row;
    }
    public int Col;
    public int Row;

}
