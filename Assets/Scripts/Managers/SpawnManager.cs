using UnityEngine;

// SpawnManager handles spawning logic.
// [ABSTRACTION] GameManager tells SpawnManager to Start/Stop
public class SpawnManager : MonoBehaviour
{
    // -------------------------------------------------------
    // Singleton setup: one SpawnManager in the scene.
    // -------------------------------------------------------
    public static SpawnManager Instance { get; private set; }

    // -------------------------------------------------------
    // [ENCAPSULATION] private fields, exposed to Inspector via SerializeField
    // -------------------------------------------------------
    [SerializeField] private GameObject _cubePrefab;
    [SerializeField] private GameObject _spherePrefab;
    [SerializeField] private GameObject _cylinderPrefab;

    // -------------------------------------------------------
    // [ENCAPSULATION] private fields with SerializeField
    // -------------------------------------------------------
    [SerializeField] private float _spawnInterval = 1.2f;   // seconds between spawns
    [SerializeField] private float _spawnHeight = 10f;       // how high shapes spawn
    [SerializeField] private float _spawnRangeX = 4f;        // how wide the spawn area is

    // -------------------------------------------------------
    // Private state: no outside script needs to see these
    // -------------------------------------------------------
    private float _spawnTimer = 0f;
    private bool _isSpawning = false;

    // -------------------------------------------------------
    // how often each shape appears   
    // Cube: 60% chance, Sphere: 30% chance, Cylinder: 10% chance
    // [ENCAPSULATION]  
    // -------------------------------------------------------
    private int _cubeWeight = 6;
    private int _sphereWeight = 3;
    private int _cylinderWeight = 1;

    // -------------------------------------------------------
    // Awake() — singleton setup, runs before Start()
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
    // Update() checks the timer every frame.
    // [ABSTRACTION] spawning detail inside SpawnShape()
    // -------------------------------------------------------
    private void Update()
    {
        if (!_isSpawning) return;           // do nothing if spawning is stopped

        _spawnTimer += Time.deltaTime;

        if (_spawnTimer >= _spawnInterval)
        {
            _spawnTimer = 0f;
            SpawnShape();                   // [ABSTRACTION] detail below
        }
    }

    // -------------------------------------------------------
    // [ABSTRACTION]  
    // -------------------------------------------------------
    public void StartSpawning()
    {
        _isSpawning = true;
        _spawnTimer = 0f;
        Debug.Log("SpawnManager: Spawning started.");
    }

    public void StopSpawning()
    {
        _isSpawning = false;
        Debug.Log("SpawnManager: Spawning stopped.");
    }

    // -------------------------------------------------------
    // [ABSTRACTION] SpawnShape() hides all spawning complexity    
    // -------------------------------------------------------
    private void SpawnShape()
    {
        GameObject prefabToSpawn = GetWeightedRandomPrefab();   // rarity logic 
        Vector3 spawnPosition = GetRandomSpawnPosition();        // position math 

        if (prefabToSpawn != null)
        {
            Instantiate(prefabToSpawn, spawnPosition, Quaternion.identity);
        }
        else
        {
            Debug.LogWarning("SpawnManager: A prefab is not assigned in the Inspector!");
        }
    }

    // -------------------------------------------------------
    // [ABSTRACTION] Rarity logic here.
    // Total weight = 6 + 3 + 1 = 10
    // Roll 1-6 = Cube, Roll 7-9 = Sphere, Roll 10 = Cylinder
    // -------------------------------------------------------
    private GameObject GetWeightedRandomPrefab()
    {
        int totalWeight = _cubeWeight + _sphereWeight + _cylinderWeight;
        int roll = Random.Range(1, totalWeight + 1);

        if (roll <= _cubeWeight)
            return _cubePrefab;
        else if (roll <= _cubeWeight + _sphereWeight)
            return _spherePrefab;
        else
            return _cylinderPrefab;
    }

    // -------------------------------------------------------
    // [ABSTRACTION] Position math here.
    // Shapes spawn at a random X position along the spawn bar height.
    // -------------------------------------------------------
    private Vector3 GetRandomSpawnPosition()
    {
        float randomX = Random.Range(-_spawnRangeX, _spawnRangeX);
        return new Vector3(randomX, _spawnHeight, 0f);
    }
}