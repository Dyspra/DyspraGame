using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(ArmSetter))]
public class ArmDisplayer : MonoBehaviour
{
    ArmSetter armSetter;

    [ReadOnly, SerializeField] Vector3 HandAngles;

    [ReadOnly, SerializeField] Vector2 Finger1Angles;
    [ReadOnly, SerializeField] Vector2 Finger1Joint1Angles;

    [ReadOnly, SerializeField] Vector2 Finger2Angles;
    [ReadOnly, SerializeField] Vector2 Finger2Joint1Angles;
    [ReadOnly, SerializeField] Vector2 Finger2Joint2Angles;

    [ReadOnly, SerializeField] Vector2 Finger3Angles;
    [ReadOnly, SerializeField] Vector2 Finger3Joint1Angles;
    [ReadOnly, SerializeField] Vector2 Finger3Joint2Angles;

    [ReadOnly, SerializeField] Vector2 Finger4Angles;
    [ReadOnly, SerializeField] Vector2 Finger4Joint1Angles;
    [ReadOnly, SerializeField] Vector2 Finger4Joint2Angles;

    [ReadOnly, SerializeField] Vector2 Finger5Angles;
    [ReadOnly, SerializeField] Vector2 Finger5Joint1Angles;
    [ReadOnly, SerializeField] Vector2 Finger5Joint2Angles;
    void Start()
    {
        armSetter = GetComponent<ArmSetter>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // HandAngles = TransformUtils.GetInspectorRotation(armSetter.ArticulationsDict[Articulations.Hand].transform);
        // Finger1Angles = TransformUtils.GetInspectorRotation(armSetter.ArticulationsDict[Articulations.Finger1].transform);
        // Finger1Joint1Angles = TransformUtils.GetInspectorRotation(armSetter.ArticulationsDict[Articulations.Finger1Joint1].transform);
        // Finger2Angles = TransformUtils.GetInspectorRotation(armSetter.ArticulationsDict[Articulations.Finger2].transform);
        // Finger2Joint1Angles = TransformUtils.GetInspectorRotation(armSetter.ArticulationsDict[Articulations.Finger2Joint1].transform);
        // Finger2Joint2Angles = TransformUtils.GetInspectorRotation(armSetter.ArticulationsDict[Articulations.Finger2Joint2].transform);
        // Finger3Angles = TransformUtils.GetInspectorRotation(armSetter.ArticulationsDict[Articulations.Finger3].transform);
        // Finger3Joint1Angles = TransformUtils.GetInspectorRotation(armSetter.ArticulationsDict[Articulations.Finger3Joint1].transform);
        // Finger3Joint2Angles = TransformUtils.GetInspectorRotation(armSetter.ArticulationsDict[Articulations.Finger3Joint2].transform);
        // Finger4Angles = TransformUtils.GetInspectorRotation(armSetter.ArticulationsDict[Articulations.Finger4].transform);
        // Finger4Joint1Angles = TransformUtils.GetInspectorRotation(armSetter.ArticulationsDict[Articulations.Finger4Joint1].transform);
        // Finger4Joint2Angles = TransformUtils.GetInspectorRotation(armSetter.ArticulationsDict[Articulations.Finger4Joint2].transform);
        // Finger5Angles = TransformUtils.GetInspectorRotation(armSetter.ArticulationsDict[Articulations.Finger5].transform);
        // Finger5Joint1Angles = TransformUtils.GetInspectorRotation(armSetter.ArticulationsDict[Articulations.Finger5Joint1].transform);
        // Finger5Joint2Angles = TransformUtils.GetInspectorRotation(armSetter.ArticulationsDict[Articulations.Finger5Joint2].transform);
        // without TransformUtils:
        HandAngles = armSetter.ArticulationsDict[Articulations.Hand].transform.localEulerAngles;
        Finger1Angles = armSetter.ArticulationsDict[Articulations.Finger1].transform.localEulerAngles;
        Finger1Joint1Angles = armSetter.ArticulationsDict[Articulations.Finger1Joint1].transform.localEulerAngles;
        Finger2Angles = armSetter.ArticulationsDict[Articulations.Finger2].transform.localEulerAngles;
        Finger2Joint1Angles = armSetter.ArticulationsDict[Articulations.Finger2Joint1].transform.localEulerAngles;
        Finger2Joint2Angles = armSetter.ArticulationsDict[Articulations.Finger2Joint2].transform.localEulerAngles;
        Finger3Angles = armSetter.ArticulationsDict[Articulations.Finger3].transform.localEulerAngles;
        Finger3Joint1Angles = armSetter.ArticulationsDict[Articulations.Finger3Joint1].transform.localEulerAngles;
        Finger3Joint2Angles = armSetter.ArticulationsDict[Articulations.Finger3Joint2].transform.localEulerAngles;
        Finger4Angles = armSetter.ArticulationsDict[Articulations.Finger4].transform.localEulerAngles;
        Finger4Joint1Angles = armSetter.ArticulationsDict[Articulations.Finger4Joint1].transform.localEulerAngles;
        Finger4Joint2Angles = armSetter.ArticulationsDict[Articulations.Finger4Joint2].transform.localEulerAngles;
        Finger5Angles = armSetter.ArticulationsDict[Articulations.Finger5].transform.localEulerAngles;
        Finger5Joint1Angles = armSetter.ArticulationsDict[Articulations.Finger5Joint1].transform.localEulerAngles;
        Finger5Joint2Angles = armSetter.ArticulationsDict[Articulations.Finger5Joint2].transform.localEulerAngles;
    }
}
