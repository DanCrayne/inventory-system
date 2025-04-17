using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    public ItemData[] placeableItems;
    public int numberOfItemsToSpawn;
    public GameObject parentContainer; // GameObject to store the spawned items in for cleanliness
    public Vector2 origin = new Vector2(0, 0);

    void Start()
    {
        SpawnItems();
    }

    public void SpawnItem(ItemData itemData, int quantity, Vector3 position)
    {
        // Instantiate item and set quantity
        var item = Instantiate(itemData.prefab, position, Quaternion.identity);
        var itemBehavior = item.GetComponent<ItemBehavior>();
        itemBehavior.quantity = quantity;

        item.transform.SetParent(parentContainer.transform, false);
    }

    private void SpawnItems()
    {
        for (int i = 0; i < numberOfItemsToSpawn; i++)
        {
            var randomIndex = Random.Range(0, placeableItems.Length);
            var itemData = placeableItems[randomIndex];
            var randomPoint = new Vector3(Random.Range(-10, 10), Random.Range(-8, 8), 0);

            SpawnItem(itemData, 1, randomPoint);
        }
    }
}
