using UnityEngine;

// [INHERITANCE] CubeCollectible inherits everything from Collectible:
// _pointValue, _fallSpeed, _collectibleName, HandleClick(), Fall(), OnMissed()
// Here is only what will make the Cube different.
public class CubeCollectible : Collectible
{
    
    private Renderer _renderer;
    private Color _originalColor;

    // -------------------------------------------------------
    // Start() runs once when this object appears in the scene.    
    // -------------------------------------------------------
    private void Start()
    {
        _renderer = GetComponent<Renderer>();
        _originalColor = _renderer.material.color;

        // [INHERITANCE] Calling the parent's protected method
        
        InitializeCollectible("Cube", 10, 2f);
    }

    // -------------------------------------------------------
    // [POLYMORPHISM] override, Cube's unique collect behavior.    
    // -------------------------------------------------------
    public override void OnCollect()
    {
        AddPoints(PointValue);              // [INHERITANCE] method from parent
        StartCoroutine(FlashEffect());      // [ABSTRACTION] flash detail hidden below
        Debug.Log($"{CollectibleName} collected! +{PointValue} points");
    }

    // -------------------------------------------------------
    // [ABSTRACTION] The flash complexity here.    
    // -------------------------------------------------------
    private System.Collections.IEnumerator FlashEffect()
    {
        _renderer.material.color = Color.red;           // flash red
        yield return new WaitForSeconds(0.2f);
        _renderer.material.color = _originalColor;      // restore original
    }

    
}