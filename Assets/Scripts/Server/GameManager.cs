using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;

public class GameManager : MonoBehaviourPunCallbacks
{
    public static GameManager Instance;

    /* Game objects to be instantiated on the network */
    [SerializeField] private List<GameObject> characters;
    private List<CBaseManager> characterManagers;

    /* Local Selected Character. 
     * Needs to keep this always updated, since it's a single source of infomation about the character. */
    private CBaseManager selectedCharacter;

    #region Photon Callbacks

    /* Called when local player left the room. We load the lobby then. */
    public override void OnLeftRoom() {
        SceneManager.LoadScene(0);
    }
    
    #endregion


    void Start() {
        Instance = this;
        InstantiateCharacters();
    }

    private void InstantiateCharacters() {
        // we're in a room. spawn a character for the local player. it gets synced by using PhotonNetwork.Instantiate
        if (Interloper.LocalInterloperInstance == null) {
            var spawnPos = (int[]) PhotonNetwork.LocalPlayer.CustomProperties["positions"];

            for (int i = 0; i < characters.Count; i++) {
                characters[i] = PhotonNetwork.Instantiate(characters[i].name,
                    BoardManager.Instance.GetVectorFromTileId((byte) spawnPos[i]), Quaternion.identity, 0);
            }
            
            characterManagers = characters.Select(go => go.GetComponent<CBaseManager>()).ToList();
        }
    }

    #region Public methods

    /* A wrapper on PhotonNetwork.LeaveRoom(). We might need to do more logic when players leave.*/
    public void LeaveRoom() {
        PhotonNetwork.LeaveRoom();
    }

    /* If we are allowed to move there -> Fire an event. */
    public void MovePlayerToTile(Tile selectedTile) {
        if (selectedCharacter != null) {
            Debug.Log("Selected tileid: " + selectedTile.Id + " - IsMyTurn: " + TurnManager.Instance.IsMyTurn() +
                      " - isOccupied: " + selectedTile.isOccupied + " - stateEnum: " + selectedCharacter.StateEnum);


            if (TurnManager.Instance.IsMyTurn()
                && !selectedTile.isOccupied
                && selectedCharacter.StateEnum == CStateEnum.SELECTED
                && selectedCharacter.IsInMoveRange(selectedTile.Id)) {
                
                
                EventHub.Instance.FireEvent(new MovePlayerEvent());
                selectedCharacter.OnMoveCharacter();
                selectedCharacter.GetComponent<CMovement>().UpdateCurrentTile(new TileSelectedEvent(selectedTile));
                
                
                if (AreCharactersDone()) {
                    TurnManager.Instance.EndTurn();
                    ResetCharacters();
                }
            }
            
        }
    }

    public void SelectPlayer(CBaseManager characterHit) {
        Debug.Log(" - IsMyTurn: " + TurnManager.Instance.IsMyTurn() + " - stateEnum: " + characterHit.StateEnum +
                  " - isMine " + characterHit.photonView.IsMine);

        if (!characterHit.photonView.IsMine || !TurnManager.Instance.IsMyTurn()) {
            return;
        }

        //Remove selection of other characters
        UnselectCharacters();

        if (characterHit.StateEnum == CStateEnum.IDLE) {
            selectedCharacter = characterHit;

            var charSelectedEvent =
                new CharacterSelectedEvent(
                    characterHit.Properties.CharacterID,
                    characterHit.transform.position,
                    characterHit.Properties.MoveRange);

            selectedCharacter.OnSelectCharacter(charSelectedEvent);
            EventHub.Instance.FireEvent(charSelectedEvent);
        }
        else if (characterHit.StateEnum == CStateEnum.DONE) {
            selectedCharacter = null;
        }
    }

    #endregion

    private bool AreCharactersDone() {
        var areDone = true;
        foreach (var cBaseManager in characterManagers) {
            if (cBaseManager.StateEnum != CStateEnum.DONE) {
                areDone = false;
                break;
            }
        }

        return areDone;
    }

    private void ResetCharacters() {
        foreach (var cBaseManager in characterManagers) {
            cBaseManager.OnTurnReset();
        }
    }

    private void UnselectCharacters() {
        foreach (var cBaseManager in characterManagers) {
            cBaseManager.OnUnselectCharacter();
        }
    }
    
}