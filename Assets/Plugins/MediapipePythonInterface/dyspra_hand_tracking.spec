# -*- mode: python ; coding: python-*-


block_cipher = None

import os
import mediapipe

a = Analysis(
    ['dyspra_hand_tracking.py'],
    pathex=[],
    binaries=[],
    datas=[(
        os.path.join(
            os.path.dirname(mediapipe.__file__),
            'modules'),
        'mediapipe/modules'
    )],
    hiddenimports=[],
    hookspath=[],
    hooksconfig={},
    runtime_hooks=[],
    excludes=[],
    win_no_prefer_redirects=False,
    win_private_assemblies=False,
    cipher=block_cipher,
    noarchive=False,
)
pyz = PYZ(a.pure, a.zipped_data, cipher=block_cipher)

exe = EXE(
    pyz,
    a.scripts,
    [],
    name='dyspra_hand_tracking',
    exclude_binaries=False,
    debug='bootloader',
    bootloader_ignore_signals=False,
    upx=True,
    strip=False,
    onedir=True,
)

coll = COLLECT(
    exe,
    a.binaries,
    a.zipfiles,
    a.datas,
    strip=False,
    upx=True,
    name='dyspra_hand_tracking'
)
    
