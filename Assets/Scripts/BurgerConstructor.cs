using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;

public enum BurgerComponentEnum
{
    BunBot,
    Salad,
    Tomato,
    Patty,
    Cheese,
    BunTop
}

public class BurgerConstructor : MonoBehaviour
{
    [SerializeField] private GameObject burgerConstructorHint;
    [SerializeField] private Renderer bunBotHint;
    [SerializeField] private Renderer saladHint;
    [SerializeField] private Renderer tomatoHint;
    [SerializeField] private Renderer pattyHint;
    [SerializeField] private Renderer cheeseHint;
    [SerializeField] private Renderer bunTopHint;

    [SerializeField] private GameObject burgerConstructorDone;
    [SerializeField] private Renderer bunBotDone;
    [SerializeField] private Renderer saladDone;
    [SerializeField] private Renderer tomatoDone;
    [SerializeField] private Renderer pattyDone;
    [SerializeField] private Renderer cheeseDone;
    [SerializeField] private Renderer bunTopDone;

    [SerializeField] private Color highlightHintColor;
    [SerializeField] private Color initialHintColor;

    [SerializeField] private ParticleSystem doneParticleSystem;
    [SerializeField] private AudioSource audioSource;

    [SerializeField] private GameObject burgerPrefab;
    [SerializeField] private Transform burgerPrefabSpawnPoint;

    [SerializeField] private AudioClip constructedSound;
    [SerializeField] private AudioClip componentSound;

    private BurgerComponentEnum[] _burgerRecipe;
    private List<BurgerComponentEnum> _currentBurgerComponents;

    private bool _constructing = true;

    private void Start()
    {
        _burgerRecipe = new[] {
            BurgerComponentEnum.BunBot,
            BurgerComponentEnum.Salad,
            BurgerComponentEnum.Tomato,
            BurgerComponentEnum.Patty,
            BurgerComponentEnum.Cheese,
            BurgerComponentEnum.BunTop
        };

        ResetConstruction();
    }

    private void ResetConstruction()
    {
        _currentBurgerComponents = new List<BurgerComponentEnum>();

        burgerConstructorHint.SetActive(true);
        burgerConstructorDone.SetActive(true);

        _constructing = true;

        UpdateVisuals();
    }

    private void StopConstructing()
    {
        burgerConstructorHint.SetActive(false);
        burgerConstructorDone.SetActive(false);

        _constructing = false;
    }

    private void UpdateVisuals()
    {
        for (int i = 0; i < _burgerRecipe.Length; i++)
        {
            if (i < _currentBurgerComponents.Count)
            {
                GetComponentHint(_burgerRecipe[i]).gameObject.SetActive(false);
                GetComponentDone(_burgerRecipe[i]).gameObject.SetActive(true);
            }
            else if (i == _currentBurgerComponents.Count)
            {
                GetComponentHint(_burgerRecipe[i]).gameObject.SetActive(true);
                GetComponentDone(_burgerRecipe[i]).gameObject.SetActive(false);
            }
            else
            {
                GetComponentHint(_burgerRecipe[i]).gameObject.SetActive(false);
                GetComponentDone(_burgerRecipe[i]).gameObject.SetActive(false);
            }
        }
    }

    private void Update()
    {
        if (_constructing)
        {
            burgerConstructorHint.transform.Rotate(Vector3.forward * 30f * Time.deltaTime);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        var component = other.GetComponent<BurgerComponent>();

        if (component != null && _currentBurgerComponents.Count <= _burgerRecipe.Length
            && _burgerRecipe[_currentBurgerComponents.Count] == component.BurgerComponentType)
        {
            if ((component.SpatulaGrabbable != null && component.SpatulaGrabbable.BeingGrabbed) || component.Grabbable.BeingHeld)
            {
                GetComponentHint(_burgerRecipe[_currentBurgerComponents.Count]).material.color = highlightHintColor;
            }
            else
            {
                GetComponentHint(_burgerRecipe[_currentBurgerComponents.Count]).material.color = initialHintColor;

                _currentBurgerComponents.Add(component.BurgerComponentType);

                UpdateVisuals();

                if (_currentBurgerComponents.Count == _burgerRecipe.Length)
                {
                    StopConstructing();

                    doneParticleSystem.Play();
                    audioSource.PlayOneShot(constructedSound);

                    ResetConstruction();

                    Instantiate(burgerPrefab, burgerPrefabSpawnPoint.position, burgerPrefabSpawnPoint.rotation);
                }
                else
                {
                    audioSource.PlayOneShot(componentSound);
                }

                component.gameObject.SetActive(false);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        var component = other.GetComponent<BurgerComponent>();

        if (component != null && _currentBurgerComponents.Count <= _burgerRecipe.Length
            && _burgerRecipe[_currentBurgerComponents.Count] == component.BurgerComponentType)
        {
            if ((component.SpatulaGrabbable != null && component.SpatulaGrabbable.BeingGrabbed) || component.Grabbable.BeingHeld)
            {
                GetComponentHint(_burgerRecipe[_currentBurgerComponents.Count]).material.color = initialHintColor;
            }
            else
            {
                GetComponentHint(_burgerRecipe[_currentBurgerComponents.Count]).material.color = initialHintColor;
            }
        }
    }

    private Renderer GetComponentHint(BurgerComponentEnum component)
    {
        return component switch
        {
            BurgerComponentEnum.BunBot => bunBotHint,
            BurgerComponentEnum.Salad => saladHint,
            BurgerComponentEnum.Tomato => tomatoHint,
            BurgerComponentEnum.Patty => pattyHint,
            BurgerComponentEnum.Cheese => cheeseHint,
            BurgerComponentEnum.BunTop => bunTopHint,
            _ => null,
        };
    }

    private Renderer GetComponentDone(BurgerComponentEnum component)
    {
        return component switch
        {
            BurgerComponentEnum.BunBot => bunBotDone,
            BurgerComponentEnum.Salad => saladDone,
            BurgerComponentEnum.Tomato => tomatoDone,
            BurgerComponentEnum.Patty => pattyDone,
            BurgerComponentEnum.Cheese => cheeseDone,
            BurgerComponentEnum.BunTop => bunTopDone,
            _ => null,
        };
    }
}
