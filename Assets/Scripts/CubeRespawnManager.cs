using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeRespawnManager : MonoBehaviour
{
    public List<Transform> cubeTransforms; // Liste des Transform des cubes
    public float movementThreshold = 5.0f; // Seuil de mouvement pour déclencher le respawn

    private List<Vector3> initialPositions; // Positions initiales des cubes

    void Start()
    {
        // Enregistrez les positions initiales des cubes
        initialPositions = new List<Vector3>();
        foreach (Transform cubeTransform in cubeTransforms)
        {
            initialPositions.Add(cubeTransform.position);
        }
    }

    void Update()
    {
        // Vérifiez si un cube a dépassé le seuil de mouvement
        for (int i = 0; i < cubeTransforms.Count; i++)
        {
            Transform cubeTransform = cubeTransforms[i];
            Vector3 initialPosition = initialPositions[i];

            if (Vector3.Distance(cubeTransform.position, initialPosition) > movementThreshold)
            {
                RespawnCube(cubeTransform, initialPosition);
            }
        }
    }

    void RespawnCube(Transform cubeTransform, Vector3 initialPosition)
    {
        // Réinitialisez la position du cube au point de départ
        cubeTransform.position = initialPosition;

        Rigidbody rb = cubeTransform.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
    }
}
