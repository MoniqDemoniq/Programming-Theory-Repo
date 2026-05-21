using UnityEngine;

// [INHERITANCE] This is the abstract base class.
// CubeCollectible, SphereCollectible, CylinderCollectible all inherit from this.
public abstract class Collectible : MonoBehaviour
{
    // -------------------------------------------------------
    // [ENCAPSULATION] Private fields, cannot be touched by any outside script directly.
    // SerializeField lets us see and edit them in the Unity Inspector.
    // -------------------------------------------------------
    [SerializeField] private int _pointValue;
    [SerializeField] private float _fallSpeed;
    [SerializeField] private string _collectibleName;
    private bool _hasBeenCollected = false;

    // -------------------------------------------------------
    // [ENCAPSULATION] Properties, the controlled gateway to private fields.
    // Outside scripts can READ these but cannot SET them freely. 
    // -------------------------------------------------------
    public int PointValue
    {
        get => _pointValue;
        protected set => _pointValue = Mathf.Max(0, value); // never goes negative
    }

    public float FallSpeed
    {
        get => _fallSpeed;
        protected set => _fallSpeed = Mathf.Clamp(value, 0.5f, 15f); // speed cap
    }

    public string CollectibleName
    {
        get => _collectibleName;
        protected set => _collectibleName =
            string.IsNullOrEmpty(value) ? "Unknown Shape" : value;
    }

    public bool HasBeenCollected
    {
        get => _hasBeenCollected;
        private set => _hasBeenCollected = value;
    }

    // -------------------------------------------------------
    // [ABSTRACTION] InitializeCollectible()
    // Child classes call this one method to set themselves up. 
    // -------------------------------------------------------
    protected virtual void InitializeCollectible(string name, int points, float speed)
    {
        CollectibleName = name;
        PointValue = points;
        FallSpeed = speed;
        HasBeenCollected = false;
    }

    // -------------------------------------------------------
    // [ABSTRACTION] + [ENCAPSULATION]
    // The only way an outside script can trigger a collection. 
    // -------------------------------------------------------
    public void HandleClick()
    {
        if (HasBeenCollected) return;   // guard: prevents double-collecting
        HasBeenCollected = true;
        OnCollect();                    // [POLYMORPHISM] the child's version runs here
        Destroy(gameObject, 0.3f);     // small delay so collect effect can play
    }

    // -------------------------------------------------------
    // [POLYMORPHISM] Method overloading: same name, diff. parameters 
    // -------------------------------------------------------
    protected void AddPoints(int amount)
    {
        ScoreManager.Instance.AddScore(amount); 
        Debug.Log($"{CollectibleName} added {amount} points");

    }

    protected void AddPoints(int amount, float multiplier)
    {
        int finalAmount = Mathf.RoundToInt(amount * multiplier);
        ScoreManager.Instance.AddScore(finalAmount);
        Debug.Log($"{CollectibleName} added {finalAmount} points (multiplied)");           
    }

    // -------------------------------------------------------
    // [POLYMORPHISM] virtual, default behavior.
    // Child classes CAN override this but don't have to.
    // -------------------------------------------------------
    public virtual void Fall()
    {
        transform.position += Vector3.down * FallSpeed * Time.deltaTime;
    }

    public virtual void OnMissed()
    {
        Destroy(gameObject); // default: disappear quietly
    }

    // -------------------------------------------------------
    // [POLYMORPHISM] abstract — NO default behavior exists.
    // Every child class WILL write their own version of this.
    // -------------------------------------------------------
    public abstract void OnCollect();

    // -------------------------------------------------------
    // [ABSTRACTION] Unity lifecycle methods
    // Update calls Fall() every frame, child's version runs automatically.
    // OnTriggerEnter handles the floor hit.
    // -------------------------------------------------------
    protected virtual void Update()
    {
        if (!HasBeenCollected)
        {
            Fall();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Floor") && !HasBeenCollected)
        {
            OnMissed();
        }
    }
}