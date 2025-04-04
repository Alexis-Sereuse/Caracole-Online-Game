using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RoomsInfo", menuName = "AddressableSingleton/RoomsInfo", order = 2)]
public class RoomsInfoScriptableObject : AddressableSingleton<RoomsInfoScriptableObject>
{
    private const string PATH = "Assets/ScriptableObjects/RoomsInfo.asset";
    public static RoomsInfoScriptableObject Instance => GetInstance(PATH);
    public static void Init() => Load(PATH);

    [field: SerializeField] public int DefaultMaximumPlayersCount { get; private set; }
    [field: SerializeField] public int GreatestMaximumPlayersCount { get; private set; }
    [field: SerializeField] public int SmallestMaximumPlayersCount { get; private set; }
    [field: SerializeField] public bool IsRoomPublicByDefault { get; private set; }
    [field: SerializeField] public bool IsGroupListDefaultGroupMenu { get; private set; }
}
