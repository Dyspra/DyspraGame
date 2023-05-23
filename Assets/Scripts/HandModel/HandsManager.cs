using System;
using UnityEditor;
using UnityEngine;

public class HandsManager : MonoBehaviour
{
    public ArmSetter armLeft;
    public ArmSetter armRight;
    void Start()
    {
        // Probably useless sice it is already set in the inspector
        //armLeft = transform.Find("LeftHand").GetComponent<ArmSetter>();
        //armRight = transform.Find("RightHand").GetComponent<ArmSetter>();
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

    public bool IsDirectionGood(Direction dir, bool isLeft)
    {
        Transform trans;
        RaycastHit hit;

        if (isLeft == true)
            trans = armLeft.ArticulationsDict[Articulations.Hand].transform;
        else
            trans = armRight.ArticulationsDict[Articulations.Hand].transform;

        Debug.DrawRay(trans.position, -trans.up, Color.green);

        if (Physics.Raycast(trans.position, -trans.up, out hit))
        {
            switch (hit.collider.gameObject.name)
            {
                case "Top":
                    if (dir == Direction.UP)
                        return true;
                    break;
                case "Down":
                    if (dir == Direction.DOWN)
                        return true;
                    break;
                case "Front":
                    if (dir == Direction.FRONT)
                        return true;
                    break;
                case "Back":
                    if (dir == Direction.BACK)
                        return true;
                    break;
                case "Left":
                    if (dir == Direction.LEFT)
                        return true;
                    break;
                case "Right":
                    if (dir == Direction.RIGHT)
                        return true;
                    break;
                default:
                    if (dir == Direction.ANY)
                        return true;
                    break;
            }
        }
        return false;
    }

    public bool CheckAllArticulationsSuccess(Demo demo)
    {
        if (!demo.difficultyCustom.SyncArticulations)
        {
            //foreach (ArticulationMove movement in demo.Movements)
            //{
            //    if (CheckArticulationSuccess(movement, demo.difficultyCustom.PrecisionNeeded))
            //    {
            //        movement._done = true;
            //    }
            //}
            //if (demo.Movements.All(movement => movement._done == true))
            //    return true;
            //return false;

            // On check juste si UNE des conditions est valide
            bool isOneSuccess = false;
            foreach (ArticulationMove movement in demo.Movements)
            {
                if (!CheckArticulationSuccess(movement, demo.difficultyCustom.PrecisionNeeded))
                    isOneSuccess = true;
            }
            return isOneSuccess;
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
        // R�cup�re les valeurs d'articulation � tourner pour les deux mains
        float articulationToRotateLeft = ReturnArticulationToRotate(movement, armLeft.ArticulationsDict[movement.articulation].transform);
        float articulationToRotateRight = ReturnArticulationToRotate(movement, armRight.ArticulationsDict[movement.articulation].transform);

        float articulationRotation = 0f;

        if (movement.ExampleHand == Hand.Left)
            articulationRotation = ReturnArticulationToRotate(movement, armLeft.ArticulationsDict[movement.articulation].transform);
        else
            articulationRotation = ReturnArticulationToRotate(movement, armRight.ArticulationsDict[movement.articulation].transform);

        // R�cup�re la marge de pr�cision
        float precisionMarge = GetPrecisionMarge(precisionNeeded);


        // Verifier si le poignet est dans la bonne direction, si non, retourner faux
        if (movement.articulation == Articulations.Hand)
        {
            bool isWristGood = IsDirectionGood(movement.direction, (movement.ExampleHand == Hand.Left));

            if (movement.isOppositeDirection == false && isWristGood == true)
                return true;
            else if (movement.isOppositeDirection == true && isWristGood == false)
                return true;
            else
                return false;
        }

        // Check if if if if if if if if l'angle d'un doigt est en dessous ou au dessus de la limite demandée
        if (movement.MoveDir == true)
        {
            if (articulationRotation <= movement.ValueLimit + precisionMarge)
                return true;
            else
                return false;
        }
        else
        {
            if (articulationRotation >= movement.ValueLimit + precisionMarge)
                return true;
            else
                return false;
        }

        // V�rifie si la position de d�part est atteinte (si activ�e)
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

        // V�rifie si la main gauche ou droite est satisfaite par les limites de mouvement
        bool leftCheck = articulationToRotateLeft <= movement.ValueLimit + precisionMarge && articulationToRotateLeft >= movement.ValueLimit - precisionMarge;
        bool rightCheck = articulationToRotateRight <= movement.ValueLimit + precisionMarge && articulationToRotateRight >= movement.ValueLimit - precisionMarge;

        // Retourne vrai si les deux mains sont satisfaites par les limites de mouvement et pos de d�part respect�e
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
            // (oui tout aurait pu tenir dans une condition mais �a aurait �t� illisible)
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
                //les valeurs r�elles du player
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
