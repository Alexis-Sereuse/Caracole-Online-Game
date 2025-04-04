using Photon.Pun;
using TMPro;
using UnityEngine;

public class NicknameMenuManager : MonoBehaviour
{
    [SerializeField] private TMP_InputField _nicknameInput;
    [SerializeField] private TMP_Text _errorMessage;

    private void Start()
    {
        _errorMessage.gameObject.SetActive(false);
    }

    public void OnNicknameValidated()
    {
        if (_nicknameInput.text == "")
        {
            _errorMessage.text = "You must enter a nickname";
            _errorMessage.gameObject.SetActive(true);
            return;
        }

        PhotonNetwork.LocalPlayer.NickName = _nicknameInput.text;
        SaveManager.SetPlayerName(_nicknameInput.text);
        SaveManager.SaveData();
        MenusManager.Instance.ChangeMenu(eMenu.MainMenu);
    }
}
