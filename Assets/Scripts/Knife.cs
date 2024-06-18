using EzySlice;
using UnityEngine;

public class Knife : MonoBehaviour
{
    [SerializeField] private Transform startSlicePoint;
    [SerializeField] private Transform endSlicePoint;
    [SerializeField] private LayerMask sliceableLayer;
    [SerializeField] private VelocityEstimator velocityEstimator;

    private void FixedUpdate()
    {
        bool hasHit = Physics.Linecast(startSlicePoint.position, endSlicePoint.position, out RaycastHit hit, sliceableLayer);

        if (hasHit)
        {
            var target = hit.transform.gameObject;
            var sliceable = target.GetComponent<Sliceable>();

            Slice(sliceable);
        }
    }

    private void Slice(Sliceable target)
    {
        Vector3 velocity = velocityEstimator.GetVelocityEstimate();
        Vector3 planeNormal = Vector3.Cross(endSlicePoint.position - startSlicePoint.position, velocity);
        planeNormal.Normalize();

        SlicedHull hull = target.gameObject.Slice(endSlicePoint.position, planeNormal);

        if (hull != null)
        {
            GameObject upperHull = hull.CreateUpperHull(target.gameObject, target.CrossSectionMaterial);
            upperHull.name = $"{target.gameObject.name} (Upper)";
            SetupHull(target, upperHull);

            GameObject lowerHull = hull.CreateLowerHull(target.gameObject, target.CrossSectionMaterial);
            upperHull.name = $"{target.name} (Lower)";
            SetupHull(target, lowerHull);
        }

        Destroy(target.gameObject);
    }

    private void SetupHull(Sliceable target, GameObject hull)
    {
        target.SetupHull(hull.AddComponent<Sliceable>());
        hull.layer = LayerMask.NameToLayer("Sliceable");
    }
}