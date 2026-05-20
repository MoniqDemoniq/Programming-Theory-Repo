using UnityEngine;
using System.Collections;

// [INHERITANCE] CylinderCollectible inherits everything from Collectible.
// The Cylinder is the RAREST and most VALUABLE shape.
// It has an extra collect effect: floats UPWARD before disappearing.
// It also overrides OnMissed(). 
public class CylinderCollectible : Collectible
{
    // Cylinder's own private data
    private float _floatSpeed = 4f;         // how fast it floats up on collect
    private float _floatDuration = 0.4f;    // how long the float effect lasts
    private Renderer _renderer;

    // -------------------------------------------------------
    // Start() initializes with unique high-value stats.
    // [ABSTRACTION] InitializeCollectible() hides validation.
    // [INHERITANCE] Method comes from Collectible.cs.    
    // -------------------------------------------------------
    private void Start()
    {
        _renderer = GetComponent<Renderer>();

        // [INHERITANCE] Calling the parent's protected setup method
        InitializeCollectible("Cylinder", 50, 1.5f);
    }

    // -------------------------------------------------------
    // [POLYMORPHISM] override: Cylinder's unique collect behavior.
    // floats upward before vanishing
    // HandleClick() in the parent calls OnCollect(): THIS version runs for Cylinders.
    // -------------------------------------------------------
    public override void OnCollect()
    {
        AddPoints(PointValue);                  // [INHERITANCE] method from parent

        // [POLYMORPHISM] overloaded AddPoints: passing a multiplier this time
        // Cylinder is rare so it gets a small bonus multiplier logged!
        AddPoints(PointValue, 1.0f);

        StartCoroutine(FloatUpEffect());        // [ABSTRACTION] detail hidden below
        Debug.Log($"RARE {CollectibleName} collected! +{PointValue} points!");
    }

    // -------------------------------------------------------
    // [POLYMORPHISM] override: Cylinder reacts to being MISSED.
    // Cube and Sphere use the parent's silent default OnMissed().
    // Cylinder overrides this.  
    // -------------------------------------------------------
    public override void OnMissed()
    {
        Debug.Log($"You missed the rare {CollectibleName}! No points for you!");
        FlashMissColor();           // [ABSTRACTION] flash detail hidden below

        // [INHERITANCE] base.OnMissed() calls the PARENT's version after the code.   
        
        base.OnMissed();
    }

    // -------------------------------------------------------
    // [ABSTRACTION] Float up effect here.       
    // -------------------------------------------------------
    private IEnumerator FloatUpEffect()
    {
        float elapsed = 0f;

        // Briefly change color to purple/white flash
        if (_renderer != null)
            _renderer.material.color = Color.white;

        // Float upward for _floatDuration seconds
        while (elapsed < _floatDuration)
        {
            transform.position += Vector3.up * _floatSpeed * Time.deltaTime;
            elapsed += Time.deltaTime;
            yield return null;
        }

        // Destroy after floating: HandleClick already set HasBeenCollected = true
        Destroy(gameObject);
    }

    // -------------------------------------------------------
    // [ABSTRACTION] Miss flash here.  
    // -------------------------------------------------------
    private void FlashMissColor()
    {
        if (_renderer != null)
            _renderer.material.color = Color.grey;  // dims out when missed
    }
        
}