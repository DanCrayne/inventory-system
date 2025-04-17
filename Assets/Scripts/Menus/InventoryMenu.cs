using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryMenu : MonoBehaviour, IMenu
{
    public static InventoryMenu Instance;

    public GameObject parentSlotComponent;
    public Canvas InventoryCanvas;
    public Transform InventoryContent;
    public GameObject ItemSlotPrefab;
    public GameObject SelectedItemImage;
    public TMP_Text SelectedItemText;
    public float DropOffset = 0.5f; // TODO: get offset from player collider size
    public GameObject DropItemPanelPrefab;
    public GameObject MainPopupPanel;

    private bool _menuActivated = false;
    private GameObject _dropItemPanel;
    private Stack _submenuStack = new Stack();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        if (InventoryCanvas != null)
        {
            InventoryCanvas.gameObject.SetActive(false);
        }
    }

    private void OnEnable()
    {
        UiController.ItemDropEvent += OnDropItem;
    }

    private void OnDisable()
    {
        UiController.ItemDropEvent -= OnDropItem;
    }

    private void Update()
    {
        UpdateSelectedItemDetails();
    }

    public bool CloseCurrentSubmenu()
    {
        if (_submenuStack.Count > 0)
        {
            Destroy((GameObject)_submenuStack.Pop());
            return true;
        }

        return false;
    }

    public bool IsSubmenuOpen()
    {
        return _submenuStack.Count > 0;
    }

    public Canvas GetMenuCanvas()
    {
        return InventoryCanvas;
    }

    public void InitializeMenu()
    {
        PopulateInventoryDisplay();
    }

    private void UpdateSelectedItemDetails()
    {
        var selectedSlot = EventSystem.current.currentSelectedGameObject;
        if (selectedSlot != null)
        {
            var itemSlot = selectedSlot.GetComponent<ItemSlot>();
            if (itemSlot != null && !itemSlot.IsEmpty)
            {
                var item = itemSlot.ItemData;
                SelectedItemImage.GetComponent<Image>().sprite = item.icon;
                SelectedItemText.text = item.description;
            }
            else
            {
                ClearSelectedItemDetails();
            }
        }
        else
        {
            ClearSelectedItemDetails();
        }
    }

    private void ClearSelectedItemDetails()
    {
        SelectedItemImage.GetComponent<Image>().sprite = null;
        SelectedItemText.text = string.Empty;
    }

    public void PopulateInventoryDisplay()
    {
        StartCoroutine(PopulateInventoryDisplayCoroutine());
    }

    private IEnumerator PopulateInventoryDisplayCoroutine()
    {
        ClearInventoryDisplay();

        foreach (Item item in GameManager.Instance.GetInventoryManager().items)
        {
            AddItemToInventoryDisplay(item);
            yield return null; // Wait for the next frame to ensure the item is added
        }

        // Ensure all items are added before continuing
        yield return new WaitForEndOfFrame();

        SetDefaultSelectedSlot();
    }

    private void ClearInventoryDisplay()
    {
        foreach (Transform child in InventoryContent)
        {
            Destroy(child.gameObject);
        }
    }

    private void SetDefaultSelectedSlot()
    {
        EventSystem.current.SetSelectedGameObject(null); // clear previous selection
        if (parentSlotComponent == null)
        {
            Debug.LogError("Parent Slot Component is not assigned in the Inspector.");
            return;
        }
        if (parentSlotComponent.transform.childCount == 0)
        {
            Debug.LogWarning("Parent Slot Component has no child slots.");
            return;
        }

        // Set the first child of the parent slot component as the default selected slot
        var defaultSelectedSlot = parentSlotComponent.transform.GetChild(0).gameObject;

        if (defaultSelectedSlot != null)
        {
            EventSystem.current.SetSelectedGameObject(defaultSelectedSlot);
        }
    }

    private void AddItemToInventoryDisplay(Item item)
    {
        var itemSlot = Instantiate(ItemSlotPrefab, InventoryContent);
        var itemSlotBehavior = itemSlot.GetComponent<ItemSlot>();

        if (itemSlotBehavior != null)
        {
            if (item?.itemData == null)
            {
                SetEmptyItemSlot(itemSlotBehavior);
                return;
            }

            SetItemSlot(itemSlotBehavior, item);
        }
        else
        {
            Debug.LogError("ItemSlot component not found on ItemSlotPrefab.");
        }
    }

    private void SetEmptyItemSlot(ItemSlot itemSlotBehavior)
    {
        itemSlotBehavior.IsEmpty = true;
        itemSlotBehavior.ItemData = null;
        itemSlotBehavior.SlotText.text = string.Empty;
        itemSlotBehavior.SlotIcon.GetComponent<Image>().color = new Color(1, 1, 1, 0); // Make it transparent
    }

    private void SetItemSlot(ItemSlot itemSlotBehavior, Item item)
    {
        itemSlotBehavior.ItemData = item.itemData;
        itemSlotBehavior.IsEmpty = false;
        itemSlotBehavior.SlotIcon.GetComponent<Image>().sprite = item.itemData.icon;
        itemSlotBehavior.SlotText.text = item.quantity > 1 ? $"{item.itemData.itemName} ({item.quantity})" : item.itemData.itemName;
    }

    private void OnDropItem()
    {
        var selectedSlot = EventSystem.current.currentSelectedGameObject;

        if (selectedSlot == null)
        {
            Debug.LogWarning("No item selected to drop.");
            return;
        }

        var itemSlot = selectedSlot.GetComponent<ItemSlot>();
        if (itemSlot == null || itemSlot.IsEmpty)
        {
            Debug.LogWarning("Selected slot is empty or not an item slot.");
            return;
        }

        var itemData = itemSlot.ItemData;
        var quantityInInventory = GameManager.Instance.GetInventoryManager().GetQuantityOfItemInInventory(itemData);

        if (quantityInInventory <= 0)
        {
            Debug.LogWarning("Item quantity is zero or negative.");
            return;
        }

        if (quantityInInventory > 1)
        {
            _dropItemPanel = Instantiate(DropItemPanelPrefab, MainPopupPanel.transform);

            // Open drop item panel to select quantity
            var dropItemPanelBehavior = _dropItemPanel.GetComponent<DropItemPanel>();
            dropItemPanelBehavior.SelectedItemSlot = itemSlot;
            dropItemPanelBehavior.maxDropItems = quantityInInventory;

            // Set the slider values
            dropItemPanelBehavior.Slider.value = 1; // Default to 1
            dropItemPanelBehavior.Slider.maxValue = quantityInInventory;

            // Subscribe to the drop event
            dropItemPanelBehavior.DropEvent += OnDropItemConfirmed;

            // Debug logs to verify values
            Debug.Log($"Setting slider value to 1");
            Debug.Log($"Setting slider maxValue to {quantityInInventory}");

            // Set the slider as the selected GameObject
            EventSystem.current.SetSelectedGameObject(dropItemPanelBehavior.Slider.gameObject);

            _submenuStack.Push(_dropItemPanel);
        }
        else
        {
            // Drop the item immediately if only one exists in the slot
            DropItemInSlot(itemSlot, quantityInInventory);
        }
    }

    public void OnDropItemConfirmed(ItemSlot itemSlot, int quantity)
    {
        DropItemInSlot(itemSlot, quantity);
        Destroy(_dropItemPanel);
    }

    private void DropItemInSlot(ItemSlot itemSlot, int quantity)
    {
        var dropPosition = GameManager.Instance.player.transform.position + new Vector3(DropOffset, DropOffset);
        Debug.Log($"Character at {GameManager.Instance.player.transform.position} and drop position at {dropPosition} (drop offset is {DropOffset})");

        if (itemSlot != null)
        {
            if (itemSlot != null && !itemSlot.IsEmpty)
            {
                GameManager.Instance.GetItemSpawner().SpawnItem(itemSlot.ItemData, quantity, dropPosition);
                GameManager.Instance.GetInventoryManager().RemoveItem(itemSlot.ItemData, quantity);
                Destroy(itemSlot);
                PopulateInventoryDisplay();
            }
        }
    }
}
