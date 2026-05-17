using System;
using UnityEditor.Rendering.Universal;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class StatueMovement : MonoBehaviour
{
    public static StatueMovement instance { get; private set; }
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
    private InputAction moveAction, splitAndMergeAction;

    [SerializeField] private float speed = 3.5f;
    [SerializeField] private float flingFactor = 2.5f;
    [SerializeField] private float flingDecay = 0.0025f;

    private SpriteRenderer statueSkin;
    private Rigidbody2D statueRb;

    private Vector2 movementDirection;
    private Vector2 flingVector;
    bool split = false;


    void Start()
    {
        statueRb = GetComponent<Rigidbody2D>();
        statueSkin = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (SceneManager.instance.gameRunning)
        {
            Move();
            DecayFling();
        }       
    }

    void Move()
    {
        statueRb.linearVelocityX = speed * movementDirection.x + flingVector.x;

        if (split)
        {
            if (transform.position.x < SceneManager.instance.center.x - SceneManager.instance.cameraWidth) { statueRb.linearVelocityX = 10; }
            else if (transform.position.x > SceneManager.instance.center.x + SceneManager.instance.cameraWidth) { statueRb.linearVelocityX = -10; }
            else if (transform.position.y < SceneManager.instance.center.y - SceneManager.instance.cameraHeight) { statueRb.linearVelocityY = 10; }
            else if (transform.position.y > SceneManager.instance.center.y + SceneManager.instance.cameraHeight) { statueRb.linearVelocityY = -10; }
        }
        else
        {
            if (transform.position.x < SceneManager.instance.center.x - SceneManager.instance.cameraWidth) { SceneManager.instance.MoveScene("x", true); }
            else if (transform.position.x > SceneManager.instance.center.x + SceneManager.instance.cameraWidth) { SceneManager.instance.MoveScene("x", false); }
            else if (transform.position.y < SceneManager.instance.center.y - SceneManager.instance.cameraHeight) { SceneManager.instance.MoveScene("y", true); }
            else if (transform.position.y > SceneManager.instance.center.y + SceneManager.instance.cameraHeight) { SceneManager.instance.MoveScene("y", false); }
        }
    }
    void DecayFling()
    {
        flingVector.x -= flingDecay * flingVector.x;
    }

    void OnEnable()
    {
        movement.Enable();
        moveAction = movement.FindAction("Move");
        moveAction.performed += MoveAction;
        moveAction.canceled += MoveAction;
        splitAndMergeAction = movement.FindAction("SplitAndMerge");
        splitAndMergeAction.started += SplitAndMergeAction;
    }

    void MoveAction(InputAction.CallbackContext phase)
    {
        if (!split)
            movementDirection = phase.ReadValue<Vector2>();
    }

    void SplitAndMergeAction(InputAction.CallbackContext phase)
    {
        if (SceneManager.instance.gameRunning) 
        {
            if (split)
            {
                split = false;

                flingVector = (Vector2)transform.position - (Vector2)SpiritMovement.instance.transform.position;
                flingVector *= flingFactor;
                statueRb.linearVelocityY = flingVector.y;

                SpiritMovement.instance.SplitAndMerge(false);
            }
            else
            {
                split = true;
                SpiritMovement.instance.SplitAndMerge(true);
                movementDirection = Vector2.zero;
            }
        }      
    }

    private void OnTriggerEnter2D(Collider2D Player)
    {
        if (Player.CompareTag("Finish") && (!split))
        {
            Debug.Log("win");
            statueRb.linearVelocity = Vector2.zero;
            SceneManager.instance.stopGame();
        }
        
    }
}
