using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MenuManager : MonoBehaviour
{
    public static MenuManager Instance;

    public GameObject InventoryMenu;
    public GameObject MainMenu;
    public Canvas SettingsCanvas;
    public Canvas PauseMenuCanvas;

    private Dictionary<MenuType, IMenu> _menus;
    private MenuType _currentMenu;

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

        _menus = new Dictionary<MenuType, IMenu>
        {
            { MenuType.Inventory, InventoryMenu.GetComponent<IMenu>() },
            //{ MenuType.MainMenu, MainMenuCanvas.GetComponent<IMenu>() },
            //{ MenuType.Settings, SettingsCanvas.GetComponent<IMenu>() },
            //{ MenuType.PauseMenu, PauseMenuCanvas.GetComponent<IMenu>() }
        };

        foreach (var menu in _menus.Values)
        {
            menu.GetMenuCanvas().gameObject.SetActive(false);
        }
    }

    private void OnEnable()
    {
        UiController.CancelEvent += CloseCurrentMenu;
    }

    private void OnDisable()
    {
        UiController.CancelEvent -= CloseCurrentMenu;
    }

    public void OpenMenu(MenuType menuType)
    {
        if (_menus.ContainsKey(menuType))
        {
            CloseCurrentMenu();
            _menus[menuType].GetMenuCanvas().gameObject.SetActive(true);
            _currentMenu = menuType;
            EventSystem.current.SetSelectedGameObject(null);

            // Populate the inventory display if the inventory menu is opened
            if (menuType == MenuType.Inventory)
            {
                global::InventoryMenu.Instance.InitializeMenu();
            }

            // Disable player controls and enable UI controls
            PlayerController.Instance.DisablePlayerControls();
            UiController.Instance.EnableUiControls();
        }
    }

    public void CloseCurrentMenu()
    {
        if (_menus.ContainsKey(_currentMenu))
        {
            // if any submenus are open, close the current one
            if (_menus[_currentMenu].IsSubmenuOpen())
            {
                _menus[_currentMenu].CloseCurrentSubmenu();
            }
            else
            {
                _menus[_currentMenu].GetMenuCanvas().gameObject.SetActive(false);

                // Enable player controls and disable UI controls
                PlayerController.Instance.EnablePlayerControls();
                UiController.Instance.DisableUiControls();
            }
        }
    }

    public void ToggleMenu(MenuType menuType)
    {
        if (_menus.ContainsKey(menuType))
        {
            if (_menus[menuType].GetMenuCanvas().gameObject.activeSelf)
            {
                CloseCurrentMenu();
            }
            else
            {
                OpenMenu(menuType);
            }
        }
    }
}

public enum MenuType
{
    Inventory,
    MainMenu,
    Settings,
    PauseMenu
}
