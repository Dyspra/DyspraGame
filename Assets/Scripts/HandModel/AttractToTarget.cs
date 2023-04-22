using NaughtyAttributes;
using System;
using System.Collections.Generic;
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
    //[OnValueChanged("SetupRigidbody")]
    public List<AttractedTarget> attractedTargets;
    public float speed = 100f; // La vitesse d'attraction
    public float maxSpeed = 2.5f; // La vitesse d'attraction max
    private float drag = 8f;

    public GameObject ParticleLight;
    public GameObject ParticleAttract;
    public bool doAttraction = false;

    void Start()
    {
        foreach (var attractedTarget in attractedTargets)
        {
            SetupRigidbody(attractedTarget);
        }
        
        realTarget = new GameObject("real Target").transform;
        realTarget.position = new Vector3(target.position.x, target.position.y, target.position.z + 0.18f);
        GameObject attractionEffect = Instantiate(ParticleAttract, realTarget);
    }

    void FixedUpdate()
    {
        if (doAttraction)
        {
            foreach (var attractedTarget in attractedTargets)
                Attract(attractedTarget.rb);
        }
    }

    void Attract(Rigidbody movedObject)
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
        SetupRigidbody(new AttractedTarget(newRb, null));
    }

    private void SetupRigidbody(AttractedTarget attractedTarget)
    {
        ParticleSystem particle = attractedTarget.rb.GetComponentInChildren<ParticleSystem>(true);
        if (!particle)
        {
            GameObject newParticle = Instantiate(AssetDatabase.LoadAssetAtPath<GameObject>("Assets/JMO Assets/Cartoon FX Remaster/CFXR Prefabs/Light/CFXR3 LightGlow A (Loop).prefab"), attractedTarget.rb.transform);
            particle = newParticle.transform.GetComponentInChildren<ParticleSystem>();
            particle.Play();
        }
        else
        {
            particle.Play();
        }
        attractedTarget.particle = particle.gameObject;

        if (doAttraction)
            StartAttraction();
    }

    public void StartAttraction()
    {
        doAttraction = true;
        foreach(var attractedTarget in attractedTargets)
        {
            attractedTarget.rb.drag = drag;
            attractedTarget.particle.SetActive(true);
            attractedTarget.rb.useGravity = false;
            attractedTarget.rb.freezeRotation = true;
        }
    }

    public void StopAttraction()
    {
        doAttraction = false;
        foreach(var attractedTarget in attractedTargets)
        {
            attractedTarget.particle.SetActive(false);
            attractedTarget.rb.drag = 0;
            attractedTarget.rb.useGravity = true;
            attractedTarget.rb.velocity = Vector3.zero;
            attractedTarget.rb.freezeRotation = false;
        }
    }
}