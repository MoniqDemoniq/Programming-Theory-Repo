using UnityEngine;

// [INHERITANCE] SphereCollectible inherits everything from Collectible.
// Unique: The Sphere is FASTER than the Cube and WOBBLES as it falls, has a SPIN effect when collected.
// Everything else is inherited.
public class SphereCollectible : Collectible
{
    // Sphere's own private data — controls the wobble while falling
    private float _wobbleAmount = 0.8f;
    private float _wobbleSpeed = 3f;

    // -------------------------------------------------------
    // Start() — initializes this shape with its unique values.
    // [ABSTRACTION] InitializeCollectible() hides all validation logic.
    // [INHERITANCE] InitializeCollectible() is defined in Collectible.cs. 
    // -------------------------------------------------------
    private void Start()
    {
        // [INHERITANCE] Calling the parent's protected setup method
        InitializeCollectible("Sphere", 20, 5f);
    }

    // -------------------------------------------------------
    // [POLYMORPHISM] override, Sphere's unique collect behavior.
    // Same method name as Cube (OnCollect) but completely different behavior. 
    // -------------------------------------------------------
    public override void OnCollect()
    {
        AddPoints(PointValue);              // [INHERITANCE] method from parent
        SpinEffect();                       // [ABSTRACTION] spin detail hidden below
        Debug.Log($"{CollectibleName} collected! +{PointValue} points");
    }

    // -------------------------------------------------------
    // [POLYMORPHISM] override, Sphere falls DIFFERENTLY than the base class.
    // Cube uses the default Fall() from Collectible.cs — straight down.
    // Sphere overrides Fall() to add a left/right wobble as it drops. 
    // -------------------------------------------------------
    public override void Fall()
    {
        // Wobble left and right using a sine wave
        float wobble = Mathf.Sin(Time.time * _wobbleSpeed) * _wobbleAmount;

        // Move down AND sideways at the same time
        transform.position += new Vector3(wobble, -FallSpeed, 0) * Time.deltaTime;
    }

    // -------------------------------------------------------
    // [ABSTRACTION] Spin complexity here. 
    // -------------------------------------------------------
    private void SpinEffect()
    {
        // Instantly rotate 720 degrees around Y axis for a satisfying spin
        transform.Rotate(0f, 720f, 0f);
    }

    
}