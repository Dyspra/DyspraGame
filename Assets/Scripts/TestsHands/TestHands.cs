using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEditor;

public class UnitTest
{
    int i_test = 0;
    void intChanger()
    {
        i_test++;
    }

    [UnityTest]
    public IEnumerator BasicMovementDetector()
    {
        GameObject Player = Object.Instantiate(AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Player.prefab"));
        GameObject MovementDetector = Object.Instantiate(AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/MovementDetectors/MovementDetector BasicHandsBack.prefab"));

        i_test = 0;
        MovementDetector.GetComponent<MovementDetector>().successFunction.AddListener(intChanger);
        Player.transform.Find("RootLeft").Find("LeftHand").GetComponent<ArmControl>().HandAngles = new Vector3(0f, 0f, 0.5f);

        yield return new WaitForSeconds(2f);
        Assert.AreEqual(1, i_test);

        Object.Destroy(Player);
        Object.Destroy(MovementDetector);
    }

    [UnityTest]
    public IEnumerator FailedObjectAttraction()
    {
        GameObject Player = Object.Instantiate(AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Player.prefab"));
        AttractToTarget AttractToTarget = Object.Instantiate(AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Object Attraction.prefab")).GetComponent<AttractToTarget>();
        AttractToTarget.target = Player.transform.Find("RootRight").Find("RightHand");
        GameObject Cube = Object.Instantiate(AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Cube.prefab"));
        Cube.transform.position = new Vector3(16.01f, 0.05f, 17.38f);
        Cube.GetComponent<Rigidbody>().useGravity = false;

        yield return new WaitForSeconds(0.1f);
        AttractToTarget.StartAttraction();
        yield return new WaitForSeconds(2f);
        Assert.AreEqual(new Vector3(16.01f, 0.05f, 17.38f), Cube.transform.position);

        Object.Destroy(Player);
        Object.Destroy(AttractToTarget.gameObject);
        Object.Destroy(Cube);
    }

    [UnityTest]
    public IEnumerator SuccessfulObjectAttraction()
    {
        GameObject Player = Object.Instantiate(AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Player.prefab"));
        AttractToTarget AttractToTarget = Object.Instantiate(AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Object Attraction.prefab")).GetComponent<AttractToTarget>();
        AttractToTarget.target = Player.transform.Find("RootRight").Find("RightHand");
        GameObject Cube = Object.Instantiate(AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Cube.prefab"));
        Cube.transform.position = new Vector3(20f, 3.1f, 15f);

        yield return new WaitForSeconds(0.1f);
        AttractToTarget.StartAttraction();
        yield return new WaitForSeconds(2f);
        Debug.Log(Cube.transform.position);
        Assert.LessOrEqual(Vector3.Distance(new Vector3(19.7f, 0.6f, 12.5f), Cube.transform.position), 0.1f);

        Object.Destroy(Player);
        Object.Destroy(AttractToTarget.gameObject);
        Object.Destroy(Cube);
    }

    [UnityTest]
    public IEnumerator ApplyBasicTuto()
    {
        GameObject Player = Object.Instantiate(AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Player.prefab"));
        GameObject ExempleCamera = Object.Instantiate(AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/ExempleCamera.prefab"));

        ExempleCamera.GetComponent<FirstExercice>().PlayerHandsManager = Player.transform.Find("Hands").GetComponent<HandsManager>();
        ExempleCamera.GetComponent<FirstExercice>().PlayedDemo = 0;

        yield return new WaitForSeconds(2f);
        Assert.AreEqual(40.2f, TransformUtils.GetInspectorRotation(ExempleCamera.transform.Find("Hands").Find("LeftHand").transform).x);

        Object.Destroy(Player);
        Object.Destroy(ExempleCamera);
    }

    [UnityTest]
    public IEnumerator ApplyBasicPlayerMovement()
    {
        GameObject Player = Object.Instantiate(AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Player.prefab"));
        GameObject ExempleCamera = Object.Instantiate(AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/ExempleCamera.prefab"));

        ExempleCamera.GetComponent<FirstExercice>().PlayerHandsManager = Player.transform.Find("Hands").GetComponent<HandsManager>();
        ExempleCamera.GetComponent<FirstExercice>().PlayedDemo = 0;

        Player.transform.Find("RootLeft").Find("LeftHand").GetComponent<ArmControl>().HandAngles = new Vector3(0.5f, 0f, 0f);

        yield return new WaitForSeconds(1.4f);
        
        Player.transform.Find("RootLeft").Find("LeftHand").GetComponent<ArmControl>().HandAngles = new Vector3(0f, 0f, 0f);
        
        yield return new WaitForSeconds(2f);
        
        Assert.AreEqual(true, ExempleCamera.GetComponent<FirstExercice>().isSuccess);

        Object.Destroy(Player);
        Object.Destroy(ExempleCamera);
    }

    [UnityTest]
    public IEnumerator ApplyBasicPlayerMovementCustomPos()
    {
        GameObject Player = Object.Instantiate(AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Player.prefab"));
        GameObject ExempleCamera = Object.Instantiate(AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/ExempleCamera.prefab"));

        ExempleCamera.GetComponent<FirstExercice>().PlayerHandsManager = Player.transform.Find("Hands").GetComponent<HandsManager>();
        ExempleCamera.GetComponent<FirstExercice>().PlayedDemo = 3;
        ExempleCamera.GetComponent<FirstExercice>().Demos[3].hasCustomPosLeft = true;

        Player.transform.Find("RootLeft").Find("LeftHand").GetComponent<ArmControl>().HandAngles = new Vector3(0.5f, 0f, 0f);
        Player.transform.Find("RootRight").Find("RightHand").GetComponent<ArmControl>().HandAngles = new Vector3(-0.5f, 0f, 0f);
        
        Player.transform.Find("RootLeft").Find("LeftHand").GetComponent<ArmSetter>().arm.Finger2.transform.localRotation = Quaternion.Euler(-37f, 0f, 0f);

        yield return new WaitForSeconds(1.8f);

        Player.transform.Find("RootLeft").Find("LeftHand").GetComponent<ArmControl>().HandAngles = new Vector3(0f, 0f, 0f);
        Player.transform.Find("RootRight").Find("RightHand").GetComponent<ArmControl>().HandAngles = new Vector3(0f, 0f, 0f);

        yield return new WaitForSeconds(2f);

        Assert.AreEqual(true, ExempleCamera.GetComponent<FirstExercice>().isSuccess);

        Object.Destroy(Player);
        Object.Destroy(ExempleCamera);
    }

    [UnityTest]
    public IEnumerator ApplyBasicPlayerMovementCustomStartPos()
    {
        GameObject Player = Object.Instantiate(AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Player.prefab"));
        GameObject ExempleCamera = Object.Instantiate(AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/ExempleCamera.prefab"));

        ExempleCamera.GetComponent<FirstExercice>().PlayerHandsManager = Player.transform.Find("Hands").GetComponent<HandsManager>();
        ExempleCamera.GetComponent<FirstExercice>().PlayedDemo = 4;

        Player.transform.Find("RootLeft").Find("LeftHand").GetComponent<ArmControl>().Finger2Joint1Angles = new Vector3(-0.5f, 0f, 0f);
        Player.transform.Find("RootLeft").Find("LeftHand").GetComponent<ArmControl>().Finger3Joint1Angles = new Vector3(-0.5f, 0f, 0f);

        yield return new WaitForSeconds(1.9f);

        Player.transform.Find("RootLeft").Find("LeftHand").GetComponent<ArmControl>().Finger2Joint1Angles = new Vector3(0f, 0f, 0f);
        Player.transform.Find("RootLeft").Find("LeftHand").GetComponent<ArmControl>().Finger3Joint1Angles = new Vector3(0f, 0f, 0f);

        yield return new WaitForSeconds(0.5f);
        
        Player.transform.Find("RootLeft").Find("LeftHand").GetComponent<ArmControl>().Finger2Joint1Angles = new Vector3(0.5f, 0f, 0f);
        Player.transform.Find("RootLeft").Find("LeftHand").GetComponent<ArmControl>().Finger3Joint1Angles = new Vector3(0.5f, 0f, 0f);
        
        yield return new WaitForSeconds(1.9f);

        Player.transform.Find("RootLeft").Find("LeftHand").GetComponent<ArmControl>().Finger2Joint1Angles = new Vector3(0f, 0f, 0f);
        Player.transform.Find("RootLeft").Find("LeftHand").GetComponent<ArmControl>().Finger3Joint1Angles = new Vector3(0f, 0f, 0f);
        
        yield return new WaitForSeconds(2f);

        Assert.AreEqual(true, ExempleCamera.GetComponent<FirstExercice>().isSuccess);

        Object.Destroy(Player);
        Object.Destroy(ExempleCamera);
    }

    [UnityTest]
    public IEnumerator Instantiate_Player()
    {
        var Player_Prefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Player.prefab");
        GameObject Player = Object.Instantiate(Player_Prefab);

        yield return null;
        Assert.AreEqual(Player_Prefab.transform.Find("RootLeft"), Player.transform.Find("RootLeft"));
        Assert.AreEqual(Player_Prefab.transform.Find("RootRight"), Player.transform.Find("RootRight"));
    }

    //[UnityTest]
    //public IEnumerator Instantiate_Player_OLD_MODEL()
    //{
    //    var Old_Player_Prefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Old Player.prefab");
    //    GameObject Old_Player = Object.Instantiate(Old_Player_Prefab);
    //
    //    yield return null;
    //    Assert.AreEqual(Old_Player_Prefab.transform.Find("Arms High"), Old_Player.transform.Find("Arms High"));
    //}
}
