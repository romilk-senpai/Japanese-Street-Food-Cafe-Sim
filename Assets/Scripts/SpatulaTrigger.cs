using System.Collections.Generic;
using UnityEngine;

public class SpatulaTrigger : MonoBehaviour
{
    private List<SpatulaGrabbable> _grabbablesInTrigger = new List<SpatulaGrabbable>();
    private SpatulaGrabbable _nearestGrabbable;

    public IReadOnlyList<SpatulaGrabbable> GrabbablesInTrigger => _grabbablesInTrigger;
    public SpatulaGrabbable NearestGrabbable => _nearestGrabbable;

    private void OnTriggerEnter(Collider other)
    {
        var grabbable = other.GetComponent<SpatulaGrabbable>();

        if (grabbable != null && grabbable.CanGrab && !_grabbablesInTrigger.Contains(grabbable))
        {
            _grabbablesInTrigger.Add(grabbable);

            UpdateNearestGrabbable();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        var grabbable = other.GetComponent<SpatulaGrabbable>();

        if (grabbable != null && _grabbablesInTrigger.Contains(grabbable))
        {
            _grabbablesInTrigger.Remove(grabbable);

            UpdateNearestGrabbable();
        }
    }

    private void UpdateNearestGrabbable()
    {
        if (_grabbablesInTrigger.Count == 0)
        {
            _nearestGrabbable = null;
        }
        else if (_grabbablesInTrigger.Count == 1)
        {
            _nearestGrabbable = _grabbablesInTrigger[0];
        }
        else
        {
            var leastDistance = Mathf.Infinity;

            foreach (var grabbableInTrigger in _grabbablesInTrigger)
            {
                var distanceToGrabbable = Vector3.Distance(transform.position, grabbableInTrigger.transform.position);

                if (distanceToGrabbable < leastDistance)
                {
                    leastDistance = distanceToGrabbable;
                    _nearestGrabbable = grabbableInTrigger;
                }
            }
        }
    }

    private void Update()
    {
        UpdateNearestGrabbable();
    }
}
