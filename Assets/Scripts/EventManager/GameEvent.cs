
/* Used in EventHub. Should extend and add data that needs to be passed in the event! */

using UnityEngine;

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
    public CharacterSelectedEvent(int characterID, Vector3 characterPosition, int moveDistance)
    {
        this.CharacterID = characterID;
        this.TileId = BoardManager.Instance.GetTileIdFromVector(characterPosition);
        this.MoveDistance = moveDistance;
    }

    /* Photon view id of the game object selected. */
    public int CharacterID;
    public byte TileId;
    public int MoveDistance;
}

public class MovePlayerEvent : GameEvent
{
    public MovePlayerEvent() {
    }
}

public class EnablePlayerEvent: GameEvent
{
    public EnablePlayerEvent(int _actorNumber)
    {
        this.ActorNumber = (byte)_actorNumber;
    }

    public byte ActorNumber;
}
