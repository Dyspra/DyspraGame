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
    public Transform target; // La cible vers laquelle l'objet doit être attiré
    private Transform realTarget; // La position réelle vers laquelle l'objet sera attiré
    private Transform realTargetDisplay; // La position du cône et des particules
    
    //[OnValueChanged("SetupRigidbody")]
    public List<AttractedTarget> attractedTargets;
    List<AttractedTarget> targetsToRemove = new List<AttractedTarget>();
    
    public float speed = 100f; // La vitesse d'attraction
    public float maxSpeed = 2.5f; // La vitesse d'attraction max
    private float drag = 8f;
    public bool doAttraction = false;

    public GameObject ParticleLight; //prefab for the light particles
    public GameObject ParticleAttract; //prefab for the attract particles
    ParticleSystem attractionEffect;
    public GameObject AttractionCone; //prefab for the cone
    GameObject currentCone;

    void Start()
    {
        foreach (var attractedTarget in attractedTargets)
        {
            SetupRigidbody(attractedTarget);
        }
        
        realTarget = new GameObject("real Target").transform;
        realTargetDisplay = new GameObject("real Target Display").transform;
        realTargetDisplay.parent = realTarget;
        
        attractionEffect = Instantiate(ParticleAttract, realTargetDisplay).GetComponent<ParticleSystem>();
        attractionEffect.Stop();

        currentCone = Instantiate(AttractionCone, realTargetDisplay, true);
        currentCone.SetActive(false);
        
        realTargetDisplay.localPosition = new Vector3(0, 0, 0.13f);
    }

    void FixedUpdate()
    {
        if (doAttraction)
        {
            foreach (var attractedTarget in attractedTargets)
            {
                if (attractedTarget.rb)
                    Attract(attractedTarget.rb);
                else
                    targetsToRemove.Add(attractedTarget);
            }

            foreach (var targetToRemove in targetsToRemove)
            {
                attractedTargets.Remove(targetToRemove);
            }
        }
    }

    private void Update()
    {
        realTarget.position = new Vector3(target.position.x, target.position.y, target.position.z + 0.05f);
        realTarget.rotation = target.rotation;
        //realTarget.rotation = Quaternion.Euler(-target.rotation.eulerAngles.z + 29.7f, target.rotation.eulerAngles.y + 90, realTarget.rotation.eulerAngles.z);
    }

    void Attract(Rigidbody movedObject)
    {
        // Calcule la distance entre l'objet et la cible
        float distance = Vector3.Distance(movedObject.transform.position, realTarget.position);
        // Calcule la force à appliquer en utilisant Lerp pour lisser la force en fonction de la distance
        float force = Mathf.Lerp(0f, speed, distance);
        // Applique la force sur l'objet en direction de la cible
        movedObject.AddForce((realTarget.position - movedObject.transform.position) * speed);

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
        currentCone.SetActive(true);
    }

    public void StopAttraction()
    {
        doAttraction = false;
        attractionEffect.Stop();
        attractedTargets.ForEach(attractedTarget => DisableAttraction(attractedTarget));
        attractedTargets.Clear();
        currentCone.SetActive(false);
    }

    void EnableAttraction(AttractedTarget attractedTarget)
    {
        attractedTarget.particle.SetActive(true);
        if (attractedTarget.rb)
        {
            attractedTarget.rb.drag = drag;
            attractedTarget.rb.useGravity = false;
            attractedTarget.rb.freezeRotation = true;
        }
    }

    void DisableAttraction(AttractedTarget attractedTarget)
    {
        attractedTarget.particle.SetActive(false);
        if (attractedTarget.rb != null)
        {
            attractedTarget.rb.drag = 0;
            attractedTarget.rb.useGravity = true;
            attractedTarget.rb.velocity = Vector3.zero;
            attractedTarget.rb.freezeRotation = false;
        }
    }
}