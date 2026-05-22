# Shape Collector
### Unity Junior Programmer Pathway, Programming Theory in Action

---

## About the Project
Shape Collector is an easy Unity game where 3 diff. colorful shape types fall 
from the sky and the player must click them before they hit the floor.
Each shape type behaves differently when collected, demonstrating
the four pillars of Object-Oriented Programming in C#.

---

## How to Play
- Shapes fall from above, click them before they hit the floor
- Each shape is worth different points:
  - Red Cube      10 points  (slow, common)
  - Yellow Sphere    20 points  (fast, wobbles)
  - Purple Cylinder  50 points  (slow, rare, floats up when collected)
- You have 30 seconds, collect as many as possible!
- Press RESTART when the game ends to play again.

---

## Four OOP Pillars Demonstrated

### 1. INHERITANCE
**File:** `Assets/Scripts/Core/Collectible.cs`

`Collectible` is the abstract base class. All three shapes inherit
from it automatically receiving shared data and behavior:
- `_pointValue`, `_fallSpeed`, `_collectibleName` fields
- `HandleClick()`, `Fall()`, `OnMissed()`, `AddPoints()` methods
No shared code is duplicated across the three shape classes. 
Everything common lives once in `Collectible.cs`.

---

### 2. POLYMORPHISM
**Files:** `Assets/Scripts/Collectibles/`
Each shape overrides methods from the base class differently:

| Method | CubeCollectible | SphereCollectible | CylinderCollectible |
|---|---|---|---|
| `OnCollect()` | Flashes red | Spins | Floats upward |
| `Fall()` | Default (straight) | Overridden (wobble) | Default (straight) |
| `OnMissed()` | Default (silent) | Default (silent) | Overridden (reacts) |

`InputManager` calls `collectible.HandleClick()` on any shape 
the correct child behavior fires automatically at runtime.

Method overloading is demonstrated in `Collectible.cs`:
- `AddPoints(int amount)`
- `AddPoints(int amount, float multiplier)`

---

### 3. ENCAPSULATION
**File:** `Assets/Scripts/Core/Collectible.cs`
Private fields are protected from direct external access.
All data is accessed through validated properties:

- `PointValue` never goes negative
- `FallSpeed`  clamped between 0.5 and 15
- `CollectibleName` rejects null or empty strings
- `HasBeenCollected` private set, prevents double-collecting

`ScoreManager.cs` also demonstrates encapsulation:
- `_currentScore` is fully private
- Only modifiable through `AddScore()` which validates input

---

### 4. ABSTRACTION
**Files:** `Assets/Scripts/Core/`, `Assets/Scripts/Managers/`

Complex logic is hidden behind simple method calls:

| Method | What it hides |
|---|---|
| `HandleClick()` | Guard logic, score call, destroy timing |
| `InitializeCollectible()` | All field setup and validation |
| `SpawnShape()` | Position math, rarity weighting, instantiation |
| `HandleMouseClick()` | Entire raycast system |
| `StartGame()` | Coordinates all managers in one call |
| `EndGame()` | Cleanly shuts down all systems |

---
## Tools Used
- Unity 2022 LTS
- C# / Visual Studio
- GitHub Desktop
- TextMeshPro

---