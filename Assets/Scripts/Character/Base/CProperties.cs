/**
 * All properties for a player character.
 * Not changing during the game.
 * */

public class CProperties
{
    public CProperties(byte health, byte moveRange)
    {
        this.Health = health;
        this.MoveRange = moveRange;
    }
    public byte Health;
    public byte MoveRange;

    /* Used for switiching turns and knowing which characters the player controls. */
    public int PlayerID;

    /* Unique to an instance of a character. It's set as the photonView.viewID*/
    public int CharacterID;
}

