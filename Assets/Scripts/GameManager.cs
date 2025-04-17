using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject player;
    public static GameManager Instance;

    private InventoryManager _inventoryManager;
    private ItemSpawner _itemSpawner;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        _inventoryManager = GetComponent<InventoryManager>();
        _itemSpawner = GetComponent<ItemSpawner>();
    }

    public ItemSpawner GetItemSpawner()
    {
        return _itemSpawner;
    }

    public InventoryManager GetInventoryManager()
    {
        return _inventoryManager;
    }

    public UiController GetUiController()
    {
        return GetComponent<UiController>();
    }

    public void PauseGame()
    {
        Time.timeScale = 0f;
    }

    public void UnpauseGame()
    {
        Time.timeScale = 1f;
    }
}
