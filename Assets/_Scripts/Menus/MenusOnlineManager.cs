using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;

public class MenusOnlineManager : MonoBehaviourPunCallbacks
{
    public static MenusOnlineManager Instance;
    public bool IsClientConnectedToMaster { get; private set; }
    private bool _isSaveLoaded;

    private void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;

        SaveManager.SaveLoadedEvent += OnSaveLoaded;
        _isSaveLoaded = false;

        IsClientConnectedToMaster = false;

        if (Instance == null)
            Instance = this;
    }

    private void Start()
    {
        if (PhotonNetwork.IsConnected)
        {
            IsClientConnectedToMaster = true;
        }
        else
        {
            PhotonNetwork.ConnectUsingSettings();
            MenusManager.Instance.ChangeMenu(eMenu.LoadingScreen);
            IsClientConnectedToMaster = false;
        }

        PhotonNetwork.GameVersion = GameInfo.Instance.GameVersion;

        DontDestroyOnLoad(gameObject);
    }

    public override void OnConnectedToMaster()
    {
        IsClientConnectedToMaster = true;
        if (_isSaveLoaded)
        {
            if (SaveManager.IsSaveBlank)
            {
                MenusManager.Instance.ChangeMenu(eMenu.NicknameMenu);
            }
            else
            {
                PhotonNetwork.LocalPlayer.NickName = SaveManager.GetPlayerName();
                MenusManager.Instance.ChangeMenu(eMenu.MainMenu);
            }
        }
        _isSaveLoaded = false;
    }

    private void OnSaveLoaded()
    {
        _isSaveLoaded = true;
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        GroupManagementMenusManager.Instance.UpdateRoomsList(roomList);
    }

    public override void OnJoinedRoom()
    {
        if (PhotonNetwork.LocalPlayer.IsMasterClient)
            GroupMenuManager.Instance.InitializeRoomButtonsForMasterClient();

        GroupMenuManager.Instance.OnLocalPlayerJoinedRoom();
    }
}
