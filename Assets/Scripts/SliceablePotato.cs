using BNG;
using UnityEngine;

public class SliceablePotato : Sliceable
{
    [SerializeField] private Grabbable grabbable;

    public override void SetupHull(Sliceable newSliceable)
    {
        base.SetupHull(newSliceable);

        //newSliceable.gameObject.AddComponent<Grabbable>();
    }
}
