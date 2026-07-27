# 2D Platformer Toolkit Documentation

This document provides an overview of the scripts within the 2D Platformer Toolkit project. The toolkit is designed to provide a comprehensive set of scripts for creating a 2D platformer game in Unity.

## Core Scripts

### Movement2D.cs

This is the heart of the toolkit. It's a robust character controller for 2D platformer games.

**Features:**

*   **Basic Movement:** Standard left/right movement with acceleration and deceleration.
*   **Jumping:** Includes features like coyote time, jump buffering, and variable jump height.
*   **Wall Jump & Slide:** Allows the player to slide down walls and jump off them.
*   **Dashing:** Horizontal and vertical dashing with cooldowns and reset conditions.
*   **Ledge Grab & Climb:** Allows the player to grab onto ledges and climb up.
*   **Customizable:** A large number of public variables to tweak the feel of the controller.
*   **State Machine:** A simple state machine to track the player's current state (Grounded, Jumping, Falling).

### Movement2DEditor.cs

A custom Unity Editor script for `Movement2D.cs`. It provides a much more organized and user-friendly inspector for tweaking the player's movement properties. It groups related variables under headers and provides tooltips and debugging information.

## Camera & Animation

### CameraFollow.cs

A simple script to make the camera follow a target object (the player) smoothly. It includes adjustable smoothing and offset.

### AnimationController.cs

This script bridges the `Movement2D` script and the player's `Animator` component. It sets animator parameters based on the player's current state (e.g., `isDashing`, `isGrounded`, `currentVerticalSpeed`).

## Game Logic

### RaceTimer.cs

A singleton script that implements a race timer. It can be started and stopped, and it keeps track of the high score.

### StartFinishCheck.cs

Works in conjunction with `RaceTimer.cs`. It uses trigger colliders to detect when the player passes through the start and finish lines of a race, controlling the timer accordingly.

### PauseManager.cs

A simple script to pause and unpause the game. It also handles restarting the level and returning to the main menu.

### ButtonController.cs

A simple script for handling UI button clicks, specifically for loading scenes.

### TeleportOnFall.cs

A script that teleports the player to a specific position if they fall into a trigger collider. This is useful for handling pits and death zones.

## Editor Tools

### SetPlayerToSceneViewPosition.cs

An editor-only script that allows you to quickly set the player's starting position to the current position of the Scene View camera. This is useful for testing specific sections of a level without having to manually move the player.

## How to Use

1.  Attach the `Movement2D.cs` script to your player character.
2.  Customize the movement properties in the inspector using the `Movement2DEditor`.
3.  Attach the `AnimationController.cs` script to your player character and connect it to your `Animator`.
4.  Use the `CameraFollow.cs` script on your main camera to follow the player.
5.  For race levels, use the `RaceTimer.cs`, `StartFinishCheck.cs`, and UI elements to display the timer.
6.  Use the other scripts as needed for your game's specific requirements.
