using UnityEngine;

public class Character : MonoBehaviour
{
    public string characterName;
    public ItemData[] items;
    public int maxItems = 20;

    void Start()
    {
        items = new ItemData[maxItems];
    }

    public void AddItem(ItemData item)
    {
        for (int i = 0; i < maxItems; i++)
        {
            if (items[i] == null)
            {
                items[i] = item;
                Debug.Log("Added " + item.itemName + " to inventory.");
                return;
            }
        }
        Debug.Log("Inventory is full. Cannot add " + item.itemName);
    }

    public void RemoveItem(ItemData item)
    {
        for (int i = 0; i < maxItems; i++)
        {
            if (items[i] == item)
            {
                items[i] = null;
                Debug.Log("Removed " + item.itemName + " from inventory.");
                return;
            }
        }
        Debug.Log(item.itemName + " not found in inventory.");
    }

    public void UseItem(ItemData item)
    {
        for (int i = 0; i < maxItems; i++)
        {
            if (items[i] == item)
            {
                // Implement item usage logic here
                Debug.Log("Used " + item.itemName);
                return;
            }
        }
        Debug.Log(item.itemName + " not found in inventory.");
    }

    public void MoveItem(ItemData item, int index)
    {
        for (int i = 0; i < maxItems; i++)
        {
            if (items[i] == item)
            {
                if (index >= 0 && index < maxItems)
                {
                    items[i] = null;
                    items[index] = item;
                    Debug.Log("Moved " + item.itemName + " to slot " + index);
                }
                else
                {
                    Debug.Log("Invalid index. Cannot move " + item.itemName);
                }
                return;
            }
        }

        Debug.Log(item.itemName + " not found in inventory.");
    }
}
