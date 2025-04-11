using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryUiController : MonoBehaviour
{
    public GameObject parentSlotComponent;
    public Canvas InventoryCanvas;
    public Transform InventoryContent;
    public GameObject ItemSlotPrefab;
    public GameObject SelectedItemImage;
    public TMP_Text SelectedItemText;

    private bool _menuActivated = false;

    private void Awake()
    {
        if (InventoryCanvas != null)
        {
            InventoryCanvas.gameObject.SetActive(false);
        }

        PlayerController.OnToggleInventory += OnToggleInventory;
    }

    private void OnDestroy()
    {
        PlayerController.OnToggleInventory -= OnToggleInventory;
    }

    private void Update()
    {
        var selectedSlot = EventSystem.current.currentSelectedGameObject;
        if (selectedSlot != null)
        {
            var itemSlot = selectedSlot.GetComponent<ItemSlot>();
            if (itemSlot != null && !itemSlot.IsEmpty)
            {
                var item = itemSlot.ItemPrefab.GetComponent<Item>();
                SelectedItemImage.GetComponent<Image>().sprite = item.itemData.icon;
                SelectedItemText.text = item.itemData.description;
            }
            else
            {
                SelectedItemImage.GetComponent<Image>().sprite = null;
                SelectedItemText.text = string.Empty;
            }
        }
    }

    private void OnToggleInventory()
    {
        if (InventoryCanvas != null)
        {
            _menuActivated = !_menuActivated;
            if (_menuActivated)
            {
                InventoryCanvas.gameObject.SetActive(true);
                GameManager.Instance.PauseGame();
            }
            else
            {
                InventoryCanvas.gameObject.SetActive(false);
                GameManager.Instance.UnpauseGame();
            }
        }
        PopulateInventoryDisplay();
    }

    public void PopulateInventoryDisplay()
    {
        StartCoroutine(PopulateInventoryDisplayCoroutine());
    }

    private IEnumerator PopulateInventoryDisplayCoroutine()
    {
        foreach (Transform child in InventoryContent)
        {
            Destroy(child.gameObject);
        }

        foreach (ItemData item in GameManager.Instance.playerCharacter.items)
        {
            AddItemToInventoryDisplay(item);
            yield return null; // Wait for the next frame to ensure the item is added
        }

        // Ensure all items are added before continuing
        yield return new WaitForEndOfFrame();

        EventSystem.current.SetSelectedGameObject(null); // clear previous selection
        if (parentSlotComponent == null)
        {
            Debug.LogError("Parent Slot Component is not assigned in the Inspector.");
            yield break;
        }
        if (parentSlotComponent.transform.childCount == 0)
        {
            Debug.LogWarning("Parent Slot Component has no child slots.");
            yield break;
        }

        // Set the first child of the parent slot component as the default selected slot
        var defaultSelectedSlot = parentSlotComponent.transform.GetChild(0).gameObject;

        if (defaultSelectedSlot != null)
        {
            EventSystem.current.SetSelectedGameObject(defaultSelectedSlot);
        }
    }

    private void AddItemToInventoryDisplay(ItemData item = null)
    {
        var itemSlot = Instantiate(ItemSlotPrefab, InventoryContent);
        var itemSlotScript = itemSlot.GetComponent<ItemSlot>();

        if (itemSlotScript != null)
        {
            if (item == null)
            {
                itemSlotScript.IsEmpty = true;
                itemSlotScript.ItemPrefab = null;
                itemSlotScript.SlotText.text = string.Empty;
                itemSlotScript.SlotIcon.GetComponent<Image>().color = new Color(1, 1, 1, 0); // Make it transparent
                return;
            }

            itemSlotScript.ItemPrefab = item.prefab;
            itemSlotScript.SlotIndex = InventoryContent.childCount - 1; // Set the index based on the current child count
            itemSlotScript.IsEmpty = false;
            itemSlotScript.SlotText.text = item.itemName;
            itemSlotScript.SlotIcon.GetComponent<Image>().sprite = item.icon;
        }
        else
        {
            Debug.LogError("ItemSlot component not found on ItemSlotPrefab.");
        }
    }
}
