using System.Linq;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public Item[] items;
    public int maxInventorySize = 20;
    public int maxStackSize = 999;

    void Awake()
    {
        items = new Item[maxInventorySize];
    }

    public int GetQuantityOfItemInInventory(ItemData itemData)
    {
        return items.Where(x => x != null && x.itemData == itemData).Sum(x => x.quantity);
    }

    public void AddItem(ItemData itemData, int quantity)
    {
        for (int i = 0; i < maxInventorySize; i++)
        {
            if (items[i] == null)
            {
                items[i] = new Item { itemData = itemData, quantity = quantity };
                Debug.Log("Added " + itemData.itemName + " to inventory.");
                return;
            }
            else if (items[i].itemData == itemData)
            {
                items[i].quantity += quantity;
                if (items[i].quantity > maxStackSize)
                {
                    // TODO: Handle overflow
                    items[i].quantity = maxStackSize;
                }
                Debug.Log("Updated quantity of " + itemData.itemName + " in inventory.");
                return;
            }
        }
        Debug.Log("Inventory is full. Cannot add " + itemData.itemName);
    }

    public void RemoveItem(ItemData itemData, int quantity)
    {
        for (int i = 0; i < maxInventorySize; i++)
        {
            if (items[i] != null && items[i].itemData == itemData)
            {
                items[i].quantity -= quantity;
                if (items[i].quantity <= 0)
                {
                    items[i] = null; // Remove item if quantity is zero
                    Debug.Log("Removed " + itemData.itemName + " from inventory.");
                }
                else
                {
                    Debug.Log("Updated quantity of " + itemData.itemName + " in inventory.");
                }

                return;
            }
        }

        Debug.Log(itemData.itemName + " not found in inventory.");
    }
}
