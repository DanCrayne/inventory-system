using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Character playerCharacter;

    public static GameManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // Initialize player character or other game elements here
        if (playerCharacter == null)
        {
            Debug.LogError("Player character is not assigned in the GameManager.");
        }
    }

    public void PauseGame()
    {
        Time.timeScale = 0f;
    }

    public void UnpauseGame()
    {
        Time.timeScale = 1f;
    }
}
