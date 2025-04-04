using TMPro;
using UnityEngine;

public class RoomInformationBanner : MonoBehaviour
{
    [SerializeField] private TMP_Text _roomNameText;
    [SerializeField] private TMP_Text _accessStatusText;
    [SerializeField] private TMP_Text _playersCountText;

    public void SetRoomName(string roomName)
    {
        _roomNameText.text = roomName;
    }

    public void SetAccessStatus(bool isPublic)
    {
        _accessStatusText.text = isPublic ? "Public" : "Private";
    }

    public void SetPlayersCount(int playersCount, int maximumPlayersCount)
    {
        _playersCountText.text = $"{playersCount} / {maximumPlayersCount}";
    }

    public void OnRoomInformationBannerClicked()
    {

    }
}
