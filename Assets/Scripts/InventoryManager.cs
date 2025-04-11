using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public GameObject InventoryItemPrefab;
    public Transform InventoryContent;
    public GameObject ItemSlotPrefab;

    public static InventoryManager Instance;

    private bool _menuActivated = false;
    private GameObject _inventoryCanvas;

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
    }

    private void Start()
    {
        _inventoryCanvas = GameObject.Find("InventoryCanvas");

        if (_inventoryCanvas != null)
        {
            _inventoryCanvas.SetActive(false);
        }

        PlayerController.OnToggleInventory += ToggleInventory;
    }

    private void Update()
    {
        if (_menuActivated)
        {
            PopulateInventoryDisplay();
        }
    }

    private void OnDestroy()
    {
        PlayerController.OnToggleInventory -= ToggleInventory;
    }

    public void ToggleInventory()
    {
        if (_inventoryCanvas != null)
        {
            _menuActivated = !_menuActivated;
            if (_menuActivated)
            {
                _inventoryCanvas.SetActive(true);
                Time.timeScale = 0f; // Pause the game
            }
            else
            {
                _inventoryCanvas.SetActive(false);
                Time.timeScale = 1f; // Resume the game
            }
        }
    }

    public void AddItemToInventoryDisplay(ItemData item = null)
    {
        var itemSlot = Instantiate(ItemSlotPrefab, InventoryContent);
        var itemSlotScript = itemSlot.GetComponent<ItemSlot>();

        if (itemSlotScript != null)
        {
            if (item == null)
            {
                itemSlotScript.IsEmpty = true;
                itemSlotScript.ItemPrefab = null;
                itemSlotScript.ItemInstance = null;
                itemSlotScript.SlotText.text = string.Empty;
                itemSlotScript.SlotIcon.GetComponent<Image>().color = new Color(1, 1, 1, 0); // Make it transparent
                return;
            }

            itemSlotScript.ItemPrefab = item.prefab;
            itemSlotScript.SlotIndex = InventoryContent.childCount - 1; // Set the index based on the current child count
            itemSlotScript.IsEmpty = false;
            itemSlotScript.ItemInstance = Instantiate(item.prefab, itemSlot.transform);
            itemSlotScript.SlotText.text = item.itemName;
            itemSlotScript.SlotIcon.GetComponent<Image>().sprite = item.icon;
        }
        else
        {
            Debug.LogError("ItemSlot component not found on ItemSlotPrefab.");
        }
    }

    private void PopulateInventoryDisplay()
    {
        foreach (Transform child in InventoryContent)
        {
            Destroy(child.gameObject);
        }
        foreach (ItemData item in GameManager.Instance.playerCharacter.items)
        {
            AddItemToInventoryDisplay(item);
        }
    }
}
