using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MovementDetector : MonoBehaviour
{
    public Demo Demo;
    public UnityEvent successFunction;
    HandsManager PlayerHandsManager;

    void Start()
    {
        PlayerHandsManager = FindObjectOfType<HandsManager>();
    }

    void Update()
    {
        if (PlayerHandsManager.CheckAllArticulationsSuccess(Demo) && PlayerHandsManager.checkStaticPos(Demo))
        {
            successFunction.Invoke();
            ResetMovement();
        }
    }

    void ResetMovement()
    {
        Demo.Movements.ForEach(x => x._done = false);
        Demo.Movements.ForEach(x => x._startPosLeftDone = false);
        Demo.Movements.ForEach(x => x._startPosRightDone = false);
    }
}
