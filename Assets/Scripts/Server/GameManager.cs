using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


using Photon.Pun;
using Photon.Realtime;

public class GameManager : MonoBehaviourPunCallbacks
{

    public static GameManager Instance;

    [Tooltip("The prefab to use for representing the player")]
    public GameObject playerPrefab;

    public GameObject gridPrefab;

    #region Photon Callbacks

    /* Called when local player left the room. We load the lobby then. */
    public override void OnLeftRoom()
    {
        SceneManager.LoadScene(0);
    }

    public override void OnPlayerEnteredRoom(Player other)
    {
        LoadArena();
    }


    public override void OnPlayerLeftRoom(Player other)
    {
        LoadArena();
    }

    #endregion

    void Start()
    {
        Instance = this;

        if (Interloper.LocalInterloperInstance == null) {
            // we're in a room. spawn a character for the local player. it gets synced by using PhotonNetwork.Instantiate
            PhotonNetwork.Instantiate(this.playerPrefab.name, new Vector3(0f, 5f, 0f), Quaternion.identity, 0);
        }
    } 

    /* Loads the MainBoard level if we are master client. */
    private void LoadArena()
    {
        if (PhotonNetwork.IsMasterClient) {
            PhotonNetwork.LoadLevel("MainBoard");
        }
    }

    #region Public methods

    /* A wrapper on PhotonNetwork.LeaveRoom(). We might need to do more logic when players leave.*/
    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

    /* If we are allowed to move there -> Fire an event. */
    public void MovePlayerToTile(Tile selectedTile)
    {
        if (!selectedTile.isOccupied) {
            EventHub.Instance.FireEvent(new TileSelectedEvent(selectedTile));
        }
    }

    #endregion
}