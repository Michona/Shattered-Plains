using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    #region Private Fields

    [Tooltip("UI Text to display Player's Name")]
    [SerializeField]
    private Text playerNameText;

    [Tooltip("UI Slider to display Player's Health")]
    [SerializeField]
    private Slider playerHealthSlider;

    #endregion

    private CharacterManager target;

    public void SetTarget(CharacterManager _target)
    {
        if (_target == null) {
            Debug.LogError("Missing PlayerManager", this);
            return;
        }
        // Cache references for efficiency
        target = _target;
        if (playerNameText != null) {
            playerNameText.text = target.photonView.Owner.NickName;
        }
    }

    public void Awake()
    {
        this.transform.SetParent(GameObject.Find("Canvas").GetComponent<Transform>(), false);
    }

    public void Update()
    {
        if (playerHealthSlider != null) {
            playerHealthSlider.value = target.Properties.Health;
        }

        if (target == null) {
            Destroy(this.gameObject);
            return;
        }
    }

}
