using Photon.Pun;
using UnityEngine;

/* Generic animator script. Characters should extend and add/override logic. Attached to playerPrefab. */
[RequireComponent(typeof(Animator))]
public class CharacterAnimator : MonoBehaviourPunCallbacks
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

        //Animation disabled (for now).
        //photonView.RPC("SetRunningStateRPC", RpcTarget.All);
    }

    [PunRPC]
    void SetRunningStateRPC()
    {
        characterAnimator.SetBool("ShouldRun", true);
    }

    /* Should implement StopRunning() method, listening to event when the player reaches the destination! */
}
