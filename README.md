# Pixel-Platformer
A complete 2D pixel-art platformer game independently developed from scratch using the Unity engine and C#. This project was my BSc thesis, covering the entire lifecycle from concept art design to final testing.

**Game link:** https://github.com/DDavidasd/Pixel-Platformer/releases/tag/v1.0

**Youtube link:** https://www.youtube.com/watch?v=-Fo3tjfQaIk

**Scripts link:** https://github.com/DDavidasd/Pixel-Platformer/tree/main/Scripts

*   **Movement:** Run, jump, double jump, wall slide (with fast slide mechanics), glide, and dash.
*   **Dynamic gameplay elements:** Moving platforms (multi-point pathing), one-way platforms (jump through from below), and parallax background scrolling (5 independent layers).
*   **Enemies and traps:** 
    *   *Enemies:* Skeleton (patrolling with platform edge detection using gizmos) and Ghost.
    *   *Traps:* Saw (moving along specific points), Fire Trap (deactivatable via switches), and falling Boulder (with custom timing delays).
*   **Game systems:** 
    *   3 Difficulty levels (Easy with deadzone protection, Normal with 3 lives, Hard with 1 life).
    *   Save and checkpoint system.
    *   Main Menu, settings (volume sliders utilizing `PlayerPrefs` for persistent storage).
    *   In-game shop system to unlock character skins using collected coins.
    *   Dynamic audio feedback (pitch shifting effect on coin collection).


# **Software Architecture, Testing, Clean Code (SOLID)**
The project was built using an **iterative development** workflow with a strict focus on code quality:
*   **Iterative Testing and Playtesting:** Each development cycle was paired with rigorous manual testing and playtesting. This ensured that newly introduced mechanics (like wall-sliding or dashing) seamlessly integrated with existing physics without breaking character controller constraints.
*   **Gizmos-Assisted Debugging:** Leveraged Unity's `OnDrawGizmos` rendering to visually test and verify collision radiuses, ground checks, and enemy edge-detection zones in real-time within the editor, drastically reducing physics-related bugs.
*   **Raycast-Based Collision Detection:** Utilized `Physics2D.Raycast` and `Physics2D.OverlapCircleAll` for highly precise environment and enemy detection. This bypasses the typical latency of standard physics triggers, ensuring responsive controls and accurate hit registration.

### **SOLID Principles:**
*   **Single Responsibility Principle (SRP):** Classes have one clear responsibility. For example, the `Trap` class handles interaction logic exclusively, without managing player health or other mechanics.
*   **Open/Closed Principle (OCP):** The system is easily extendable. The base `Trap` class provides basic functionality, allowing various trap types to inherit and expand upon it without modifying existing code.
*   **Liskov Substitution Principle (LSP):** The base `Enemy` class establishes core behaviors. Derived classes (e.g., `Skeleton`) override methods correctly without breaking the base logic.

### **Design Patterns:** 
*   **Singleton Pattern:** Applied to global managers (like `AudioManager` and `PlayerManager`) to ensure a single, globally accessible instance throughout the game's lifecycle, preventing duplicate background tracks or broken state references.
*   **Factory Method Approach:** Utilized via Unity's dynamic `Instantiate` system to handle runtime visual assets. For instance, the destruction mechanism safely spawns configured particle prefabs (`deathFx`) at runtime without hardcoding specific effect types into the core gameplay logic.
*   **Implicit Finite State Machine (FSM):** The `Player` controller tracks movement variables (`isGrounded`, `isWallSliding`, `isDashing`, `isGlide`) to coordinate complex 2D platformer mechanics. These logical states are directly mapped and synchronized with Unity's state-driven **Animator** system.
*   **Strategy Pattern (Implicit):** Used in the player's dynamic skinning system (`LayerSkinAnim`). Instead of using heavy conditional branches (`if/else`) to check which character skin is selected, the system dynamically switches behavioral graphics at runtime by shifting Unity Animator layer weights based on the chosen skin ID.
*   **Template Method Pattern:** Implemented across the obstacle and enemy systems. The base `Trap` and `Enemy` classes define core physics, collision, and interaction rules, while specialized derived classes (like `Trap_Fire`, `Ghost`, or `Enemy_Skeleton`) safely override and extend behaviors without rewriting fundamental logic.

### **Clean Code Principles:** 
Followed **DRY** (Don't Repeat Yourself) via proper inheritance, **KISS** (Keep It Simple, Stupid) for readability, and **YAGNI** (You Aren't Gonna Need It) to avoid over-engineering.

# **Retrospective, Future Improvements**
*   **Dedicated State Machine:** While the current implicit FSM successfully manages state transitions via Boolean values and the Unity Animator, refactoring the physics and input logic into a dedicated C# State Pattern architecture would further decouple the code as the player's moveset expands.
*   **Audio Enhancement:** Implement **3D AudioSource** component mechanics (e.g., spatial audio for the saw trap where volume depends on proximity).
*   **Expansion:** Add multiplayer support and introduce new character types, and hazards.
