// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Mediapipe.Unity.Dyspra
{
  public class MyMediaPipeGraph : GraphRunner
  {
    public int maxNumHands = 2;
    public enum ModelComplexity
    {
      Lite = 0,
      Full = 1,
    }

    public ModelComplexity modelComplexity = ModelComplexity.Lite;

    public event EventHandler<OutputEventArgs<ImageFrame>> OnOutput
    {
      add => _outputVideoStream.AddListener(value);
      remove => _outputVideoStream.RemoveListener(value);
    }

    public event EventHandler<OutputEventArgs<List<NormalizedLandmarkList>>> OnHandLandmarks
    {
      add => _handLandmarksStream.AddListener(value);
      remove => _handLandmarksStream.RemoveListener(value);
    }

    public event EventHandler<OutputEventArgs<List<ClassificationList>>> OnHandedness
    {
      add => _handednessStream.AddListener(value);
      remove => _handednessStream.RemoveListener(value);
    }

    private const string _InputStreamName = "input_video";

    private GpuBufferPacket _outputGpuBufferPacket;
    private string _destinationBufferName;
    private TextureFrame _destinationTexture;

    private const string _OutputVideoStreamName = "output_video";
    private OutputStream<ImageFramePacket, ImageFrame> _outputVideoStream;


    private const string _HandLandmarksStreamName = "multi_hand_landmarks";
    public OutputStream<NormalizedLandmarkListVectorPacket, List<NormalizedLandmarkList>> _handLandmarksStream { get; private set; }

    private const string _HandWorldLandmarksStreamName = "multi_hand_world_landmarks";
    private OutputStream<LandmarkListVectorPacket, List<LandmarkList>> _handWorldLandmarksStream;

    private const string _HandednessStreamName = "multi_handedness";
    public OutputStream<ClassificationListVectorPacket, List<ClassificationList>> _handednessStream { get; private set; }


    public override void StartRun(ImageSource imageSource)
    {
      if (configType != ConfigType.OpenGLES)
      {
        _outputVideoStream.StartPolling().AssertOk();
        _handLandmarksStream.StartPolling().AssertOk();
        _handWorldLandmarksStream.StartPolling().AssertOk();
        _handednessStream.StartPolling().AssertOk();
      }
      StartRun(BuildSidePacket(imageSource));
    }

    public override void Stop()
    {
      _outputVideoStream?.Close();
      _outputVideoStream = null;

      _handLandmarksStream?.Close();
      _handLandmarksStream = null;

      _handWorldLandmarksStream?.Close();
      _handWorldLandmarksStream = null;

      _handednessStream?.Close();
      _handednessStream = null;

      base.Stop();
    }


    public override IEnumerator Initialize(RunningMode runningMode)
    {
      // if (runningMode == RunningMode.Async)
      // {
      //   throw new ArgumentException("Asynchronous mode is not supported");
      // }
      return base.Initialize(runningMode);
    }

    public void SetupOutputPacket(TextureFrame textureFrame)
    {
      if (configType != ConfigType.OpenGLES)
      {
        throw new InvalidOperationException("This method is only supported for OpenGL ES");
      }
      _destinationTexture = textureFrame;
      _outputGpuBufferPacket = new GpuBufferPacket(_destinationTexture.BuildGpuBuffer(GpuManager.GlCalculatorHelper.GetGlContext()));
    }

    public void AddTextureFrameToInputStream(TextureFrame textureFrame)
    {
      AddTextureFrameToInputStream(_InputStreamName, textureFrame);
    }

    public bool TryGetNextVideo(out ImageFrame outputVideo, bool allowBlock = true)
    {
      return TryGetNext(_outputVideoStream, out outputVideo, allowBlock, GetCurrentTimestampMicrosec());
    }

    public bool TryGetNextHandLandmarks(out List<NormalizedLandmarkList> handLandmarks, bool allowBlock = true)
    {
      return TryGetNext(_handLandmarksStream, out handLandmarks, allowBlock, GetCurrentTimestampMicrosec());
    }

    public bool TryGetNextHandWorldLandmarks(out List<LandmarkList> handWorldLandmarks, bool allowBlock = true)
    {
      return TryGetNext(_handWorldLandmarksStream, out handWorldLandmarks, allowBlock, GetCurrentTimestampMicrosec());
    }

    public bool TryGetNextHandedness(out List<ClassificationList> handedness, bool allowBlock = true)
    {
      return TryGetNext(_handednessStream, out handedness, allowBlock, GetCurrentTimestampMicrosec());
    }

    protected override Status ConfigureCalculatorGraph(CalculatorGraphConfig config)
    {
      if (configType == ConfigType.OpenGLES)
      {
        var sinkNode = config.Node.Last((node) => node.Calculator == "GlScalerCalculator");
        _destinationBufferName = Tool.GetUnusedSidePacketName(config, "destination_buffer");

        sinkNode.InputSidePacket.Add($"DESTINATION:{_destinationBufferName}");
      }

      if (runningMode == RunningMode.NonBlockingSync)
      {
        _outputVideoStream = new OutputStream<ImageFramePacket, ImageFrame>(
            calculatorGraph, _OutputVideoStreamName, config.AddPacketPresenceCalculator(_OutputVideoStreamName), timeoutMicrosec);
        
        _handLandmarksStream = new OutputStream<NormalizedLandmarkListVectorPacket, List<NormalizedLandmarkList>>(
            calculatorGraph, _HandLandmarksStreamName, config.AddPacketPresenceCalculator(_HandLandmarksStreamName), timeoutMicrosec);

        _handWorldLandmarksStream = new OutputStream<LandmarkListVectorPacket, List<LandmarkList>>(
            calculatorGraph, _HandWorldLandmarksStreamName, config.AddPacketPresenceCalculator(_HandWorldLandmarksStreamName), timeoutMicrosec);

        _handednessStream = new OutputStream<ClassificationListVectorPacket, List<ClassificationList>>(
            calculatorGraph, _HandednessStreamName, config.AddPacketPresenceCalculator(_HandednessStreamName), timeoutMicrosec);
        
      }
      else
      {
        _outputVideoStream = new OutputStream<ImageFramePacket, ImageFrame>(calculatorGraph, _OutputVideoStreamName, true, timeoutMicrosec);
        _handLandmarksStream = new OutputStream<NormalizedLandmarkListVectorPacket, List<NormalizedLandmarkList>>(calculatorGraph, _HandLandmarksStreamName, true, timeoutMicrosec);
        _handWorldLandmarksStream = new OutputStream<LandmarkListVectorPacket, List<LandmarkList>>(calculatorGraph, _HandWorldLandmarksStreamName, true, timeoutMicrosec);
        _handednessStream = new OutputStream<ClassificationListVectorPacket, List<ClassificationList>>(calculatorGraph, _HandednessStreamName, true, timeoutMicrosec);
      }

      return calculatorGraph.Initialize(config);
    }

    protected override IList<WaitForResult> RequestDependentAssets()
    {
      return new List<WaitForResult> {
        WaitForHandLandmarkModel(),
        WaitForPalmsDetectionModel(),
        WaitForAsset("hand_recrop.bytes"),
        WaitForAsset("handedness.txt"),
      };
    }

    private WaitForResult WaitForHandLandmarkModel()
    {
      return modelComplexity switch
      {
          ModelComplexity.Lite => WaitForAsset("hand_landmark_lite.bytes"),
          ModelComplexity.Full => WaitForAsset("hand_landmark_full.bytes"),
          _ => throw new InternalException($"Invalid model complexity: {modelComplexity}"),
      };
    }

    private WaitForResult WaitForPalmsDetectionModel()
    {
      return modelComplexity switch
      {
          ModelComplexity.Lite => WaitForAsset("palm_detection_lite.bytes"),
          ModelComplexity.Full => WaitForAsset("palm_detection_full.bytes"),
          _ => throw new InternalException($"Invalid model complexity: {modelComplexity}"),
      };
    }

    private SidePacket BuildSidePacket(ImageSource imageSource)
    {
      var sidePacket = new SidePacket();

      SetImageTransformationOptions(sidePacket, imageSource, true);
      sidePacket.Emplace("output_rotation", new IntPacket((int)imageSource.rotation));
      sidePacket.Emplace("model_complexity", new IntPacket((int)modelComplexity));
      sidePacket.Emplace("num_hands", new IntPacket(maxNumHands));
      sidePacket.Emplace("use_prev_landmarks", new BoolPacket(true));

      if (configType == ConfigType.OpenGLES)
      {
        sidePacket.Emplace(_destinationBufferName, _outputGpuBufferPacket);
      }

      return sidePacket;
    }
  }
}
