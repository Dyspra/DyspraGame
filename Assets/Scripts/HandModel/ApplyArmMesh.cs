using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SkinnedMeshRenderer))]
[RequireComponent(typeof(MeshCollider))]
public class ApplyArmMesh : MonoBehaviour
{
    private SkinnedMeshRenderer meshRenderer;
    private Mesh colliderMesh;

    private MeshCollider skinCollider;
    void Start()
    {
        meshRenderer = GetComponent<SkinnedMeshRenderer>();
        skinCollider = GetComponent<MeshCollider>();
    }

    void FixedUpdate()
    {
        colliderMesh = new Mesh();
        meshRenderer.BakeMesh(colliderMesh, true);
        skinCollider.sharedMesh = null;
        skinCollider.sharedMesh = colliderMesh;
    }
}
