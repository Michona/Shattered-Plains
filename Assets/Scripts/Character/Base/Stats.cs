using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * All properties for a player character.
 * */
public struct Stats
{
    public Stats(byte health, byte moveRange)
    {
        this.health = health;
        this.moveRange = moveRange;
    }
    public byte health;
    public byte moveRange;
}
