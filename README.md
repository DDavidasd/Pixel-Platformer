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


# **Software Architecture, Clean Code (SOLID)**
The project was built using an **iterative development** workflow with a strict focus on code quality:
*   **Single Responsibility Principle (SRP):** Classes have one clear responsibility. For example, the `Trap` class handles interaction logic exclusively, without managing player health or other mechanics.
*   **Open/Closed Principle (OCP):** The system is easily extendable. The base `Trap` class provides basic functionality, allowing various trap types to inherit and expand upon it without modifying existing code.
*   **Liskov Substitution Principle (LSP):** The base `Enemy` class establishes core behaviors. Derived classes (e.g., `Skeleton`) override methods correctly without breaking the base logic.
*   **Design Patterns:** Implemented the **Singleton** pattern for the `AudioManager` to ensure a single instance and prevent duplicate background music instances.
*   **Clean Code Principles:** Followed **DRY** (Don't Repeat Yourself) via proper inheritance, **KISS** (Keep It Simple, Stupid) for readability, and **YAGNI** (You Aren't Gonna Need It) to avoid over-engineering.

# **Retrospective, Future Improvements**
*   **State Pattern:** In hindsight, handling complex player movement states with methods led to large `if-else` structures. Refactoring the movement system into a proper **State Machine** would significantly clean up the code and animation handling.
*   **Audio Enhancement:** Implement **3D AudioSource** component mechanics (e.g., spatial audio for the saw trap where volume depends on proximity).
*   **Expansion:** Add multiplayer support and introduce new character types, and hazards.
