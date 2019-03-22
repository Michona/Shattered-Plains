using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interloper : PlayerManager
{

    /* The local player instance. Use this to know if the local player is represented in the Scene" */
    public static GameObject LocalInterloperInstance;

    #region Fields from PlayerManager

    [SerializeField]
    private Stats stats = new Stats(4, 3);
    public override Stats StatsData { get => stats; set => stats = value; }

    [SerializeField]
    private byte currentTilePosition;
    public override byte CurrentTilePosition { 
        get => currentTilePosition;
        set {
            /* When positon is changed -> move to tile. */
            MovePlayerToTile(this.gameObject, value);
            currentTilePosition = value;
        } 
    }

    #endregion

    public override void OnEnable()
    {
        EventHub.Instance.AddListener<TileSelectedEvent>(UpdateCurrentTile);
        base.OnEnable();
    }

    public override void OnDisable()
    {
        EventHub.Instance.RemoveListener<TileSelectedEvent>(UpdateCurrentTile);
        base.OnDisable();
    }

    void Awake()
    {
        if (!photonView.IsMine && PhotonNetwork.IsConnected) {
            return;
        }

        Interloper.LocalInterloperInstance = this.gameObject;

        if (PlayerUiPrefab != null) {
            InstantiatePlayerUI();
        }

        // #Critical
        // we flag as don't destroy on load so that instance survives level synchronization, thus giving a seamless experience when levels load.
        DontDestroyOnLoad(this.gameObject);
    }

    /* Called after GameManager approves that we can move. We call RPC to update position on all players. */
    private void UpdateCurrentTile(TileSelectedEvent tileEvent)
    {
        if (!photonView.IsMine && PhotonNetwork.IsConnected) {
            return;
        }
        byte destinationTileId = tileEvent.SelectedTile.Id;

        photonView.RPC("UpdateCurrentTileRPC", RpcTarget.All, destinationTileId);
    }

    [PunRPC]
    private void UpdateCurrentTileRPC(byte destinationTileId, PhotonMessageInfo info)
    {
        BoardManager.Instance.SetTileState(currentTilePosition, false);
        BoardManager.Instance.SetTileState(destinationTileId, true);
        CurrentTilePosition = destinationTileId;
    }


}
