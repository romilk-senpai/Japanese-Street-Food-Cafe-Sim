using UnityEngine;

public class PreparedFood : MonoBehaviour
{
    [SerializeField] private bool isFood = false;
    [SerializeField] private OrderableFood orderableFoodType;
    [SerializeField] private bool isDrink = false;
    [SerializeField] private OrderableDrinks orderableDrinkType;

    public OrderableFood OrderableFoodType
    {
        get => orderableFoodType;
        set
        {
            isFood = true;
            isDrink = false;
            orderableFoodType = value;
        }
    }

    public OrderableDrinks OrderableDrinkType
    {
        get => orderableDrinkType;
        set
        {
            isFood = false;
            isDrink = true;
            orderableDrinkType = value;
        }
    }

    public bool IsFood => isFood;
    public bool IsDrink => isDrink;
}
