using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class Interloper : CBaseManager
{

    /* The local player instance. Use this to know if the local player is represented in the Scene" */
    public static GameObject LocalInterloperInstance;

    #region Fields from PlayerManager

    [SerializeField]
    private CProperties properties = new CProperties(2,3); //just mock values. Change later!
    public override CProperties Properties { get => properties; }

    private CState state = new CState();
    public override CState State { get => state; }

    #endregion

    #region Subscribe to EventHub

    public override void OnEnable()
    {
        EventHub.Instance.AddListener<CharacterSelectedEvent>(UpdateIsSelected);
        EventHub.Instance.AddListener<EnablePlayerEvent>(UpdateEnabledState);
        base.OnEnable();
    }

    public override void OnDisable()
    {
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
        properties.CharacterID = photonView.ViewID;
        LocalInterloperInstance = this.gameObject;

        if (PlayerUiPrefab != null) {
            InstantiatePlayerUI();
        }

        // we flag as don't destroy on load so that instance survives level synchronization, thus giving a seamless experience when levels load.
        DontDestroyOnLoad(this.gameObject);
    }
    
    /* Called after GameManager approves. */
    private void UpdateIsSelected(CharacterSelectedEvent characterSelectedEvent)
    {
        if (!photonView.IsMine && PhotonNetwork.IsConnected) {
            return;
        }

        if (characterSelectedEvent.CharacterID == properties.CharacterID) {
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
        if (properties.PlayerID == actorNumber) {
            state.ResetTurn();
            state.CanMove = true;
        }
        else {
            state.CanMove = false;
        }
    }
}
