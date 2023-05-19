using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEditor;

public class UnitTest
{
    [UnityTest]
    public IEnumerator ApplyBasicTuto()
    {
        // GameObject Player = Object.Instantiate(AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Player.prefab"));
        // without AssetDatabase:
        GameObject Player = Object.Instantiate(Resources.Load<GameObject>("Player"));
        // GameObject ExempleCamera = Object.Instantiate(AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/ExempleCamera.prefab"));
        // without AssetDatabase:
        GameObject ExempleCamera = Object.Instantiate(Resources.Load<GameObject>("ExempleCamera"));

        ExempleCamera.GetComponent<FirstExercice>().PlayerHandsManager = Player.transform.Find("Hands").GetComponent<HandsManager>();
        ExempleCamera.GetComponent<FirstExercice>().PlayedDemo = 0;

        yield return new WaitForSeconds(2f);
        // Assert.AreEqual(40.2f, TransformUtils.GetInspectorRotation(ExempleCamera.transform.Find("Hands").Find("LeftHand").transform).x);
        // without TransformUtils:
        Assert.AreEqual(40.2f, ExempleCamera.transform.Find("Hands").Find("LeftHand").transform.localRotation.eulerAngles.x);

        Object.Destroy(Player);
        Object.Destroy(ExempleCamera);
    }

    [UnityTest]
    public IEnumerator ApplyBasicPlayerMovement()
    {
        // GameObject Player = Object.Instantiate(AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Player.prefab"));
        // without AssetDatabase:
        GameObject Player = Object.Instantiate(Resources.Load<GameObject>("Player"));
        // GameObject ExempleCamera = Object.Instantiate(AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/ExempleCamera.prefab"));
        // without AssetDatabase:
        GameObject ExempleCamera = Object.Instantiate(Resources.Load<GameObject>("ExempleCamera"));

        ExempleCamera.GetComponent<FirstExercice>().PlayerHandsManager = Player.transform.Find("Hands").GetComponent<HandsManager>();
        ExempleCamera.GetComponent<FirstExercice>().PlayedDemo = 0;

        Player.transform.Find("Hands").Find("LeftHand").GetComponent<ArmControl>().HandAngles = new Vector3(0.5f, 0f, 0f);

        yield return new WaitForSeconds(1.4f);
        
        Player.transform.Find("Hands").Find("LeftHand").GetComponent<ArmControl>().HandAngles = new Vector3(0f, 0f, 0f);
        
        yield return new WaitForSeconds(2f);
        
        Assert.AreEqual(true, ExempleCamera.GetComponent<FirstExercice>().isSuccess);

        Object.Destroy(Player);
        Object.Destroy(ExempleCamera);
    }

    [UnityTest]
    public IEnumerator ApplyBasicPlayerMovementCustomPos()
    {
        // GameObject Player = Object.Instantiate(AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Player.prefab"));
        // without AssetDatabase:
        GameObject Player = Object.Instantiate(Resources.Load<GameObject>("Player"));
        // GameObject ExempleCamera = Object.Instantiate(AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/ExempleCamera.prefab"));
        // without AssetDatabase:
        GameObject ExempleCamera = Object.Instantiate(Resources.Load<GameObject>("ExempleCamera"));

        ExempleCamera.GetComponent<FirstExercice>().PlayerHandsManager = Player.transform.Find("Hands").GetComponent<HandsManager>();
        ExempleCamera.GetComponent<FirstExercice>().PlayedDemo = 3;
        ExempleCamera.GetComponent<FirstExercice>().Demos[3].hasCustomPosLeft = true;

        Player.transform.Find("Hands").Find("LeftHand").GetComponent<ArmControl>().HandAngles = new Vector3(0.5f, 0f, 0f);
        Player.transform.Find("Hands").Find("RightHand").GetComponent<ArmControl>().HandAngles = new Vector3(-0.5f, 0f, 0f);
        
        Player.transform.Find("Hands").Find("LeftHand").GetComponent<ArmSetter>().arm.Finger2.transform.localRotation = Quaternion.Euler(-37f, 0f, 0f);

        yield return new WaitForSeconds(1.8f);

        Player.transform.Find("Hands").Find("LeftHand").GetComponent<ArmControl>().HandAngles = new Vector3(0f, 0f, 0f);
        Player.transform.Find("Hands").Find("RightHand").GetComponent<ArmControl>().HandAngles = new Vector3(0f, 0f, 0f);

        yield return new WaitForSeconds(2f);

        Assert.AreEqual(true, ExempleCamera.GetComponent<FirstExercice>().isSuccess);

        Object.Destroy(Player);
        Object.Destroy(ExempleCamera);
    }

    [UnityTest]
    public IEnumerator ApplyBasicPlayerMovementCustomStartPos()
    {
        // GameObject Player = Object.Instantiate(AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Player.prefab"));
        // without AssetDatabase:
        GameObject Player = Object.Instantiate(Resources.Load<GameObject>("Player"));
        // GameObject ExempleCamera = Object.Instantiate(AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/ExempleCamera.prefab"));
        // without AssetDatabase:
        GameObject ExempleCamera = Object.Instantiate(Resources.Load<GameObject>("ExempleCamera"));

        ExempleCamera.GetComponent<FirstExercice>().PlayerHandsManager = Player.transform.Find("Hands").GetComponent<HandsManager>();
        ExempleCamera.GetComponent<FirstExercice>().PlayedDemo = 4;

        Player.transform.Find("Hands").Find("LeftHand").GetComponent<ArmControl>().Finger2Joint1Angles = new Vector3(-0.5f, 0f, 0f);
        Player.transform.Find("Hands").Find("LeftHand").GetComponent<ArmControl>().Finger3Joint1Angles = new Vector3(-0.5f, 0f, 0f);

        yield return new WaitForSeconds(1.9f);

        Player.transform.Find("Hands").Find("LeftHand").GetComponent<ArmControl>().Finger2Joint1Angles = new Vector3(0f, 0f, 0f);
        Player.transform.Find("Hands").Find("LeftHand").GetComponent<ArmControl>().Finger3Joint1Angles = new Vector3(0f, 0f, 0f);

        yield return new WaitForSeconds(0.5f);
        
        Player.transform.Find("Hands").Find("LeftHand").GetComponent<ArmControl>().Finger2Joint1Angles = new Vector3(0.5f, 0f, 0f);
        Player.transform.Find("Hands").Find("LeftHand").GetComponent<ArmControl>().Finger3Joint1Angles = new Vector3(0.5f, 0f, 0f);
        
        yield return new WaitForSeconds(1.9f);

        Player.transform.Find("Hands").Find("LeftHand").GetComponent<ArmControl>().Finger2Joint1Angles = new Vector3(0f, 0f, 0f);
        Player.transform.Find("Hands").Find("LeftHand").GetComponent<ArmControl>().Finger3Joint1Angles = new Vector3(0f, 0f, 0f);
        
        yield return new WaitForSeconds(2f);

        Assert.AreEqual(true, ExempleCamera.GetComponent<FirstExercice>().isSuccess);

        Object.Destroy(Player);
        Object.Destroy(ExempleCamera);
    }

    [UnityTest]
    public IEnumerator Instantiate_Player()
    {
        // var Player_Prefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Player.prefab");
        // without AssetDatabase:
        var Player_Prefab = Resources.Load<GameObject>("Player");
        GameObject Player = Object.Instantiate(Player_Prefab);

        yield return null;
        Assert.AreEqual(Player_Prefab.transform.Find("Hands"), Player.transform.Find("Hands"));
    }

    [UnityTest]
    public IEnumerator Instantiate_Player_OLD_MODEL()
    {
        // var Old_Player_Prefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Old Player.prefab");
        // without AssetDatabase:
        var Old_Player_Prefab = Resources.Load<GameObject>("Old Player");
        GameObject Old_Player = Object.Instantiate(Old_Player_Prefab);

        yield return null;
        Assert.AreEqual(Old_Player_Prefab.transform.Find("Arms High"), Old_Player.transform.Find("Arms High"));
    }
}
