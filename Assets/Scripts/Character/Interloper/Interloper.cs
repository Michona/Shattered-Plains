using System;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class Interloper : CBaseManager
{

    /* The local player instance. Use this to know if the local player is represented in the Scene" */
    public static GameObject LocalInterloperInstance;

    #region Fields from PlayerManager

    [SerializeField]
    private CProperties properties = new CProperties(3,2); //just mock values. Change later!
    public override CProperties Properties => properties;
    
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
}
