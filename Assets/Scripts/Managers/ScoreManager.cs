using UnityEngine;


public class ScoreManager : MonoBehaviour
{
    // -------------------------------------------------------
    // private set means only THIS class can assign it.
    // -------------------------------------------------------
    public static ScoreManager Instance { get; private set; }

    // -------------------------------------------------------
    // [ENCAPSULATION] Private field: nothing outside can touch _currentScore. 
    // -------------------------------------------------------
    private int _currentScore = 0;
    private int _highScore = 0;

    // -------------------------------------------------------
    // [ENCAPSULATION] Read-only properties. Outside scripts can READ the score but cannot SET it directly.
    // -------------------------------------------------------
    public int CurrentScore
    {
        get => _currentScore;
        private set => _currentScore = Mathf.Max(0, value); // score never goes negative!
    }

    public int HighScore
    {
        get => _highScore;
        private set => _highScore = Mathf.Max(0, value);
    }

    // -------------------------------------------------------
    // Awake() runs before Start()
    // If an Instance already exists and it isn't us, destroy this duplicate.
    // If no Instance exists yet, we become the Instance.
    // -------------------------------------------------------
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    // -------------------------------------------------------
    // [ENCAPSULATION] + [ABSTRACTION]
    // -------------------------------------------------------
    public void AddScore(int amount)
    {
        if (amount <= 0) return;            // guard: ignore zero or negative amounts

        CurrentScore += amount;             // uses property validates automatically
        UpdateHighScore();                  // [ABSTRACTION] high score logic below
        UpdateScoreUI();                    // [ABSTRACTION] UI update below

        Debug.Log($"Score: {CurrentScore} | High Score: {HighScore}");
    }

    // -------------------------------------------------------
    // [ABSTRACTION] Called by GameManager when session ends or restarts. 
    // -------------------------------------------------------
    public void ResetScore()
    {
        CurrentScore = 0;
        UpdateScoreUI();
        Debug.Log("Score reset.");
    }

    // -------------------------------------------------------
    // [ABSTRACTION] High score logic here. 
    // -------------------------------------------------------
    private void UpdateHighScore()
    {
        if (CurrentScore > HighScore)
        {
            HighScore = CurrentScore;
        }
    }

    // -------------------------------------------------------
    // [ABSTRACTION] UI update here.     
    // -------------------------------------------------------
    private void UpdateScoreUI()
    {
        // UIManager.Instance.UpdateScoreDisplay(CurrentScore);
        // ↑ We'll uncomment this once UIManager exists
        Debug.Log($"UI updated: {FormatScore(CurrentScore)}");
    }

    // -------------------------------------------------------
    // [ABSTRACTION] Score formatting here.
    // Displays score as 5 digits with leading zeros: 00150 
    // -------------------------------------------------------
    public string FormatScore(int score)
    {
        return score.ToString("D5");    // D5 = at least 5 digits, padded with zeros
    }
}