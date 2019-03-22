using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LayerMask))]
public class Selector : MonoBehaviour
{
    [SerializeField]
    private LayerMask clickableLayer;

    // Update is called once per frame
    void Update()
    {

        if (Input.GetMouseButtonDown(0)) {

            RaycastHit hit;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, clickableLayer)) {

                Tile tileHit;
                if (tileHit = hit.collider.GetComponent<Tile>()) {
                    GameManager.Instance.MovePlayerToTile(tileHit);
                }
            }
        }
        
    }
}
