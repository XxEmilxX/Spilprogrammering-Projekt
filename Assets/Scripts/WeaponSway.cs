using UnityEngine;

public class WeaponSway : MonoBehaviour
{
    PlayerInput playerInput;

    [Header("Direction")]
    [SerializeField] private bool oppositeDir = false;

    [Header("Position")]
    public float amount = 0.02f;
    public float maxAmount = 0.06f;
    public float smoothAmount = 6f;

    [Header("Rotation")]
    public float rotationAmount = 4f;
    public float maxRotationAmount = 5f;
    public float smoothRotation = 12f;

    [Space]
    public bool rotationX = true;
    public bool rotationY = true;
    public bool rotationZ = true;

    /* Internal Privates */
    private Vector3 initialPosition;
    private Quaternion initialRotation;

    private float InputX;
    private float InputY;

    private float MoveX;
    private float MoveY;

    private Transform cam;

    // Start is called before the first frame update
    void Start()
    {
        playerInput = PlayerController.playerInput;

        initialPosition = transform.localPosition;
        initialRotation = transform.localRotation;

        cam = PlayerController.Instance().playerCam;
    }

    // Update is called once per frame
    void Update()
    {
        CalculateSway();

        MoveDelay();
        MoveSway();
        TiltSway();
    }

    public void SetInitialRotation(Quaternion newRotation)
    {
        initialRotation = newRotation;
    }
    public void ResetInitialRotation()
    {
        initialRotation = Quaternion.Euler(0,0,0);
    }

    private void MoveDelay()
    {
        float moveX = Mathf.Clamp(MoveX * amount, -maxAmount, maxAmount);
        float moveY = Mathf.Clamp(MoveY * amount, -maxAmount, maxAmount);

        Vector3 finalPosition = new Vector3(moveX, moveY, 0);

        transform.localPosition = Vector3.Lerp(transform.localPosition, finalPosition + initialPosition, Time.deltaTime * smoothAmount);
    }

    private void CalculateSway()
    {
        InputX = oppositeDir ? playerInput.Player.Look.ReadValue<Vector2>().x : -playerInput.Player.Look.ReadValue<Vector2>().x;
        InputY = oppositeDir ? playerInput.Player.Look.ReadValue<Vector2>().y : -playerInput.Player.Look.ReadValue<Vector2>().y;

        MoveX = oppositeDir ? playerInput.Player.Move.ReadValue<Vector2>().x : -playerInput.Player.Move.ReadValue<Vector2>().x;
        MoveY = oppositeDir ? playerInput.Player.Move.ReadValue<Vector2>().y : -playerInput.Player.Move.ReadValue<Vector2>().y;
    }

    private void MoveSway()
    {
        float moveX = Mathf.Clamp(InputX * amount, -maxAmount, maxAmount);
        float moveY = Mathf.Clamp(InputY * amount, -maxAmount, maxAmount);

        Vector3 finalPosition = new Vector3(moveX, moveY, 0);

        transform.localPosition = Vector3.Lerp(transform.localPosition, finalPosition + initialPosition, Time.deltaTime * smoothAmount);
    }

    private void TiltSway()
    {
        float tiltY = Mathf.Clamp(InputX * rotationAmount, -maxRotationAmount, maxRotationAmount);
        float tiltX = Mathf.Clamp(InputY * rotationAmount, -maxRotationAmount, maxRotationAmount);

        Quaternion finalRotation = Quaternion.Euler(new Vector3(
            rotationX ? -tiltX : 0f,
            rotationY ? tiltY : 0f,
            rotationZ ? tiltY : 0
            ));

        transform.localRotation = Quaternion.Slerp(transform.localRotation, finalRotation * initialRotation, Time.deltaTime * smoothRotation);
    }
}
