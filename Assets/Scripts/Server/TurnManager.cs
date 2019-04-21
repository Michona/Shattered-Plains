using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using UnityEngine;

/**
 * Manages the turns. Public methods to end the turn
 * Contains information about the current player's turn. 
 * */
public class TurnManager : MonoBehaviourPunCallbacks
{
    /* Singleton */
    public static TurnManager Instance;

    /* Player that has the current turn. */
    private Player currentPlayer;

    private Player[] playerList;

    void Start()
    {
        Instance = this;
        playerList = PhotonNetwork.PlayerList;

        if (PhotonNetwork.IsMasterClient) {
            //Initially the master client has the first turn
            currentPlayer = PhotonNetwork.LocalPlayer;
        }
    }

    public override void OnPlayerEnteredRoom(Player other)
    {
        playerList = PhotonNetwork.PlayerList;
    }

    /* Calls an RPC to swith the turn using the playerList. */
    public void EndTurn()
    {
        if (IsMyTurn()) {
            photonView.RPC("RPCSwitchTurn", RpcTarget.All, PhotonNetwork.LocalPlayer.ActorNumber);
        }
    }

    /* Important public method. Should be single source of truth. */
    public bool IsMyTurn()
    {
        return currentPlayer == PhotonNetwork.LocalPlayer;
    }

    [PunRPC]
    private void RPCSwitchTurn(int callingPlayerActorNumber)
    {
        foreach (Player p in playerList) {
            if (p.ActorNumber == callingPlayerActorNumber) {
                if (p.GetNext() != null) {
                    currentPlayer = p.GetNext();
                    Debug.Log(currentPlayer.NickName);
                }
            }
        }
    }
}
