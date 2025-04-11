using TMPro;
using UnityEngine;

public class ItemSlot : MonoBehaviour
{
    public GameObject ItemPrefab; // The prefab of the item to be placed in the slot
    public int SlotIndex; // The index of the slot in the inventory
    public bool IsEmpty = true; // Flag to check if the slot is empty
    public GameObject ItemInstance; // The instance of the item in the slot

    public GameObject SlotBackground; // The background object for the slot
    public GameObject SlotHighlight; // The highlight object for the slot
    public GameObject SlotIcon; // The icon object for the slot
    public TMP_Text SlotText; // The text object for the slot
}
