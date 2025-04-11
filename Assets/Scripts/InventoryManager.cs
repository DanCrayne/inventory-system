using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public Transform InventoryContent;
    public GameObject ItemSlotPrefab;
    public GameObject InventoryCanvas;
    public InventoryUiController InventoryUiController;

    public static InventoryManager Instance;

    private bool _menuActivated = false;

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

        //if (InventoryCanvas != null)
        //{
        //    InventoryCanvas.SetActive(false);
        //}

        PlayerController.OnToggleInventory += ToggleInventory;
    }

    private void OnDestroy()
    {
        PlayerController.OnToggleInventory -= ToggleInventory;
    }

    public void ToggleInventory()
    {
        //if (InventoryCanvas != null)
        //{
        //    _menuActivated = !_menuActivated;
        //    if (_menuActivated)
        //    {
        //        InventoryCanvas.SetActive(true);
        //        Time.timeScale = 0f; // Pause the game
        //    }
        //    else
        //    {
        //        InventoryCanvas.SetActive(false);
        //        Time.timeScale = 1f; // Resume the game
        //    }
        //}
    }
}
