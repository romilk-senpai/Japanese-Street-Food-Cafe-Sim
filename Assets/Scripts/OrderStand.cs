using BNG;
using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum OrderableFood
{
    Burger
}

public enum OrderableDrinks
{
    Cola,
    Fanta,
    Sprite,
    Pepsi
}

public class OrderStand : MonoBehaviour
{
    public event System.Action OnOrderFulfilled;

    [SerializeField] private Transform orderAiSpot;
    [SerializeField] private CanvasGroup orderCanvasGroup;

    [SerializeField] private Image burgerSprite;

    [SerializeField] private Image colaImage;
    [SerializeField] private Image spriteImage;
    [SerializeField] private Image fantaImage;
    [SerializeField] private Image pepsiImage;

    private bool _isBusy = false;
    private bool _hasOrder = false;
    private OrderableFood[] _orderedFood;
    private OrderableDrinks[] _orderedDrinks;

    private List<PreparedFood> _foodInTrigger = new List<PreparedFood>();

    public Transform OrderAiSpot => orderAiSpot;
    public bool HasOrder => _hasOrder;
    public bool IsBusy { get => _isBusy; set => _isBusy = value; }

    public void GenerateRandomOrder()
    {
        if (_hasOrder)
            return;

        _orderedFood = new[] { OrderableFood.Burger };
        _orderedDrinks = new[] { (OrderableDrinks)Random.Range(0, 4) };

        _hasOrder = true;

        foreach (var food in _orderedFood)
        {
            GetFoodImage(food).gameObject.SetActive(true);
        }

        foreach (var drink in _orderedDrinks)
        {
            GetDrinkImage(drink).gameObject.SetActive(true);
        }

        orderCanvasGroup.gameObject.SetActive(true);
        orderCanvasGroup.DOFade(1f, 1f);
    }

    private void FinishOrder()
    {
        _hasOrder = false;

        orderCanvasGroup.DOFade(0f, 1f).OnComplete(() =>
        {
            orderCanvasGroup.gameObject.SetActive(false);

            foreach (var food in _orderedFood)
            {
                GetFoodImage(food).gameObject.SetActive(false);
            }

            foreach (var drink in _orderedDrinks)
            {
                GetDrinkImage(drink).gameObject.SetActive(false);
            }
        });
    }

    private void OnTriggerEnter(Collider other)
    {
        var preparedFood = other.GetComponent<PreparedFood>();

        if (preparedFood != null && !_foodInTrigger.Contains(preparedFood))
        {
            _foodInTrigger.Add(preparedFood);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        var preparedFood = other.GetComponent<PreparedFood>();

        if (preparedFood != null && _foodInTrigger.Contains(preparedFood))
        {
            _foodInTrigger.Add(preparedFood);
        }
    }

    private void Update()
    {
        if (!_hasOrder)
            return;

        bool foodFullfilled = true;
        List<PreparedFood> rightFood = new List<PreparedFood>();

        foreach (var food in _orderedFood)
        {
            var foodInTrigger = _foodInTrigger.Find(foodInTrigger => foodInTrigger.IsFood && foodInTrigger.OrderableFoodType == food);

            if (foodInTrigger == null || foodInTrigger.GetComponent<Grabbable>().BeingHeld)
            {
                foodFullfilled = false;
                break;
            }
            else
            {
                rightFood.Add(foodInTrigger);
            }
        }

        foreach (var drink in _orderedDrinks)
        {
            var foodInTrigger = _foodInTrigger.Find(foodInTrigger => foodInTrigger.IsDrink && foodInTrigger.OrderableDrinkType == drink);

            if (foodInTrigger == null || foodInTrigger.GetComponent<Grabbable>().BeingHeld)
            {
                foodFullfilled = false;
                break;
            }
            else
            {
                rightFood.Add(foodInTrigger);
            }
        }

        if (foodFullfilled)
        {
            foreach (var food in rightFood)
            {
                _foodInTrigger.Remove(food);
                Destroy(food.gameObject);
            }

            OnOrderFulfilled?.Invoke();

            FinishOrder();
        }
    }

    private Image GetFoodImage(OrderableFood food)
    {
        return food switch
        {
            OrderableFood.Burger => burgerSprite,
            _ => null
        };
    }

    private Image GetDrinkImage(OrderableDrinks food)
    {
        return food switch
        {
            OrderableDrinks.Cola => colaImage,
            OrderableDrinks.Fanta => fantaImage,
            OrderableDrinks.Sprite => spriteImage,
            OrderableDrinks.Pepsi => pepsiImage,
            _ => null
        };
    }
}
