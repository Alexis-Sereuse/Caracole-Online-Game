using UnityEngine;

[CreateAssetMenu(fileName = "GameInfo", menuName = "AddressableSingleton/GameInfo", order = 1)]
public class GameInfo : AddressableSingleton<GameInfo>
{
    [field: SerializeField] public string GameVersion { get; private set; }

    private const string PATH = "Assets/ScriptableObjects/GameInfo.asset";
    public static void Init() => Load(PATH);
    public static GameInfo Instance => GetInstance(PATH);
}
