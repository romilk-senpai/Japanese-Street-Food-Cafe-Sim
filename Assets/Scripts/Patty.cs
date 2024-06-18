using BNG;
using Cysharp.Threading.Tasks;
using UnityEngine;
using DG.Tweening;

public class Patty : MonoBehaviour
{
    [SerializeField] private Grabbable grabbable;
    [SerializeField] private SpatulaGrabbable spatulaGrabbable;
    [SerializeField] private int prepareTime = 10;
    [SerializeField] private MeshRenderer gfx;
    [SerializeField] private ParticleSystem bakingEffect;
    [SerializeField] private Rigidbody pattyRigidbody;
    [SerializeField] private BurgerComponent burgerComponent;

    [SerializeField] private Color bakedColor = new Color32(121, 141, 82, 255);

    private float _doneness = 0f;
    private Color _gfxMaterialInitialColor;

    public bool BeingBaked { get; private set; }
    public bool Done;
    public Grabbable Grabbable => grabbable;
    public Rigidbody PattyRigidbody => pattyRigidbody;

    public float Doneness
    {
        get
        {
            return _doneness;
        }
        set
        {
            if (value != _doneness)
            {
                OnDonenessUpdated(_doneness, value);
            }

            _doneness = value;
        }
    }

    private void Start()
    {
        spatulaGrabbable.CanGrab = false;
        _gfxMaterialInitialColor = gfx.material.color;
    }

    public void StartBaking()
    {
        BeingBaked = true;

        pattyRigidbody.isKinematic = true;

        bakingEffect.Play();
    }

    public void StopBaking()
    {
        BeingBaked = false;

        bakingEffect.Stop();
    }

    private void OnDonenessUpdated(float prevValue, float newValue)
    {
        if (Done)
        {
            return;
        }

        var donenessValue = newValue;

        if (newValue > prepareTime)
        {
            donenessValue = prepareTime;

            pattyRigidbody.isKinematic = false;
            spatulaGrabbable.CanGrab = true;

            burgerComponent.CanUse = true;

            bakingEffect.Stop();

            Done = true;
        }

        var color = Color.Lerp(_gfxMaterialInitialColor, bakedColor, donenessValue / prepareTime);

        gfx.material.DOColor(color, Time.deltaTime).SetEase(Ease.Linear);
    }
}
