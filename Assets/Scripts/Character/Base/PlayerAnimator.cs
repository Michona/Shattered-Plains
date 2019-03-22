using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Generic animator script. Characters should extend and add/override logic. Attached to playerPrefab. */
[RequireComponent(typeof(Animator))]
public class PlayerAnimator : MonoBehaviourPunCallbacks
{

    private Animator characterAnimator;

    void Awake()
    {
        characterAnimator = gameObject.GetComponent<Animator>();
    }

    public override void OnEnable() {
        EventHub.Instance.AddListener<TileSelectedEvent>(SetRunningState);
        base.OnEnable();
    }

    public override void OnDisable() {
        EventHub.Instance.RemoveListener<TileSelectedEvent>(SetRunningState);
        base.OnDisable();
    }

    public void SetRunningState(TileSelectedEvent e)
    {
        if (!photonView.IsMine && PhotonNetwork.IsConnected) {
            return;
        }

        photonView.RPC("SetRunningStateRPC", RpcTarget.All);
    }

    [PunRPC]
    void SetRunningStateRPC()
    {
        characterAnimator.SetBool("ShouldRun", true);
    }

    /* Should implement StopRunning() method, listening to event when the player reaches the destination! */
}
