using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThunderBolt_Script : MonoBehaviour
{
    private enum ThunderBoltPowerState
    {
        CHARGING,
        READY_TO_SHOT,
        NONE,
        LEFT_THUNDER,
        RIGHT_THUNDER
    }

    [SerializeField] private Material _sphereReadyMat;
    [SerializeField] private GameObject _leftHand;
    [SerializeField] private GameObject _rightHand;
    [SerializeField] private GameObject _thunderBoltObj;
    [SerializeField] private GameObject _currentBoltObj;
    [SerializeField] private GameObject _targetCylinderObj;
    [SerializeField] private ThunderBoltPowerState _state = ThunderBoltPowerState.NONE;
    [SerializeField] private ThunderBoltPowerState _handSideState = ThunderBoltPowerState.NONE;
    [SerializeField] private float _timeToCharge = 2.0f;
    [SerializeField] private float _timer = 0.0f;
    [SerializeField] private float _projectionVelocity = 700.0f;

    #region Unity methods
    private void Start()
    {
        if (_targetCylinderObj != null)
            _targetCylinderObj.SetActive(false);
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
    #endregion

    #region Start charging
    public void StartChargingLeftThunder()
    {
        if (_state != ThunderBoltPowerState.NONE)
            return;
        _handSideState = ThunderBoltPowerState.LEFT_THUNDER;
        //Debug.Log("StartCharging");
        _state = ThunderBoltPowerState.CHARGING;
        _currentBoltObj = Instantiate(_thunderBoltObj, this.transform.position, this.transform.rotation);
    }

    public void StartChargingRightThunder()
    {
        if (_state != ThunderBoltPowerState.NONE)
            return;
        _handSideState = ThunderBoltPowerState.RIGHT_THUNDER;
        //Debug.Log("StartCharging");
        _state = ThunderBoltPowerState.CHARGING;
        _currentBoltObj = Instantiate(_thunderBoltObj, this.transform.position, this.transform.rotation);
    }
    #endregion

    #region Thunderbolt launching
    private void LaunchThunderBolt()
    {
        //Debug.Log("Fire!");
        if (_currentBoltObj != null)
        {
            _currentBoltObj.GetComponent<Rigidbody>().isKinematic = false;
            _currentBoltObj.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            _currentBoltObj.GetComponent<Rigidbody>().AddRelativeForce((_targetCylinderObj.transform.position - _currentBoltObj.transform.position) * _projectionVelocity);
            _currentBoltObj = null;
        }
        _targetCylinderObj.SetActive(false);
        _state = ThunderBoltPowerState.NONE;
    }
    public void LaunchRightThunderBolt()
    {
        if (_state != ThunderBoltPowerState.READY_TO_SHOT)
            return;
        //Debug.Log("Try Fire Right");
        if (_handSideState == ThunderBoltPowerState.RIGHT_THUNDER && _state == ThunderBoltPowerState.READY_TO_SHOT)
            LaunchThunderBolt();
    }

    public void LaunchLeftThunderBolt()
    {
        if (_state != ThunderBoltPowerState.READY_TO_SHOT)
            return;
        //Debug.Log("Try Fire Left");
        if (_handSideState == ThunderBoltPowerState.LEFT_THUNDER && _state == ThunderBoltPowerState.READY_TO_SHOT)
            LaunchThunderBolt();
    }
    #endregion

    #region Updates
    private void UpdateCharging()
    {
        //Debug.Log("Charging");
        _timer += Time.deltaTime;

        if (_timer >= _timeToCharge)
        {
            _currentBoltObj.transform.localScale = new Vector3(0.4f, 0.4f, 0.4f);
            _state = ThunderBoltPowerState.READY_TO_SHOT;
            _timer = 0.0f;
            _currentBoltObj.GetComponent<MeshRenderer>().material = _sphereReadyMat;
            return;
        }
        if (_currentBoltObj != null)
        {
            if (_handSideState == ThunderBoltPowerState.RIGHT_THUNDER)
                _currentBoltObj.transform.position = _leftHand.transform.position;
            else
                _currentBoltObj.transform.position = _rightHand.transform.position;
            if (_currentBoltObj.transform.localScale.x < 0.4f)
            {
                _currentBoltObj.transform.localScale = Vector3.Lerp(Vector3.zero, new Vector3(0.4f, 0.4f, 0.4f), _timer / _timeToCharge);
            }
        }
    }

    private void UpdateReadyToShot()
    {
        if (_state != ThunderBoltPowerState.READY_TO_SHOT)
            return;
        if (_currentBoltObj != null)
        {
            if (_handSideState == ThunderBoltPowerState.RIGHT_THUNDER)
                _currentBoltObj.transform.position = _leftHand.transform.position;
            else
                _currentBoltObj.transform.position = _rightHand.transform.position;
        }
        //Debug.Log("Ready to shot!");

    }

    private void UpdateNoState()
    {
        _timer = 0.0f;
    }
    #endregion

    #region Power canceling
    private void CancelPower()
    {
        if (_state == ThunderBoltPowerState.NONE)
            return;
        //Debug.Log("Cancel power");
        _state = ThunderBoltPowerState.NONE;
        _handSideState = ThunderBoltPowerState.NONE;
        _timer = 0.0f;
        if (_currentBoltObj != null)
        {
            Destroy(_currentBoltObj);
            _currentBoltObj = null;
        }
        _targetCylinderObj.SetActive(false);
    }

    public void CancelChargingRightPower()
    {
        if (_state == ThunderBoltPowerState.NONE)
            return;
        //Debug.Log("Try Cancel Right");
        if (_handSideState == ThunderBoltPowerState.RIGHT_THUNDER && _state == ThunderBoltPowerState.CHARGING)
            CancelPower();
    }
    public void CancelChargingLeftPower()
    {
        if (_state == ThunderBoltPowerState.NONE)
            return;
        //Debug.Log("Try Cancel Left");
        if (_handSideState == ThunderBoltPowerState.LEFT_THUNDER && _state == ThunderBoltPowerState.CHARGING)
            CancelPower();
    }

    public void CancelReadyToShotRight()
    {
        if (_state == ThunderBoltPowerState.NONE)
            return;
        //Debug.Log("Try Cancel ready Right");
        _targetCylinderObj.SetActive(true);
        if (_handSideState == ThunderBoltPowerState.RIGHT_THUNDER && _state == ThunderBoltPowerState.READY_TO_SHOT)
            CancelPower();
    }

    public void CancelReadyToShotLeft()
    {
        if (_state == ThunderBoltPowerState.NONE)
            return;
        _targetCylinderObj.SetActive(true);
        //Debug.Log("Try Cancel ready Left");
        if (_handSideState == ThunderBoltPowerState.LEFT_THUNDER && _state == ThunderBoltPowerState.READY_TO_SHOT)
            CancelPower();
    }
    #endregion

}
