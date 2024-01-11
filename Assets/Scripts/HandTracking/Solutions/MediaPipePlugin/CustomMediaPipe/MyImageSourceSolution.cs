// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using System.Collections;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Threading;
using UnityEngine;

namespace Mediapipe.Unity.Dyspra
{
  public abstract class MyImageSourceSolution<T> : MySolution where T : MyGraphRunner
  {
    [SerializeField] public Mediapipe.Unity.Dyspra.Screen screen;
    [SerializeField] public T graphRunner;
    [SerializeField] public MyTextureFramePool textureFramePool;

    private Task _task;

    public RunningMode runningMode;

    public long timeoutMillisec
    {
      get => graphRunner.timeoutMillisec;
      set => graphRunner.timeoutMillisec = value;
    }

    public override void Play()
    {
        UnityEngine.Debug.Log($"[S] Current thread of play Solution: {Thread.CurrentThread.ManagedThreadId}");
      if (_task != null && _task.Status == TaskStatus.Running)
      {
        UnityEngine.Debug.Log("[S] Stopping play Solution");
        Stop();
      }
      UnityEngine.Debug.Log("[S] Starting base play Solution");
      base.Play();
      _task = Task.Run(() => {
        UnityEngine.Debug.Log($"[S] Starting run Solution");
        Run();
      });
    }

    public override void Pause()
    {
      base.Pause();
      MyImageSourceProvider.ImageSource.Pause();
    }

    public override void Resume()
    {
      base.Resume();
      Task.Run(() => {
        MyImageSourceProvider.ImageSource.Resume();
      });
    }

    public override void Stop()
    {
      base.Stop();
      _task.Dispose();
      MyImageSourceProvider.ImageSource.Stop();
      graphRunner.Stop();
    }

    private void Run()
    {
      Logger.LogInfo(TAG, $"Running MediaPipe inside MyImageSourceSolution...");
      var graphInitRequest = graphRunner.WaitForInit(runningMode);
      UnityEngine.Debug.Log("graph launch but not waiting");
      var imageSource = MyImageSourceProvider.ImageSource;
      UnityEngine.Debug.Log($"imaceSource: {imageSource}");

      // imageSource.Play().Wait();
      // UnityEngine.Debug.Log($"imaceSource is playing: {imageSource.isPlaying}");
      if (!imageSource.isPrepared)
      {
        Logger.LogError(TAG, "Failed to start ImageSource, exiting...");
        throw new System.InvalidOperationException("Failed to start ImageSource, exiting...");
      }

      UnityEngine.Debug.Log("imageSource isPrepared, now textureFramePool.ResizeTexture");
      // Use RGBA32 as the input format.
      // TODO: When using GpuBuffer, MediaPipe assumes that the input format is BGRA, so the following code must be fixed.
      // textureFramePool.ResizeTexture(imageSource.textureWidth, imageSource.textureHeight, TextureFormat.RGBA32);
      textureFramePool.ResizeTexture(1280, 720, TextureFormat.RGBA32);
      // UnityEngine.Debug.Log("textureFramePool.ResizeTexture done");
      // SetupScreen(imageSource);

      UnityEngine.Debug.Log("Graph init request");
      graphInitRequest.Wait();
      if (graphInitRequest.IsFaulted)
      {
        UnityEngine.Debug.Log($"failed to init graph: {graphInitRequest.Exception}");
        Logger.LogError(TAG, graphInitRequest.Exception.ToString());
        throw graphInitRequest.Exception;
      }
      UnityEngine.Debug.Log($"Graph is initialized: {graphInitRequest.IsCompleted}");

      OnStartRun();
      graphRunner.StartRun(imageSource);
      UnityEngine.Debug.Log("Graph is started and OnStartRun, go boucle inf");

      while (true)
      {
        if (isPaused)
        {
            UnityEngine.Debug.Log("[L] isPaused");
            continue;
        }

        UnityEngine.Debug.Log("[L] textureFramePool.TryGetTextureFrame:");

        if (!textureFramePool.TryGetTextureFrame(out var textureFrame))
        {
          UnityEngine.Debug.Log("[L] textureFramePool.TryGetTextureFrame failed");
          // yield return new WaitForEndOfFrame();
          continue;
        }

        UnityEngine.Debug.Log("[L] read from image source");
        // Copy current image to TextureFrame
        ReadFromImageSource(imageSource, textureFrame);
        AddTextureFrameToInputStream(textureFrame);
        // yield return new WaitForEndOfFrame();

        // if (runningMode.IsSynchronous())
        // {
          RenderCurrentFrame(textureFrame);
        //   yield return WaitForNextValue();
        // }
      }
    }

    public virtual void SetupScreen(MyImageSource imageSource)
    {
      // NOTE: The screen will be resized later, keeping the aspect ratio.
      if (screen == null)
      {
        return;
      }
      screen.Initialize(imageSource);
    }

    protected virtual void RenderCurrentFrame(MyTextureFrame textureFrame)
    {
      if (screen == null)
      {
        return;
      }
      screen.ReadSync(textureFrame);
    }

    protected abstract void OnStartRun();

    protected abstract void AddTextureFrameToInputStream(MyTextureFrame textureFrame);

    protected abstract IEnumerator WaitForNextValue();
  }
}
