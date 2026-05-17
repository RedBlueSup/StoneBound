using UnityEngine;
using UnityEngine.Animations;

public class SceneManager : MonoBehaviour
{
    
    public static SceneManager instance { get; private set; }

    public float cameraHeight;
    public float cameraWidth;
    public Vector2 center;
    private bool transition = false;
    public bool gameRunning { get; private set; } = true;


    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;

        DontDestroyOnLoad(gameObject);

        float a = (float)Screen.width / Screen.height / (16f / 9f);
        Camera.main.orthographicSize *= a;

        cameraHeight = Camera.main.orthographicSize;
        cameraWidth = cameraHeight * Camera.main.aspect;
        center = Camera.main.transform.position;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
         
    }

    // Update is called once per frame
    void Update()
    {
    
    }

    public void MoveScene(string axis, bool ahead)
    {
        if (axis == "x" && !transition)
        {
            if (ahead) transform.position = new Vector3(transform.position.x - cameraWidth * 2, transform.position.y, transform.position.z);
            else transform.position = new Vector3(transform.position.x + cameraWidth * 2, transform.position.y, transform.position.z);
        }
        else if (axis == "y" && !transition)
        {
            if (ahead) transform.position = new Vector3(transform.position.x, transform.position.y - cameraHeight * 2, transform.position.z);
            else transform.position = new Vector3(transform.position.x, transform.position.y + cameraHeight * 2, transform.position.z);
        }

        transition = true;

        cameraHeight = Camera.main.orthographicSize;
        cameraWidth = cameraHeight * Camera.main.aspect;
        center = Camera.main.transform.position;

        transition = false;
    }

    public void stopGame()
    {
        gameRunning = false;
    }
}
