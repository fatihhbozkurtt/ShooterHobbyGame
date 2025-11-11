# 🧠 Strategy Demo Project
A modular, Unity demo showcasing dynamic enemy spawning, player feedback systems, and scalable architecture designed for mobile strategy gameplay.

#region 🚀 FEATURES

## 🎮 Joystick-Based Player Movement
- Modular movement system with third-party joystick integration  
- Compatible with Unity’s NavMesh navigation  

## 💥 Dynamic Enemy Spawning & Difficulty Scaling
- Enemies spawn in timed waves using **UniTask**  
- Difficulty automatically scales: *Easy → Medium → Hard* based on game time  
- NavMesh-safe spawn validation for every enemy  

## 🧠 Enemy AI & Type System
- AI behavior and visuals change dynamically over time  
- Object pooling for optimized runtime performance  

## 🩸 Player Damage & Feedback System
- Invincibility window after taking damage  
- Flicker visual feedback using **UniTask** with cancellation support  

## 🔢 Kill Count System
- Centralized tracking and event-driven UI updates  
- Uses **TextMeshPro** for responsive display  

#endregion


#region 🧱 ARCHITECTURE HIGHLIGHTS

## 🧩 Inspector-Driven Modular Design
- Clear separation between **config** (ScriptableObjects) and **logic**  
- Editor-friendly workflow for quick iteration  

## 🔁 Event-Based Communication
- Decoupled flow using **C# events** and **Action** delegates  
- Easy to extend with new systems (camera shake, sound effects, etc.)  

## 🧵 UniTask Integration
- Replaces coroutines for improved performance  
- Used for flicker feedback, wave spawning, and timed delays  

#endregion


#region 📦 PROJECT STRUCTURE

Assets/
├─ Scripts/
│ ├─ Player/
│ ├─ Enemies/
│ ├─ Systems/
│ ├─ UI/
│ └─ ScriptableObjects/
├─ Prefabs/
├─ Scenes/
└─ Materials/

#endregion


#region 🛠 REQUIREMENTS

- Unity **2022.3+**  
- **TextMeshPro**  
- **Cysharp UniTask**  
- **NavMesh** baked in scene  
- *(Optional)* Joystick plugin (e.g. InControl, Rewired)

#endregion


#region 🧪 HOW TO TEST

1. Open the main gameplay scene  
2. Press **Play**  
3. Use the joystick to move the player  
4. Observe enemy waves spawning and difficulty scaling  
5. Check **Kill Count UI** and **flicker feedback** on player damage  

#endregion


#region ✨ CREDITS

Developed by **Fatih Bozkurt**  
Built with a focus on **clean code**, **modular design**, and **mobile performance**

#endregion
