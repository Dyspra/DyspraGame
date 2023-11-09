using System.Collections;
using System.Threading.Tasks;

namespace Mediapipe.Unity
{
  public static class MyAssetLoader
  {
    private static ResourceManager _ResourceManager;

    public static void Provide(ResourceManager manager)
    {
      _ResourceManager = manager;
    }

    public static async Task PrepareAssetAsync(string name, string uniqueKey, bool overwrite = false)
    {
      if (_ResourceManager == null)
      {
#if UNITY_EDITOR
        Logger.LogWarning("ResourceManager is not provided, so default LocalResourceManager will be used");
        _ResourceManager = new LocalResourceManager();
#else
        throw new System.InvalidOperationException("ResourceManager is not provided");
#endif
      }
      await Task.Run(() => _ResourceManager.PrepareAssetAsync(name, uniqueKey, overwrite));
    }

    public static async Task PrepareAssetAsync(string name, bool overwrite = false)
    {
      await PrepareAssetAsync(name, name, overwrite);
    }
  }
}
