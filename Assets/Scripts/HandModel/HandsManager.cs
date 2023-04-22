using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

public class HandsManager : MonoBehaviour
{
    public ArmSetter armLeft;
    public ArmSetter armRight;
    void Start()
    {
        armLeft = transform.Find("LeftHand").GetComponent<ArmSetter>();
        armRight = transform.Find("RightHand").GetComponent<ArmSetter>();
    }

    public bool MoveArticulation(ArticulationMove movement, float moveSpeed)
    {
        bool isMoving = false;
        float articulationToRotateLeft = ReturnArticulationToRotate(movement, armLeft.ArticulationsDict[movement.articulation].transform);
        float articulationToRotateRight = ReturnArticulationToRotate(movement, armRight.ArticulationsDict[movement.articulation].transform);
        float moveValue = (movement.MoveDir ? -moveSpeed : moveSpeed) * Time.deltaTime;

        //check and rotation left hand
        if (movement.ExampleHand != Hand.Right && ((movement.MoveCheck && articulationToRotateLeft > movement.ValueLimit)
                                                || (!movement.MoveCheck && articulationToRotateLeft < movement.ValueLimit)))
        {
            RotateArticulation(movement, armLeft.ArticulationsDict[movement.articulation].transform, moveValue);
            isMoving = true;
        }
        //check and rotation right hand
        if (movement.ExampleHand != Hand.Left && ((movement.MoveCheck && articulationToRotateRight > movement.ValueLimit)
                                                || (!movement.MoveCheck && articulationToRotateRight < movement.ValueLimit)))
        {
            RotateArticulation(movement, armRight.ArticulationsDict[movement.articulation].transform, moveValue);
            isMoving = true;
        }

        return isMoving;
    }

    private void RotateArticulation(ArticulationMove movement, Transform transform, float moveValue)
    {
        switch (movement.AxisRotation)
        {
            case (Axis.X):
                transform.Rotate(moveValue, 0f, 0f, Space.Self);
                break;
            case (Axis.Y):
                transform.Rotate(0f, moveValue, 0f, Space.Self);
                break;
            case (Axis.Z):
                transform.Rotate(0f, 0f, moveValue, Space.Self);
                break;
        }
    }

    private float ReturnArticulationToRotate(ArticulationMove movement, Transform transform)
    {
        Vector3 ArticulationRotation = TransformUtils.GetInspectorRotation(transform);
        switch (movement.AxisRotation)
        {
            case (Axis.X):
                return ArticulationRotation.x;
            case (Axis.Y):
                return ArticulationRotation.y;
            case (Axis.Z):
                return ArticulationRotation.z;
        }
        return 0;
    }

    public bool CheckAllArticulationsSuccess(Demo demo)
    {
        if (!demo.difficultyCustom.SyncArticulations)
        {
            foreach (ArticulationMove movement in demo.Movements)
            {
                if (CheckArticulationSuccess(movement, demo.difficultyCustom.PrecisionNeeded))
                {
                    movement._done = true;
                }
            }
            if (demo.Movements.All(movement => movement._done == true))
                return true;
            return false;
        }
        else
        {
            bool isAllSuccess = true;
            foreach (ArticulationMove movement in demo.Movements)
            {
                if (!CheckArticulationSuccess(movement, demo.difficultyCustom.PrecisionNeeded))
                    isAllSuccess = false;
            }
            return isAllSuccess;
        }
    }

    public bool CheckArticulationSuccess(ArticulationMove movement, PrecisionNeeded precisionNeeded = PrecisionNeeded.Flexible)
    {
        // Récupère les valeurs d'articulation à tourner pour les deux mains
        float articulationToRotateLeft = ReturnArticulationToRotate(movement, armLeft.ArticulationsDict[movement.articulation].transform);
        float articulationToRotateRight = ReturnArticulationToRotate(movement, armRight.ArticulationsDict[movement.articulation].transform);

        // Récupère la marge de précision
        float precisionMarge = GetPrecisionMarge(precisionNeeded);

        // Vérifie si la position de départ est atteinte (si activée)
        if (movement.hasCustomStartPos)
        {
            bool leftCheckStartPos = articulationToRotateLeft <= movement.customStartPos + precisionMarge && articulationToRotateLeft >= movement.customStartPos - precisionMarge;
            bool rightCheckStartPos = articulationToRotateRight <= movement.customStartPos + precisionMarge && articulationToRotateRight >= movement.customStartPos - precisionMarge;

            if (leftCheckStartPos && movement.ExampleHand != Hand.Right)
            {
                movement._startPosLeftDone = true;
            }
            else if (rightCheckStartPos && movement.ExampleHand != Hand.Left)
            {
                movement._startPosRightDone = true;
            }
        }

        // Vérifie si la main gauche ou droite est satisfaite par les limites de mouvement
        bool leftCheck = articulationToRotateLeft <= movement.ValueLimit + precisionMarge && articulationToRotateLeft >= movement.ValueLimit - precisionMarge;
        bool rightCheck = articulationToRotateRight <= movement.ValueLimit + precisionMarge && articulationToRotateRight >= movement.ValueLimit - precisionMarge;

        // Retourne vrai si les deux mains sont satisfaites par les limites de mouvement et pos de départ respectée
        if ((movement.ExampleHand == Hand.Right || leftCheck) && (movement.ExampleHand == Hand.Left || rightCheck))
        {
            if (!movement.hasCustomStartPos)
                return true;
            else if (movement.ExampleHand == Hand.Right && movement._startPosRightDone)
                return true;
            else if (movement.ExampleHand == Hand.Left && movement._startPosLeftDone)
                return true;
            else if (movement.ExampleHand == Hand.Both && movement._startPosLeftDone && movement._startPosRightDone)
                return true;
            // (oui tout aurait pu tenir dans une condition mais ça aurait été illisible)
        }
        return false;
    }

    public bool checkStaticPos(Demo demo)
    {
        float precisionMarge = GetPrecisionMarge(demo.difficultyCustom.PrecisionNeeded);

        bool posLeftOk = !demo.hasCustomPosLeft || checkStaticPosArm(armLeft, demo.customPosLeft, precisionMarge);
        bool posRightOk = !demo.hasCustomPosRight || checkStaticPosArm(armRight, demo.customPosRight, precisionMarge);

        return posLeftOk && posRightOk;
    }

    private bool checkStaticPosArm(ArmSetter arm, ArmRotationDatas customPosDatas, float precisionMarge)
    {
        foreach (Articulations articulationEnum in Enum.GetValues(typeof(Articulations)))
        {
            Vector3? staticAnglesTargetPos = typeof(ArmRotationDatas).GetField(articulationEnum.ToString()).GetValue(customPosDatas) as Vector3?;

            if (staticAnglesTargetPos != null && staticAnglesTargetPos != Vector3.zero)
            {
                //les valeurs réelles du player
                Vector3 currentAngles = arm.ArticulationsDict[articulationEnum].transform.localRotation.eulerAngles;

                if ((staticAnglesTargetPos?.x != 0.0f && !(currentAngles.x >= staticAnglesTargetPos?.x - precisionMarge && currentAngles.x <= staticAnglesTargetPos?.x + precisionMarge))
                   || (staticAnglesTargetPos?.y != 0.0f && !(currentAngles.y >= staticAnglesTargetPos?.y - precisionMarge && currentAngles.y <= staticAnglesTargetPos?.y + precisionMarge))
                   || (staticAnglesTargetPos?.z != 0.0f && !(currentAngles.z >= staticAnglesTargetPos?.z - precisionMarge && currentAngles.z <= staticAnglesTargetPos?.z + precisionMarge)))
                {
                    return false;
                }
            }
        }
        return true;
    }

    private float GetPrecisionMarge(PrecisionNeeded precisionNeeded)
    {
        switch (precisionNeeded)
        {
            case PrecisionNeeded.Perfect:
                return 5;
            case PrecisionNeeded.Precise:
                return 10;
            case PrecisionNeeded.Flexible:
                return 15;
            case PrecisionNeeded.Indulgent:
                return 20;
            default:
                break;
        }
        return 0;
    }
}
