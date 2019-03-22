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

    public Vector3 GetVectorFromTileId(byte tileId)
    {
        return GetTileFromList(tileId).gameObject.transform.position;
    }

    public void SetTileState(byte tileId, bool _isOccupied)
    {
        GetTileFromList(tileId).isOccupied = _isOccupied;
    }

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
