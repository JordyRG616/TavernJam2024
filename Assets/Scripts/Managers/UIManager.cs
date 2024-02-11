using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    [Header("Final Fruit")]
    [SerializeField] GameObject finalFruitObject;
    [SerializeField] Image finalFruitFill;
    
    [Header("Textos Na Tela")]
    [SerializeField] TextMeshProUGUI coinsText;
    [SerializeField] TextMeshProUGUI newDogValueText;
    [SerializeField] TextMeshProUGUI newBullValueText;
    [SerializeField] TextMeshProUGUI newBunnyValueText;
    [SerializeField] TextMeshProUGUI upgradeDogValueText;
    [SerializeField] TextMeshProUGUI upgradeBullValueText;
    [SerializeField] TextMeshProUGUI upgradeBunnyValueText;

    [Header("Botões e Objetos")]
    [SerializeField] Button newDogButton;
    [SerializeField] Button newBullButton;
    [SerializeField] Button newBunnyButton;
    [SerializeField] GameObject upgradeDogObject;
    [SerializeField] GameObject upgradeBullObject;
    [SerializeField] GameObject upgradeBunnyObject;

    [Header("Prefabs")]
    [SerializeField] GameObject dogPrefab;
    [SerializeField] GameObject bullPrefab;
    [SerializeField] GameObject bunnyPrefab;

    [Header("Custos Iniciais")]
    [SerializeField] int newDogInicialValue;
    [SerializeField] int newBullInicialValue;
    [SerializeField] int newBunnyInicialValue;
    [SerializeField] int upgradeDogInicialValue;
    [SerializeField] int upgradeBullInicialValue;
    [SerializeField] int upgradeBunnyInicialValue;

    [Header("Aumento de Custo por Compra")]
    [SerializeField] int newDogPlusValue;
    [SerializeField] int newBullPlusValue;
    [SerializeField] int newBunnyPlusValue;
    [SerializeField] int upgradeDogPlusValue;
    [SerializeField] int upgradeBullPlusValue;
    [SerializeField] int upgradeBunnyPlusValue;

    int newDogValue;
    int newBullValue;
    int newBunnyValue;
    int upgradeDogValue;
    int upgradeBullValue;
    int upgradeBunnyValue;
    bool finalPhase;

    private InventoryManager inventory;
    private RanchManager ranch;
    private Bonsai bonsaiTree;

    void Start()
    {
        finalPhase = false;

        inventory = FindObjectOfType<InventoryManager>();
        ranch = FindObjectOfType<RanchManager>();
        bonsaiTree = FindObjectOfType<Bonsai>();

        newDogValue = newDogInicialValue;
        newBullValue = newBullInicialValue;
        newBunnyValue = newBunnyInicialValue;
        upgradeDogValue = upgradeDogInicialValue;
        upgradeBullValue = upgradeBullInicialValue;
        upgradeBunnyValue = upgradeBunnyInicialValue;
    }

    void Update()
    {
        coinsText.text = inventory.FruitAmount.ToString();

        newDogValueText.text = newDogValue.ToString();
        newBullValueText.text = newBullValue.ToString();
        newBunnyValueText.text = newBunnyValue.ToString();
        upgradeDogValueText.text = upgradeDogValue.ToString();
        upgradeBullValueText.text = upgradeBullValue.ToString();
        upgradeBunnyValueText.text = upgradeBunnyValue.ToString();

        CheckGoldValue();

        if (finalPhase) FinalFruitFillAmount();
    }

    void CheckGoldValue()
    {
        newDogButton.interactable = inventory.HasEnoughFruits(newDogValue);
        newBullButton.interactable = inventory.HasEnoughFruits(newBullValue);
        newBunnyButton.interactable = inventory.HasEnoughFruits(newBunnyValue);

        upgradeDogObject.SetActive(inventory.HasEnoughFruits(upgradeDogValue) && (ranch.GetBixinhoAmount(BixinhoType.Gatherer) > 0));
        upgradeBullObject.SetActive(inventory.HasEnoughFruits(upgradeBullValue) && (ranch.GetBixinhoAmount(BixinhoType.Headbanger) > 0));
        upgradeBunnyObject.SetActive(inventory.HasEnoughFruits(upgradeBunnyValue) && (ranch.GetBixinhoAmount(BixinhoType.Pooper) > 0));
    }

    // Funções das compras dos botões

    public void NewDog()
    {
        if (!inventory.HasEnoughFruits(newDogValue)) return;
        inventory.SpendFruits(newDogValue);
        Instantiate(dogPrefab);
        newDogValue += newDogPlusValue;
    }

    public void NewBull()
    {
        if (!inventory.HasEnoughFruits(newBullValue)) return;
        inventory.SpendFruits(newBullValue);
        Instantiate(bullPrefab);
        newBullValue += newBullPlusValue;
    }

    public void NewBunny()
    {
        if (!inventory.HasEnoughFruits(newBunnyValue)) return;
        inventory.SpendFruits(newBunnyValue);
        Instantiate(bunnyPrefab);
        newBunnyValue += newBunnyPlusValue;
    }

    public void UpgradeDog()
    {
        if (!inventory.HasEnoughFruits(upgradeDogValue)) return;
        inventory.SpendFruits(upgradeDogValue);
        ranch.UpgradeRandomBixinho(BixinhoType.Gatherer);
        upgradeDogValue += upgradeDogPlusValue;
    }

    public void UpgradeBull()
    {
        if (!inventory.HasEnoughFruits(upgradeBullValue)) return;
        inventory.SpendFruits(upgradeBullValue);
        ranch.UpgradeRandomBixinho(BixinhoType.Headbanger);
        upgradeBullValue += upgradeBullPlusValue;
    }

    public void UpgradeBunny()
    {
        if (!inventory.HasEnoughFruits(upgradeBunnyValue)) return;
        inventory.SpendFruits(upgradeBunnyValue);
        ranch.UpgradeRandomBixinho(BixinhoType.Pooper);
        upgradeBunnyValue += upgradeBunnyPlusValue;
    }

    public void FinalFruit()
    {
        finalFruitObject.SetActive(true);
        finalPhase = true;
    }

    void FinalFruitFillAmount()
    {
        finalFruitFill.fillAmount = bonsaiTree.FinalPhaseFillAmount();
    }
}
