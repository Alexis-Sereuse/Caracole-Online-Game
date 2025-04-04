using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GroupMenuManager : MonoBehaviour
{
    public static GroupMenuManager Instance { get; private set; }

    private PhotonView _photonView;

    [SerializeField] private GameObject _playButton;
    [SerializeField] private GameObject _roomButtonsParent;
    [SerializeField] private TMP_Text _roomNameText;
    [SerializeField] private TMP_Text _roomPlayersCountText;
    [SerializeField] private Image _readyButtonImage;
    [SerializeField] private Sprite _readyImage;
    [SerializeField] private Sprite _notReadyImage;
    [SerializeField] private GameObject _playerBannerPrefab;
    [SerializeField] private List<GameObject> _playerBannerSlots;
    private bool _isLocalPlayerReady;
    private int _playersInformationCountReceived;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    private void Start()
    {
        _photonView = PhotonView.Get(this);
    }

    public void InitializeRoomButtonsForMasterClient()
    {
        _playButton.SetActive(true);
        _playButton.transform.parent = _roomButtonsParent.transform;
    }

    public void OnOtherPlayerJoinedRoom(Player otherPlayer)
    {

    }

    public void OnLocalPlayerJoinedRoom()
    {
        _roomNameText.text = PhotonNetwork.CurrentRoom.Name;
        _roomPlayersCountText.text = $"{PhotonNetwork.CurrentRoom.PlayerCount}/{PhotonNetwork.CurrentRoom.MaxPlayers}";

        if (PhotonNetwork.IsMasterClient)
        {
            InstantiateRoomPlayerBanner(PhotonNetwork.NickName, false, true, 0);
            MenusManager.Instance.ChangeMenu(eMenu.GroupMenu);
            return;
        }

        MenusManager.Instance.ChangeMenu(eMenu.LoadingScreen);
        _photonView.RPC(nameof(AskForRoomPlayersInformation), RpcTarget.Others, PhotonNetwork.LocalPlayer);
    }

    [PunRPC]
    public void AskForRoomPlayersInformation(Player askingPlayer)
    {
        _playersInformationCountReceived = 0;
        _photonView.RPC(nameof(SendRoomPlayerInformation), askingPlayer, PhotonNetwork.NickName, _isLocalPlayerReady, PhotonNetwork.IsMasterClient);
    }

    [PunRPC]
    public void SendRoomPlayerInformation(string playerName, bool isReady, bool isMaster)
    {
        _playersInformationCountReceived++;

        int slotNumber = 0;
        while (slotNumber < _playerBannerSlots.Count && _playerBannerSlots[slotNumber].transform.childCount == 1)
            slotNumber++;

        InstantiateRoomPlayerBanner(playerName, isReady, isMaster, slotNumber);

        if (_playersInformationCountReceived == PhotonNetwork.CurrentRoom.Players.Count)
        {
            if (_playerBannerSlots[slotNumber].transform.childCount == 1)
                slotNumber++;

            InstantiateRoomPlayerBanner(playerName, isReady, isMaster, slotNumber);

            MenusManager.Instance.ChangeMenu(eMenu.GroupMenu);
        }
    }

    private void InstantiateRoomPlayerBanner(string playerName, bool isReady, bool isMaster, int lineNumber)
    {
        GameObject playerBannerObject = Instantiate(_playerBannerPrefab, _playerBannerSlots[lineNumber].transform);
        PlayerInformationBanner playerBanner = playerBannerObject.GetComponent<PlayerInformationBanner>();
        playerBanner.SetPlayerName(playerName);
        playerBanner.DisplayMasterClientStatus(isMaster);
        playerBanner.SetReadyState(isReady);
    }
}
