using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public abstract class CBaseManager : MonoBehaviourPunCallbacks
{
    public GameObject PlayerUiPrefab;

    #region Abstract Fields

    /* Character properties that don't change during the game. Contains PlayerID and CharacterID. */
    public abstract CProperties Properties { get; }

    /* Represents the current state of the character. It changes during the game. */
    private CStateEnum _stateEnum = CStateEnum.IDLE;
    public CStateEnum StateEnum => _stateEnum;

    #endregion

    private GameObject playerHUD;
            
    #region Subscribing to SceneManager

    /* Subscribe to sceneLoaded */
    public override void OnEnable() {
        SceneManager.sceneLoaded += OnLevelFinishedLoading;
        base.OnEnable();
    }

    /* Unsubscribe to sceneLoaded */
    public override void OnDisable() {
        SceneManager.sceneLoaded -= OnLevelFinishedLoading;
        base.OnDisable();
    }

    #endregion

    protected void InstantiatePlayerUI() {
        playerHUD = Instantiate(this.PlayerUiPrefab);
        playerHUD.SendMessage("SetTarget", this, SendMessageOptions.RequireReceiver);
        playerHUD.SetActive(false);
    }

    private void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode) {
        InstantiatePlayerUI();
    }

    public bool IsInMoveRange(byte tileId) {
        return GetComponent<CMovement>().IsInMoveRange(tileId);
    }


    
    public void OnTurnReset() {
        if (!photonView.IsMine && PhotonNetwork.IsConnected) {
            return;
        }

        _stateEnum = CStateEnum.IDLE;
        playerHUD.SetActive(false);
    }

    public void OnAttacked() {
        if (!photonView.IsMine && PhotonNetwork.IsConnected) {
            return;
        }
    }
    
    /** Called when GameManager approves. */
    public void OnMoveCharacter() {
        if (!photonView.IsMine && PhotonNetwork.IsConnected) {
            return;
        }

        _stateEnum = CStateEnum.DONE;
        playerHUD.SetActive(false);
    }
    
    public void OnUnselectCharacter() {
        if (!photonView.IsMine && PhotonNetwork.IsConnected) {
            return;
        }

        if (StateEnum == CStateEnum.SELECTED) {
            _stateEnum = CStateEnum.IDLE;
            playerHUD.SetActive(false);
        }
    }

    /* Called after GameManager approves. */
    public void OnSelectCharacter(CharacterSelectedEvent characterSelectedEvent) {
        if (!photonView.IsMine && PhotonNetwork.IsConnected) {
            return;
        }

        if (characterSelectedEvent.CharacterID == Properties.CharacterID) {
            _stateEnum = CStateEnum.SELECTED;
            playerHUD.SetActive(true);
        }
    }
}