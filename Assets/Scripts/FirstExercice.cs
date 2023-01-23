using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;
using NaughtyAttributes;

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

[Serializable]
public class Demo
{
    public float ExampleMoveSpeed = 30f;
    public float ResetTime = 1;
    public List<ArticulationMove> Movements = new List<ArticulationMove>();
    public DifficultyCustom difficultyCustom = new DifficultyCustom();
    public bool hasCustomPosLeft = false;
    [ShowIf("hasCustomPosLeft"), AllowNesting] public ArmRotationDatas customStartPosLeft = new ArmRotationDatas();
    public bool hasCustomPosRight = false;
    [ShowIf("hasCustomPosRight"), AllowNesting] public ArmRotationDatas customStartPosRight = new ArmRotationDatas();
}

public class FirstExercice : MonoBehaviour
{
    [SerializeField]
    public HandsManager ExampleHandsManager;

    private ArmRotationDatas ExampleArmLeftStartPos = new ArmRotationDatas();
    private ArmRotationDatas ExampleArmRightStartPos = new ArmRotationDatas();

    public int PlayedDemo = 0;
    public List<Demo> Demos = new List<Demo>();

    [SerializeField]
    public HandsManager PlayerHandsManager;

    [HideInInspector]
    public bool isSuccess = false;

    void Start()
    {
        LogStartPos();
        ResetExampleHands();
    }

    public void LogStartPos()
    {
        ExampleArmLeftStartPos.GetRotationToArm(ExampleHandsManager.armLeft.arm);
        ExampleArmRightStartPos.GetRotationToArm(ExampleHandsManager.armRight.arm);
    }

    public void FixedUpdate()
    {
        if (false)//si changement dans l'inspecteur (à gérer plus tard)
        {
            //ResetExampleHands();
        }
        ApplyExample();
    }

    public void ApplyExample()
    {
        // If the example has been successfully completed, we don't do anything
        if (isSuccess)
            return;

        // If the demo played is not in the range of valid demos, we display a warning message and exit the function
        if (PlayedDemo < 0 || PlayedDemo >= Demos.Count)
        {
            Debug.LogWarning("The demo played (" + PlayedDemo + ") does not exist!!!", this);
            return;
        }

        bool isMoving = false;
        foreach (ArticulationMove movement in Demos[PlayedDemo].Movements)
        {
            if (ExampleHandsManager.MoveArticulation(movement, Demos[PlayedDemo].ExampleMoveSpeed))
            {
                //if one articulation is still moving, we don't reset the example
                isMoving = true;
            }
        }

        if (PlayerHandsManager.CheckAllArticulationsSuccess(Demos[PlayedDemo]) && PlayerHandsManager.checkStaticPos(Demos[PlayedDemo], ExampleHandsManager))
        {
            isSuccess = true;
            StartCoroutine(ExampleResetSuccess(3));
        }
        else if (!isMoving)
        {
            StartCoroutine(ExampleReset(Demos[PlayedDemo].ResetTime));
        }

    }

    IEnumerator ExampleReset(float secondsWait)
    {
        yield return new WaitForSeconds(secondsWait);
        ResetExampleHands();
    }

    IEnumerator ExampleResetSuccess(float secondsWait)
    {
        yield return new WaitForSeconds(secondsWait);
        isSuccess = false;
        Demos[PlayedDemo].Movements.ForEach(x => x._done = false);
        Demos[PlayedDemo].Movements.ForEach(x => x._startPosLeftDone = false);
        Demos[PlayedDemo].Movements.ForEach(x => x._startPosRightDone = false);
        ResetExampleHands();
    }

    void ResetExampleHands()
    {
        ArmRotationDatas ResetArmLeft = new ArmRotationDatas(ExampleArmLeftStartPos);
        if (Demos[PlayedDemo].hasCustomPosLeft)
            ResetArmLeft.Combine(Demos[PlayedDemo].customStartPosLeft);

        ArmRotationDatas ResetArmRight = new ArmRotationDatas(ExampleArmRightStartPos);
        if (Demos[PlayedDemo].hasCustomPosRight)
            ResetArmRight.Combine(Demos[PlayedDemo].customStartPosRight);
        
        ExampleHandsManager.armLeft.arm.SetRotationToArm(ResetArmLeft);
        ExampleHandsManager.armRight.arm.SetRotationToArm(ResetArmRight);

        foreach (ArticulationMove movement in Demos[PlayedDemo].Movements)
        {
            if (movement.hasCustomStartPos)
            {
                if (movement.ExampleHand != Hand.Right)
                {
                    SetCustomStartPosMove(ExampleHandsManager.armLeft.ArticulationsDict[movement.articulation].transform, movement);
                }
                if (movement.ExampleHand != Hand.Left)
                {
                    SetCustomStartPosMove(ExampleHandsManager.armRight.ArticulationsDict[movement.articulation].transform, movement);
                }
            }
        }
    }

    private void SetCustomStartPosMove(Transform articulationTransform, ArticulationMove movement)
    {
        Vector3 newExampleAngles = TransformUtils.GetInspectorRotation(articulationTransform);

        newExampleAngles.x = movement.AxisRotation == Axis.X ? movement.customStartPos : newExampleAngles.x;
        newExampleAngles.y = movement.AxisRotation == Axis.Y ? movement.customStartPos : newExampleAngles.y;
        newExampleAngles.z = movement.AxisRotation == Axis.Z ? movement.customStartPos : newExampleAngles.z;

        TransformUtils.SetInspectorRotation(articulationTransform, newExampleAngles);
    }
}
