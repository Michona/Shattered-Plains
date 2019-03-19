using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

    private PlayerManager target;

    public void SetTarget(PlayerManager _target)
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
            playerHealthSlider.value = target.StatsData.health;
        }

        if (target == null) {
            Destroy(this.gameObject);
            return;
        }
    }


}
