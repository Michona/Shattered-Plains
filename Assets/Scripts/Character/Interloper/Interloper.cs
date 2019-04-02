using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class Interloper : CharacterManager
{

    /* The local player instance. Use this to know if the local player is represented in the Scene" */
    public static GameObject LocalInterloperInstance;

    #region Fields from PlayerManager

    [SerializeField]
    private CharacterProperties properties = new CharacterProperties(2,3); //just mock values. Change later!
    public override CharacterProperties Properties { get => properties; set => properties = value; }

    private CharacterState state = new CharacterState();
    protected override CharacterState State { get => state; set => state = value; }

    [SerializeField]
    private byte currentTilePosition = 1;
    public override byte CurrentTilePosition { 
        get => currentTilePosition;
        set {
            /* When positon is changed -> move to tile. */
            MovePlayerToTile(this.gameObject, value);
            currentTilePosition = value;
            state.IsSelected = false;
        } 
    }


    #endregion

    #region Subscribe to EventHub

    public override void OnEnable()
    {
        EventHub.Instance.AddListener<TileSelectedEvent>(UpdateCurrentTile);
        EventHub.Instance.AddListener<CharacterSelectedEvent>(UpdateIsSelected);
        EventHub.Instance.AddListener<EnablePlayerEvent>(UpdateEnabledState);
        base.OnEnable();
    }

    public override void OnDisable()
    {
        EventHub.Instance.RemoveListener<TileSelectedEvent>(UpdateCurrentTile);
        EventHub.Instance.RemoveListener<CharacterSelectedEvent>(UpdateIsSelected);
        EventHub.Instance.RemoveListener<EnablePlayerEvent>(UpdateEnabledState);
        base.OnDisable();
    }

    #endregion

    void Awake()
    {
        if (!photonView.IsMine && PhotonNetwork.IsConnected) {
            return;
        }

        properties.PlayerID = PhotonNetwork.LocalPlayer.ActorNumber;
        Interloper.LocalInterloperInstance = this.gameObject;

        if (PlayerUiPrefab != null) {
            InstantiatePlayerUI();
        }

        // we flag as don't destroy on load so that instance survives level synchronization, thus giving a seamless experience when levels load.
        DontDestroyOnLoad(this.gameObject);
    }
    
    void Start()
    {
        if (!photonView.IsMine && PhotonNetwork.IsConnected) {
            return;
        }
        //Set the current tile position to where it spawned.
        currentTilePosition = BoardManager.Instance.GetTileIdFromVector(this.gameObject.transform.position);
    }

    /* Called after GameManager approves that we can move. We call RPC to update position on all players. */
    private void UpdateCurrentTile(TileSelectedEvent tileEvent)
    {
        if (!state.CanMove) {
            return;
        }

        if (!photonView.IsMine && PhotonNetwork.IsConnected) {
            return;
        }
        byte destinationTileId = tileEvent.SelectedTile.Id;

        photonView.RPC("UpdateCurrentTileRPC", RpcTarget.All, destinationTileId);
    }

    [PunRPC]
    private void UpdateCurrentTileRPC(byte destinationTileId, PhotonMessageInfo info)
    {
        BoardManager.Instance.SetTileState(currentTilePosition, false);
        CurrentTilePosition = destinationTileId;
        BoardManager.Instance.SetTileState(destinationTileId, true);
    }

    /* Called after GameManager approves. */
    private void UpdateIsSelected(CharacterSelectedEvent charSelectedEvent)
    {
        if (!photonView.IsMine && PhotonNetwork.IsConnected) {
            return;
        }

        if (charSelectedEvent.ID == photonView.ViewID) {
            // If its clicked twice -> deselect it.
            state.IsSelected = state.IsSelected ^ true;
        }
        else {
            state.IsSelected = false;
        }
    }

    /* Needs to run on all instances of the class. */
    private void UpdateEnabledState(EnablePlayerEvent enabledPlayerEvent)
    {
        photonView.RPC("UpdateEnabledStateRPC", RpcTarget.All, enabledPlayerEvent.ActorNumber);
    }

    [PunRPC]
    private void UpdateEnabledStateRPC(byte actorNumber)
    {
        state.CanMove = properties.PlayerID == actorNumber;
    }
}
