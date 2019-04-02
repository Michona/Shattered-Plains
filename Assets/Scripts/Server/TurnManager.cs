using Photon.Pun;
using Photon.Realtime;

/* Runs only on the master client */
public class TurnManager : MonoBehaviourPunCallbacks
{

    public static TurnManager Instance;

    /* Player that has the current turn. */
    private Player currentPlayer;
    
    void Start()
    {
        Instance = this;

        if (PhotonNetwork.IsMasterClient) {
            currentPlayer = PhotonNetwork.LocalPlayer;
        }
    }

    public override void OnPlayerEnteredRoom(Player player)
    {
    }

    /* #critical Runs only on the master client!! */
    public void SwitchTurn()
    {
        if (PhotonNetwork.IsMasterClient && currentPlayer.GetNext() != null) {
            EventHub.Instance.FireEvent(new EnablePlayerEvent(currentPlayer.GetNext().ActorNumber));
            currentPlayer = currentPlayer.GetNext();
        }
    }
}
