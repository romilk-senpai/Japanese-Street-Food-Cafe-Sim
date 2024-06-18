using BNG;
using System.Collections;
using UnityEngine;
using DG.Tweening;

public class DrinkSpot : MonoBehaviour
{
    [SerializeField] private SnapZone snapZone;
    [SerializeField] private OrderableDrinks drinkType;
    [SerializeField] private ParticleSystem fillParticleSystem;

    [SerializeField] private Color liquidColor = Color.white;
    [SerializeField] private Vector3 liquidTargetScale = new Vector3(0.1050095f, 0.0002893895f, 0.1050095f);
    [SerializeField] private float liquidTargetY = 0.1491f;

    private bool drinkSnapped = false;

    private void Start()
    {
        snapZone.OnSnapEvent.AddListener(OnDrinkSnapped);
    }

    private void OnDrinkSnapped(Grabbable grabbableDrink)
    {
        drinkSnapped = true;

        grabbableDrink.enabled = true;

        grabbableDrink.gameObject.AddComponent<PreparedFood>().OrderableDrinkType = drinkType;
        grabbableDrink.gameObject.name = drinkType.ToString();

        StartCoroutine(FillDrink(grabbableDrink));
    }

    private IEnumerator FillDrink(Grabbable grabbableDrink)
    {
        fillParticleSystem.Play();

        var liquid = grabbableDrink.transform.GetChild(1);

        liquid.gameObject.SetActive(true);
        liquid.GetComponent<Renderer>().material.color = liquidColor;
        liquid.DOScale(liquidTargetScale, 10f).SetEase(Ease.Linear);
        liquid.DOLocalMoveY(liquidTargetY, 10f).SetEase(Ease.Linear);

        yield return new WaitForSeconds(10f);

        yield return null;

        fillParticleSystem.Stop();

        grabbableDrink.enabled = true;

        drinkSnapped = false;
    }
}
