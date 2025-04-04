using Photon.Pun;
using System;
using System.Collections.Generic;
using UnityEngine;

public class MenusManager : MonoBehaviour
{
    public static MenusManager Instance { get; private set; }

    [Serializable]
    private struct MenuObjectByEnumItem
    {
        public eMenu EnumItem;
        public GameObject MenuParentObject;
    }

    [Header("General")]
    [SerializeField] private List<MenuObjectByEnumItem> _menuObjectEnumElements;
    private eMenu _currentMenu;

    private void Awake()
    {
        SaveManager.SaveLoadedEvent += OnSaveLoaded;

        if (Instance == null)
            Instance = this;
    }

    private void Start()
    {
        ChangeMenu(eMenu.LoadingScreen);
        SaveManager.GetSave();
    }

    private void OnSaveLoaded()
    {
        if (MenusOnlineManager.Instance.IsClientConnectedToMaster)
        {
            if (SaveManager.IsSaveBlank)
            {
                ChangeMenu(eMenu.NicknameMenu);
            }
            else
            {
                PhotonNetwork.LocalPlayer.NickName = SaveManager.GetPlayerName();
                ChangeMenu(eMenu.MainMenu);
            }
        }
    }

    public void ChangeMenu(eMenu nextMenu)
    {
        foreach (var item in _menuObjectEnumElements)
            item.MenuParentObject.SetActive(item.EnumItem == nextMenu);
        _currentMenu = nextMenu;
    }

    #region ON BUTTON CLICK METHODS

    public void OnGroupsMenuButtonClicked()
    {
        ChangeMenu(eMenu.GroupsMenu);
    }

    public void OnRulesMenuButtonClicked()
    {
        ChangeMenu(eMenu.RulesMenu);
    }

    public void OnOptionsMenuButtonClicked()
    {
        ChangeMenu(eMenu.OptionsMenu);
    }

    public void OnCreditsMenuButtonClicked()
    {
        ChangeMenu(eMenu.CreditsMenu);
    }

    public void OnReturnToMainMenuButtonClicked()
    {
        ChangeMenu(eMenu.MainMenu);
    }

    #endregion
}

[Serializable]
public enum eMenu
{
    LoadingScreen,
    NicknameMenu,
    MainMenu,
    GroupsMenu,
    GroupMenu,
    RulesMenu,
    OptionsMenu,
    CreditsMenu
}
