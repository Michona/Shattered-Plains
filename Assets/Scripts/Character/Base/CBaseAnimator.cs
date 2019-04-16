using Photon.Pun;
using UnityEngine;

/* Generic animator script. Characters should extend and add/override logic. Attached to playerPrefab. */
[RequireComponent(typeof(Animator))]
public class CBaseAnimator : MonoBehaviourPunCallbacks
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

    }
}
