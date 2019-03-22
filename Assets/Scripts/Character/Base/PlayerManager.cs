using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public abstract class PlayerManager : MonoBehaviourPunCallbacks
{

    #region Private Serializable Fields

    [SerializeField]
    public GameObject PlayerUiPrefab;

    #endregion

    #region Abstract Fields

    public abstract Stats StatsData { get; set; }

    /* An id of the tile we currently are. */
    public abstract byte CurrentTilePosition {get; set; }

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

    #region Private/Protected Methods

    /* Called from subclasses when currentTilePosition is changed. */
    protected void MovePlayerToTile(GameObject playerObject, byte tileId)
    {
        Vector3 destination = BoardManager.Instance.GetVectorFromTileId(tileId);

        //rotate object
        float rotationAngle = Vector3.SignedAngle(playerObject.transform.forward, destination - playerObject.transform.position, Vector3.up);
        playerObject.transform.Rotate(0, rotationAngle, 0);

        //start coroutine to move to destination
        StartCoroutine(MoveOverSeconds(playerObject, destination, 2));
    }

    private void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        InstantiatePlayerUI();
    }

    /* This should get overriden if players have different HUD's */
    protected void InstantiatePlayerUI()
    {
        GameObject uiGameObject = Instantiate(this.PlayerUiPrefab);
        uiGameObject.SendMessage("SetTarget", this, SendMessageOptions.RequireReceiver);
    }

    /* Moves in a straght line. TODO: change so it's "manhattan" movement */
    private IEnumerator MoveOverSeconds(GameObject objectToMove, Vector3 end, float seconds)
    {
        float elapsedTime = 0;
        Vector3 startingPos = objectToMove.transform.position;
        while (elapsedTime < seconds) {
            objectToMove.transform.position = Vector3.Lerp(startingPos, end, (elapsedTime / seconds));
            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        objectToMove.transform.position = end;
    }

    #endregion
}
