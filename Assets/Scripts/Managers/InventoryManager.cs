using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : ManagerBehaviour
{
    [Header("Signals")]
    public Signal<int> OnFruitsGained;
    public Signal<int> OnFruitsSpended;
    
    [SerializeField] private int initialFruitAmount;

    public int FruitAmount { get; private set; }


    private void Start()
    {
        FruitAmount = initialFruitAmount;
    }

    public bool HasEnoughFruits(int amount)
    {
        return FruitAmount >= amount;
    }

    public void SpendFruits(int amount)
    {
        if (!HasEnoughFruits(amount)) return;

        FruitAmount -= amount;
        OnFruitsSpended.Fire(amount);
    }

    public void GainFruits(int amount)
    {
        FruitAmount += amount;
        OnFruitsGained.Fire(amount);
    }
}
