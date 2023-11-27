using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : Singleton<PlayerController> //Singleton makes this script callable from everywhere using the code: PlayerController.Instance()
{
    public static PlayerInput playerInput;

    [Header("Move Properties")]
    [SerializeField] private float speed = 2f;
    [SerializeField] private float moveSmoothness = 10f;

    [Header("Look Properties")]
    [SerializeField] private float mouseSensitivity = 1f;
    [SerializeField] private float lookSmoothenss = 10;


    //Cached variables
    private CharacterController cc;
    public Transform playerCam { get; private set; }

    private float xRotation;
    private Vector3 fallVelocity;

    private Vector3 move;
    private Vector2 targetMove;
    private Vector2 smoothedMove;

    private Vector2 targetLook;
    private Vector2 smoothedLook;

    private void Awake()
    {
        playerInput = new PlayerInput();
        playerInput.Enable();

        playerCam = Camera.main.transform;
        cc = GetComponent<CharacterController>();
    }

    void Update()
    {
        UpdateMovement();
        UpdateLook();

        if(Keyboard.current.escapeKey.wasPressedThisFrame) { SceneLoader.LoadScene("Main Menu"); }
    }

    private void UpdateMovement()
    {
        targetMove = playerInput.Player.Move.ReadValue<Vector2>();
        smoothedMove = Vector2.Lerp(smoothedMove, targetMove, moveSmoothness * Time.deltaTime);
        move = (transform.forward * smoothedMove.y + transform.right * smoothedMove.x) * speed;

        if(fallVelocity.y < 0 && cc.isGrounded)
        {
            fallVelocity.y = -2f;
        }

        cc.Move((move + fallVelocity) * Time.deltaTime);
    }

    private void SetCrouch(bool isCrouched)
    {
        cc.height = isCrouched ? 1f : 2f;
    }

    private void UpdateLook()
    {
        targetLook = playerInput.Player.Look.ReadValue<Vector2>();
        smoothedLook = Vector2.Lerp(smoothedLook, targetLook, Time.deltaTime * lookSmoothenss);

        xRotation -= smoothedLook.y * mouseSensitivity;
        xRotation = Mathf.Clamp(xRotation, -85f, 85f);

        transform.Rotate(Vector3.up * smoothedLook.x * mouseSensitivity);
        playerCam.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
    }
}
