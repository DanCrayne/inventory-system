using TMPro;
using UnityEngine;

public class ItemSlot : MonoBehaviour
{
    public ItemData ItemData;
    public bool IsEmpty = true; // Flag to check if the slot is empty

    public GameObject SlotIcon; // The icon object for the slot
    public TMP_Text SlotText; // The text object for the slot
}
