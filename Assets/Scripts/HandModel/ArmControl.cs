using NaughtyAttributes;
using System;
using UnityEngine;

public enum PrecisionNeeded
{
    Perfect,
    Precise,
    Flexible,
    Indulgent,
}

[Serializable]
public class DifficultyCustom
{
    public PrecisionNeeded PrecisionNeeded = PrecisionNeeded.Flexible;
    public bool SyncArticulations = true;
}

[Serializable]
public class ArticulationMove
{
    public Hand ExampleHand = Hand.Left;
    public Articulations articulation = Articulations.Hand;
    public bool isOppositeDirection = false;
    public Direction direction = Direction.ANY;
    [HideInInspector] public Axis AxisRotation = Axis.X;
    [Tooltip("true : the value of the rotation will decrease with the movement (false : it will increase)")]
    public bool MoveDir = true;
    [Tooltip("true : the value of the rotation must be inferior of the ValueLimit to validate the tutorial (false : must be superior)")]
    public bool MoveCheck = true;
    public float ValueLimit = 0f;

    [HideInInspector]  public bool hasCustomStartPos = false;
    [ShowIf("hasCustomStartPos"), AllowNesting] public float customStartPos = 0f;
    [HideInInspector] public bool _startPosLeftDone = false;
    [HideInInspector] public bool _startPosRightDone = false;
    
    [HideInInspector] public bool _done = false;
}

[RequireComponent(typeof(ArmSetter))]
public class ArmControl : MonoBehaviour
{
    private ArmSetter armSetter;

    public Vector3 HandAngles;

    public Vector2 Finger1Angles;
    public Vector2 Finger1Joint1Angles;
    
    public Vector2 Finger2Angles;
    public Vector2 Finger2Joint1Angles;
    public Vector2 Finger2Joint2Angles;

    public Vector2 Finger3Angles;
    public Vector2 Finger3Joint1Angles;
    public Vector2 Finger3Joint2Angles;

    public Vector2 Finger4Angles;
    public Vector2 Finger4Joint1Angles;
    public Vector2 Finger4Joint2Angles;

    public Vector2 Finger5Angles;
    public Vector2 Finger5Joint1Angles;
    public Vector2 Finger5Joint2Angles;

    void Start()
    {
        armSetter = GetComponent<ArmSetter>();
    }

    void FixedUpdate()
    {
        armSetter.arm.Hand.transform.Rotate(HandAngles.x, HandAngles.y, HandAngles.z, Space.Self);
        armSetter.arm.Finger1.transform.Rotate(Finger1Angles.x, Finger1Angles.y, 0f, Space.Self);
        armSetter.arm.Finger1Joint1.transform.Rotate(Finger1Joint1Angles.x, Finger1Joint1Angles.y, 0f, Space.Self);
        armSetter.arm.Finger2.transform.Rotate(Finger2Angles.x, Finger2Angles.y, 0f, Space.Self);
        armSetter.arm.Finger2Joint1.transform.Rotate(Finger2Joint1Angles.x, Finger2Joint1Angles.y, 0f, Space.Self);
        armSetter.arm.Finger2Joint2.transform.Rotate(Finger2Joint2Angles.x, Finger2Joint2Angles.y, 0f, Space.Self);
        armSetter.arm.Finger3.transform.Rotate(Finger3Angles.x, Finger3Angles.y, 0f, Space.Self);
        armSetter.arm.Finger3Joint1.transform.Rotate(Finger3Joint1Angles.x, Finger3Joint1Angles.y, 0f, Space.Self);
        armSetter.arm.Finger3Joint2.transform.Rotate(Finger3Joint2Angles.x, Finger3Joint2Angles.y, 0f, Space.Self);
        armSetter.arm.Finger4.transform.Rotate(Finger4Angles.x, Finger4Angles.y, 0f, Space.Self);
        armSetter.arm.Finger4Joint1.transform.Rotate(Finger4Joint1Angles.x, Finger4Joint1Angles.y, 0f, Space.Self);
        armSetter.arm.Finger4Joint2.transform.Rotate(Finger4Joint2Angles.x, Finger4Joint2Angles.y, 0f, Space.Self);
        armSetter.arm.Finger5.transform.Rotate(Finger5Angles.x, Finger5Angles.y, 0f, Space.Self);
        armSetter.arm.Finger5Joint1.transform.Rotate(Finger5Joint1Angles.x, Finger5Joint1Angles.y, 0f, Space.Self);
        armSetter.arm.Finger5Joint2.transform.Rotate(Finger5Joint2Angles.x, Finger5Joint2Angles.y, 0f, Space.Self);

        //Add a call to applyArmMesh to actualize meshes
    }
}
