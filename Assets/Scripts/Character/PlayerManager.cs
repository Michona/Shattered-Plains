using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Animator))]
public class PlayerManager : MonoBehaviourPunCallbacks, IPunObservable
{
    #region Private Serializable Fields

    [Tooltip("The Player's UI GameObject Prefab")]
    [SerializeField]
    public GameObject PlayerUiPrefab;

    #endregion

    public byte health;

    [Tooltip("The local player instance. Use this to know if the local player is represented in the Scene")]
    public static GameObject LocalPlayerInstance;

    private Animator animator;

    #region Public Methods

    public void MovePlayerToTile(Vector3 destination)
    {
        if (!photonView.IsMine && PhotonNetwork.IsConnected) {
            return;
        }

        //rotate object
        float rotationAngle = Vector3.SignedAngle(gameObject.transform.forward, destination - gameObject.transform.position, Vector3.up);
        gameObject.transform.Rotate(0, rotationAngle, 0);

        //set animation
        animator.SetBool("ShouldRun", true);

        //start coroutine to move to destination
        StartCoroutine(MoveOverSeconds(gameObject, destination, 2));
    }

    #endregion

    #region IPunObservable implemantation

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting) {
            stream.SendNext(health);
        }
        else {
            this.health = (byte)stream.ReceiveNext();
        }
    }

    #endregion

    void Awake()
    {
        // #Important
        // used in GameManager.cs: we keep track of the localPlayer instance to prevent instantiation when levels are synchronized

        if (photonView.IsMine) {
            PlayerManager.LocalPlayerInstance = this.gameObject;
        }

        animator = GetComponent<Animator>();
        // #Critical
        // we flag as don't destroy on load so that instance survives level synchronization, thus giving a seamless experience when levels load.
        DontDestroyOnLoad(this.gameObject);
    }

    void Start()
    {
        if (!photonView.IsMine && PhotonNetwork.IsConnected) {
            return;
        }

        if (PlayerUiPrefab != null) {
            InstantiatePlayerUI();
        }
        else {
            Debug.LogWarning("Missing PlayerUIPrefab", this);
        }
    }

    void Update()
    {
        if (!photonView.IsMine && PhotonNetwork.IsConnected) {
            return;
        }
        procesInput();

        if (health <= 0f) {
            GameManager.Instance.LeaveRoom();
        }
    }

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

    private void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        InstantiatePlayerUI();
    }

    private void InstantiatePlayerUI()
    {
        GameObject _uiGo = Instantiate(this.PlayerUiPrefab);
        _uiGo.SendMessage("SetTarget", this, SendMessageOptions.RequireReceiver);
    }

    private void procesInput()
    {
        if (Input.GetKeyDown(KeyCode.Q)) {
            health--;
        }
    }

    //TODO mby move these to helper class
    private IEnumerator MoveOverSpeed(Vector3 destination, float speed)
    {
        while (gameObject.transform.position != destination) {
            gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, destination, speed * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }
    }

    private IEnumerator MoveOverSeconds(GameObject objectToMove, Vector3 end, float seconds)
    {
        float elapsedTime = 0;
        Vector3 startingPos = objectToMove.transform.position;
        while (elapsedTime < seconds) {
            objectToMove.transform.position = Vector3.Lerp(startingPos, end, (elapsedTime / seconds));
            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        objectToMove.transform.position = end;
    }

}
