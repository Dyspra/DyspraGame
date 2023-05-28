using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;

public enum Hand
{
    Left,
    Right,
    Both
}

public enum Axis
{
    X,
    Y,
    Z,
}

public enum Direction
{
    FRONT,
    BACK,
    DOWN,
    UP,
    LEFT,
    RIGHT,
    ANY
}

[Serializable]
public class Demo
{
    public float ExampleMoveSpeed = 30f;
    public float ResetTime = 1;
    public List<ArticulationMove> Movements = new List<ArticulationMove>();
    public DifficultyCustom difficultyCustom = new DifficultyCustom();
    public bool hasCustomPosLeft = false;
    [ShowIf("hasCustomPosLeft"), AllowNesting] public ArmRotationDatas customPosLeft = new ArmRotationDatas();
    public bool hasCustomPosRight = false;
    [ShowIf("hasCustomPosRight"), AllowNesting] public ArmRotationDatas customPosRight = new ArmRotationDatas();
}