using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThunderBolt_Script : MonoBehaviour
{
    private enum ThunderBoltPowerState
    {
        CHARGING,
        READY_TO_SHOT,
        NONE
    }

    private ThunderBoltPowerState _state = ThunderBoltPowerState.NONE;
    [SerializeField] private GameObject _thunderBoltObj;
    [SerializeField] private float _timeToCharge = 2.0f;
    private float _timer = 0.0f;
    [SerializeField] private bool _isReadyToShot = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Update()
    {
        switch (_state)
        {
            case ThunderBoltPowerState.CHARGING:
                UpdateCharging();
                break;
            case ThunderBoltPowerState.READY_TO_SHOT:
                UpdateReadyToShot();
                break;
            default:
                UpdateNoState();
                break;
        }
    }

    public void StartChargingThunder()
    {
        Debug.Log("StartCharging");
    }

    public void LaunchThunderBolt()
    {
        Debug.Log("Fire!");
    }

    public void GetReadyToShot()
    {
        Debug.Log("Ready to shot!");
    }

    private void UpdateCharging()
    {
        Debug.Log("Charging");
        _timer += Time.deltaTime;

        if (_timer >= _timeToCharge)
        {
            _state = ThunderBoltPowerState.READY_TO_SHOT;
            _timer = 0.0f;
        }
    }

    private void UpdateReadyToShot()
    {

    }

    private void UpdateNoState()
    {
        _timer = 0.0f;
    }
}
