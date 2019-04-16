using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using UnityEngine;

/* Runs only on the master client */
public class TurnManager : MonoBehaviourPunCallbacks
{
    public static TurnManager Instance;

    /* Player that has the current turn. */
    private Player currentPlayer;

    private Player[] playerList;
    Dictionary<int, TurnData> turnDataMap = new Dictionary<int, TurnData>();

    void Start()
    {
        Instance = this;

        if (PhotonNetwork.IsMasterClient) {
            //Initially the master client has the first turn
            currentPlayer = PhotonNetwork.LocalPlayer;
        }
    }

    public override void OnPlayerEnteredRoom(Player player)
    {
        playerList = PhotonNetwork.PlayerList;

        turnDataMap.Add(playerList[0].ActorNumber, new TurnData());
        turnDataMap.Add(playerList[1].ActorNumber, new TurnData());
    }

    /* #critical Runs only on the master client!! */
    public void SwitchTurn()
    {
        if (!PhotonNetwork.IsMasterClient) {
            return;
        }

        if (currentPlayer.GetNext() == null) {
            //Only one player present (for testing purpose).
            EventHub.Instance.FireEvent(new EnablePlayerEvent(currentPlayer.ActorNumber));
        }
        else {
            EventHub.Instance.FireEvent(new EnablePlayerEvent(currentPlayer.GetNext().ActorNumber));
            currentPlayer = currentPlayer.GetNext();
        }
    }
}

public class TurnData
{
    public TurnData()
    {
        characterTurns = new Dictionary<int, bool>();
    }
    public Dictionary<int, bool> characterTurns;
}
