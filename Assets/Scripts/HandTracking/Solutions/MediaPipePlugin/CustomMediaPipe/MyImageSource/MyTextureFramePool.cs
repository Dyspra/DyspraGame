// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mediapipe.Unity
{
  public class MyTextureFramePool
  {
    private const string _TAG = nameof(MyTextureFramePool);

    [SerializeField] private int _poolSize = 10;

    private readonly object _formatLock = new object();
    private int _textureWidth = 0;
    private int _textureHeight = 0;
    private TextureFormat _format = TextureFormat.RGBA32;

    private Queue<MyTextureFrame> _availableTextureFrames;
    /// <remarks>
    ///   key: TextureFrame's instance ID
    /// </remarks>
    private Dictionary<Guid, MyTextureFrame> _textureFramesInUse;

    /// <returns>
    ///   The total number of texture frames in the pool.
    /// </returns>
    public int frameCount
    {
      get
      {
        var availableTextureFramesCount = _availableTextureFrames == null ? 0 : _availableTextureFrames.Count;
        var textureFramesInUseCount = _textureFramesInUse == null ? 0 : _textureFramesInUse.Count;

        return availableTextureFramesCount + textureFramesInUseCount;
      }
    }

    public MyTextureFramePool()
    {
      _availableTextureFrames = new Queue<MyTextureFrame>(_poolSize);
      _textureFramesInUse = new Dictionary<Guid, MyTextureFrame>();
    }

    ~MyTextureFramePool()
    {
      lock (((ICollection)_availableTextureFrames).SyncRoot)
      {
        _availableTextureFrames.Clear();
        _availableTextureFrames = null;
      }

      lock (((ICollection)_textureFramesInUse).SyncRoot)
      {
        foreach (var textureFrame in _textureFramesInUse.Values)
        {
          // textureFrame.OnRelease.RemoveListener(OnTextureFrameRelease);
        }
        _textureFramesInUse.Clear();
        _textureFramesInUse = null;
      }
    }

    public void ResizeTexture(int textureWidth, int textureHeight, TextureFormat format)
    {
      lock (_formatLock)
      {
        _textureWidth = textureWidth;
        _textureHeight = textureHeight;
        _format = format;
      }
    }

    public void ResizeTexture(int textureWidth, int textureHeight)
    {
      ResizeTexture(textureWidth, textureHeight, _format);
    }

    public bool TryGetTextureFrame(out MyTextureFrame outFrame)
    {
      MyTextureFrame nextFrame = null;

      UnityEngine.Debug.Log("[L] TryGetTextureFrame");
      lock (((ICollection)_availableTextureFrames).SyncRoot)
      {
        UnityEngine.Debug.Log("[L] TryGetTextureFrame 2");
        if (_poolSize <= frameCount)
        {
          UnityEngine.Debug.Log("[L] TryGetTextureFrame 3");
          while (_availableTextureFrames.Count > 0)
          {
            UnityEngine.Debug.Log("[L] TryGetTextureFrame 4");
            var textureFrame = _availableTextureFrames.Dequeue();

            if (!IsStale(textureFrame))
            {
              UnityEngine.Debug.Log("[L] TryGetTextureFrame 5");
              nextFrame = textureFrame;
              break;
            }
          }
        }
        else
        {
          UnityEngine.Debug.Log("[L] TryGetTextureFrame 6");
          nextFrame = CreateNewTextureFrame();
        }
      }
      UnityEngine.Debug.Log("[L] TryGetTextureFrame 7");

      if (nextFrame == null)
      {
        outFrame = null;
        return false;
      }

      UnityEngine.Debug.Log("[L] TryGetTextureFrame 8");
      lock (((ICollection)_textureFramesInUse).SyncRoot)
      {
        UnityEngine.Debug.Log("[L] TryGetTextureFrame 9");
        _textureFramesInUse.Add(nextFrame.GetInstanceID(), nextFrame);
      }

      UnityEngine.Debug.Log("[L] TryGetTextureFrame 10");
      nextFrame.WaitUntilReleased();
      UnityEngine.Debug.Log("[L] TryGetTextureFrame 11");
      outFrame = nextFrame;
      UnityEngine.Debug.Log("[L] TryGetTextureFrame 12");
      return true;
    }

    private void OnTextureFrameRelease(MyTextureFrame textureFrame)
    {
      lock (((ICollection)_textureFramesInUse).SyncRoot)
      {
        if (!_textureFramesInUse.Remove(textureFrame.GetInstanceID()))
        {
          // won't be run
          Logger.LogWarning(_TAG, "The released texture does not belong to the pool");
          return;
        }

        if (frameCount > _poolSize || IsStale(textureFrame))
        {
          return;
        }
        _availableTextureFrames.Enqueue(textureFrame);
      }
    }

    private bool IsStale(MyTextureFrame textureFrame)
    {
      lock (_formatLock)
      {
        return textureFrame.width != _textureWidth || textureFrame.height != _textureHeight;
      }
    }

    private MyTextureFrame CreateNewTextureFrame()
    {
      UnityEngine.Debug.Log("[L] CreateNewTextureFrame");
      UnityEngine.Debug.Log("[L] CreateNewTextureFrame _textureWidth: " + _textureWidth);
      UnityEngine.Debug.Log("[L] CreateNewTextureFrame _textureHeight: " + _textureHeight);
      UnityEngine.Debug.Log("[L] CreateNewTextureFrame _format: " + _format);
      var textureFrame = new MyTextureFrame(_textureWidth, _textureHeight, _format);
      // textureFrame.OnRelease.AddListener(OnTextureFrameRelease);

      UnityEngine.Debug.Log("[L] CreateNewTextureFrame 2");
      return textureFrame;
    }
  }
}
