using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interloper : PlayerManager
{

    /* The local player instance. Use this to know if the local player is represented in the Scene" */
    public static GameObject LocalInterloperInstance;

    #region Fields from PlayerManager

    [SerializeField]
    private Stats stats = new Stats(4, 3);
    public override Stats StatsData { get => stats; set => stats = value; }

    [SerializeField]
    private byte currentTilePosition;
    public override byte CurrentTilePosition { 
        get => currentTilePosition;
        set {
            MovePlayerToTile(this.gameObject, value);
            currentTilePosition = value;
        } 
    }

    private Animator interloperAnimator;
    protected override Animator CustomAnimator { get => interloperAnimator; set => interloperAnimator = value; }

    #endregion

    void Awake()
    {
        //Has to be set for all interlopers!
        interloperAnimator = GetComponent<Animator>();

        if (!photonView.IsMine && PhotonNetwork.IsConnected) {
            return;
        }

        Interloper.LocalInterloperInstance = this.gameObject;

        if (PlayerUiPrefab != null) {
            InstantiatePlayerUI();
        }

        // #Critical
        // we flag as don't destroy on load so that instance survives level synchronization, thus giving a seamless experience when levels load.
        DontDestroyOnLoad(this.gameObject);
    }

    public void Update()
    {
        //Test for tile pos. TODO: remove later
        if (Input.GetKeyDown(KeyCode.Q)) {
            Debug.Log(this.photonView.Owner.NickName + " -- " + currentTilePosition);
        }
    }

}
