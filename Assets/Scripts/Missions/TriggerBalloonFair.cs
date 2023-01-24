using System.Collections;
using UnityEngine;
using Dyspra;

// The balloon fair trigger notify when player gets a balloon or trigger another mission step
public class TriggerBalloonFair : MonoBehaviour, ISubject
{
    private Subject _subject = new Subject();
    [SerializeField] private int _score = 0;
    [SerializeField] private int _actualStep = 1;
    [SerializeField] private int _nbrToTriggerStep2 = 10;
    [SerializeField] private int _nbrToTriggerStep3 = 20;
    [SerializeField] private int _nbrToTriggerEnd = 30;
    [SerializeField] private float _spaceCooldown = 2; 
    [SerializeField] private float _timeToWaitBeforeTrigger = 5; 
    private float _timer;
    private bool _canTriggerNext = true;

    void Start()
    {
        AbstractObserver[] obsFounded = FindObjectsOfType<AbstractObserver>();
        for (int i = 0; i < obsFounded.Length; i++)
            AddObserver(ref obsFounded[i]);
    }

    // Update is called once per frame
    void Update()
    {
        if (_timer < _spaceCooldown)
            _timer += Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.Space) && _actualStep < 4 && _timer >= _spaceCooldown && _canTriggerNext == true)
        {
            StartCoroutine(WaitBeforeMove());
            switch (_actualStep)
            {
                case 1:
                    _score = _nbrToTriggerStep2;
                    break;
                case 2:
                    _score = _nbrToTriggerStep3;
                    break;
                case 3:
                    _score = _nbrToTriggerEnd;
                    break;
                default:
                    break;
            }
            _actualStep++;
            NotifyObservers(this.gameObject, Dyspra.E_Event.MISSION_STEP_COMPLETE);
            _timer = 0f;
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            GetBalloon();
        }
    }

    void GetBalloon()
    {
        if (_actualStep >= 4)
            return;
        _score++;
        NotifyObservers(this.gameObject, Dyspra.E_Event.MISSION_GET_BALLOON);
        switch (_actualStep)
        {
            case 1:
                if (_score >= _nbrToTriggerStep2 && _canTriggerNext == true)
                {
                    StartCoroutine(WaitBeforeMove());
                    _actualStep++;
                    NotifyObservers(this.gameObject, Dyspra.E_Event.MISSION_STEP_COMPLETE);
                }
                break;
            case 2:
                if (_score >= _nbrToTriggerStep3 && _canTriggerNext == true)
                {
                    StartCoroutine(WaitBeforeMove());
                    _actualStep++;
                    NotifyObservers(this.gameObject, Dyspra.E_Event.MISSION_STEP_COMPLETE);
                }
                break;
            case 3:
                if (_score >= _nbrToTriggerEnd && _canTriggerNext == true)
                {
                    StartCoroutine(WaitBeforeMove());
                    _actualStep++;
                    NotifyObservers(this.gameObject, Dyspra.E_Event.MISSION_STEP_COMPLETE);
                }
                break;
            default:
                break;
        }
    }

    private IEnumerator WaitBeforeMove()
    {
        _canTriggerNext = false;
        yield return new WaitForSeconds(_timeToWaitBeforeTrigger);
        _canTriggerNext = true;
    }

    #region Subject initialization
    public void NotifyObservers(GameObject entity, E_Event eventToTrigger)
    {
        _subject.NotifyObservers(entity, eventToTrigger);
    }

    public void AddObserver(ref AbstractObserver observer)
    {
        _subject.AddObserver(ref observer);
    }

    public void RemoveObserver(ref AbstractObserver observer)
    {
        _subject.RemoveObserver(ref observer);
    }
    #endregion
}
