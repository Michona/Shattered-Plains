using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviourPunCallbacks
{
        
    public static BoardManager Instance;

    public byte GridSize = 3;
    public Tile[] Tiles;

    void Awake()
    {
        Instance = this;
    }

    public byte ParseName(string tileName)
    {
        return byte.Parse(tileName);
    }

    public Vector2 GetColRowFromTileId(byte tileId)
    {
        int row = tileId / GridSize;
        int col = tileId - (row * GridSize);
        return new Vector2(row, col);
    }

    public Vector3 GetVectorFromTileId(byte tileId)
    {
        return GetTileFromList(tileId).gameObject.transform.position;
    }

    /* Runs RPC to notify players that state of tile has changed*/
    public void SetTileOccupied(byte tileId)
    {
        photonView.RPC("UpdateTileStatus", RpcTarget.All, tileId, true);
    }

    /* Runs RPC to notify players that state of tile has changed*/
    public void SetTileFree(byte tileId)
    {
        photonView.RPC("UpdateTileStatus", RpcTarget.All, tileId, false);
    }

    #region RPC's

    [PunRPC]
    private void UpdateTileStatus(byte tileId, bool _isOccupied, PhotonMessageInfo info)
    {
        GetTileFromList(tileId).isOccupied = _isOccupied;
    }

    #endregion

    /* Look to optimize this (order the elements in editor so this can be O(1)).*/
    private Tile GetTileFromList(byte tileId)
    {
        foreach (Tile tile in Tiles) {
            if (tile.Id == tileId) {
                return tile;
            }
        }
        Debug.LogError("Tile Not Found");
        throw new Exception();
    }

}
