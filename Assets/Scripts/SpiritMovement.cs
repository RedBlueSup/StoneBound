using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.SocialPlatforms.GameCenter;
using UnityEngine.UIElements;

public class SpiritMovement : MonoBehaviour
{
    public static SpiritMovement instance {  get; private set; }
    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;

        DontDestroyOnLoad(gameObject);
    }
        // Start is called once before the first execution of Update after the MonoBehaviour is created

    [SerializeField] private InputActionAsset movement;
    private InputAction moveAction;

    [SerializeField] private float speedCap = 10f;
    [SerializeField] private float acceleration = 0.1f;
    [SerializeField] private float speedDecay = 0.1f;

    private SpriteRenderer spiritSkin;
    private Rigidbody2D spiritRb;
    public Transform spiritTransform { get; private set; }
    public Vector2 spiritPosition;
    private Vector2 movementDirection;
    private Vector2 velocity;
    private Vector2 speedTarget;
    private Vector2 remainingSpeed;
    private bool active = false;

    void Start()
    {
        spiritRb = GetComponent<Rigidbody2D>();
        spiritSkin = GetComponent<SpriteRenderer>();
        spiritSkin.enabled = false;
    }

    // Update is called once per frame

    void FixedUpdate()
    {
        if (SceneManager.instance.gameRunning) Move();
    }

    void Move()
    {
        speedTarget = movementDirection * speedCap;
        velocity = spiritRb.linearVelocity;

        if (movementDirection != Vector2.zero)
        {
            remainingSpeed = speedTarget - velocity;
            velocity += remainingSpeed * acceleration;
        }
        else
        {
            velocity -= velocity * speedDecay;
        }

        if (transform.position.x < SceneManager.instance.center.x - SceneManager.instance.cameraWidth) { velocity.x = 10; }
        else if (transform.position.x > SceneManager.instance.center.x + SceneManager.instance.cameraWidth) { velocity.x = -10; }
        else if (transform.position.y < SceneManager.instance.center.y - SceneManager.instance.cameraHeight) { velocity.y = 10; }
        else if (transform.position.y > SceneManager.instance.center.y + SceneManager.instance.cameraHeight) { velocity.y = -10; }

        if (active) spiritRb.linearVelocity = velocity;
    }

    public void SplitAndMerge(bool activate)
    {
        if (activate)
        {
            transform.position = StatueMovement.instance.transform.position;
            spiritSkin.enabled = true;
            movementDirection = Vector2.zero;
            active = true;
        }
        else
        {
            transform.position = StatueMovement.instance.transform.position;
            spiritSkin.enabled = false;
            active = false;
        }
    }

    void OnEnable()
    {
        movement.Enable();
        moveAction = movement.FindAction("Move");
        moveAction.performed += MoveAction;
        moveAction.canceled += MoveAction;
    }

    void MoveAction(InputAction.CallbackContext phase)
    {
            movementDirection = phase.ReadValue<Vector2>().normalized;             
    }
}
