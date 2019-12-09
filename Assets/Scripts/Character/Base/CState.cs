/**
 * Represents the character state. 
 * Properties and booleans that can be changed during the game.
 * */
public class CState
{
    public CState()
    {
        Disable();

        ResetTurn();
    }
    
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

    public bool HasMoved = false;
    //TODO change to false.
    public bool HasAttacked = true;

    public bool IsCharacterTurnOver()
    {
        return HasMoved && HasAttacked;
    }

    public void ResetTurn()
    {

        CanMove = true;
        CanAttack = true;

        HasMoved = false;
        //TODO change to false
        HasAttacked = true;
    }

    public void CharacterMoved()
    {
        HasMoved = true;
        IsSelected = false;
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

