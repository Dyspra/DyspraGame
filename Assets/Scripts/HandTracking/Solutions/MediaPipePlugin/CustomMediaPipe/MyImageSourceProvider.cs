// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

namespace Mediapipe.Unity.Dyspra
{
  public static class MyImageSourceProvider
  {
    private static MyImageSource _ImageSource;
    public static MyImageSource ImageSource
    {
      get {
        UnityEngine.Debug.Log("[S] Getting ImageSource");
        UnityEngine.Debug.Log($"[S] ImageSource: {_ImageSource}");
        return _ImageSource;
      }
      set
      {
        // if (value != null && !value.enabled)
        // {
        //   value.enabled = true;
        // }
        _ImageSource = value;
      }
    }

    public static MyImageSourceType CurrentSourceType
    {
      get
      {
        if (_ImageSource is MyWebCamSource)
        {
          return MyImageSourceType.WebCamera;
        }
        if (_ImageSource is MyStaticImageSource)
        {
          return MyImageSourceType.Image;
        }
        return MyImageSourceType.Unknown;
      }
    }
  }
}
