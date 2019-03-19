using Photon.Pun;

public class Tile : MonoBehaviourPunCallbacks
{
    /* Id of the tile */
    public byte Id;

    public bool isOccupied;

    void Start()
    {
        Id = BoardManager.Instance.ParseName(this.name);
        isOccupied = false;
    }

    public void Selected()
    {
        GameManager.Instance.MovePlayerToTile(this);
    }

}
