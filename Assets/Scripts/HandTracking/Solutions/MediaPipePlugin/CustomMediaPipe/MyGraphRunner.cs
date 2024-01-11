// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Rendering;

using Stopwatch = System.Diagnostics.Stopwatch;

namespace Mediapipe.Unity.Dyspra
{
    public abstract class MyGraphRunner : object
    {
        public enum ConfigType
        {
            None,
            CPU,
            GPU,
            OpenGLES,
        }

#pragma warning disable IDE1006
        // TODO: make it static
        protected string TAG => GetType().Name;
#pragma warning restore IDE1006

        [SerializeField] public string _cpuConfig = null;
        [SerializeField] public string _gpuConfig = null;
        [SerializeField] public string _openGlEsConfig = null;
        [SerializeField] public long _timeoutMicrosec = 0;

        // private static readonly GlobalInstanceTable<int, MyGraphRunner> _InstanceTable = new GlobalInstanceTable<int, MyGraphRunner>(5);
        private static readonly Dictionary<IntPtr, int> _NameTable = new Dictionary<IntPtr, int>();

        protected RunningMode runningMode { get; private set; } = RunningMode.Async;
        private bool _isRunning = false;

        public InferenceMode inferenceMode => configType == ConfigType.CPU ? InferenceMode.CPU : InferenceMode.GPU;
        public virtual ConfigType configType
        {
            get
            {
                if (GpuManager.IsInitialized)
                {
#if UNITY_ANDROID
                    if (SystemInfo.graphicsDeviceType == GraphicsDeviceType.OpenGLES3 && _openGlEsConfig != null)
                    {
                        return ConfigType.OpenGLES;
                    }
#endif
                    if (_gpuConfig != null)
                    {
                        return ConfigType.GPU;
                    }
                }
                return _cpuConfig != null ? ConfigType.CPU : ConfigType.None;
            }
        }
        public string textConfig
        {
            get
            {
                switch (configType)
                {
                    case ConfigType.CPU: return _cpuConfig;
                    case ConfigType.GPU: return _gpuConfig;
                    case ConfigType.OpenGLES: return _openGlEsConfig;
                    case ConfigType.None:
                    default: return null;
                }
            }
        }

        public long timeoutMicrosec
        {
            get => _timeoutMicrosec;
            set => _timeoutMicrosec = (long)Mathf.Max(0, value);
        }
        public long timeoutMillisec
        {
            get => timeoutMicrosec / 1000;
            set => timeoutMicrosec = value * 1000;
        }

        public RotationAngle rotation { get; private set; } = 0;

        private Stopwatch _stopwatch;
        protected CalculatorGraph calculatorGraph { get; private set; }
        protected Timestamp latestTimestamp;

        // protected virtual void Start()
        // {
        //   // _InstanceTable.Add(GetInstanceID(), this);
        // }

        // protected virtual void OnDestroy()
        // {
        //   Stop();
        // }
        ~MyGraphRunner()
        {
            Stop();
        }

        public async Task WaitForInit(RunningMode runningMode)
        {
            // return new WaitForResult(this, Initialize(runningMode));
            await Initialize(runningMode);
        }

        public virtual async Task Initialize(RunningMode runningMode)
        {
            this.runningMode = runningMode;

            Logger.LogInfo(TAG, $"Config Type = {configType}");
            Logger.LogInfo(TAG, $"Running Mode = {runningMode}");
            // last here

            InitializeCalculatorGraph().AssertOk();
            // not here 
            Logger.LogInfo(TAG, "Successfully initialized the calculator graph.");
            _stopwatch = new Stopwatch();
            _stopwatch.Start();
            Logger.LogInfo(TAG, "Start stopwatch.");

            Logger.LogInfo(TAG, "Loading dependent assets...");
            var assetRequests = RequestDependentAssets();
            await Task.Run(() => {
                while (assetRequests.Any((request) => !request.IsCompleted))
                {
                    Task.Delay(1000).Wait();
                }
            });
            Logger.LogInfo(TAG, "Successfully loaded dependent assets.");

            var errors = assetRequests.Where((request) => request.IsFaulted).Select((request) => request.Exception.ToString()).ToList();
            if (errors.Count > 0)
            {
                foreach (var error in errors)
                {
                    Logger.LogError(TAG, error);
                }
                throw new InternalException("Failed to prepare dependent assets");
            }
            Logger.LogInfo(TAG, "Successfully prepared dependent assets.");
        }

        public abstract void StartRun(MyImageSource imageSource);

        protected void StartRun(SidePacket sidePacket)
        {
            UnityEngine.Debug.Log("StartRun MyGraphRunner");
            calculatorGraph.StartRun(sidePacket).AssertOk();
            _isRunning = true;
            UnityEngine.Debug.Log("StartRun MyGraphRunner after start run");
        }

        public virtual void Stop()
        {
            if (calculatorGraph != null)
            {
                if (_isRunning)
                {
                    using (var status = calculatorGraph.CloseAllPacketSources())
                    {
                        if (!status.Ok())
                        {
                            Logger.LogError(TAG, status.ToString());
                        }
                    }

                    using (var status = calculatorGraph.WaitUntilDone())
                    {
                        if (!status.Ok())
                        {
                            Logger.LogError(TAG, status.ToString());
                        }
                    }
                }

                _isRunning = false;
                var _ = _NameTable.Remove(calculatorGraph.mpPtr);
                calculatorGraph.Dispose();
                calculatorGraph = null;
            }

            if (_stopwatch != null && _stopwatch.IsRunning)
            {
                _stopwatch.Stop();
            }
        }

        protected void AddPacketToInputStream<T>(string streamName, Packet<T> packet)
        {
            calculatorGraph.AddPacketToInputStream(streamName, packet).AssertOk();
        }

        protected void AddTextureFrameToInputStream(string streamName, MyTextureFrame textureFrame)
        {
            latestTimestamp = GetCurrentTimestamp();

            if (configType == ConfigType.OpenGLES)
            {
                var gpuBuffer = textureFrame.BuildGpuBuffer(GpuManager.GlCalculatorHelper.GetGlContext());
                AddPacketToInputStream(streamName, new GpuBufferPacket(gpuBuffer, latestTimestamp));
                return;
            }

            var imageFrame = textureFrame.BuildImageFrame();
            textureFrame.Release();

            AddPacketToInputStream(streamName, new ImageFramePacket(imageFrame, latestTimestamp));
        }

        protected bool TryGetNext<TPacket, TValue>(OutputStream<TPacket, TValue> stream, out TValue value, bool allowBlock, long currentTimestampMicrosec) where TPacket : Packet<TValue>, new()
        {
            var result = stream.TryGetNext(out value, allowBlock);
            return result || allowBlock || stream.ResetTimestampIfTimedOut(currentTimestampMicrosec, timeoutMicrosec);
        }

        protected long GetCurrentTimestampMicrosec()
        {
            return _stopwatch == null || !_stopwatch.IsRunning ? -1 : _stopwatch.ElapsedTicks / (TimeSpan.TicksPerMillisecond / 1000);
        }

        protected Timestamp GetCurrentTimestamp()
        {
            var microsec = GetCurrentTimestampMicrosec();
            return microsec < 0 ? Timestamp.Unset() : new Timestamp(microsec);
        }

        protected Status InitializeCalculatorGraph()
        {
            UnityEngine.Debug.Log("init calculator graph 1");
            calculatorGraph = new CalculatorGraph();
            UnityEngine.Debug.Log("init calculator graph 2");
            _NameTable.Add(calculatorGraph.mpPtr, calculatorGraph.GetHashCode());

            // NOTE: There's a simpler way to initialize CalculatorGraph.
            //
                // calculatorGraph = new CalculatorGraph(textConfig.text);
                // return Status.Ok();
            //
            //   However, if the config format is invalid, this code does not initialize CalculatorGraph and does not throw exceptions either.
            //   The problem is that if you call ObserveStreamOutput in this state, the program will crash.
            //   The following code is not very efficient, but it will return Non-OK status when an invalid configuration is given.
            try
            {
                UnityEngine.Debug.Log("init calculator graph 3");
                if (textConfig == null) throw new InvalidOperationException("Failed to get the text config. Check if the config is set to GraphRunner");
                UnityEngine.Debug.Log("init calculator graph 4");
                UnityEngine.Debug.Log("config type: " + configType);
                UnityEngine.Debug.Log("is text cpu null: " + _cpuConfig == null);
                var baseConfig = CalculatorGraphConfig.Parser.ParseFromTextFormat(textConfig);
                // throw new InvalidOperationException("stop here");
                UnityEngine.Debug.Log("init calculator graph 5");
                var status = ConfigureCalculatorGraph(baseConfig);
                UnityEngine.Debug.Log("init calculator graph 6");
                var returnValue = !status.Ok() || inferenceMode == InferenceMode.CPU ? status : calculatorGraph.SetGpuResources(GpuManager.GpuResources);
                UnityEngine.Debug.Log("init calculator graph return value: " + returnValue);
                return returnValue;
            }
            catch (Exception e)
            {
                UnityEngine.Debug.LogError('[' + TAG + "] " + e);
                return Status.FailedPrecondition(e.ToString());
            }
        }

        /// <summary>
        ///   Configure and initialize the <see cref="CalculatorGraph" />.
        /// </summary>
        /// <remarks>
        ///   This is the main process in <see cref="InitializeCalculatorGraph" />.<br />
        ///   At least, <c>calculatorGraph.Initialize</c> must be called here.
        ///   In addition to that, <see cref="OutputStream" /> instances should be initialized.
        /// </remarks>
        /// <param name="config">
        ///   A <see cref="CalculatorGraphConfig" /> instance corresponding to <see cref="textConfig" />.<br />
        ///   It can be dynamically modified here.
        /// </param>
        protected virtual Status ConfigureCalculatorGraph(CalculatorGraphConfig config)
        {
            return calculatorGraph.Initialize(config);
        }

        protected void SetImageTransformationOptions(SidePacket sidePacket, MyImageSource imageSource, bool expectedToBeMirrored = false)
        {
            UnityEngine.Debug.Log("SetImageTransformationOptions");
            // NOTE: The origin is left-bottom corner in Unity, and right-top corner in MediaPipe.
            rotation = imageSource.rotation.Reverse();
            UnityEngine.Debug.Log("SetImageTransformationOptions 1");
            var inputRotation = rotation;
            var isInverted = CoordinateSystem.ImageCoordinate.IsInverted(rotation);
            var shouldBeMirrored = imageSource.isHorizontallyFlipped ^ expectedToBeMirrored;
            var inputHorizontallyFlipped = isInverted ^ shouldBeMirrored;
            var inputVerticallyFlipped = !isInverted;
            UnityEngine.Debug.Log("SetImageTransformationOptions 2");

            if ((inputHorizontallyFlipped && inputVerticallyFlipped) || rotation == RotationAngle.Rotation180)
            {
                inputRotation = inputRotation.Add(RotationAngle.Rotation180);
                inputHorizontallyFlipped = !inputHorizontallyFlipped;
                inputVerticallyFlipped = !inputVerticallyFlipped;
            }
            UnityEngine.Debug.Log("SetImageTransformationOptions 3");
            Logger.LogDebug($"input_rotation = {inputRotation}, input_horizontally_flipped = {inputHorizontallyFlipped}, input_vertically_flipped = {inputVerticallyFlipped}");

            sidePacket.Emplace("input_rotation", new IntPacket((int)inputRotation));
            sidePacket.Emplace("input_horizontally_flipped", new BoolPacket(inputHorizontallyFlipped));
            sidePacket.Emplace("input_vertically_flipped", new BoolPacket(inputVerticallyFlipped));
        }

        protected Task WaitForAsset(string assetName, string uniqueKey, long timeoutMillisec, bool overwrite = false)
        {
            return MyAssetLoader.PrepareAssetAsync(assetName, uniqueKey, overwrite);
        }

        protected Task WaitForAsset(string assetName, long timeoutMillisec, bool overwrite = false)
        {
            return WaitForAsset(assetName, assetName, timeoutMillisec, overwrite);
        }

        protected Task WaitForAsset(string assetName, string uniqueKey, bool overwrite = false)
        {
            return MyAssetLoader.PrepareAssetAsync(assetName, uniqueKey, overwrite);
        }

        protected Task WaitForAsset(string assetName, bool overwrite = false)
        {
            return WaitForAsset(assetName, assetName, overwrite);
        }

        protected abstract IList<Task> RequestDependentAssets();
    }
}
