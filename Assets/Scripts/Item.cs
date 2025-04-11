using System;
using UnityEngine;

public class Item : MonoBehaviour
{
    public ItemData itemData;

    public int Quantity;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Trigger entered with " + collision.gameObject.name);
        if (collision.gameObject.CompareTag("Player"))
        {
            // Assuming you have a method to add the item to the player's inventory
            collision.gameObject.GetComponent<Character>().AddItem(itemData);
            //InventoryManager.Instance.AddItem(Item, Quantity);
            Debug.Log("Item added to inventory: " + itemData.name);
            Destroy(gameObject); // Destroy the item after adding it to the inventory
        }
    }
}
