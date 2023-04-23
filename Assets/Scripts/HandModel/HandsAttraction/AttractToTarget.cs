using NaughtyAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

[Serializable]
public class AttractedTarget
{
    public Rigidbody rb;
    public GameObject particle;
    public AttractedTarget(Rigidbody rb, GameObject particle)
    {
        this.rb = rb;
        this.particle = particle;
    }
}

public class AttractToTarget : MonoBehaviour
{
    public Transform target; // La cible vers laquelle l'objet doit �tre attir�
    private Transform realTarget; // La position r�elle vers laquelle l'objet sera attir�
    //[OnValueChanged("SetupRigidbody")]
    public List<AttractedTarget> attractedTargets;
    public float speed = 100f; // La vitesse d'attraction
    public float maxSpeed = 2.5f; // La vitesse d'attraction max
    private float drag = 8f;
    public bool doAttraction = false;

    public GameObject ParticleLight;
    public GameObject ParticleAttract;
    ParticleSystem attractionEffect;
    public GameObject AttractionCone;

    void Start()
    {
        foreach (var attractedTarget in attractedTargets)
        {
            SetupRigidbody(attractedTarget);
        }
        
        realTarget = new GameObject("real Target").transform;
        
        attractionEffect = Instantiate(ParticleAttract, realTarget).GetComponent<ParticleSystem>();
        attractionEffect.Stop();

        Instantiate(AttractionCone, realTarget, true);
    }

    void FixedUpdate()
    {
        if (doAttraction)
        {
            foreach (var attractedTarget in attractedTargets)
                Attract(attractedTarget.rb);
        }
    }

    private void Update()
    {
        realTarget.position = new Vector3(target.position.x, target.position.y, target.position.z + 0.18f);
        realTarget.rotation = Quaternion.Euler(realTarget.rotation.eulerAngles.x, target.rotation.eulerAngles.y + 90, realTarget.rotation.eulerAngles.z);
    }

    void Attract(Rigidbody movedObject)
    {
        // Calcule la distance entre l'objet et la cible
        float distance = Vector3.Distance(movedObject.transform.position, realTarget.position);
        // Calcule la force � appliquer en utilisant Lerp pour lisser la force en fonction de la distance
        float force = Mathf.Lerp(0f, speed, distance);
        // Applique la force sur l'objet en direction de la cible
        movedObject.AddForce((realTarget.position - movedObject.transform.position) * force);

        movedObject.velocity = Vector3.ClampMagnitude(movedObject.velocity, maxSpeed);
    }

    public void SetRigidbodyAttracted(Rigidbody newRb)
    {
        AttractedTarget newTarget = new AttractedTarget(newRb, null);
        attractedTargets.Add(newTarget);
        SetupRigidbody(newTarget);
    }
    public void RemoveRigidbodyAttracted(Rigidbody removeRb)
    {
        var targetToRemove = attractedTargets.FirstOrDefault(target => target.rb == removeRb);
        DisableAttraction(targetToRemove);
        if (targetToRemove != null)
        {
            attractedTargets.Remove(targetToRemove);
        }
    }

    private void SetupRigidbody(AttractedTarget attractedTarget)
    {
        ParticleSystem particle = attractedTarget.rb.GetComponentInChildren<ParticleSystem>(true);
        if (!particle)
        {
            GameObject newParticle = Instantiate(AssetDatabase.LoadAssetAtPath<GameObject>("Assets/JMO Assets/Cartoon FX Remaster/CFXR Prefabs/Light/CFXR3 LightGlow A (Loop).prefab"), attractedTarget.rb.transform);
            particle = newParticle.transform.GetComponentInChildren<ParticleSystem>();
        }
        attractedTarget.particle = particle.gameObject;

        if (doAttraction)
            EnableAttraction(attractedTarget);
    }

    public void StartAttraction()
    {
        doAttraction = true;
        attractionEffect.Play();
        attractedTargets.ForEach(attractedTarget => EnableAttraction(attractedTarget));

    }

    public void StopAttraction()
    {
        doAttraction = false;
        attractionEffect.Stop();
        attractedTargets.ForEach(attractedTarget => DisableAttraction(attractedTarget));
    }

    void EnableAttraction(AttractedTarget attractedTarget)
    {
        attractedTarget.rb.drag = drag;
        attractedTarget.particle.SetActive(true);
        attractedTarget.rb.useGravity = false;
        attractedTarget.rb.freezeRotation = true;
    }

    void DisableAttraction(AttractedTarget attractedTarget)
    {
        attractedTarget.particle.SetActive(false);
        attractedTarget.rb.drag = 0;
        attractedTarget.rb.useGravity = true;
        attractedTarget.rb.velocity = Vector3.zero;
        attractedTarget.rb.freezeRotation = false;
    }
}