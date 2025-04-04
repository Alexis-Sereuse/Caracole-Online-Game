using TMPro;
using UnityEngine;

public class PlayerInformationBanner : MonoBehaviour
{
    [SerializeField] private TMP_Text _playerNameText;
    [SerializeField] private GameObject _masterClientImageObject;

    public void SetPlayerName(string playerName)
    {
        _playerNameText.text = playerName;
    }

    public void DisplayMasterClientStatus(bool isMaster)
    {
        _masterClientImageObject.SetActive(isMaster);
    }

    public void SetReadyState(bool isReady)
    {

    }
}
