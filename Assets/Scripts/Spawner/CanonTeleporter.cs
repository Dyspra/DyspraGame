using UnityEngine;

public class CanonTeleporter : MonoBehaviour
{
    private float _timer = 0.0f;
    private bool _isLeft = true;

    [SerializeField] private float _minTimeTeleport = 3.0f;
    [SerializeField] private float _maxTimeTeleport = 6.0f;
    [Header("Left")]
    [SerializeField] private Vector3 _leftPosition;
    [SerializeField] private Vector3 _leftRotation;
    [Header("Right")]
    [SerializeField] private Vector3 _rightPosition;
    [SerializeField] private Vector3 _rightRotation;

    // Start is called before the first frame update
    void Start()
    {
        _timer = Random.Range(_minTimeTeleport, _maxTimeTeleport);
        this.transform.localPosition = _leftPosition;
        this.transform.localRotation = Quaternion.Euler(_leftRotation);
    }

    // Update is called once per frame
    void Update()
    {
        _timer -= Time.deltaTime;
        if (_timer <= 0.0f)
            ResetTimerAndTeleportCanon();
    }

    private void ResetTimerAndTeleportCanon()
    {
        _timer = Random.Range(_minTimeTeleport, _maxTimeTeleport);

        if (_isLeft)
        {
            this.transform.localPosition = _rightPosition;
            this.transform.localRotation = Quaternion.Euler(_rightRotation);
            _isLeft = false;
        }
        else
        {
            this.transform.localPosition = _leftPosition;
            this.transform.localRotation = Quaternion.Euler(_leftRotation);
            _isLeft = true;
        }
    }
}
