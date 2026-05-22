using UnityEngine;
using UnityEngine.UI;
using TMPro;

// UIManager handles ALL visual UI updates in the game.
// [ABSTRACTION] Other scripts just call UpdateScoreDisplay() or ShowGameOverUI() 
public class UIManager : MonoBehaviour
{
    // -------------------------------------------------------
    // Singleton setup: one UIManager exists in the scene.
    // -------------------------------------------------------
    public static UIManager Instance { get; private set; }

    // -------------------------------------------------------
    // [ENCAPSULATION] All UI references are private.
    // SerializeField lets us assign them in the Inspector.   
    // -------------------------------------------------------

    [Header("--- GAME UI ---")]
    [SerializeField] private TextMeshProUGUI _scoreText;
    [SerializeField] private TextMeshProUGUI _timerText;

    [Header("--- GAME OVER UI ---")]
    [SerializeField] private GameObject _gameOverPanel;
    [SerializeField] private TextMeshProUGUI _finalScoreText;
    [SerializeField] private TextMeshProUGUI _highScoreText;
    [SerializeField] private Button _restartButton;

    [Header("--- OOP LEGEND UI ---")]
    [SerializeField] private GameObject _oopLegendPanel;

    // -------------------------------------------------------
    // Awake() singleton setup
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
    // Start() initial UI state setup.
    // [ABSTRACTION] InitializeUI() hides all setup detail.
    // -------------------------------------------------------
    private void Start()
    {
        InitializeUI();
    }

    // -------------------------------------------------------
    // [ABSTRACTION] Sets up the UI into its correct starting state.    
    // -------------------------------------------------------
    private void InitializeUI()
    {
        // Hide game over panel at start
        if (_gameOverPanel != null)
            _gameOverPanel.SetActive(false);

        // Show OOP legend panel if assigned
        if (_oopLegendPanel != null)
            _oopLegendPanel.SetActive(true);

        // Wire up restart button click
        if (_restartButton != null)
            _restartButton.onClick.AddListener(OnRestartButtonClicked);

        // Set default display values
        UpdateScoreDisplay(0);
        UpdateTimerDisplay(30f);

        Debug.Log("UIManager: UI initialized.");
    }

    // -------------------------------------------------------
    // [ABSTRACTION] Public display methods 
    // ScoreManager calls UpdateScoreDisplay().
    // GameManager calls UpdateTimerDisplay().    
    // -------------------------------------------------------
    public void UpdateScoreDisplay(int score)
    {
        if (_scoreText != null)
            _scoreText.text = $"Score: {FormatScore(score)}";
    }

    public void UpdateTimerDisplay(float timeRemaining)
    {
        if (_timerText != null)
        {
            // Ceiling so timer shows 30 not 29 at the very start
            int seconds = Mathf.CeilToInt(timeRemaining);
            _timerText.text = $"Time: {seconds}";
        }
    }

    // -------------------------------------------------------
    // [ABSTRACTION] 
    // All panel show/hide logic is hidden inside here.
    // -------------------------------------------------------
    public void ShowGameUI()
    {
        if (_gameOverPanel != null)
            _gameOverPanel.SetActive(false);

        Debug.Log("UIManager: Game UI shown.");
    }

    public void ShowGameOverUI(int finalScore)
    {
        if (_gameOverPanel != null)
            _gameOverPanel.SetActive(true);

        if (_finalScoreText != null)
            _finalScoreText.text = $"Final Score: {FormatScore(finalScore)}";

        if (_highScoreText != null)
            _highScoreText.text = $"High Score: {FormatScore(ScoreManager.Instance.HighScore)}";

        Debug.Log($"UIManager: Game Over UI shown. Final Score: {finalScore}");
    }

    // -------------------------------------------------------
    // [ABSTRACTION] Restart button click hidden here.    
    // -------------------------------------------------------
    private void OnRestartButtonClicked()
    {
        if (_gameOverPanel != null)
            _gameOverPanel.SetActive(false);

        GameManager.Instance.StartGame();
        Debug.Log("UIManager: Restart button clicked.");
    }

    // -------------------------------------------------------
    // [ABSTRACTION] Score formatting hidden here.      
    // -------------------------------------------------------
    private string FormatScore(int score)
    {
        return score.ToString("D5");    // formats as 00150 etc.
    }
}