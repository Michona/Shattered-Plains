using Photon.Pun;

public class Tile : MonoBehaviourPunCallbacks
{
    /* Id of the tile */
    public byte Id;

    public bool isOccupied;

    void Start()
    {
        Id = BoardHelper.ParseName(this.name);
        isOccupied = false;
    }
}
