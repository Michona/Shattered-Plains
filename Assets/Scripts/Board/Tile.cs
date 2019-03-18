using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
    }

    public void Selected()
    {
        GameManager.Instance.MovePlayerToTile(gameObject.transform.position);
    }
}
