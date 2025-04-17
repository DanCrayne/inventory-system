using UnityEngine;

/// <summary>
/// Contains the behavior for an item prefab
/// </summary>
public class ItemBehavior : MonoBehaviour
{
    public ItemData itemData;
    public int quantity = 1;

    /// <summary>
    /// Detects a collision between the item and player, adds the item to the inventory, and then deletes the item instance
    /// </summary>
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            GameManager.Instance.GetInventoryManager().AddItem(itemData, quantity);
            Debug.Log("Item added to inventory: " + itemData.name);
            Destroy(gameObject);
        }
    }
}
