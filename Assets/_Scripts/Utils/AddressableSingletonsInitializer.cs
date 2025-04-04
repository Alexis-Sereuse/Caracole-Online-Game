using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.AddressableAssets.ResourceLocators;
using UnityEngine.ResourceManagement.AsyncOperations;

public class AddressableSingletonsInitializer : MonoBehaviour
{
    private void Awake()
    {
        var initializationOperation = Addressables.InitializeAsync();
        initializationOperation.Completed += InitAllAddressables;
        DontDestroyOnLoad(gameObject);
    }

    private static void InitAllAddressables(AsyncOperationHandle<IResourceLocator> handle)
    {
        GameInfo.Init();
        RoomsInfoScriptableObject.Init();
    }
}
