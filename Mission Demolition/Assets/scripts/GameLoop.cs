using System.Collections;
using UnityEngine;

public class GameLoop : MonoBehaviour
{
    private Level currentLevel;
    public GameObject Music;
    private Settings settings = new Settings();
    public GameObject level1prefab;
    public GameObject level2prefab;
    public GameObject level3prefab;
    public GameObject GameOverUI;
    public float transitionDuration = 2.0f; // Duration of the camera transition
    public float pauseDuration = 0.5f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Set music volume
        Music.GetComponent<AudioSource>().volume = 0.2f * ((float)settings.volume / 100f);
    }
    void Awake()
    {
        StartCoroutine(InitializeLevel());
        print("Level maxshots: " + currentLevel.maxShots);
    }

    public IEnumerator InitializeLevel()
    {
        // Ensure Music has an AudioSource component
        if (Music != null && Music.GetComponent<AudioSource>() != null)
        {
            AudioSource audioSource = Music.GetComponent<AudioSource>();
            audioSource.volume = 0.2f * ((float)settings.volume / 100f);
        }
        else
        {
            Debug.LogWarning("Music GameObject or AudioSource component is missing.");
        }

        // Get current level and load it
        currentLevel = GameController.currentLevel;
        if (currentLevel != null)
        {
            currentLevel.LoadLevel();
        }
        else
        {
            Debug.LogError("Current level is null.");
            yield break;
        }

        // Wait a frame to ensure the level has loaded
        yield return null;

        // Get main camera width
        float width = Camera.main.orthographicSize * 2.0f * Camera.main.aspect;

        // Find level max width
        GameObject levelObject = GameObject.Find(currentLevel.name + "(Clone)");
        if (levelObject == null)
        {
            Debug.LogError("Level object not found. Ensure the level is loaded correctly.");
            yield break;
        }

        float levelWidth = GameObject.Find("Border (1)").transform.position.x;

        // Move camera smoothly to the end of the level
        Vector3 endPosition = new Vector3(levelWidth - width / 2, Camera.main.transform.position.y, Camera.main.transform.position.z);
        yield return StartCoroutine(SmoothCameraTransition(endPosition, transitionDuration));

        yield return new WaitForSeconds(pauseDuration);

        // Move camera smoothly back to the start
        Vector3 startPosition = new Vector3(0, Camera.main.transform.position.y, Camera.main.transform.position.z);
        yield return StartCoroutine(SmoothCameraTransition(startPosition, transitionDuration));
    }

    IEnumerator SmoothCameraTransition(Vector3 targetPosition, float duration)
    {
        float elapsedTime = 0;
        Vector3 startPosition = Camera.main.transform.position;

        while (elapsedTime < duration)
        {
            Camera.main.transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null; // Wait until next frame
        }

        Camera.main.transform.position = targetPosition; // Ensure it reaches the exact position
    }

    // Update is called once per frame
    void Update()
    {

    }
}
