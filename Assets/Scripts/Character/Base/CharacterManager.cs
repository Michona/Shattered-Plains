using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public abstract class CharacterManager : MonoBehaviourPunCallbacks
{

    #region Private Serializable Fields

    [SerializeField]
    public GameObject PlayerUiPrefab;

    #endregion

    #region Abstract Fields

    /* Character properties that don't change during the game. */
    public abstract CharacterProperties Properties { get; set; }

    /* Represents the current state of the character. It changes during the game. */
    protected abstract CharacterState State { get; set; }

    /* An id of the tile we currently are. We have it separate so that it can get networked. */
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

    #region Protected Methods called from subclasses

    /* Called from subclasses when currentTilePosition is changed. */
    protected void MovePlayerToTile(GameObject playerObject, byte tileId)
    {
        StartCoroutine(ManhattanMove(playerObject, tileId));
    }

    /* This should get overriden if players have different HUD's */
    protected void InstantiatePlayerUI()
    {
        GameObject uiGameObject = Instantiate(this.PlayerUiPrefab);
        uiGameObject.SendMessage("SetTarget", this, SendMessageOptions.RequireReceiver);
    }


    #endregion

    private void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        InstantiatePlayerUI();
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
}
