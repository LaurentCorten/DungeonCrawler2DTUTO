using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMouvement : MonoBehaviour
{
    public float moveSpeed = 0.1f;
    public Rigidbody2D rb2D;
    public Animator animator;
    public SpriteRenderer spriteRenderer;

    private Transform _transform;

    [SerializeField]
    InputPlayerMouvement _input;
    [SerializeField]
    private Vector2 _movement;

    private void Awake()
    {
        Debug.Log("Awake starts!");

        _input = new InputPlayerMouvement();
        Debug.Log("_input initialized!");

        InputListeningOn();
        Debug.Log("Listening to Inputs!");

        _input.Player.Enable();
        Debug.Log("_input Enabled!");

        Debug.Log("Awake finished!");
    }

    private void Start()
    {
        Debug.Log("Start starts!");

        _transform = rb2D.GetComponent<Transform>();
        Debug.Log("_transform got rigidboby<Transform>!");

        Debug.Log("Start Finished!");
    }

    void Update()
    {
        animator.SetFloat("Speed", _movement.sqrMagnitude);

        if(_movement.x != 0)
        {
            spriteRenderer.flipX = _movement.x < 0;
        }
    }

    private void FixedUpdate()
    {
        ApplyMove();
    }


    private void OnDestroy()
    {
        InputListeningOff();
        Debug.Log("Listening to Inputs No More!");
    }

    private void ApplyMove()
    {
        _transform.position += new Vector3(_movement.x,_movement.y,0) * moveSpeed;
    }

    private void InputListeningOn()
    {
        Debug.Log("Start InputSuscription");
        _input.Player.Move.performed += onMovePerformed;
        _input.Player.Move.canceled += onMoveCanceled;
        Debug.Log("InputSuscription Finished");
    }

    private void InputListeningOff()
    {
        Debug.Log("Start InputUnsuscription");
        _input.Player.Move.performed -= onMovePerformed;
        _input.Player.Move.canceled -= onMoveCanceled;
        Debug.Log("InputUnsuscription Finished");
    }

    private void onMovePerformed(InputAction.CallbackContext context)
    {
        Debug.Log("Start onMovePerformed");
        _movement = context.ReadValue<Vector2>();
        Debug.Log("Stop onMovePerformed");
    }

    private void onMoveCanceled(InputAction.CallbackContext context)
    {
        Debug.Log("Start onMoveCanceled");
        _movement = Vector2.zero;
        Debug.Log("Stop onMoveCanceled");
    }

}
