using UnityEngine;

public class ScreenShaker : MonoBehaviour
{
    // Desired duration of the shake effect
    private float shakeDuration = 0f;

    // A measure of magnitude for the shake. Tweak based on your preference
    [SerializeField] private float shakeMagnitude = 0.7f;

    // A measure of how quickly the shake effect should evaporate
    [SerializeField] private float dampingSpeed = 1.0f;

    // The camera to shake
    private GameObject cameraToShake = null;

    // The initial position of the GameObject
    private Vector3 initialPosition;

    void Update()
    {
        if (cameraToShake == null)
            return;
        if (shakeDuration > 0)
        {
            cameraToShake.transform.localPosition = initialPosition + Random.insideUnitSphere * (shakeMagnitude / 10);
            shakeDuration -= Time.deltaTime * dampingSpeed;
        }
        else
        {
            shakeDuration = 0f;
            cameraToShake.transform.localPosition = initialPosition;
            cameraToShake = null;
        }
    }

    public void TriggerScreenShake(float duration, GameObject camera)
    {
        cameraToShake = camera;
        initialPosition = camera.transform.position;
        shakeDuration = duration;
    }
}
