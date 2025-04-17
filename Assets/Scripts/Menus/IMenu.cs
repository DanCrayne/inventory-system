using UnityEngine;

public interface IMenu
{
    public Canvas GetMenuCanvas();

    public bool CloseCurrentSubmenu();

    public bool IsSubmenuOpen();

    public void InitializeMenu();
}
