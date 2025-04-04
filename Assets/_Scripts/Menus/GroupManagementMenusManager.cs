using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using WebSocketSharp;

public class GroupManagementMenusManager : MonoBehaviour
{
    public static GroupManagementMenusManager Instance { get; private set; }

    private PhotonView _photonView;

    [Header("General")]
    [SerializeField] private Sprite _unselectedSectionSprite;
    [SerializeField] private Color _unselectedSectionTextColor;
    [SerializeField] private Sprite _selectedSectionSprite;
    [SerializeField] private Color _selectedSectionTextColor;
    [SerializeField] private Transform _roomsInformationBannersContainer;
    [SerializeField] private GameObject _roomInformationBannerPrefab;
    [SerializeField] private GameObject _roomsListSectionButton;
    [SerializeField] private GameObject _roomCreationSectionButton;
    [SerializeField] private GameObject _roomsListSectionParent;
    [SerializeField] private GameObject _roomCreationSectionParent;
    private List<GameObject> _displayedRoomInformationBanners;
    private List<RoomInfo> _roomsInfo;

    [Header("Room Creation")]
    [SerializeField] private TMP_InputField _roomNameInput;
    [SerializeField] private TMP_Text _roomNameErrorMessage;

    [SerializeField] private TMP_Text _maximumPlayersCountText;
    [SerializeField] private Button _increaseMaximumPlayersCountButton;
    [SerializeField] private Button _decreaseMaximumPlayersCountButton;
    private int _maximumPlayersCount;

    [SerializeField] private TMP_Text _accessibilityStatusText;
    [SerializeField] private TMP_InputField _passwordInput;
    [SerializeField] private TMP_Text _passwordErrorMessage;
    private bool _isRoomPublic;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    private void Start()
    {
        _photonView = PhotonView.Get(this);
        _displayedRoomInformationBanners = new List<GameObject>();
        _roomsInfo = new List<RoomInfo>();

        ResetGroupMenusGeneralFields();
        ResetRoomCreationFields();
    }

    public void ResetGroupMenusGeneralFields()
    {
        if (RoomsInfoScriptableObject.Instance.IsGroupListDefaultGroupMenu)
            OnRoomsListSectionClicked(true);
        else
            OnRoomCreationSectionClicked(true);
    }

    public void ResetRoomCreationFields()
    {
        _roomNameInput.text = "";
        _roomNameErrorMessage.gameObject.SetActive(false);
        _maximumPlayersCount = RoomsInfoScriptableObject.Instance.DefaultMaximumPlayersCount;
        _maximumPlayersCountText.text = _maximumPlayersCount.ToString();
        if (_maximumPlayersCount == RoomsInfoScriptableObject.Instance.SmallestMaximumPlayersCount)
            _decreaseMaximumPlayersCountButton.interactable = false;
        if (_maximumPlayersCount == RoomsInfoScriptableObject.Instance.GreatestMaximumPlayersCount)
            _increaseMaximumPlayersCountButton.interactable = false;
        _isRoomPublic = RoomsInfoScriptableObject.Instance.IsRoomPublicByDefault;
        if (_isRoomPublic)
        {
            _accessibilityStatusText.text = "Public";
            _passwordInput.gameObject.SetActive(false);
        }
        else
        {
            _accessibilityStatusText.text = "Private";
            _passwordInput.gameObject.SetActive(true);
        }
        _passwordInput.text = "";
        _passwordErrorMessage.gameObject.SetActive(false);
    }

    #region ROOMS LIST

    public void UpdateRoomsList(List<RoomInfo> roomList)
    {
        foreach (GameObject banner in _displayedRoomInformationBanners)
            Destroy(banner);
        _displayedRoomInformationBanners.Clear();

        _roomsInfo.Clear();

        foreach (RoomInfo roomInfo in roomList)
        {
            if (!roomInfo.IsOpen || !roomInfo.IsVisible)
                continue;

            _roomsInfo.Add(roomInfo);

            GameObject newBanner = Instantiate(_roomInformationBannerPrefab, _roomsInformationBannersContainer);
            RoomInformationBanner newBannerComponent = newBanner.GetComponent<RoomInformationBanner>();
            newBannerComponent.SetRoomName(roomInfo.Name);
            newBannerComponent.SetPlayersCount(roomInfo.PlayerCount, roomInfo.MaxPlayers);
            newBannerComponent.SetAccessStatus((bool)roomInfo.CustomProperties["IsPublic"]);
            _displayedRoomInformationBanners.Add(newBanner);
        }
    }

    public void OnRoomsListSectionClicked(bool bypassActiveStateVerification = false)
    {
        if (!bypassActiveStateVerification && _roomsListSectionParent.activeSelf)
            return;

        _roomsListSectionParent.SetActive(true);
        _roomCreationSectionParent.SetActive(false);

        _roomsListSectionButton.GetComponent<Image>().sprite = _selectedSectionSprite;
        _roomsListSectionButton.transform.GetChild(0).GetComponent<TMP_Text>().color = _selectedSectionTextColor;
        _roomCreationSectionButton.GetComponent<Image>().sprite = _unselectedSectionSprite;
        _roomCreationSectionButton.transform.GetChild(0).GetComponent<TMP_Text>().color = _unselectedSectionTextColor;
    }

    #endregion

    #region ROOM CREATION

    public void OnRoomCreationSectionClicked(bool bypassActiveStateVerification = false)
    {
        if (!bypassActiveStateVerification && _roomCreationSectionParent.activeSelf)
            return;

        _roomCreationSectionParent.SetActive(true);
        _roomsListSectionParent.SetActive(false);

        _roomCreationSectionButton.GetComponent<Image>().sprite = _selectedSectionSprite;
        _roomCreationSectionButton.transform.GetChild(0).GetComponent<TMP_Text>().color = _selectedSectionTextColor;
        _roomsListSectionButton.GetComponent<Image>().sprite = _unselectedSectionSprite;
        _roomsListSectionButton.transform.GetChild(0).GetComponent<TMP_Text>().color = _unselectedSectionTextColor;
    }

    public void OnDecreaseMaximumPlayersCount()
    {
        _maximumPlayersCount--;
        _maximumPlayersCountText.text = _maximumPlayersCount.ToString();

        _increaseMaximumPlayersCountButton.interactable = true;
        if (_maximumPlayersCount == RoomsInfoScriptableObject.Instance.SmallestMaximumPlayersCount)
            _decreaseMaximumPlayersCountButton.interactable = false;
    }

    public void OnIncreaseMaximumPlayersCount()
    {
        _maximumPlayersCount++;
        _maximumPlayersCountText.text = _maximumPlayersCount.ToString();

        _decreaseMaximumPlayersCountButton.interactable = true;
        if (_maximumPlayersCount == RoomsInfoScriptableObject.Instance.GreatestMaximumPlayersCount)
            _increaseMaximumPlayersCountButton.interactable = false;
    }

    public void OnAccessibilityStatusChanged()
    {
        _isRoomPublic = !_isRoomPublic;
        if (_isRoomPublic)
        {
            _accessibilityStatusText.text = "Public";
            _passwordInput.gameObject.SetActive(false);
        }
        else
        {
            _accessibilityStatusText.text = "Private";
            _passwordInput.gameObject.SetActive(true);
        }
    }

    public void OnRoomCreationButtonClicked()
    {
        _roomNameErrorMessage.gameObject.SetActive(false);
        _passwordErrorMessage.gameObject.SetActive(false);

        bool errorFlag = false;

        if (_roomsInfo.Where(roomInfo => roomInfo.Name == _roomNameInput.text).Count() > 0)
        {
            _roomNameErrorMessage.text = "This group's name is already used";
            _roomNameErrorMessage.gameObject.SetActive(false);
            errorFlag = true;
        }

        if (!_isRoomPublic && _passwordInput.text.IsNullOrEmpty())
        {
            _passwordErrorMessage.text = "You need to create a password";
            _passwordErrorMessage.gameObject.SetActive(false);
            errorFlag = true;
        }

        if (errorFlag)
            return;

        string roomName = _roomNameInput.text.IsNullOrEmpty() ? $"Room {_roomsInfo.Count + 1}" : _roomNameInput.text;

        ExitGames.Client.Photon.Hashtable customRoomProperties = new ExitGames.Client.Photon.Hashtable() { { "IsPublic", _isRoomPublic.ToString() }, { "Password", _passwordInput.text } };
        string[] customRoomPropertiesForLobby = { "IsPublic", "Password" };
        RoomOptions roomOptions = new RoomOptions() { IsOpen = true, IsVisible = true, MaxPlayers = _maximumPlayersCount, CustomRoomProperties = customRoomProperties, CustomRoomPropertiesForLobby = customRoomPropertiesForLobby };
        PhotonNetwork.CreateRoom(roomName, roomOptions);
    }

    #endregion
}
