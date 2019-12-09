using ExitGames.Client.Photon;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class PlayerNameInput : MonoBehaviour
{
    // Store the PlayerPref Key to avoid typos
    const string playerNamePrefKey = "PlayerName";

    #region MonoBehaviour CallBacks

    void Start() {
        string defaultName = string.Empty;
        InputField inputField = this.GetComponent<InputField>();
        if (inputField != null) {
            if (PlayerPrefs.HasKey(playerNamePrefKey)) {
                defaultName = PlayerPrefs.GetString(playerNamePrefKey);
                inputField.text = defaultName;
            }
        }

        PhotonNetwork.NickName = defaultName;
        
        //The default team. 
        SetPlayerTeam(0);
    }

    #endregion

    public void SetPlayerName(string value) {
        if (string.IsNullOrEmpty(value)) {
            Debug.LogError("Player Name is null or empty");
            return;
        }

        PhotonNetwork.NickName = value;

        PlayerPrefs.SetString(playerNamePrefKey, value);
    }

    public void SetPlayerTeam(int value) {
        if (value == 0) {
            PhotonNetwork.LocalPlayer.SetCustomProperties(new Hashtable {{"positions", new int[] {1, 2}}});
        }
        else {
            PhotonNetwork.LocalPlayer.SetCustomProperties(new Hashtable {{"positions", new int[] {9, 10}}});
        }
    }
}