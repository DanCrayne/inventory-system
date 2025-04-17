using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class UiController : MonoBehaviour
{
    public static UiController Instance;

    private InputSystem_Actions _inputActions;

    // Delegate definitions
    public delegate void ItemDropHandler();
    public delegate void SubmitHandler();
    public delegate void CancelHandler();

    // Event definitions
    public static event ItemDropHandler ItemDropEvent;
    public static event SubmitHandler SubmitEvent;
    public static event CancelHandler CancelEvent;

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

        _inputActions = new InputSystem_Actions();
    }

    public void EnableUiControls()
    {
        _inputActions.Enable();
    }

    public void DisableUiControls()
    {
        _inputActions.Disable();
    }

    private void OnEnable()
    {
        _inputActions.UI.Enable();
        _inputActions.UI.DropItem.performed += OnDropItem;
        _inputActions.UI.Submit.performed += OnSubmit;
        _inputActions.UI.Cancel.performed += OnCancel;
    }

    private void OnDisable()
    {
        _inputActions.UI.Disable();
        _inputActions.UI.DropItem.performed += OnDropItem;
        _inputActions.UI.Submit.performed -= OnSubmit;
        _inputActions.UI.Cancel.performed -= OnCancel;
    }

    private void OnDropItem(InputAction.CallbackContext context)
    {
        ItemDropEvent?.Invoke();
    }

    private void OnSubmit(InputAction.CallbackContext context)
    {
        SubmitEvent?.Invoke();
    }

    private void OnCancel(InputAction.CallbackContext context)
    {
        CancelEvent?.Invoke();
    }
}
