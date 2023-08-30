using System.Collections;
using UnityEngine;

public class C_move : IBall
{
    public float movementSpeed = 20f;
    public float rotationSpeed = 100f;

    private bool isWandering = false;
    private bool isRotatingLeft = false;
    private bool isRotatingRight = false;
    private bool isWalking = false;

    [SerializeField] private AudioSource _sound;
    [SerializeField] private float _timeBeforeDestroy = 2.0f;

    Rigidbody rb;
    Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        _sound = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isWandering == false)
            StartCoroutine(Wander());
        if (isRotatingRight == true)
            transform.Rotate(transform.up * Time.deltaTime * rotationSpeed);
        if (isRotatingLeft == true)
            transform.Rotate(transform.up * Time.deltaTime * -rotationSpeed);
        if (isWalking == true) {
            rb.AddForce(transform.forward * movementSpeed);
            animator.SetBool("isRunning", true);
        }
    }

    IEnumerator Wander()
    {
        int rotationTime = Random.Range(1, 3);
        int rotateWait = Random.Range(1, 3);
        int rotateDirection = Random.Range(1, 2);
        int walkWait = Random.Range(1, 3);
        int walkTime = Random.Range(1, 3);

        isWandering = true;
        yield return new WaitForSeconds(walkWait);

        isWandering = true;
        yield return new WaitForSeconds(walkTime);

        isWalking = false;
        yield return new WaitForSeconds(rotateWait);

        if (rotateDirection == 1) {
            isRotatingLeft = true;
            yield return new WaitForSeconds(rotationTime);
            isRotatingLeft = false;
        }
        if (rotateDirection == 2) {
            isRotatingRight = true;
            yield return new WaitForSeconds(rotationTime);
            isRotatingRight = false;
        }
        isWandering = false;
    }

    public override void ApplyEffect()
    {
        StartCoroutine(PlaySoundFX());
    }

    private IEnumerator PlaySoundFX()
    {
        _sound.Play();
        this.gameObject.GetComponent<Collider>().enabled = false;
        //this.gameObject.SetActive(false);
        foreach (Transform child in transform)
            child.gameObject.SetActive(false);
        yield return new WaitForSeconds(_timeBeforeDestroy);
        Destroy(this.gameObject);
    }
}
