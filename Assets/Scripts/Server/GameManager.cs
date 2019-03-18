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

    #region Photon Callbacks

    /* Called when local player left the room. We load the lobby then. */
    public override void OnLeftRoom()
    {
        SceneManager.LoadScene(0);
    }

    public override void OnPlayerEnteredRoom(Player other)
    {
        Debug.LogFormat("OnPlayerEnteredRoom() {0}", other.NickName); // not seen if you're the player connecting
        if (PhotonNetwork.IsMasterClient) {
            Debug.LogFormat("OnPlayerEnteredRoom IsMasterClient {0}", PhotonNetwork.IsMasterClient); // called before OnPlayerLeftRoom
            LoadArena();
        }
    }


    public override void OnPlayerLeftRoom(Player other)
    {
        Debug.LogFormat("OnPlayerLeftRoom() {0}", other.NickName); // seen when other disconnects
        if (PhotonNetwork.IsMasterClient) {
            Debug.LogFormat("OnPlayerLeftRoom IsMasterClient {0}", PhotonNetwork.IsMasterClient); // called before OnPlayerLeftRoom
            LoadArena();
        }
    }

    #endregion


    void Start()
    {
        Instance = this;

        if (PlayerManager.LocalPlayerInstance == null) {

            // we're in a room. spawn a character for the local player. it gets synced by using PhotonNetwork.Instantiate
            PhotonNetwork.Instantiate(this.playerPrefab.name, new Vector3(0f, 5f, 0f), Quaternion.identity, 0);
        }
        else {
            Debug.LogFormat("Ignoring scene load for {0}", SceneManagerHelper.ActiveSceneName);
        }
    } 

    /* Loads the MainBoard level if we are master client. */
    private void LoadArena()
    {
        if (!PhotonNetwork.IsMasterClient) {
            Debug.LogError("PhotonNetwork : Trying to Load a level but we are not the master Client");
        }
        Debug.LogFormat("PhotonNetwork : Loading Level MainBoard");
        PhotonNetwork.LoadLevel("MainBoard");
    }


    /* A wrapper on PhotonNetwork.LeaveRoom(). We might need to do more logic when players leave.*/
    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }
}
