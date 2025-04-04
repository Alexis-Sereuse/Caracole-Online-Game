using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class AddressableSingleton<T> : ScriptableObject
{
    private static T _instance;
    protected static T GetInstance(string path)
    {
        if (_instance == null)
        {
            if (_loadProcess.IsValid())
            {
                if (_loadProcess.IsDone)
                    _instance = _loadProcess.Result;
                else
                    _instance = _loadProcess.WaitForCompletion();
            }
            else
            {
                _instance = Addressables.LoadAssetAsync<T>(path).WaitForCompletion();
            }
        }

        return _instance;
    }

    private static AsyncOperationHandle<T> _loadProcess;

    public static void Load(string path)
    {
        _loadProcess = Addressables.LoadAssetAsync<T>(path);
    }
}
