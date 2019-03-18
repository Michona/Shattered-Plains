using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviourPunCallbacks, IPunObservable
{
    #region Private Serializable Fields

    [SerializeField]
    private Vector3 destination;

    [Tooltip("The Player's UI GameObject Prefab")]
    [SerializeField]
    public GameObject PlayerUiPrefab;

    #endregion

    public byte health;

    [Tooltip("The local player instance. Use this to know if the local player is represented in the Scene")]
    public static GameObject LocalPlayerInstance;

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
        if (Input.GetKeyDown(KeyCode.Space)) {

            //rotate object
            float rotationAngle = Vector3.Angle(gameObject.transform.forward, destination);
            gameObject.transform.Rotate(0, rotationAngle, 0);

            //start coroutine to move to destination
            StartCoroutine(MoveOverSeconds(destination, 2));
        }

        if (Input.GetKeyDown(KeyCode.Q)) {
            health--;
        }
    }

    public IEnumerator MoveOverSeconds(Vector3 destination, float seconds)
    {
        float elapsedTime = 0;
        Vector3 startingPos = gameObject.transform.position;
        while (elapsedTime < seconds) {
            gameObject.transform.position = Vector3.Lerp(startingPos, destination, (elapsedTime / seconds));
            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        gameObject.transform.position = destination;
    }
}
