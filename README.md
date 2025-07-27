#  Clash Royale Mini-Clone 🏰

![Unity Version](https://img.shields.io/badge/Unity-6000.0.51f1-blue?style=for-the-badge&logo=unity)
![License](https://img.shields.io/badge/License-MIT-green?style=for-the-badge)

A feature-rich, single-player mini-clone of Clash Royale, developed in Unity. This project serves as a comprehensive case study in robust game architecture, clean code principles, and the practical application of design patterns. It was developed during a one-month internship at **Zagrava Games**.

<div align="center">
<img width="170" height="auto" alt="image" src="https://github.com/user-attachments/assets/50abafe2-19ce-4d50-a484-b261da01abaa" />
</div>

---

## 📜 Table of Contents

* [🎮 Core Gameplay Features](#-core-gameplay-features)
* [🏗️ Architectural Deep Dive & Design Patterns](#️-architectural-deep-dive--design-patterns)
* [📦 Building the Project](#-building-the-project)
* [👨‍💻 About the Authors](#-about-the-authors)
* [📄 License](#-license)

---


## 🎮 Core Gameplay Features

* 💧 **Mana Management:** A dynamic mana regeneration system that dictates the pace of gameplay.
* 🃏 **Card and Deck System:** Draw, play, and recycle cards from a deck to summon units onto the battlefield.
* ⚔️ **Unit Spawning & AI:** Place units that automatically navigate towards the nearest enemy or tower. The game features a simple but effective AI opponent that strategically plays its own cards.
* 🌐 **Grid-Based Battlefield & Pathfinding:** All interactions, from unit placement to their intelligent movement across the map, are managed on a robust grid system utilizing efficient **algorithms** for pathfinding.
* 🤖 **Diverse Unit Types:** The architecture supports a wide variety of units with unique stats and behaviors (e.g., melee, ranged, ground, flying, static structures).
* ✨ **Dynamic Animations:** Units come to life with animations that respond to their movement direction, attacks, and idle states.

---

## 🏗️ Architectural Deep Dive & Design Patterns

This project was built with a strong emphasis on clean, modular, and extensible code, heavily leveraging **Unity's component-based architecture** and **Scriptable Objects** for data management. Here are the key architectural patterns and decisions that power the game.

### 🤖 State Machine Pattern
Manages the lifecycle and behavior of all units in a clean, organized way, making it easy to add or modify behaviors.
* **Core:** `UnitStateMachine`
* **States:** `IdleState`, `MoveState`, `AttackState`

### 🏭 Factory Pattern
Dynamically attaches behaviors (like movement and attack types) to units at runtime. This promotes **loose coupling** and allows for creating new unit variations without touching the core `Unit` class.
* **Factories:** `MovementFactory`, `AttackFactory`
* **Products:**
    * `IMovementStrategy`: `GroundMovement`, `FlyingMovement`, `StaticMovement`
    * `IAttackStrategy`: `MeleeAttackStrategy`, `RangedAttackStrategy`

### 🎯 Strategy Pattern
Defines a family of interchangeable algorithms and encapsulates each one. This is used extensively to handle variations in core mechanics.
* **Movement Strategies (`IMovementStrategy`):** Governs how different units navigate the world.
    * `GroundMovement`: Critically, this strategy leverages the **A\* pathfinding algorithm** within the `Pathfinder` to intelligently navigate the grid, avoiding obstacles and finding optimal routes to targets.
    * `FlyingMovement`: Uses direct linear interpolation, unconstrained by the ground grid.
    * `StaticMovement`: Does not move, used for structures like towers.
* **Attack Strategies (`IAttackStrategy`):** Abstracts the method of dealing damage.
    * `MeleeAttackStrategy`
    * `RangedAttackStrategy`
* **Grid Region Strategies (`RegionStrategy`):** These **Scriptable Object** assets define various shapes (e.g., `CircleStrategy`, `RectangleStrategy`) used for placement validation and other area-of-effect calculations on the grid.

### 🏛️ Facade Pattern for Mana System
Provides simplified, purpose-driven interfaces to a more complex subsystem. This makes the `ManaManager` easier and safer to use from different parts of the codebase.
* **Facades:** `ManaReadOnlyFacade` (`IManaReadOnly`), `ManaSpenderFacade` (`IManaSpender`)
* **Complex Subsystem:** `ManaManager`

### ⚡ Event-Driven & Service-Oriented Design
To promote decoupling and maintainability, the systems communicate primarily through **C# events** and centralized services. Components subscribe to events they care about instead of holding direct, rigid references to each other.
* **Centralized Services:**
    * `CardPlayabilityService`: Manages card usability based on mana, notifying UI via events.
    * `TargetRegistry`: A static registry of all targetable entities (`ITargetable`) for efficient target acquisition.
    * `ParticleManager`: Handles spawning and recycling of visual effects.
    * `GridManager` & `Pathfinder`: The central hub for all grid queries and efficient **pathfinding algorithms**, providing optimal paths for ground units.
* **Key Events:** `OnCardDropped`, `OnManaChanged`, `OnHealthChanged`, `OnDied`, `OnDirectionChanged`, and many more.

### 🎨 Animation System
The project uses Unity's **Animator component** and **Animation events** to create responsive and dynamic character movements.
* `UnitAnimator`: A dedicated component that controls unit animations based on their state (moving, attacking, idle) and direction.
* `WaitBeforeLoopBehaviour`: A custom StateMachineBehaviour used in animations to introduce delays before looping, often tied to attack speed.

### 📂 Data Management with Scriptable Objects
Crucial game data is managed through **Scriptable Objects**, allowing for easy iteration and configuration without modifying code.
* `CardData`: Defines properties for each playable card.
* `UnitConfig`: Configures all stats, prefabs, and strategy types for each unit.
* `WeaponBase` (and its derivations `MeleeWeaponData`, `RangedWeaponData`): Defines weapon characteristics.
* `GridSettingsData`, `ObstacleData`, `PlacementData`: Configures the battlefield grid, obstacles, and valid placement zones.

---

## 📦 Building the Project

If you want to create a standalone executable version of the game, follow these steps:

1.  **Open Build Settings:**
    * In the Unity Editor, navigate to the top menu bar and select `File` > `Build Settings...`.

2.  **Configure Build Settings:**
    * **Target Platform:** Select your desired platform (e.g., `Android, IOS`).
    * **Scenes In Build:** Ensure that the main game scene is included. If the list is empty, click the **"Add Open Scenes"** button to add `Assets/Scenes/GameScene.unity`. Make sure the checkbox next to it is ticked.

   ![tutorial](https://github.com/user-attachments/assets/8d3a30de-305d-4241-a56f-bcef782f05b4)


3.  **Start the Build:**
    * Click the **"Build"** button. Unity will open a file dialog asking where you want to save the build.

4.  **Choose a Location:**
    * **Pro Tip:** It's good practice to create a new folder named `Builds` or `Releases` in your project's root directory (and add it to your `.gitignore` file). Save your build inside that folder.

5.  **Run the Executable:**
    * Once the build is complete, navigate to the folder you selected. You will find an executable file and a corresponding `_Data` folder.
    * Run the executable file to play the game outside of the Unity Editor.

---

## 👨‍💻 About the Authors

This project was created by **Vladyslav Dzhuha and Vladyslav Yevkov**.

Two passionate programmers and Computer Science students at the Czestochowa University of Technology😄. 

[![GitHub](https://img.shields.io/badge/GitHub-Vladyslav_Dzhuha-black?style=for-the-badge&logo=github)](https://github.com/Vladdjuga)
[![GitHub](https://img.shields.io/badge/GitHub-Vladyslav_Yevkov-black?style=for-the-badge&logo=github)](https://github.com/vlladevk)

[![LinkedIn](https://img.shields.io/badge/LinkedIn-Vladyslav_Dzhuha-blue?style=for-the-badge&logo=linkedin)](https://www.linkedin.com/in/vladyslav-dzhuha/)
[![LinkedIn](https://img.shields.io/badge/LinkedIn-Vladyslav_Yevkov-blue?style=for-the-badge&logo=linkedin)](https://www.linkedin.com/in/vladyslav-yevkov-b2a098353/)

---

## 📄 License

This project is licensed under the MIT License. See the [LICENSE](LICENSE.md) file for details.

--- 


## 🧐 Some other stuff
Also, big thanks to **smlbiobot** for the assets! [Here's the repo](https://github.com/smlbiobot/cr-assets-png)
