using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using UnityEngine;

/**
 * Responsable for movement. Attached to the character prefab (Needs to have a photonView). 
 * Sends RPC for current tile update. */
public class CMovement : MonoBehaviourPunCallbacks
{
    private byte currentTilePosition;
    public byte CurrentTilePosition {
        get => currentTilePosition;
        set {
            /* When positon is changed -> move to tile. */
            if (isSpawned) {
                MovePlayerToTile(this.gameObject, value);
            }
            currentTilePosition = value;
        }
    }

    private bool isSpawned = false;

   void Start()
    {
        if (photonView.IsMine) {
            currentTilePosition = BoardManager.Instance.GetTileIdFromVector(this.gameObject.transform.position);
            photonView.RPC("UpdateCurrentTileRPC", RpcTarget.All, BoardManager.Instance.GetTileIdFromVector(this.gameObject.transform.position));
        }

        isSpawned = true;
        
        Debug.Log("CURRENT TILE :  " + currentTilePosition);
    }

    #region EventHub Setup 
    public override void OnEnable()
    {
        EventHub.Instance.AddListener<TileSelectedEvent>(UpdateCurrentTile);
        base.OnEnable();
    }

    public override void OnDisable()
    {
        EventHub.Instance.RemoveListener<TileSelectedEvent>(UpdateCurrentTile);
        base.OnDisable();
    }
    #endregion

    private void UpdateCurrentTile(TileSelectedEvent e)
    {
        if (!photonView.IsMine) {
            return;
        }

        if (GameManager.Instance.GetSelectedCharacter().State.CanMove) {
            photonView.RPC("UpdateCurrentTileRPC", RpcTarget.All, e.SelectedTile.Id);
        }
    }

    [PunRPC]
    private void UpdateCurrentTileRPC(byte destinationTileId, PhotonMessageInfo info)
    {
        BoardManager.Instance.SetTileState(currentTilePosition, false);
        CurrentTilePosition = destinationTileId;
        BoardManager.Instance.SetTileState(destinationTileId, true);
    }

    private void MovePlayerToTile(GameObject playerObject, byte tileId)
    {
        StartCoroutine(ManhattanMove(playerObject, tileId));
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
