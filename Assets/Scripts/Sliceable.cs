using UnityEngine;

public class Sliceable : MonoBehaviour
{
    [SerializeField] private Material crossSectionMaterial;

    public Material CrossSectionMaterial { get => crossSectionMaterial; set => crossSectionMaterial = value; }

    public virtual void SetupHull(Sliceable newSliceable)
    {
        newSliceable.gameObject.AddComponent<Rigidbody>();
        newSliceable.gameObject.AddComponent<MeshCollider>().convex = true;
        newSliceable.CrossSectionMaterial = CrossSectionMaterial;
    }
}
