// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using System.Collections;
using System.IO;
using UnityEngine;

#if UNITY_ANDROID
using UnityEngine.Android;
#endif

namespace Mediapipe.Unity.Dyspra
{
  public class MyBootstrap : MonoBehaviour
  {
    [System.Serializable]
    public enum AssetLoaderType
    {
      StreamingAssets,
      AssetBundle,
      Local,
    }

    private const string _TAG = nameof(Bootstrap);

    [SerializeField] public ImageSourceType _defaultImageSource = ImageSourceType.WebCamera;
    [SerializeField] public InferenceMode _preferableInferenceMode = InferenceMode.GPU;
    [SerializeField] public AssetLoaderType _assetLoaderType = AssetLoaderType.StreamingAssets;
    [SerializeField] public bool _enableGlog = true;

    public InferenceMode inferenceMode { get; private set; }
    public bool isFinished { get; private set; }
    private bool _isGlogInitialized;

    private void OnEnable()
    {
      var _ = StartCoroutine(Init());
    }

    private IEnumerator Init()
    {
      Logger.SetLogger(new MemoizedLogger(100));
      Logger.MinLogLevel = Logger.LogLevel.Debug;

      Protobuf.SetLogHandler(Protobuf.DefaultLogHandler);

      Logger.LogInfo(_TAG, "Setting global flags...");
      GlobalConfigManager.SetFlags();

      if (_enableGlog)
      {
        if (Glog.LogDir != null)
        {
          if (!Directory.Exists(Glog.LogDir))
          {
            Directory.CreateDirectory(Glog.LogDir);
          }
          Logger.LogVerbose(_TAG, $"Glog will output files under {Glog.LogDir}");
        }
        Glog.Initialize("MediaPipeUnityPlugin");
        _isGlogInitialized = true;
      }

      Logger.LogInfo(_TAG, "Initializing AssetLoader...");
      switch (_assetLoaderType)
      {
        case AssetLoaderType.AssetBundle:
          {
            MyAssetLoader.Provide(new AssetBundleResourceManager("mediapipe"));
            break;
          }
        case AssetLoaderType.StreamingAssets:
          {
            MyAssetLoader.Provide(new StreamingAssetsResourceManager());
            break;
          }
        case AssetLoaderType.Local:
          {
#if UNITY_EDITOR
            MyAssetLoader.Provide(new LocalResourceManager());
            break;
#else
            Logger.LogError("LocalResourceManager is only supported on UnityEditor");
            yield break;
#endif
          }
        default:
          {
            Logger.LogError($"AssetLoaderType is unknown: {_assetLoaderType}");
            yield break;
          }
      }

      DecideInferenceMode();
      if (inferenceMode == InferenceMode.GPU)
      {
        Logger.LogInfo(_TAG, "Initializing GPU resources...");
        yield return GpuManager.Initialize();

        if (!GpuManager.IsInitialized)
        {
          Logger.LogWarning("If your native library is built for CPU, change 'Preferable Inference Mode' to CPU from the Inspector Window for Bootstrap");
        }
      }

      Logger.LogInfo(_TAG, "Preparing ImageSource...");

#if UNITY_ANDROID
        if (!Permission.HasUserAuthorizedPermission(Permission.Camera))
        {
          Permission.RequestUserPermission(Permission.Camera);
          yield return new WaitForSeconds(0.1f);
        }
#elif UNITY_IOS
        if (!Application.HasUserAuthorization(UserAuthorization.WebCam)) {
          yield return Application.RequestUserAuthorization(UserAuthorization.WebCam);
        }
#endif

#if UNITY_ANDROID
        if (!Permission.HasUserAuthorizedPermission(Permission.Camera))
        {
          Logger.LogWarning(_TAG, "Not permitted to use Camera");
          yield break;
        }
#elif UNITY_IOS
        if (!Application.HasUserAuthorization(UserAuthorization.WebCam)) {
          Logger.LogWarning(_TAG, "Not permitted to use WebCam");
          yield break;
        }
#endif
      MyImageSourceProvider.ImageSource = GetImageSource(_defaultImageSource);


      UnityEngine.Debug.Log("_defaultImageSource: " + _defaultImageSource);
      UnityEngine.Debug.Log("MyImageSourceProvider.ImageSource: " + MyImageSourceProvider.ImageSource);

      isFinished = true;
    }

    public MyImageSource GetImageSource(ImageSourceType imageSourceType)
    {
      switch (imageSourceType)
      {
        case ImageSourceType.WebCamera:
          {
            var webCamSource = new MyWebCamSource(WebCamTexture.devices);
            // UnityEngine.Debug.Log($"start MyWebCamSource inside bootstrat");
            // webCamSource.availableSources = WebCamTexture.devices;
            // UnityEngine.Debug.Log($"availableSources: {availableSources}");

            // if (availableSources != null && availableSources.Length > 0)
            // {
            //   webCamSource.webCamDevice = availableSources[0];
            // }
            // UnityEngine.Debug.Log($"webCamDevice: {webCamDevice}");
            // // webCamSource.Initialize(webCamDevice.name);


            return webCamSource;
          }
        case ImageSourceType.Image:
          {
            return new MyStaticImageSource();
          }
        case ImageSourceType.Unknown:
        default:
          {
            throw new System.ArgumentException($"Unsupported source type: {imageSourceType}");
          }
      }
    }

    private void DecideInferenceMode()
    {
#if UNITY_EDITOR_OSX || UNITY_EDITOR_WIN
      if (_preferableInferenceMode == InferenceMode.GPU) {
        Logger.LogWarning(_TAG, "Current platform does not support GPU inference mode, so falling back to CPU mode");
      }
      inferenceMode = InferenceMode.CPU;
#else
      inferenceMode = _preferableInferenceMode;
#endif
    }

    private void OnApplicationQuit()
    {
      GpuManager.Shutdown();

      if (_isGlogInitialized)
      {
        Glog.Shutdown();
      }

      Protobuf.ResetLogHandler();
      Logger.SetLogger(null);
    }
  }
}
