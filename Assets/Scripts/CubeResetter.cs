using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeResetter : MonoBehaviour
{
    private Vector3 initialPosition;
    private Quaternion initialRotation;
    private Rigidbody rb;
    private bool hasMoved = false; // Indicateur pour savoir si le cube a bougé

    void Start()
    {
        // Enregistrez la position, la rotation et le Rigidbody initial de chaque cube
        initialPosition = transform.position;
        initialRotation = transform.rotation;
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // Vérifiez si le cube a bougé
        if (!hasMoved && rb.velocity.magnitude > 5.0f)
        {
            hasMoved = true;
            // Détroyez le cube lorsque sa vélocité dépasse 5
            Destroy(gameObject, 1.0f); // Vous pouvez ajuster le délai avant la destruction si nécessaire
        }
    }

    public void ResetCube()
    {
        // Réinitialisez la position et la rotation du cube
        transform.position = initialPosition;
        transform.rotation = initialRotation;

        // Réinitialisez le Rigidbody si nécessaire
        if (rb != null)
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        // Réinitialisez l'indicateur de déplacement
        hasMoved = false;
    }
}


