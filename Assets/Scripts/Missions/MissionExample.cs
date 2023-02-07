using UnityEngine;

public class MissionExample : Dyspra.AbstractMission
{

    [SerializeField] private GameObject sphere;

    public void Step1()
    {
        Debug.Log("STEP 1 DONE");
        Instantiate(sphere, new Vector3(7, 10, -5), Quaternion.identity);
        Instantiate(sphere, new Vector3(-7, 10, -5), Quaternion.identity);
        Instantiate(sphere, new Vector3(-7, 10, 5), Quaternion.identity);
        MissionEventComplete();
    }

    public void Step2()
    {
        Debug.Log("STEP 2 DONE");
        Instantiate(sphere, new Vector3(7, 10, 5), Quaternion.identity);
        MissionEventComplete();
    }

    public void Step3()
    {
        Debug.Log("STEP 3 DONE");
        Debug.Log("Mission complete!");
        MissionEventComplete();
    }

}
