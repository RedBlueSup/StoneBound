using UnityEngine;

public class DoorManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    [SerializeField] private Vector2 targetPosition;
    [SerializeField] private float doorSpeed = 1f;
    private Vector2 basePosition;
    private bool doorOpen = false;

    void Start()
    {
        basePosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (!doorOpen)
        {
            transform.position = Vector2.MoveTowards(transform.position, basePosition, doorSpeed * Time.deltaTime);
        }
        else if (doorOpen)
        {
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, doorSpeed * Time.deltaTime);
        }
    }

    public void ToggleDoor()
    {
        doorOpen = !doorOpen;
    }
}
