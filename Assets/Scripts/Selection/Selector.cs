using Photon.Pun;
using UnityEngine;

[RequireComponent(typeof(LayerMask))]
public class Selector : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private LayerMask clickableLayer;

    void Update()
    {

        if (Input.GetMouseButtonDown(0)) {

            RaycastHit hit;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, clickableLayer)) {

                Tile tileHit;
                Interloper interloperHit;
                //We hit a tile.
                if (tileHit = hit.collider.GetComponent<Tile>()) {
                    GameManager.Instance.MovePlayerToTile(tileHit);
                }
                //We hit interloper character.
                else if (interloperHit = hit.collider.GetComponent<Interloper>()) {
                    GameManager.Instance.SelectPlayer(interloperHit);
                }
            }
        }

        //Temporaraly here for testing purpose. 
        if (Input.GetKeyDown(KeyCode.Q)) {
            TurnManager.Instance.SwitchTurn();
        }
    }
}
