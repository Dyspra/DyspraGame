using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public enum Articulations
{
    Hand,
    Finger1,
    Finger1Joint1,
    Finger2,
    Finger2Joint1,
    Finger2Joint2,
    Finger3,
    Finger3Joint1,
    Finger3Joint2,
    Finger4,
    Finger4Joint1,
    Finger4Joint2,
    Finger5,
    Finger5Joint1,
    Finger5Joint2,
}

[Serializable]
public class ArmRotationDatas
{
    public Vector3 Hand;
    public Vector3 Finger1;
    public Vector3 Finger1Joint1;
    public Vector3 Finger2;
    public Vector3 Finger2Joint1;
    public Vector3 Finger2Joint2;
    public Vector3 Finger3;
    public Vector3 Finger3Joint1;
    public Vector3 Finger3Joint2;
    public Vector3 Finger4;
    public Vector3 Finger4Joint1;
    public Vector3 Finger4Joint2;
    public Vector3 Finger5;
    public Vector3 Finger5Joint1;
    public Vector3 Finger5Joint2;

    public ArmRotationDatas() 
    {
        this.Hand = Vector3.zero;
        this.Finger1 = Vector3.zero;
        this.Finger1Joint1 = Vector3.zero;
        this.Finger2 = Vector3.zero;
        this.Finger2Joint1 = Vector3.zero;
        this.Finger2Joint2 = Vector3.zero;
        this.Finger3 = Vector3.zero;
        this.Finger3Joint1 = Vector3.zero;
        this.Finger3Joint2 = Vector3.zero;
        this.Finger4 = Vector3.zero;
        this.Finger4Joint1 = Vector3.zero;
        this.Finger4Joint2 = Vector3.zero;
        this.Finger5 = Vector3.zero;
        this.Finger5Joint1 = Vector3.zero;
        this.Finger5Joint2 = Vector3.zero;
    }

    public ArmRotationDatas(ArmRotationDatas armRotationDatas)
    {
        this.Hand = armRotationDatas.Hand;
        this.Finger1 = armRotationDatas.Finger1;
        this.Finger1Joint1 = armRotationDatas.Finger1Joint1;
        this.Finger2 = armRotationDatas.Finger2;
        this.Finger2Joint1 = armRotationDatas.Finger2Joint1;
        this.Finger2Joint2 = armRotationDatas.Finger2Joint2;
        this.Finger3 = armRotationDatas.Finger3;
        this.Finger3Joint1 = armRotationDatas.Finger3Joint1;
        this.Finger3Joint2 = armRotationDatas.Finger3Joint2;
        this.Finger4 = armRotationDatas.Finger4;
        this.Finger4Joint1 = armRotationDatas.Finger4Joint1;
        this.Finger4Joint2 = armRotationDatas.Finger4Joint2;
        this.Finger5 = armRotationDatas.Finger5;
        this.Finger5Joint1 = armRotationDatas.Finger5Joint1;
        this.Finger5Joint2 = armRotationDatas.Finger5Joint2;
    }

    public void GetRotationToArm(Arm arm)
    {
        this.Hand = arm.Hand.transform.localRotation.eulerAngles;
        this.Finger1 = arm.Finger1.transform.localRotation.eulerAngles;
        this.Finger1Joint1 = arm.Finger1Joint1.transform.localRotation.eulerAngles;
        this.Finger2 = arm.Finger2.transform.localRotation.eulerAngles;
        this.Finger2Joint1 = arm.Finger2Joint1.transform.localRotation.eulerAngles;
        this.Finger2Joint2 = arm.Finger2Joint2.transform.localRotation.eulerAngles;
        this.Finger3 = arm.Finger3.transform.localRotation.eulerAngles;
        this.Finger3Joint1 = arm.Finger3Joint1.transform.localRotation.eulerAngles;
        this.Finger3Joint2 = arm.Finger3Joint2.transform.localRotation.eulerAngles;
        this.Finger4 = arm.Finger4.transform.localRotation.eulerAngles;
        this.Finger4Joint1 = arm.Finger4Joint1.transform.localRotation.eulerAngles;
        this.Finger4Joint2 = arm.Finger4Joint2.transform.localRotation.eulerAngles;
        this.Finger5 = arm.Finger5.transform.localRotation.eulerAngles;
        this.Finger5Joint1 = arm.Finger5Joint1.transform.localRotation.eulerAngles;
        this.Finger5Joint2 = arm.Finger5Joint2.transform.localRotation.eulerAngles;
    }

    public void Combine(ArmRotationDatas armRotationDatasCombine)
    {
        //this.Hand *= armRotationDatasCombine.Hand;
        //this.Finger1 *= armRotationDatasCombine.Finger1;
        //this.Finger1Joint1 *= armRotationDatasCombine.Finger1Joint1;
        //this.Finger2 *= armRotationDatasCombine.Finger2;
        //this.Finger2Joint1 *= armRotationDatasCombine.Finger2Joint1;
        //this.Finger2Joint2 *= armRotationDatasCombine.Finger2Joint2;
        //this.Finger3 *= armRotationDatasCombine.Finger3;
        //this.Finger3Joint1 *= armRotationDatasCombine.Finger3Joint1;
        //this.Finger3Joint2 *= armRotationDatasCombine.Finger3Joint2;
        //this.Finger4 *= armRotationDatasCombine.Finger4;
        //this.Finger4Joint1 *= armRotationDatasCombine.Finger4Joint1;
        //this.Finger4Joint2 *= armRotationDatasCombine.Finger4Joint2;
        //this.Finger5 *= armRotationDatasCombine.Finger5;
        //this.Finger5Joint1 *= armRotationDatasCombine.Finger5Joint1;
        //this.Finger5Joint2 *= armRotationDatasCombine.Finger5Joint2;
    }
}

[Serializable]
public class Arm
{
    public GameObject Hand;
    [HideInInspector]
    public GameObject Finger1;
    [HideInInspector]
    public GameObject Finger1Joint1;
    [HideInInspector]
    public GameObject Finger2;
    [HideInInspector]
    public GameObject Finger2Joint1;
    [HideInInspector]
    public GameObject Finger2Joint2;
    [HideInInspector]
    public GameObject Finger3;
    [HideInInspector]
    public GameObject Finger3Joint1;
    [HideInInspector]
    public GameObject Finger3Joint2;
    [HideInInspector]
    public GameObject Finger4;
    [HideInInspector]
    public GameObject Finger4Joint1;
    [HideInInspector]
    public GameObject Finger4Joint2;
    [HideInInspector]
    public GameObject Finger5;
    [HideInInspector]
    public GameObject Finger5Joint1;
    [HideInInspector]
    public GameObject Finger5Joint2;

    public void SetRotationToArm(ArmRotationDatas armRotation)
    {
        this.Hand.transform.localRotation = GetQuaternionFromVector3(armRotation.Hand);
        this.Finger1.transform.localRotation = GetQuaternionFromVector3(armRotation.Finger1);
        this.Finger1Joint1.transform.localRotation = GetQuaternionFromVector3(armRotation.Finger1Joint1);
        this.Finger2.transform.localRotation = GetQuaternionFromVector3(armRotation.Finger2);
        this.Finger2Joint1.transform.localRotation = GetQuaternionFromVector3(armRotation.Finger2Joint1);
        this.Finger2Joint2.transform.localRotation = GetQuaternionFromVector3(armRotation.Finger2Joint2);
        this.Finger3.transform.localRotation = GetQuaternionFromVector3(armRotation.Finger3);
        this.Finger3Joint1.transform.localRotation = GetQuaternionFromVector3(armRotation.Finger3Joint1);
        this.Finger3Joint2.transform.localRotation = GetQuaternionFromVector3(armRotation.Finger3Joint2);
        this.Finger4.transform.localRotation = GetQuaternionFromVector3(armRotation.Finger4);
        this.Finger4Joint1.transform.localRotation = GetQuaternionFromVector3(armRotation.Finger4Joint1);
        this.Finger4Joint2.transform.localRotation = GetQuaternionFromVector3(armRotation.Finger4Joint2);
        this.Finger5.transform.localRotation = GetQuaternionFromVector3(armRotation.Finger5);
        this.Finger5Joint1.transform.localRotation = GetQuaternionFromVector3(armRotation.Finger5Joint1);
        this.Finger5Joint2.transform.localRotation = GetQuaternionFromVector3(armRotation.Finger5Joint2);
    }

    public Quaternion GetQuaternionFromVector3(Vector3 vector)
    {
        Quaternion rot = new Quaternion();
        rot.eulerAngles = vector;
        return rot;
    }
}

[RequireComponent(typeof(ArmControl))]
public class ArmSetter : MonoBehaviour
{
    [SerializeField]
    public Arm arm = new Arm();
    public bool oldModel = false;
    //Dictionnaire pour lier l'enum Articulations aux GameObjects d'articulations, pour trouver le bon gameobject au moment du mouvement
    public Dictionary<Articulations, GameObject> ArticulationsDict = new Dictionary<Articulations, GameObject>();

    public void Awake()
    {
        if (oldModel)
            SetEachFingerAndJoints_OLD_MODEL();
        else
            SetEachFingerAndJoints();
        SetArticulationsDict();
    }

    void SetEachFingerAndJoints()
    {
        foreach (Transform child in arm.Hand.transform.Find("hand").Find("root"))
        {
            if (child.name == "R_FK_Thumb_null")
            {
                arm.Finger1 = child.Find("R_Thumb_null").Find("R_NB_FK_Thumb_Root_jnt").Find("R_NB_FK_Thumb_Base_jnt").gameObject;
                arm.Finger1Joint1 = arm.Finger1.transform.Find("R_NB_FK_Thumb_Mid_jnt").gameObject;
            }
            if (child.name == "R_FK_Finger_null")
            {
                arm.Finger2 = child.Find("R_FK_Finger_null_2_3").Find("R_NB_FK_Finger_Palm_Base_jnt").Find("R_NB_FK_Finger_Base_jnt").gameObject;
                arm.Finger2Joint1 = arm.Finger2.transform.GetChild(0).gameObject;
                arm.Finger2Joint2 = arm.Finger2Joint1.transform.GetChild(0).gameObject;
            }
            if (child.name == "R_FK_Finger_null_1")
            {
                arm.Finger3 = child.Find("R_FK_Finger_null_1_2").Find("R_NB_FK_Finger_Palm_Base_jnt_1").Find("R_NB_FK_Finger_Base_jnt_1").gameObject;
                arm.Finger3Joint1 = arm.Finger3.transform.GetChild(0).gameObject;
                arm.Finger3Joint2 = arm.Finger3Joint1.transform.GetChild(0).gameObject;
            }
            if (child.name == "R_FK_Finger_null_2")
            {
                arm.Finger4 = child.Find("R_FK_Finger_null_2_2").Find("R_NB_FK_Finger_Palm_Base_jnt_2").Find("R_NB_FK_Finger_Base_jnt_2").gameObject;
                arm.Finger4Joint1 = arm.Finger4.transform.GetChild(0).gameObject;
                arm.Finger4Joint2 = arm.Finger4Joint1.transform.GetChild(0).gameObject;
            }
            if (child.name == "R_FK_Finger_null_3")
            {
                arm.Finger5 = child.Find("R_FK_Finger_null_3_2").Find("R_NB_FK_Finger_Palm_Base_jnt_3").Find("R_NB_FK_Finger_Base_jnt_3").gameObject;
                arm.Finger5Joint1 = arm.Finger5.transform.GetChild(0).gameObject;
                arm.Finger5Joint2 = arm.Finger5Joint1.transform.GetChild(0).gameObject;
            }
        }
    }

    void SetEachFingerAndJoints_OLD_MODEL()
    {
        //all hands components will be found from the hand component automatically
        String armSide = (arm.Hand.name == "LeftHand") ? "Left" : "Right";

        foreach (Transform child in arm.Hand.transform)
        {
            if (child.name == armSide + "HandThumb1")
            {
                arm.Finger1 = child.gameObject;
                arm.Finger1Joint1 = arm.Finger1.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject;
            }
            if (child.name == armSide + "HandIndex1")
            {
                arm.Finger2 = child.gameObject;
                arm.Finger2Joint1 = arm.Finger2.transform.GetChild(0).gameObject;
                arm.Finger2Joint2 = arm.Finger2Joint1.transform.GetChild(0).gameObject;
            }
            if (child.name == armSide + "HandMiddle1")
            {
                arm.Finger3 = child.gameObject;
                arm.Finger3Joint1 = arm.Finger3.transform.GetChild(0).gameObject;
                arm.Finger3Joint2 = arm.Finger3Joint1.transform.GetChild(0).gameObject;
            }
            if (child.name == armSide + "HandRing1")
            {
                arm.Finger4 = child.gameObject;
                arm.Finger4Joint1 = arm.Finger4.transform.GetChild(0).gameObject;
                arm.Finger4Joint2 = arm.Finger4Joint1.transform.GetChild(0).gameObject;
            }
            if (child.name == armSide + "HandPinky1")
            {
                arm.Finger5 = child.gameObject;
                arm.Finger5Joint1 = arm.Finger5.transform.GetChild(0).gameObject;
                arm.Finger5Joint2 = arm.Finger5Joint1.transform.GetChild(0).gameObject;
            }
        }
    }

    void SetArticulationsDict()
    {
        //set the dictionary of joints to be able to select the articulations to move in the inspector easily
        foreach (FieldInfo articulation in typeof(Arm).GetFields())
        {
            foreach (Articulations articulationEnum in Enum.GetValues(typeof(Articulations)))
            {
                if (articulation.Name == articulationEnum.ToString())
                {
                    ArticulationsDict.Add(articulationEnum, articulation.GetValue(this.arm) as GameObject);
                    continue;
                }

            }
        }
    }
}
