// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

namespace Mediapipe.Unity.Dyspra
{
  public class MyMediaPipeSolution : MyImageSourceSolution<MyMediaPipeGraph>
  {
    private Texture2D _outputTexture;
    public List<NormalizedLandmarkList> handLandmarks { get; private set; }
    public List<LandmarkList> handWorldLandmarks { get; private set; }
    public List<ClassificationList> handedness { get; private set; }

    public Vector3[] LeftHandLandmarks { get; private set; } = new Vector3[21];
    public Vector3[] RightHandLandmarks { get; private set; } = new Vector3[21];

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
              List<LandmarkList> _handWorldLandmarks = null;
              List<ClassificationList> _handedness = null;

              if (runningMode == RunningMode.Sync)
              {
                var _ = graphRunner.TryGetNextVideo(out outputVideo, true);
                var __ = graphRunner.TryGetNextHandLandmarks(out _handLandmarks, true);
                var ___ = graphRunner.TryGetNextHandWorldLandmarks(out _handWorldLandmarks, true);
                var ____ = graphRunner.TryGetNextHandedness(out _handedness, true);

              }
              else if (runningMode == RunningMode.NonBlockingSync)
              {
                yield return new WaitUntil(() => graphRunner.TryGetNextVideo(out outputVideo, false));
                yield return new WaitUntil(() => graphRunner.TryGetNextHandLandmarks(out _handLandmarks, false));
                yield return new WaitUntil(() => graphRunner.TryGetNextHandWorldLandmarks(out _handWorldLandmarks, false));
                yield return new WaitUntil(() => graphRunner.TryGetNextHandedness(out _handedness, false));
              }
              handLandmarks = _handLandmarks;
              handWorldLandmarks = _handWorldLandmarks;
              handedness = _handedness;



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
                // UnityEngine.Debug.Log("handLandmarks count: " + handLandmarks.Count);
              // }

              // // debug log handedness
              // if (handedness != null)
              // {
              //   foreach (var hand in handedness)
              //   {
              //     foreach (var classification in hand.Classification)
              //     {
              //       Debug.Log(classification.Index + " " + classification.Score);
              //     }
              //   }
                // UnityEngine.Debug.Log("handedness count: " + handedness.Count);
                // // Detect if the hand is left or right or both
              //   if (handedness.Count == 2)
              //   {
              //     if (handedness[0].Classification[0].Index == 0 && handedness[1].Classification[0].Index == 1)
              //     {
              //       UnityEngine.Debug.Log("Left and Right");
              //     }
              //     else if (handedness[0].Classification[0].Index == 1 && handedness[1].Classification[0].Index == 0)
              //     {
              //       UnityEngine.Debug.Log("Right and Left");
              //     }
              //     else
              //     {
              //       UnityEngine.Debug.Log("Both");
              //     }
              //   }
              //   else if (handedness.Count == 1)
              //   {
              //     if (handedness[0].Classification[0].Index == 0)
              //     {
              //       UnityEngine.Debug.Log("Left");
              //     }
              //     else if (handedness[0].Classification[0].Index == 1)
              //     {
              //       UnityEngine.Debug.Log("Right");
              //     }
              //   }
              // }

              if (handLandmarks != null && handedness != null)
              {
                Task.Run(() => ApplyHandLandmarks(handedness, handWorldLandmarks, 0));
                Task.Run(() => ApplyHandLandmarks(handedness, handWorldLandmarks, 1));
              }
              DrawNow(outputVideo);
            }

            private void ApplyHandLandmarks(List<ClassificationList> handedness, List<LandmarkList> handLandmarks, int handIndex)
            {
              if (handedness[handIndex].Classification.Count > 0 && handLandmarks[handIndex].Landmark.Count > 0)
              {
                var handLandmarksArray = new Vector3[21];

                for (int landmarkIndex = 0; landmarkIndex < handLandmarks[handIndex].Landmark.Count; landmarkIndex++)
                {
                  // Add the landmark position to the hand landmarks array
                  handLandmarksArray[landmarkIndex] = new Vector3(handLandmarks[handIndex].Landmark[landmarkIndex].X, handLandmarks[handIndex].Landmark[landmarkIndex].Y, handLandmarks[handIndex].Landmark[landmarkIndex].Z);
                }

                if (handedness[handIndex].Classification[0].Index == 0) // Left hand
                {
                  LeftHandLandmarks = handLandmarksArray;
                }
                else if (handedness[handIndex].Classification[0].Index == 1) // Right hand
                {
                  RightHandLandmarks = handLandmarksArray;
                }
              }
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
