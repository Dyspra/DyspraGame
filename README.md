# Dyspra - A Video Game to help Dyspraxic Children

This is a Unity (2020.3.23f1) project.

## Prerequisites

You need to install the following software to build this project.

- PyInstaller (for building the executable of MediaPipePythonInterface)
- Unity 2020.3.23f1
- MediaPipeUnityPlugin (https://github.com/homuler/MediaPipeUnityPlugin/wiki/Installation-Guide)

<!-- explain how to install MediapipeUnityPlugin -->

## How Hand Tracking works in Dyspra Unity Project

The hand tracking is done by MediaPipe.
We first create an interface with python to get the hand landmarks from MediaPipe called MediapipePythonInterface.

Then we create a 2nd interface with a Unity plugin called MediaPipeUnityPlugin

MediapipePythonInterface and MediaPipeUnityPlugin do the same thing, but they are written in different languages.
MediapipePythonInterface need python dependencies and can be unstable.

## How to build MediaPipePythonInterface

1. Install PyInstaller globally

```bash
pip install pyinstaller
```

2. Check if pyinstaller is installed

```bash
pyinstaller --version
```

3. Install the dependencies

```bash
pip install -r Assets/Plugins/MediapipePythonInterface/requirements.txt
```

You're done! Now pyinstaller will be launch automatically when you build the project or when you launch it on Editor.
