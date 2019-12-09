using System;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using UnityEngine;
using UnityEngine.PlayerLoop;

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

    /** Gotten from CBaseManger*/
    private int _moveRange;
    
    private bool isSpawned = false;


    private void Awake() {
        _moveRange = gameObject.GetComponent<CBaseManager>().Properties.MoveRange;
    }


    public override void OnPlayerEnteredRoom(Player newPlayer) {
        base.OnPlayerEnteredRoom(newPlayer);
        
        UpdateCurrentTile(new TileSelectedEvent(BoardManager.Instance.GetTileFromVector(this.gameObject.transform.position)));
    }

    void Start()
    {
        if (photonView.IsMine) {
            currentTilePosition = BoardManager.Instance.GetTileIdFromVector(this.gameObject.transform.position);
            photonView.RPC("UpdateCurrentTileRPC", RpcTarget.All, currentTilePosition);
        }

        isSpawned = true;
        
        Debug.Log("CURRENT TILE :  " + currentTilePosition);
    }

    /** Called from GameManager when the character needs to move. */
    public void UpdateCurrentTile(TileSelectedEvent e)
    {
        if (!photonView.IsMine) {
            return;
        }

        if (IsInMoveRange(e.SelectedTile.Id)) {
            photonView.RPC("UpdateCurrentTileRPC", RpcTarget.All, e.SelectedTile.Id);
        }
    }

    public bool IsInMoveRange(byte tileId) {
        return BoardManager.Instance.GetDistanceBetweenTiles(currentTilePosition, tileId) <= _moveRange;
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
