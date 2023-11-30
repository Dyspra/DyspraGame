using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ThunderBolt : MonoBehaviour
{
    private enum ThunderBoltPowerState
    {
        CHARGING,
        READY_TO_SHOT,
        NONE,
        LEFT_THUNDER,
        RIGHT_THUNDER
    }

    public ScreenShaker _screenShaker;
    [SerializeField] private GameObject _camera;
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
    //[SerializeField] private LineRenderer _lineRenderer;
    public float cylinderPosDelta = 5.0f;
    [SerializeField] private MissionChambouleTout _mission;

    [SerializeField] private GameObject _sphereProjectionGO;
    private bool isAiming = false;
    private List<GameObject> lineSphereList = new List<GameObject>();

    #region Unity methods
    private void Start()
    {
        if (_targetCylinderObj != null)
            _targetCylinderObj.GetComponent<MeshRenderer>().enabled = false;
            //_targetCylinderObj.SetActive(false);
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
        _state = ThunderBoltPowerState.CHARGING;
        _currentBoltObj = Instantiate(_thunderBoltObj, this.transform.position, this.transform.rotation);
    }

    public void StartChargingRightThunder()
    {
        if (_state != ThunderBoltPowerState.NONE)
            return;
        _handSideState = ThunderBoltPowerState.RIGHT_THUNDER;
        _state = ThunderBoltPowerState.CHARGING;
        _currentBoltObj = Instantiate(_thunderBoltObj, this.transform.position, this.transform.rotation);
    }
    #endregion

    #region Thunderbolt launching
    private void LaunchThunderBolt()
    {
        if (_currentBoltObj != null)
        {
            Vector3 tarPos = new Vector3(_targetCylinderObj.transform.position.x, _targetCylinderObj.transform.position.y + 1, _targetCylinderObj.transform.position.z);
            _currentBoltObj.GetComponent<Rigidbody>().isKinematic = false;
            _currentBoltObj.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            _currentBoltObj.GetComponent<Rigidbody>().AddRelativeForce((tarPos - _currentBoltObj.transform.position) * _projectionVelocity);
            _currentBoltObj = null;
        }
        _screenShaker.TriggerScreenShake(0.2f, _camera);
        _targetCylinderObj.GetComponent<MeshRenderer>().enabled = false;
        //_targetCylinderObj.SetActive(false);
        _state = ThunderBoltPowerState.NONE;
        _mission.ShotThunderBolt();
    }
    public void LaunchRightThunderBolt()
    {
        if (_state != ThunderBoltPowerState.READY_TO_SHOT)
            return;
        if (_handSideState == ThunderBoltPowerState.RIGHT_THUNDER && _state == ThunderBoltPowerState.READY_TO_SHOT)
            LaunchThunderBolt();
    }

    public void LaunchLeftThunderBolt()
    {
        if (_state != ThunderBoltPowerState.READY_TO_SHOT)
            return;
        if (_handSideState == ThunderBoltPowerState.LEFT_THUNDER && _state == ThunderBoltPowerState.READY_TO_SHOT)
            LaunchThunderBolt();
    }
    #endregion

    #region Updates
    private void UpdateCharging()
    {
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
            {
                //_lineRenderer.SetPosition(0, _leftHand.transform.position);
                _currentBoltObj.transform.position = _leftHand.transform.position;
                _targetCylinderObj.transform.position = new Vector3(_leftHand.transform.position.x * cylinderPosDelta, _targetCylinderObj.transform.position.y, _targetCylinderObj.transform.position.z);
            }
            else
            {
                //_lineRenderer.SetPosition(0, _rightHand.transform.position);
                _currentBoltObj.transform.position = _rightHand.transform.position;
                _targetCylinderObj.transform.position = new Vector3(_rightHand.transform.position.x * cylinderPosDelta, _targetCylinderObj.transform.position.y, _targetCylinderObj.transform.position.z);
            }
            //_lineRenderer.SetPosition(1, _targetCylinderObj.transform.position);
        }

    }

    private void UpdateNoState()
    {
        isAiming = false;
    //_lineRenderer.enabled = false;
        _timer = 0.0f;
        lineSphereList.Clear();
        StopAllCoroutines();
    }

    private IEnumerator StartProjectionLine()
    {
        float timeToSpawn = 0.5f;
        float timer = timeToSpawn;

        while (true)
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                timer = timeToSpawn;
                if (_currentBoltObj != null)
                {
                    lineSphereList.Add(Instantiate(_sphereProjectionGO, _currentBoltObj.transform));
                }
                _sphereProjectionGO.GetComponent<LineSphere>().target = _targetCylinderObj.transform;
                yield return null;
            }
        }
        yield return null;
    }
    #endregion

    #region Power canceling
    private void CancelPower()
    {
        if (_state == ThunderBoltPowerState.NONE)
            return;
        _state = ThunderBoltPowerState.NONE;
        _handSideState = ThunderBoltPowerState.NONE;
        _timer = 0.0f;
        if (_currentBoltObj != null)
        {
            Destroy(_currentBoltObj);
            _currentBoltObj = null;
        }
        _targetCylinderObj.GetComponent<MeshRenderer>().enabled = false;
    }

    public void CancelChargingRightPower()
    {
        if (_state == ThunderBoltPowerState.NONE)
            return;
        if (_handSideState == ThunderBoltPowerState.RIGHT_THUNDER && _state == ThunderBoltPowerState.CHARGING)
            CancelPower();
    }
    public void CancelChargingLeftPower()
    {
        if (_state == ThunderBoltPowerState.NONE)
            return;
        if (_handSideState == ThunderBoltPowerState.LEFT_THUNDER && _state == ThunderBoltPowerState.CHARGING)
            CancelPower();
    }

    public void CancelReadyToShotRight()
    {
        if (_state == ThunderBoltPowerState.NONE)
            return;
            //_lineRenderer.enabled = true;
        if (isAiming == false)
            StartCoroutine(StartProjectionLine());
        isAiming = true;
        _targetCylinderObj.GetComponent<MeshRenderer>().enabled = true;
        if (_handSideState == ThunderBoltPowerState.RIGHT_THUNDER && _state == ThunderBoltPowerState.READY_TO_SHOT)
            CancelPower();
    }

    public void CancelReadyToShotLeft()
    {
        if (_state == ThunderBoltPowerState.NONE)
            return;
        //_lineRenderer.enabled = true;
        if (isAiming == false)
            StartCoroutine(StartProjectionLine());
        isAiming = true;
        _targetCylinderObj.GetComponent<MeshRenderer>().enabled = true;
        if (_handSideState == ThunderBoltPowerState.LEFT_THUNDER && _state == ThunderBoltPowerState.READY_TO_SHOT)
            CancelPower();
    }
    #endregion

}
