using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
