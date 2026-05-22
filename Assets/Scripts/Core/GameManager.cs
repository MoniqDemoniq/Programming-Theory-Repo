using UnityEngine;

// GameManager controls the game state: when the game starts, runs, and ends.
// [ABSTRACTION] All other managers are told what to do from here.
public class GameManager : MonoBehaviour
{
    // -------------------------------------------------------
    // Singleton setup: one GameManager exists in the scene.
    // -------------------------------------------------------
    public static GameManager Instance { get; private set; }

    // -------------------------------------------------------
    // Game state enum: clearly defines every possible state the game can be in.   
    // -------------------------------------------------------
    public enum GameState
    {
        WaitingToStart,     // before the game begins
        Playing,            // timer is running, shapes are falling
        GameOver            // timer hit zero, session ended
    }

    // -------------------------------------------------------
    // [ENCAPSULATION] Current game state private set.    
    // -------------------------------------------------------
    public GameState CurrentState
    {
        get => _currentState;
        private set
        {
            _currentState = value;
            OnStateChanged(_currentState);      // [ABSTRACTION] reaction hidden below
        }
    }
    private GameState _currentState;

    // -------------------------------------------------------
    // [ENCAPSULATION] Timer: private field, read-only property.    
    // -------------------------------------------------------
    [SerializeField] private float _gameDuration = 30f;     // seconds per session
    private float _timeRemaining;

    public float TimeRemaining
    {
        get => _timeRemaining;
        private set => _timeRemaining = Mathf.Max(0f, value);   // never goes negative
    }

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
    // Start() game begins in WaitingToStart state.
    // -------------------------------------------------------
    private void Start()
    {
        StartGame();
    }

    // -------------------------------------------------------
    // Update() only runs the timer while Playing.
    // [ABSTRACTION] Timer logic inside HandleTimer().
    // -------------------------------------------------------
    private void Update()
    {
        if (CurrentState == GameState.Playing)
        {
            HandleTimer();                  // [ABSTRACTION] detail hidden below
        }
    }

    // -------------------------------------------------------
    // [ABSTRACTION] StartGame() coordinates all managers.    
    // -------------------------------------------------------
    public void StartGame()
    {
        TimeRemaining = _gameDuration;
        ScoreManager.Instance.ResetScore();
        SpawnManager.Instance.StartSpawning();
        InputManager.Instance.EnableInput();
        CurrentState = GameState.Playing;

        Debug.Log("GameManager: Game started!");
    }

    // -------------------------------------------------------
    // [ABSTRACTION] EndGame() cleanly shuts everything down.
    // Called automatically when timer hits zero.
    // -------------------------------------------------------
    public void EndGame()
    {
        SpawnManager.Instance.StopSpawning();
        InputManager.Instance.DisableInput();
        CurrentState = GameState.GameOver;

        Debug.Log($"GameManager: Game Over! Final Score: {ScoreManager.Instance.CurrentScore}");
    }

    // -------------------------------------------------------
    // [ABSTRACTION] Timer logic hidden here.    
    // -------------------------------------------------------
    private void HandleTimer()
    {
        TimeRemaining -= Time.deltaTime;
        UpdateTimerUI();                        // [ABSTRACTION] UI call hidden below

        if (TimeRemaining <= 0f)
        {
            EndGame();                          // time's up, end the session
        }
    }

    // -------------------------------------------------------
    // [ABSTRACTION] State change reactions hidden here.     
    // -------------------------------------------------------
    private void OnStateChanged(GameState newState)
    {
        Debug.Log($"GameManager: State changed to {newState}");

        switch (newState)
        {
            case GameState.Playing:
                UIManager.Instance.ShowGameUI();
                break;

            case GameState.GameOver:
                UIManager.Instance.ShowGameOverUI(ScoreManager.Instance.CurrentScore);
                break;
        }
    }

    // -------------------------------------------------------
    // [ABSTRACTION] UI timer update hidden here.    
    // -------------------------------------------------------
    private void UpdateTimerUI()
    {
        UIManager.Instance.UpdateTimerDisplay(TimeRemaining);
        
        // Debug.Log($"Time remaining: {Mathf.CeilToInt(TimeRemaining)}");
    }
}