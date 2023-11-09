// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using System.Collections;
using System.Threading.Tasks;
using UnityEngine;

namespace Mediapipe.Unity.Dyspra
{
  public abstract class MyImageSourceSolution<T> : MySolution where T : MyGraphRunner
  {
    [SerializeField] public Mediapipe.Unity.Screen screen;
    [SerializeField] public T graphRunner;
    [SerializeField] public TextureFramePool textureFramePool;

    private Task _task;

    public RunningMode runningMode;

    public long timeoutMillisec
    {
      get => graphRunner.timeoutMillisec;
      set => graphRunner.timeoutMillisec = value;
    }

    public override void Play()
    {
      UnityEngine.Debug.Log($"Current thread of play Solution: {System.Threading.Thread.CurrentThread.Name}");
      if (_task.Status == TaskStatus.Running)
      {
        Stop();
      }
      base.Play();
      _task = Task.Run(() => {
        Run();
      });
    }

    public override void Pause()
    {
      base.Pause();
      ImageSourceProvider.ImageSource.Pause();
    }

    public override void Resume()
    {
      base.Resume();
      Task.Run(() => {
        ImageSourceProvider.ImageSource.Resume();
      });
    }

    public override void Stop()
    {
      base.Stop();
      _task.Dispose();
      ImageSourceProvider.ImageSource.Stop();
      graphRunner.Stop();
    }

    private IEnumerator Run()
    {
      var graphInitRequest = graphRunner.WaitForInit(runningMode);
      var imageSource = ImageSourceProvider.ImageSource;

      yield return imageSource.Play();

      if (!imageSource.isPrepared)
      {
        Logger.LogError(TAG, "Failed to start ImageSource, exiting...");
        yield break;
      }

      // Use RGBA32 as the input format.
      // TODO: When using GpuBuffer, MediaPipe assumes that the input format is BGRA, so the following code must be fixed.
      textureFramePool.ResizeTexture(imageSource.textureWidth, imageSource.textureHeight, TextureFormat.RGBA32);
      SetupScreen(imageSource);

      yield return graphInitRequest;
      if (graphInitRequest.IsFaulted)
      {
        Logger.LogError(TAG, graphInitRequest.Exception.ToString());
        yield break;
      }

      OnStartRun();
      graphRunner.StartRun(imageSource);

      var waitWhilePausing = new WaitWhile(() => isPaused);

      while (true)
      {
        if (isPaused)
        {
          yield return waitWhilePausing;
        }

        if (!textureFramePool.TryGetTextureFrame(out var textureFrame))
        {
          yield return new WaitForEndOfFrame();
          continue;
        }

        // Copy current image to TextureFrame
        ReadFromImageSource(imageSource, textureFrame);
        AddTextureFrameToInputStream(textureFrame);
        yield return new WaitForEndOfFrame();

        if (runningMode.IsSynchronous())
        {
          RenderCurrentFrame(textureFrame);
          yield return WaitForNextValue();
        }
      }
    }

    public virtual void SetupScreen(ImageSource imageSource)
    {
      // NOTE: The screen will be resized later, keeping the aspect ratio.
      if (screen == null)
      {
        return;
      }
      screen.Initialize(imageSource);
    }

    protected virtual void RenderCurrentFrame(TextureFrame textureFrame)
    {
      if (screen == null)
      {
        return;
      }
      screen.ReadSync(textureFrame);
    }

    protected abstract void OnStartRun();

    protected abstract void AddTextureFrameToInputStream(TextureFrame textureFrame);

    protected abstract IEnumerator WaitForNextValue();
  }
}
