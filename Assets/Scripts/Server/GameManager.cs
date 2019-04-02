using UnityEngine;
using UnityEngine.SceneManagement;

using Photon.Pun;
using Photon.Realtime;

public class GameManager : MonoBehaviourPunCallbacks
{
    public static GameManager Instance;

    /* Game objects to be instantiated on the network */
    public GameObject interloperGO;

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
        InstantiateCharacters();
    } 

    private void InstantiateCharacters()
    {
        if (Interloper.LocalInterloperInstance == null) {
            // we're in a room. spawn a character for the local player. it gets synced by using PhotonNetwork.Instantiate
            interloperGO = PhotonNetwork.Instantiate(this.interloperGO.name, BoardManager.Instance.GetVectorFromTileId((byte)PhotonNetwork.LocalPlayer.ActorNumber), Quaternion.identity, 0);
            // #critical we use the ACTOR NUMBER to set spawn positions. TODO change that in the future!!
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

    public void SelectPlayer(Interloper interloperHit)
    {
        EventHub.Instance.FireEvent(new CharacterSelectedEvent(interloperHit.photonView.ViewID));
    }

    #endregion
}