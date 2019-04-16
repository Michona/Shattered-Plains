using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public abstract class CBaseManager : MonoBehaviourPunCallbacks
{

    public GameObject PlayerUiPrefab;

    #region Abstract Fields

    /* Character properties that don't change during the game. Contains PlayerID and CharacterID. */
    public abstract CProperties Properties { get; }

    /* Represents the current state of the character. It changes during the game. */
    public abstract CState State { get; }

    #endregion

    #region Subscribing to SceneManager

    /* Subscribe to sceneLoaded */
    public override void OnEnable()
    {
        SceneManager.sceneLoaded += OnLevelFinishedLoading;
        base.OnEnable();
    }

    /* Unsubscribe to sceneLoaded */
    public override void OnDisable()
    {
        SceneManager.sceneLoaded -= OnLevelFinishedLoading;
        base.OnDisable();
    }

    #endregion

    protected void InstantiatePlayerUI()
    {
        GameObject uiGameObject = Instantiate(this.PlayerUiPrefab);
        uiGameObject.SendMessage("SetTarget", this, SendMessageOptions.RequireReceiver);
    }

    private void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        InstantiatePlayerUI();
    }
}
