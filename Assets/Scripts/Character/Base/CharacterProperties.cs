using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * All properties for a player character.
 * */
public class CharacterProperties
{
    public CharacterProperties(byte health, byte moveRange)
    {
        this.Health = health;
        this.MoveRange = moveRange;
    }
    public byte Health;
    public byte MoveRange;

    /* Used for switiching turns and knowing which characters the player controls. */
    public int PlayerID;
}

public class CharacterState
{
    private bool canMove;
    public bool CanMove {
        get => IsSelected && canMove;
        set => canMove = value;
    }

    private bool canAttack;
    public bool CanAttack {
        get => IsSelected && canAttack;
        set => canAttack = value;
    }

    public bool IsSelected;

    public CharacterState()
    {
        Disable();
    }

    public void Disable()
    {
        CanMove = false;
        CanAttack = false;
        IsSelected = false;
    }

    public void Enable()
    {
        CanMove = true;
        CanAttack = true;
    }
}

