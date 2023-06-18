// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mediapipe.Unity.Dyspra
{
  public class MyMediaPipeSolution : MyImageSourceSolution<MyMediaPipeGraph>
  {
    private Texture2D _outputTexture;
    public List<NormalizedLandmarkList> handLandmarks { get; private set; }
    public List<ClassificationList> handedness { get; private set; }
    public List<Detection> palmLandmarks { get; private set; }

    public override void SetupScreen(ImageSource imageSource)
    {
      if (screen == null)
      {
        return;
      }
      // NOTE: The screen will be resized later, keeping the aspect ratio.
      screen.Resize(imageSource.textureWidth, imageSource.textureHeight);
      screen.Rotate(imageSource.rotation.Reverse());

      // Setup output texture
      if (graphRunner.configType == GraphRunner.ConfigType.OpenGLES)
      {
        if (textureFramePool.TryGetTextureFrame(out var textureFrame))
        {
          textureFrame.RemoveAllReleaseListeners();
          graphRunner.SetupOutputPacket(textureFrame);

          screen.texture = Texture2D.CreateExternalTexture(textureFrame.width, textureFrame.height, textureFrame.format, false, false, textureFrame.GetNativeTexturePtr());
        }
        else
        {
          throw new InternalException("Failed to get the output texture");
        }
      }
      else
      {
        _outputTexture = new Texture2D(imageSource.textureWidth, imageSource.textureHeight, TextureFormat.RGBA32, false);
        screen.texture = _outputTexture;
      }
    }

    protected override void OnStartRun()
    {
      // Do nothing
    }

    protected override void AddTextureFrameToInputStream(TextureFrame textureFrame)
    {
      graphRunner.AddTextureFrameToInputStream(textureFrame);
    }

    protected override void RenderCurrentFrame(TextureFrame textureFrame)
    {
      // Do nothing because the screen will be updated later in `DrawNow`. 
    }

    protected override IEnumerator WaitForNextValue()
    {
      if (graphRunner.configType == GraphRunner.ConfigType.OpenGLES)
      {
        yield break;
      }

      ImageFrame outputVideo = null;
      List<NormalizedLandmarkList> _handLandmarks = null;
      List<ClassificationList> _handedness = null;
      List<Detection> _palmLandmarks = null;

      if (runningMode == RunningMode.Sync)
      {
        var _ = graphRunner.TryGetNextVideo(out outputVideo, true);
        var __ = graphRunner.TryGetNextHandLandmarks(out _handLandmarks, true);
        var ___ = graphRunner.TryGetNextHandedness(out _handedness, true);
        var ____ = graphRunner.TryGetNextPalm(out _palmLandmarks, true);

      }
      else if (runningMode == RunningMode.NonBlockingSync)
      {
        yield return new WaitUntil(() => graphRunner.TryGetNextVideo(out outputVideo, false));
        yield return new WaitUntil(() => graphRunner.TryGetNextHandLandmarks(out _handLandmarks, false));
        yield return new WaitUntil(() => graphRunner.TryGetNextHandedness(out _handedness, false));
        yield return new WaitUntil(() => graphRunner.TryGetNextPalm(out _palmLandmarks, false));
      }
      handLandmarks = _handLandmarks;
      handedness = _handedness;
      palmLandmarks = _palmLandmarks;



      // debug log hand landmarks
      // if (handLandmarks != null)
      // {
      //   foreach (var handLandmark in handLandmarks)
      //   {
      //     foreach (var landmark in handLandmark.Landmark)
      //     {
      //       Debug.Log(landmark.X + " " + landmark.Y + " " + landmark.Z);
      //     }
      //   }
      // }

      // debug log handedness
      // if (handedness != null)
      // {
      //   foreach (var hand in handedness)
      //   {
      //     foreach (var classification in hand.Classification)
      //     {
      //       Debug.Log(classification.Index + " " + classification.Score);
      //     }
      //   }
      // }

      // debug log palm landmarks
      if (palmLandmarks != null)
      {
        Debug.Log("palmLandmarks count: " + palmLandmarks.Count);
      }
      DrawNow(outputVideo);
    }

    private void DrawNow(ImageFrame imageFrame)
    {
      if (imageFrame != null && _outputTexture != null)
      {
        _outputTexture.LoadRawTextureData(imageFrame.MutablePixelData(), imageFrame.PixelDataSize());
        _outputTexture.Apply();
      }
    }
  }
}
