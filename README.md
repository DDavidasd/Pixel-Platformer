# Pixel-Platformer
A complete 2D pixel-art platformer game independently developed from scratch using the Unity engine and C#. This project was my BSc Thesis, covering the entire lifecycle from concept art design to final testing.

Unity 2D pixel-art platformer game (thesis project). Independently developed in C# from concept to testing, using custom Aseprite assets. Applied SOLID (SRP, OCP, LSP), DRY, and KISS principles. Implemented PlayerPrefs saving, parallax backgrounds.

Game link: https://github.com/DDavidasd/Pixel-Platformer/releases/tag/v1.0
Youtube link: https://www.youtube.com/watch?v=-Fo3tjfQaIk

Featueres and mechanics:
  Movement: Run, jump, double jump, wall slide (with fast slide mechanics), glide, and dash.
  Dynamic Gameplay Elements: Moving platforms (multi-point pathing), one-way platforms (jump through from below), and parallax background scrolling (5 independent layers).
  Enemies & Hazards: 
    *Enemies:* Skeleton (patrolling with platform edge detection using gizmos) and Ghost.
    *Traps:* Saw (moving along specific points), Fire Trap (deactivatable via switches), and falling Boulder (with custom timing delays).


  *   **Game Systems:** 
    *   3 Difficulty levels (Easy with deadzone protection, Normal with 3 lives, Hard with 1 life).
    *   Save/Checkpoint system.
    *   Main Menu, Settings Menu (Volume sliders utilizing `PlayerPrefs` for persistent storage).
    *   In-game shop system to unlock character skins using collected coins.
    *   Dynamic audio feedback (pitch shifting effect on coin collection).



*   **Advanced Movement:** Run, jump, double jump, wall slide (with fast slide mechanics), glide, and dash (teleport).
*   **Dynamic Gameplay Elements:** Moving platforms (multi-point pathing), one-way platforms (jump through from below), and parallax background scrolling (5 independent layers).
*   **Enemies & Hazards:** 
    *   *Enemies:* Skeleton (patrolling with platform edge detection using gizmos) and Ghost.
    *   *Traps:* Saw (moving along specific points), Fire Trap (deactivatable via switches), and falling Boulder (with custom timing delays).
*   **Game Systems:** 
    *   3 Difficulty levels (Easy with deadzone protection, Normal with 3 lives, Hard with 1 life).
    *   Save/Checkpoint system.
    *   Main Menu, Settings Menu (Volume sliders utilizing `PlayerPrefs` for persistent storage).
    *   In-game shop system to unlock character skins using collected coins.
    *   Dynamic audio feedback (pitch shifting effect on coin collection).
