using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance;

    public float MovementSpeed = 5f;

    private Rigidbody2D _rigidBody;
    private Vector2 _input;
    private InputSystem_Actions _inputActions;
    private Animator _animator;

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

        _rigidBody = gameObject.GetComponent<Rigidbody2D>();
        _inputActions = new InputSystem_Actions();
        _animator = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        Vector2 velocity = _input * MovementSpeed;
        _rigidBody.linearVelocity = velocity;
    }

    public Vector2 GetColliderSize()
    {
        return GetComponent<BoxCollider2D>().size;
    }

    private void OnEnable()
    {
        _inputActions.Player.Enable();
        _inputActions.Player.Move.performed += OnMove;
        _inputActions.Player.Move.canceled += OnMove;
        _inputActions.Player.Inventory.performed += OnToggleInventory;
    }

    private void OnDisable()
    {
        _inputActions.Player.Disable();
        _inputActions.Player.Move.performed -= OnMove;
        _inputActions.Player.Move.canceled -= OnMove;
        _inputActions.Player.Inventory.performed -= OnToggleInventory;
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        _input = context.ReadValue<Vector2>().normalized;
        if (_input != Vector2.zero)
        {
            _animator.SetBool("isMoving", true);
            _animator.SetFloat("moveX", _input.x);
            _animator.SetFloat("moveY", _input.y);
        }
        else
        {
            _animator.SetBool("isMoving", false);
        }
    }

    public void DisablePlayerControls()
    {
        _inputActions.Player.Disable();
    }

    public void EnablePlayerControls()
    {
        _inputActions.Player.Enable();
    }

    private void OnToggleInventory(InputAction.CallbackContext context)
    {
        MenuManager.Instance.ToggleMenu(MenuType.Inventory);
    }

    private void OnTogglePauseMenu(InputAction.CallbackContext context)
    {
        MenuManager.Instance.ToggleMenu(MenuType.PauseMenu);
    }
}
