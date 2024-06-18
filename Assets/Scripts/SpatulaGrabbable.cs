using BNG;
using UnityEngine;

public class SpatulaGrabbable : MonoBehaviour
{
    [SerializeField] private Grabbable grabbable;
    [SerializeField] private Rigidbody spatulaRigidbody;
    [SerializeField] private Collider spatulaCollider;

    public bool BeingGrabbed { get; private set; } = false;
    public bool CanGrab { get; set; } = true;

    public Rigidbody Rigidbody => spatulaRigidbody;

    public void OnGrabbed()
    {
        spatulaCollider.enabled = false;
        spatulaRigidbody.isKinematic = true;

        BeingGrabbed = true;
    }

    public void OnRelease()
    {
        spatulaCollider.enabled = true;
        spatulaRigidbody.isKinematic = false;

        BeingGrabbed = false;
    }
}
