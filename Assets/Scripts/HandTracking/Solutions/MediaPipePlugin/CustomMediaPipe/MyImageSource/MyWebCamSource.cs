// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using UnityEngine;

#if UNITY_ANDROID
using UnityEngine.Android;
#endif

namespace Mediapipe.Unity.Dyspra
{
  public class MyWebCamSource : MyImageSource
  {
    [Tooltip("For the default resolution, the one whose width is closest to this value will be chosen")]
    [SerializeField] private int _preferableDefaultWidth = 1280;

    private const string _TAG = nameof(MyWebCamSource);

    [SerializeField]
    private ResolutionStruct[] _defaultAvailableResolutions = new ResolutionStruct[] {
      new ResolutionStruct(176, 144, 30),
      new ResolutionStruct(320, 240, 30),
      new ResolutionStruct(424, 240, 30),
      new ResolutionStruct(640, 360, 30),
      new ResolutionStruct(640, 480, 30),
      new ResolutionStruct(848, 480, 30),
      new ResolutionStruct(960, 540, 30),
      new ResolutionStruct(1280, 720, 30),
      new ResolutionStruct(1600, 896, 30),
      new ResolutionStruct(1920, 1080, 30),
    };

    private static readonly object _PermissionLock = new object();

    private WebCamTexture _webCamTexture;
    private WebCamTexture webCamTexture
    {
      get => _webCamTexture;
      set
      {
        if (_webCamTexture != null)
        {
          _webCamTexture.Stop();
        }
        _webCamTexture = value;
      }
    }

    public override int textureWidth => !isPrepared ? 0 : webCamTexture.width;
    public override int textureHeight => !isPrepared ? 0 : webCamTexture.height;

    public override bool isVerticallyFlipped => isPrepared && webCamTexture.videoVerticallyMirrored;
    public override bool isFrontFacing => isPrepared && (webCamDevice is WebCamDevice valueOfWebCamDevice) && valueOfWebCamDevice.isFrontFacing;
    public override RotationAngle rotation => !isPrepared ? RotationAngle.Rotation0 : 0; //: (RotationAngle)webCamTexture.videoRotationAngle;

    private WebCamDevice? _webCamDevice;
    private WebCamDevice? webCamDevice
    {
      get => _webCamDevice;
      set
      {
        if (_webCamDevice is WebCamDevice valueOfWebCamDevice)
        {
          if (value is WebCamDevice valueOfValue && valueOfValue.name == valueOfWebCamDevice.name)
          {
            // not changed
            return;
          }
        }
        else if (value == null)
        {
          // not changed
          return;
        }
        _webCamDevice = value;
        resolution = GetDefaultResolution();
      }
    }
    public override string sourceName => (webCamDevice is WebCamDevice valueOfWebCamDevice) ? valueOfWebCamDevice.name : null;

    private WebCamDevice[] _availableSources;
    private WebCamDevice[] availableSources
    {
      get
      {
        if (_availableSources == null)
        {
          _availableSources = WebCamTexture.devices;
        }

        return _availableSources;
      }
      set => _availableSources = value;
    }

    public override string[] sourceCandidateNames => availableSources?.Select(device => device.name).ToArray();

#pragma warning disable IDE0025
    public override ResolutionStruct[] availableResolutions
    {
      get
      {
#if (UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR
        if (webCamDevice is WebCamDevice valueOfWebCamDevice) {
          return valueOfWebCamDevice.availableResolutions.Select(resolution => new ResolutionStruct(resolution)).ToArray();
        }
#endif
        return webCamDevice == null ? null : _defaultAvailableResolutions;
      }
    }
#pragma warning restore IDE0025

    public override bool isPrepared => webCamTexture != null;
    public override bool isPlaying => webCamTexture != null && webCamTexture.isPlaying;
    private bool _isInitialized;

    public MyWebCamSource(WebCamDevice[] availableSources)
    {
      this.availableSources = availableSources;
      if (availableSources != null && availableSources.Length > 0)
      {
        webCamDevice = availableSources[0];
      }        

      InitializeWebCamTexture();
      UnityEngine.Debug.Log($"MyWebCamSource Play, webCamTexture.Play");
      webCamTexture.Play();
      // WaitForWebCamTexture().Wait();
    }

    // private async Task Start()
    // {
    //   UnityEngine.Debug.Log($"start MyWebCamSource");


    //   availableSources = WebCamTexture.devices;

    //   if (availableSources != null && availableSources.Length > 0)
    //   {
    //     webCamDevice = availableSources[0];
    //   }

    //   _isInitialized = true;
    // }


    public override void SelectSource(int sourceId)
    {
      if (sourceId < 0 || sourceId >= availableSources.Length)
      {
        throw new ArgumentException($"Invalid source ID: {sourceId}");
      }

      webCamDevice = availableSources[sourceId];
    }

    public override async Task Play()
    {
      _isInitialized = true;
      await Task.Run(() =>
      {
        UnityEngine.Debug.Log($"MyWebCamSource Play, spinwait");
        SpinWait.SpinUntil(() => _isInitialized);
        UnityEngine.Debug.Log($"MyWebCamSource Play, spinwait done");

        UnityEngine.Debug.Log($"MyWebCamSource Play, init webCamTexture");

        UnityEngine.Debug.Log($"MyWebCamSource Play, WaitForWebCamTexture");
      });
    }

    public override async Task Resume()
    {
      await Task.Run(() =>
      {
        if (!isPrepared)
        {
          throw new InvalidOperationException("WebCamTexture is not prepared yet");
        }
        if (!webCamTexture.isPlaying)
        {
          webCamTexture.Play();
        }
        WaitForWebCamTexture().Wait();
      });
    }

    public override void Pause()
    {
      if (isPlaying)
      {
        webCamTexture.Pause();
      }
    }

    public override void Stop()
    {
      if (webCamTexture != null)
      {
        webCamTexture.Stop();
      }
      webCamTexture = null;
    }

    public override Texture GetCurrentTexture()
    {
      return webCamTexture;
    }

    private ResolutionStruct GetDefaultResolution()
    {
      var resolutions = availableResolutions;
      return resolutions == null || resolutions.Length == 0 ? new ResolutionStruct() : resolutions.OrderBy(resolution => resolution, new ResolutionStructComparer(_preferableDefaultWidth)).First();
    }

    private void InitializeWebCamTexture()
    {
      UnityEngine.Debug.Log($"InitializeWebCamTexture Stop");
      Stop();
      UnityEngine.Debug.Log($"InitializeWebCamTexture webCamDevice: {webCamDevice}");
      if (webCamDevice is WebCamDevice valueOfWebCamDevice)
      {
        UnityEngine.Debug.Log($"InitializeWebCamTexture webCamTexture1: {webCamTexture}");
        webCamTexture = new WebCamTexture(valueOfWebCamDevice.name, resolution.width, resolution.height, (int)resolution.frameRate);
        UnityEngine.Debug.Log($"InitializeWebCamTexture webCamTexture2: {webCamTexture}");
        return;
      }
      throw new InvalidOperationException("Cannot initialize WebCamTexture because WebCamDevice is not selected");
    }

    private async Task WaitForWebCamTexture()
    {
      const int timeoutFrame = 5000;
      var count = 0;
      Logger.LogVerbose("Waiting for WebCamTexture to start");
      await Task.Run(() =>
      {
        while (count <= timeoutFrame && webCamTexture.width <= 16)
        {
          count++;
        }
      });


      if (webCamTexture.width <= 16)
      {
        throw new TimeoutException("Failed to start WebCam");
      }
      UnityEngine.Debug.Log($"WaitForWebCamTexture success");
    }

    private class ResolutionStructComparer : IComparer<ResolutionStruct>
    {
      private readonly int _preferableDefaultWidth;

      public ResolutionStructComparer(int preferableDefaultWidth)
      {
        _preferableDefaultWidth = preferableDefaultWidth;
      }

      public int Compare(ResolutionStruct a, ResolutionStruct b)
      {
        var aDiff = Mathf.Abs(a.width - _preferableDefaultWidth);
        var bDiff = Mathf.Abs(b.width - _preferableDefaultWidth);
        if (aDiff != bDiff)
        {
          return aDiff - bDiff;
        }
        if (a.height != b.height)
        {
          // prefer smaller height
          return a.height - b.height;
        }
        // prefer smaller frame rate
        return (int)(a.frameRate - b.frameRate);
      }
    }
  }
}
