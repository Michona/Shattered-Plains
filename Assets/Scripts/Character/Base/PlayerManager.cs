using Photon.Pun;
using System.Collections;
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
    public abstract byte CurrentTilePosition { get; set; }

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
        StartCoroutine(ManhattanMove(playerObject, tileId));
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

    private IEnumerator ManhattanMove(GameObject objectToMove, byte finalTile)
    {
        float elapsedTime = 0;
        float maxTime = Consts.MOVE_SECONDS;
        Vector3 startingPos = objectToMove.transform.position;

        /* Get positions for grid like movement according to the current tile and the selected tile! */
        Vector3[] path = BoardManager.Instance.GetVector3Path(CurrentTilePosition, finalTile);

        foreach (Vector3 destination in path) {

            float rotationAngle = Vector3.SignedAngle(objectToMove.transform.forward, destination - objectToMove.transform.position, Vector3.up);
            objectToMove.transform.Rotate(0, rotationAngle, 0);

            elapsedTime = 0;
            while (elapsedTime < maxTime) {

                objectToMove.transform.position = Vector3.Lerp(startingPos, destination, (elapsedTime / maxTime));
                elapsedTime += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }
            objectToMove.transform.position = destination;
            startingPos = objectToMove.transform.position;
        }
    }

    #endregion
}
