using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class Heartrender : CBaseManager
{

    #region Fields from PlayerManager

    [SerializeField]
    private CProperties properties = new CProperties(2,1); //just mock values. Change later!
    public override CProperties Properties => properties;
    
    #endregion

    void Awake()
    {
        if (!photonView.IsMine && PhotonNetwork.IsConnected) {
            return;
        }

        properties.PlayerID = PhotonNetwork.LocalPlayer.ActorNumber;
        properties.CharacterID = photonView.ViewID;

        if (PlayerUiPrefab != null) {
            InstantiatePlayerUI();
        }

        // we flag as don't destroy on load so that instance survives level synchronization, thus giving a seamless experience when levels load.
        DontDestroyOnLoad(this.gameObject);
    }}
