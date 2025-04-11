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

    // This method is called every frame while the trigger collider attached to this GameObject
    // is inside another trigger collider
    private void OnTriggerStay2D(Collider2D other)
    {
        Debug.Log("Still inside trigger with " + other.gameObject.name);
    }

    // This method is called when the trigger collider attached to this GameObject
    // exits another trigger collider
    private void OnTriggerExit2D(Collider2D other)
    {
        Debug.Log("Trigger exited with " + other.gameObject.name);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
