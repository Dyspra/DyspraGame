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

    // SFX
    [SerializeField] private AudioSource _loopingAudioSource;
    [SerializeField] private AudioSource _oneShotAudioSource;
    [SerializeField] private AudioSource _oneShotAudioSource2;
    [SerializeField] private AudioClip _chargingSFX;
    [SerializeField] private AudioClip _ReadySFX;
    [SerializeField] private AudioClip _LaunchSFX;
    [SerializeField] private AudioClip _ReadyOneShotSFX;

    public ScreenShaker _screenShaker;
    [SerializeField] private GameObject _player;
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
    [SerializeField] private float _timeToSpawn = 0.1f;
    [SerializeField] private int _nbrSphereToSpawn= 10;
    private bool isAiming = false;
    private List<GameObject> lineSphereList = new List<GameObject>();
    [SerializeField] private RectTransform _tutoUI;
    [SerializeField] private GameObject _chargingInfo;
    [SerializeField] private GameObject _aimingInfo;
    [SerializeField] private GameObject _throwInfo;

    #region Unity methods
    private void Start()
    {
        if (_targetCylinderObj != null)
            _targetCylinderObj.GetComponent<MeshRenderer>().enabled = false;
        _oneShotAudioSource2.clip = _LaunchSFX;
        _oneShotAudioSource.clip = _ReadyOneShotSFX;
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
        _loopingAudioSource.clip = _chargingSFX;
        _loopingAudioSource.Play();
        _tutoUI.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
        _chargingInfo.SetActive(true);
    }

    public void StartChargingRightThunder()
    {
        if (_state != ThunderBoltPowerState.NONE)
            return;
        _handSideState = ThunderBoltPowerState.RIGHT_THUNDER;
        _state = ThunderBoltPowerState.CHARGING;
        _currentBoltObj = Instantiate(_thunderBoltObj, this.transform.position, this.transform.rotation);
        _loopingAudioSource.clip = _chargingSFX;
        _loopingAudioSource.Play();
        _tutoUI.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        _chargingInfo.SetActive(true);
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
            Destroy(_currentBoltObj, 2f);
            _currentBoltObj = null;
        }
        _screenShaker.TriggerScreenShake(0.2f, _camera);
        _targetCylinderObj.GetComponent<MeshRenderer>().enabled = false;
        //_targetCylinderObj.SetActive(false);
        _state = ThunderBoltPowerState.NONE;
        _mission.ShotThunderBolt();
        _loopingAudioSource.Stop();
        _oneShotAudioSource2.Play();
        _oneShotAudioSource.Play();
        TurnOffHelperInfo();
    }
    public void LaunchRightThunderBolt()
    {
        if (_state != ThunderBoltPowerState.READY_TO_SHOT)
            return;
        if (_handSideState == ThunderBoltPowerState.RIGHT_THUNDER && _state == ThunderBoltPowerState.READY_TO_SHOT)
            LaunchThunderBolt();
        _mission._nbrOfLeftBolt++;
    }

    public void LaunchLeftThunderBolt()
    {
        if (_state != ThunderBoltPowerState.READY_TO_SHOT)
            return;
        if (_handSideState == ThunderBoltPowerState.LEFT_THUNDER && _state == ThunderBoltPowerState.READY_TO_SHOT)
            LaunchThunderBolt();
        _mission._nbrOfRightBolt++;
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
            _loopingAudioSource.Stop();
            _loopingAudioSource.clip = _ReadySFX;
            _loopingAudioSource.Play();
            _oneShotAudioSource.Play();
            _chargingInfo.SetActive(false);
            _aimingInfo.SetActive(true);
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
                _targetCylinderObj.transform.position = new Vector3(_player.transform.position.x + (_leftHand.transform.localPosition.x * cylinderPosDelta), _targetCylinderObj.transform.position.y, _targetCylinderObj.transform.position.z);
            }
            else
            {
                //_lineRenderer.SetPosition(0, _rightHand.transform.position);
                _currentBoltObj.transform.position = _rightHand.transform.position;
                _targetCylinderObj.transform.position = new Vector3(_player.transform.position.x + (_rightHand.transform.localPosition.x * cylinderPosDelta), _targetCylinderObj.transform.position.y, _targetCylinderObj.transform.position.z);
            }
        }

    }

    private void UpdateNoState()
    {
        isAiming = false;
    //_lineRenderer.enabled = false;
        _timer = 0.0f;
        if (lineSphereList.Capacity > 0)
        {
            foreach (var item in lineSphereList)
            {
                Destroy(item);
            }
            lineSphereList.Clear();
            StopAllCoroutines();
        }
    }

    private IEnumerator StartProjectionLine()
    {
        if (lineSphereList.Capacity > 0)
            yield return null;
        float timer = 0f;
        int sphereNbr = _nbrSphereToSpawn;
        _aimingInfo.SetActive(false);
        _throwInfo.SetActive(true);
        do
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                timer = _timeToSpawn;
                if (_currentBoltObj != null)
                {
                    GameObject go = Instantiate(_sphereProjectionGO, new Vector3(_currentBoltObj.transform.position.x,
                        _currentBoltObj.transform.position.y, _currentBoltObj.transform.position.z + 0.5f), _currentBoltObj.transform.rotation);
                    lineSphereList.Add(go);
                    sphereNbr--;
                    _sphereProjectionGO.GetComponent<LineSphere>().target = _targetCylinderObj.transform;
                    _sphereProjectionGO.GetComponent<LineSphere>()._initialPos = _currentBoltObj;
                }
            }
            yield return null;
        } while (sphereNbr > 0);

        yield return null;
    }
    #endregion

    #region Power canceling
    private void CancelPower()
    {
        if (_state == ThunderBoltPowerState.NONE)
            return;
        TurnOffHelperInfo();
        _state = ThunderBoltPowerState.NONE;
        _handSideState = ThunderBoltPowerState.NONE;
        _timer = 0.0f;
        if (_currentBoltObj != null)
        {
            Destroy(_currentBoltObj);
            _currentBoltObj = null;
        }
        _targetCylinderObj.GetComponent<MeshRenderer>().enabled = false;
        _loopingAudioSource.Stop();
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
        if (isAiming == false)
            StartCoroutine(StartProjectionLine());
        isAiming = true;
        _targetCylinderObj.GetComponent<MeshRenderer>().enabled = true;
        if (_handSideState == ThunderBoltPowerState.LEFT_THUNDER && _state == ThunderBoltPowerState.READY_TO_SHOT)
            CancelPower();
    }
    #endregion

    private void TurnOffHelperInfo()
    {
        _chargingInfo.SetActive(false);
        _aimingInfo.SetActive(false);
        _throwInfo.SetActive(false);
    }
}
