# 2D Platformer Toolkit - Complete Documentation

## Project Overview
The 2D Platformer Toolkit is a comprehensive Unity-based system for creating 2D platformer games. It provides a complete set of scripts for player movement, animation, camera control, UI management, and game mechanics.

**Namespace:** `Peykarimeh.PlatformerToolkit`

---

## Core Components

### 1. Movement2D
**File:** `Movement2D.cs`  
**Description:** The heart of the platformer system, handling all player movement mechanics.

#### Key Features:
- **Basic Movement:** Smooth horizontal movement with customizable acceleration and deceleration
- **Jumping System:** 
  - Variable jump height (hold jump button for higher jumps)
  - Coyote time (grace period for jumping after leaving a platform)
  - Jump buffering (register jump input slightly before landing)
  - Configurable jump height and acceleration
- **Wall Jump Mechanics:**
  - Wall sliding with customizable slide speed
  - Wall jump with directional velocity
  - Input delay after wall jump for better control
  - Variable jump height on wall jumps
- **Dash System:**
  - Horizontal and/or vertical dashing
  - Configurable dash distance, duration, and cooldown
  - Can reset dash on ground or wall
  - Air dash capability
  - Dash collider adjustment for sliding under obstacles
- **Ledge Grab/Climb:**
  - Automatic or manual ledge grabbing
  - Smooth ledge climbing animation
  - Can disable wall jump while climbing
- **Advanced Physics:**
  - Separate acceleration for jump up/down
  - Fall speed clamping
  - On-air movement control
  - Custom gravity system

#### Configurable Parameters:
- Movement speed, acceleration, deceleration
- Jump height, gravity, fall speed
- Dash distance, duration, cooldown
- Wall jump velocity and settings
- Ledge grab settings
- Ground/ceiling/wall detection layers and distances

#### Player States:
- `None` - Initial state
- `Grounded` - Player on ground
- `Jumping` - Player ascending
- `Falling` - Player descending

#### Required Components:
- `Rigidbody2D` (automatically configured)
- `CapsuleCollider2D`
- Child GameObject with sprite/animator

---

### 2. AnimationController
**File:** `AnimationController.cs`  
**Description:** Bridges movement data to Unity's Animator for smooth character animations.

#### Animator Parameters:
- `Dashing` (Bool) - Is player dashing
- `JumpSpeed` (Float) - Current vertical speed
- `MovementSpeed` (Float) - Absolute horizontal speed
- `Grounded` (Bool) - Is player on ground
- `OnWallHit` (Bool) - Is player touching wall with wall jump enabled
- `OnLedge` (Bool) - Is player on ledge
- `ClimbLedge` (Bool) - Is player climbing ledge

#### Usage:
Attach to player GameObject with Movement2D and Animator components.

---

### 3. CameraFollow
**File:** `CameraFollow.cs`  
**Description:** Smooth camera following system with customizable behavior.

#### Features:
- **Smooth Following:** Lerp-based camera movement
- **Offset Control:** Customizable camera offset from target
- **Speed Multipliers:** Different follow speeds for X, Y, Z axes
- **Vertical Damping:** Dynamic Y-axis smoothing based on distance

#### Parameters:
- `objectToFollow` - Target transform (usually player)
- `offset` - Camera offset position
- `speedMultiplier` - Per-axis speed multipliers
- `speed` - Overall follow speed (0-1)

---

### 4. PauseManager
**File:** `PauseManager.cs`  
**Description:** Handles game pause functionality and quick actions.

#### Keybindings:
- **ESC:** Toggle pause
- **R:** Restart current level
- **M:** Return to main menu

#### Features:
- Time scale control (freeze/unfreeze game)
- Pause panel UI management
- Scene reloading capability

---

### 5. RaceTimer
**File:** `RaceTimer.cs`  
**Description:** Singleton timer system for tracking race/speedrun times.

#### Features:
- **Persistent Timer:** Survives scene transitions (DontDestroyOnLoad)
- **High Score Tracking:** Automatically tracks best time
- **TextMeshPro Integration:** Real-time UI updates
- **Start/Stop Control:** Trigger-based race timing

#### Public Methods:
- `StartRace()` - Begin timing
- `FinishRace()` - Stop timing and check high score
- `ResetTimer()` - Reset timer to zero

#### Format:
Displays time as: `0.000` (seconds with 3 decimal places)

---

### 6. StartFinishCheck
**File:** `StartFinishCheck.cs`  
**Description:** Trigger handler for race start and finish lines.

#### Required Setup:
- GameObject with trigger collider
- Objects tagged as "Start" or "Finish"
- Player tagged as "Player"

#### Functionality:
- **OnTriggerExit (Start):** Begins race timer
- **OnTriggerEnter (Finish):** Stops race timer

---

### 7. TeleportOnFall
**File:** `TeleportOnFall.cs`  
**Description:** Respawn system for when player falls off map.

#### Setup:
- Attach to trigger zone (death zone)
- Add child GameObject as respawn point
- Tag player as "Player"

#### Behavior:
When player enters trigger:
- Teleports player to respawn point
- Moves camera to respawn point

---

### 8. ButtonController
**File:** `ButtonController.cs`  
**Description:** UI button handler for scene management.

#### Features:
- Scene loading by name
- Automatically destroys RaceTimer instance when returning to menu

#### Public Method:
- `LoadScene(string SceneName)` - Load specified scene

---

### 9. SetPlayerToSceneViewPosition
**File:** `SetPlayerToSceneViewPosition.cs`  
**Description:** Editor tool for testing player from Scene View position.

#### Features (Editor Only):
- **Quick Testing:** Start game with player at Scene View position
- **Visual Crosshair:** Shows spawn point in Scene View
- **Auto-Configuration:** Updates position when entering play mode

#### Parameters:
- `SetToView` - Enable/disable feature
- `CrossColor` - Crosshair color in Scene View
- `crossSize` - Size of crosshair indicator

---

## System Architecture

### Dependency Relationships:
```
Movement2D (Core)
    ├── AnimationController (Requires Movement2D)
    ├── StartFinishCheck (Optional, for racing)
    └── TeleportOnFall (Optional, for respawn)

CameraFollow (Independent)
    └── Tracks Movement2D's GameObject

RaceTimer (Singleton)
    ├── StartFinishCheck (Controls timer)
    └── ButtonController (Cleanup on menu)

PauseManager (Independent)
```

### Required Unity Setup:
1. **Layers:**
   - Ground layer
   - Wall layer
   - Ceiling layer
   - Ledge layer

2. **Tags:**
   - "Player"
   - "Start"
   - "Finish"

3. **Input Settings:**
   - Horizontal axis
   - Vertical axis
   - Jump button (default: Space)
   - Dash button (default: LeftShift)

---

## Implementation Guide

### Basic Player Setup:
1. Create GameObject for player
2. Add `Movement2D` component
3. Add `Rigidbody2D` and `CapsuleCollider2D` (auto-configured)
4. Create child GameObject with `SpriteRenderer` and `Animator`
5. Add `AnimationController` component
6. Configure movement parameters in inspector
7. Set up Animator with required parameters

### Camera Setup:
1. Add `CameraFollow` to main camera
2. Assign player transform as target
3. Adjust offset and speed settings

### Race/Timer Setup:
1. Create empty GameObject for timer
2. Add `RaceTimer` component
3. Link TextMeshPro UI elements
4. Add trigger colliders for start/finish
5. Tag them appropriately
6. Add `StartFinishCheck` to player

### Respawn Setup:
1. Create trigger zone at fall areas
2. Add `TeleportOnFall` component
3. Add child GameObject as respawn point

---

## Best Practices

### Performance:
- Use layers efficiently for collision detection
- Minimize ground check distances
- Cache component references

### Design:
- Test movement values in different scenarios
- Adjust coyote time and jump buffer for feel
- Balance wall jump velocity for fair gameplay
- Use dash cooldown to prevent spam

### Animation:
- Create smooth transitions between states
- Use blend trees for movement speeds
- Handle edge cases (wall slide, ledge grab)

---

## Common Use Cases

### Precision Platformer:
- Lower movement speed
- High jump buffer and coyote time
- Tight control (low on-air control)

### Fast-Paced Action:
- High movement and dash speed
- Wall jump enabled
- Short dash cooldown
- High on-air control

### Speedrunning:
- Enable RaceTimer system
- Multiple checkpoint spawns
- Optimized dash and wall jump

---

## Technical Notes

### Physics:
- Uses `Rigidbody2D.linearVelocity` for movement
- Custom gravity implementation
- CircleCast for ground/ceiling detection
- Raycast for wall detection

### Update Loops:
- `Update()` - Input handling and state logic
- `FixedUpdate()` - Physics calculations and movement
- `LateUpdate()` - Camera following (CameraFollow)

### Editor Integration:
- Custom inspector support for Movement2D
- Auto-value generation for collision detection
- Gizmos for visual debugging
- Scene View integration (SetPlayerToSceneViewPosition)

---

## Version Information
**Toolkit:** 2D Platformer Toolkit  
**Author:** Peykarimeh  
**Unity Version:** 2021.3+ (recommended)  
**Dependencies:** TextMeshPro (for UI), Unity 2D Physics

---

## Support and Extensions

### Extensibility Points:
- Custom player states in Movement2D
- Additional animator parameters
- New trigger behaviors (like StartFinishCheck)
- Enhanced camera behaviors

### Debugging:
- Gizmos show collision detection areas
- Inspector displays real-time values
- State visualization in Movement2D

---

## Conclusion
This toolkit provides a solid foundation for 2D platformer development with extensive customization options. The modular design allows developers to use only needed features while maintaining clean, organized code structure.
