
/* Used in EventHub. Should extend and add data that needs to be passed in the event! */
public class GameEvent
{
}

public class TileSelectedEvent : GameEvent
{
    public TileSelectedEvent(Tile _selectedTile)
    {
        this.SelectedTile = _selectedTile;
    }

    public Tile SelectedTile;
}

public class CharacterSelectedEvent : GameEvent
{
    public CharacterSelectedEvent(int _photonViewId)
    {
        this.ID = _photonViewId;
    }

    /* Photon view id of the game object selected. */
    public int ID;
}

public class EnablePlayerEvent: GameEvent
{
    public EnablePlayerEvent(int _actorNumber)
    {
        this.ActorNumber = (byte)_actorNumber;
    }

    public byte ActorNumber;
}