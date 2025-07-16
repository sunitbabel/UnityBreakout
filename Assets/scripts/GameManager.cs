using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public  static GameManager Instance => _instance;

    private Ball   ball;
    private Paddle paddle;
    public  Brick[] bricks;

    public Text livesText;
    public Text scoreText;

    public int  level;
    public int  score;
    public int  lives  = 3;
    public bool gameOver;

    private void Awake()
    {
        if (_instance && _instance != this) { DestroyImmediate(gameObject); return; }
        _instance = this;

        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;

        FindUIReferences();
    }

    private void Start()
    {
        CacheSceneObjects();
        RefreshUI();
    }

    private void OnSceneLoaded(Scene _, LoadSceneMode __)
    {
        CacheSceneObjects();
        FindUIReferences();
        RefreshUI();
    }

    private void FindUIReferences()
    {
        if (!livesText)
            livesText = GameObject.Find("LivesText")?.GetComponent<Text>();

        if (!scoreText)
            scoreText = GameObject.Find("ScoreText")?.GetComponent<Text>();
    }

    private void CacheSceneObjects()
    {
        ball   = FindObjectOfType<Ball>();
        paddle = FindObjectOfType<Paddle>();
        bricks = FindObjectsOfType<Brick>();
    }

    private void RefreshUI()
    {
        if (livesText) livesText.text = $"Lives : {lives}";
        if (scoreText) scoreText.text = $"Score : {score}";
    }

    public void UpdateLives(int delta)
    {
        lives += delta;
        if (lives <= 0) HandleGameOver();
        RefreshUI();
    }

    public void UpdateScore(int pts)
    {
        score += pts;
        RefreshUI();
    }

    public void OnBrickHit(Brick _)
    {
        if (AllBricksCleared())
            Invoke(nameof(LoadNextLevel), 1f);
    }

    private bool AllBricksCleared()
    {
        foreach (var b in bricks)
            if (b.gameObject.activeInHierarchy && !b.unbreakable) return false;

        return true;
    }

    private void LoadNextLevel()
    {
        int next = SceneManager.GetActiveScene().buildIndex + 1;
        if (next >= SceneManager.sceneCountInBuildSettings) next = 0;
        SceneManager.LoadScene(next);
    }

    private void HandleGameOver()
    {
        if (gameOver) return;
        gameOver = true;

        Debug.Log("GAME OVER – restarting...");
        SceneManager.LoadScene("Level2");
        Invoke(nameof(RestartGame), 2f);
    }

    private void RestartGame()
    {
        score    = 0;
        lives    = 3;
        gameOver = false;

        // var current = SceneManager.GetActiveScene().buildIndex;
        // SceneManager.LoadScene(current);
        SceneManager.LoadScene("Level1");
    }

}