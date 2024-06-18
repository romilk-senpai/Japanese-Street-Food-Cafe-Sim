using BNG;
using UnityEngine;

public class Stove : MonoBehaviour
{
    [SerializeField] private PattySlot[] pattySlots;
    [SerializeField] private HingeHelper hingeHelper;

    private float _hingeValue = 0f;

    private void Start()
    {
        hingeHelper.onHingeSnapChange.AddListener(OnHingeSnapChange);
    }

    private void OnHingeSnapChange(float value)
    {
        _hingeValue = value;
    }

    private void Update()
    {
        if (_hingeValue != 0f)
        {
            foreach (var pattySlot in pattySlots)
            {
                if (pattySlot.HasPatty && !pattySlot.Patty.BeingBaked)
                {
                    pattySlot.Patty.StartBaking();
                }
                else if (pattySlot.HasPatty && pattySlot.Patty.BeingBaked)
                {
                    pattySlot.Patty.Doneness += Time.deltaTime;
                }
            }
        }
        else
        {
            foreach (var pattySlot in pattySlots)
            {
                if (pattySlot.HasPatty && pattySlot.Patty.BeingBaked)
                {
                    pattySlot.Patty.StopBaking();
                }
            }
        }

        foreach (var pattySlot in pattySlots)
        {
        }
    }
}
