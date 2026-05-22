using UnityEngine;
using UnityEngine.InputSystem;

// InputManager handles ALL mouse input for the entire game.
// detects mouse clicks and check if a Collectible was hit.
public class InputManager : MonoBehaviour
{
    // -------------------------------------------------------
    // Singleton setup: one InputManager exists in the scene
    // -------------------------------------------------------
    public static InputManager Instance { get; private set; }

    // -------------------------------------------------------
    // [ENCAPSULATION] Private reference to the main camera.    
    // -------------------------------------------------------
    private Camera _mainCamera;
    private bool _isAcceptingInput = false;

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
    // Start() grab the main camera reference once at startup.    
    // -------------------------------------------------------
    private void Start()
    {
        _mainCamera = Camera.main;

        if (_mainCamera == null)
        {
            Debug.LogError("InputManager: No Main Camera found in scene!");
        }
    }

    // -------------------------------------------------------
    // Update() runs every frame, listens for left mouse click.
    // [ABSTRACTION] Click handling detail inside HandleMouseClick().
    // -------------------------------------------------------
    private void Update()
    {
        if (!_isAcceptingInput) return;         // ignore clicks if game isn't running

        if (Mouse.current.leftButton.wasPressedThisFrame)        // 0 = left mouse button
        {
            HandleMouseClick();                 // [ABSTRACTION] detail hidden below
        }
    }

    // -------------------------------------------------------
    // [ABSTRACTION] Public controls GameManager calls these.   
    // -------------------------------------------------------
    public void EnableInput()
    {
        _isAcceptingInput = true;
        Debug.Log("InputManager: Input enabled.");
    }

    public void DisableInput()
    {
        _isAcceptingInput = false;
        Debug.Log("InputManager: Input disabled.");
    }

    // -------------------------------------------------------
    // [ABSTRACTION] All raycast complexity here.    
    // Step 1: Convert mouse position to a ray in 3D world space.
    // Step 2: Check if the ray hits anything.
    // Step 3: Check if what was hit has a Collectible component.
    // Step 4: If yes, call HandleClick() on it.
    // -------------------------------------------------------
    private void HandleMouseClick()
    {
        // Convert the 2D mouse screen position into a 3D ray
        Ray ray = _mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());

        // Cast the ray into the scene 
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            // Try to get a Collectible component from the object that was hit
            Collectible collectible = hit.collider.GetComponent<Collectible>();

            if (collectible != null)
            {
                collectible.HandleClick();
            }
        }
    }
}