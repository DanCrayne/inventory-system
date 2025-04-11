using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float MovementSpeed = 5f;
    public Canvas InventoryCanvas;

    private Rigidbody2D _rigidBody;
    private Vector2 _input;
    private InputSystem_Actions _inputActions;
    private Animator _animator;

    public delegate void ToggleInventoryAction();
    public static event ToggleInventoryAction OnToggleInventory;

    private void Awake()
    {
        _rigidBody = gameObject.GetComponent<Rigidbody2D>();
        _inputActions = new InputSystem_Actions();
        _animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        _inputActions.Player.Enable();
        _inputActions.Player.Move.performed += OnMove;
        _inputActions.Player.Move.canceled += OnMove;
        _inputActions.Player.Inventory.performed += OnInventory;
    }

    private void OnDisable()
    {
        _inputActions.Player.Move.performed -= OnMove;
        _inputActions.Player.Move.canceled -= OnMove;
        _inputActions.Player.Disable();
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

    void FixedUpdate()
    {
        Vector2 velocity = _input * MovementSpeed;
        _rigidBody.linearVelocity = velocity;
    }

    private void OnInventory(InputAction.CallbackContext context)
    {
        OnToggleInventory?.Invoke();
    }
}
