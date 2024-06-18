using BNG;
using UnityEngine;

public class BurgerComponent : MonoBehaviour
{
    [SerializeField] private BurgerComponentEnum burgerComponentType;
    [SerializeField] private Grabbable grabbable;
    [SerializeField] private SpatulaGrabbable spatulaGrabbable;

    [SerializeField] private bool canUse = true;

    public BurgerComponentEnum BurgerComponentType => burgerComponentType;
    public Grabbable Grabbable => grabbable;
    public SpatulaGrabbable SpatulaGrabbable => spatulaGrabbable;
    public bool CanUse { get => canUse; set => canUse = value; }
}
