using System.Collections.Generic;
using UnityEngine;

public class Inventory_Merchant : Inventory_Base
{
    private Inventory_Player Inventory;

    [SerializeField] private ItemListDataSO shopData;
    [SerializeField] private int minItemsAmount = 4;

    protected override void Awake()
    {
        base.Awake();
        FillShopList();
    }

    public void TryBuyItem(Inventory_Item itemToBuy, bool buyFullStack)
    {
        int amountToBuy = buyFullStack ? itemToBuy.stackSize : 1;

        for (int i = 0; i < amountToBuy; i++)
        {
            if (Inventory.gold < itemToBuy.buyPrice)
            {
                Debug.Log("No Money poor guy");
                return;
            }

            if (itemToBuy.itemData.itemType == ItemType.Material)
            {
                Inventory.storage.AddMaterialToStash(itemToBuy);
            }
            else
            {
                if (Inventory.CanAddItem(itemToBuy))
                {
                    var itemToAdd = new Inventory_Item(itemToBuy.itemData);
                    Inventory.AddItem(itemToAdd);
                }
            }
            
            Inventory.gold -= itemToBuy.buyPrice;
            RemoveOneItem(itemToBuy);
        }
        
        TriggerUpdateUI();
    }

    public void TrySellItem(Inventory_Item itemToSell, bool sellFullStack)
    {
        int amountToSell = sellFullStack ? itemToSell.stackSize : 1;

        for (int i = 0; i < amountToSell; i++)
        {
            int sellPrice = Mathf.FloorToInt(itemToSell.sellPrice);
            
            Inventory.gold += sellPrice;
            Inventory.RemoveOneItem(itemToSell);
        }
        
        TriggerUpdateUI();
    }

    public void FillShopList()
    {
        itemList.Clear();
        List<Inventory_Item> possibleItems = new List<Inventory_Item>();

        foreach (var itemData in shopData.itemList)
        {
            int randomizedStack = Random.Range(itemData.minStackSizeAtShop, itemData.maxStackSizeAtShop + 1);
            int finalStack = Mathf.Clamp(randomizedStack, 1, itemData.maxStackSizeAtShop);

            Inventory_Item itemToAdd = new Inventory_Item(itemData);
            itemToAdd.stackSize = finalStack;
            
            possibleItems.Add(itemToAdd);
        }

        int randomItemAmount = Random.Range(minItemsAmount, maxInventorySize + 1);
        int finalAmount = Mathf.Clamp(randomItemAmount, 1, possibleItems.Count);

        for (int i = 0; i < finalAmount; i++)
        {
            var randomIndex = Random.Range(0, possibleItems.Count);
            var item = possibleItems[randomIndex];

            if (CanAddItem(item))
            {
                possibleItems.Remove(item);
                AddItem(item);
            }
        }
        
        TriggerUpdateUI();
    }
    
    public void SetInventory(Inventory_Player inventory) => this.Inventory = inventory;
}
