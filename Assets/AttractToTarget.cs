using NaughtyAttributes;
using UnityEditor;
using UnityEngine;

public class AttractToTarget : MonoBehaviour
{
    public Transform target; // La cible vers laquelle l'objet doit être attiré
    private Transform realTarget; // La position réelle vers laquelle l'objet sera attiré
    [OnValueChanged("SetupRigidbody")]
    public Rigidbody movedObject; // L'objet qui sera attiré
    public float speed = 100f; // La vitesse d'attraction
    public float maxSpeed = 2.5f; // La vitesse d'attraction max
    private float drag = 8f;

    public GameObject ParticleLight;
    public bool doAttraction = false;
    GameObject particles;

    void Start()
    {
        SetupRigidbody();
    }

    void FixedUpdate()
    {
        if (doAttraction)
        {
            Attract();
        }
    }

    void Attract()
    {
        // Calcule la distance entre l'objet et la cible
        float distance = Vector3.Distance(movedObject.transform.position, realTarget.position);
        // Calcule la force à appliquer en utilisant Lerp pour lisser la force en fonction de la distance
        float force = Mathf.Lerp(0f, speed, distance);
        // Applique la force sur l'objet en direction de la cible
        movedObject.AddForce((realTarget.position - movedObject.transform.position) * force);

        movedObject.velocity = Vector3.ClampMagnitude(movedObject.velocity, maxSpeed);
    }

    public void SetRigidbodyAttracted(Rigidbody newRb)
    {
        if (doAttraction)
        {
            //on désactive l'attraction sur le rb actuel mais on remets le bool à true pour que le prochain rb soit attiré
            StopAttraction();
            doAttraction = true;
        }
        movedObject = newRb;
        SetupRigidbody();
    }

    private void SetupRigidbody()
    {
        ParticleSystem particleSystem = movedObject.GetComponentInChildren<ParticleSystem>(true);
        if (!particleSystem)
        {
            particles = Instantiate(AssetDatabase.LoadAssetAtPath<GameObject>("Assets/JMO Assets/Cartoon FX Remaster/CFXR Prefabs/Light/CFXR3 LightGlow A (Loop).prefab"), movedObject.transform);
            particles.transform.GetComponentInChildren<ParticleSystem>().Play();
        }
        else
        {
            particleSystem.Play();
            particles = particleSystem.gameObject;
        }

        realTarget = new GameObject("real Target").transform;
        realTarget.position = new Vector3(target.position.x, target.position.y, target.position.z + 0.18f);

        if (doAttraction)
            StartAttraction();
    }

    public void StartAttraction()
    {
        doAttraction = true;
        movedObject.drag = drag;
        particles.SetActive(true);
        movedObject.useGravity = false;
    }

    public void StopAttraction()
    {
        doAttraction = false;
        particles.SetActive(false);
        movedObject.drag = 0;
        movedObject.useGravity = true;
        movedObject.velocity = Vector3.zero;
    }
}