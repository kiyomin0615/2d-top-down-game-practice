# 2D Top-Down Online Game

- A multiplayer 2D top-down online game built using **C#**, **Unity**, and a custom **TCP game server**.

---

## 📚 Features

### 1. Player Mechanics
- **Movement**: up, down, left, right
- **Close-Range Attack**
- **Ranged Attack**

### 2. Enemy AI
- **Movement**: Enemies roam randomly when the player is out of range.
- **Attack Capabilities**:
  - Close-range attacks.
  - Ranged attacks with arrows.
- **Player Detection**:
  1.  When the player enters the detection range, the enemy uses the **A\* algorithm** to calculate the shortest path to the player.
  2. And traces the player and initiates an attack.

### 3. Multiplayer Functionality
- A **C# TCP server** handles real-time multiplayer gameplay.
- The server utilizes **multi-threading** to efficiently manage multiple client connections.
- **Custom packet communication** ensures interaction between server and clients.

---

## 🎮 DEMO
- **A\* Pathfinding Algorithm**
    - Enemies use the **A\* algorithm** to find the shortest path to the player.
    - ![finding-path-with-a-star-algorithm](https://github.com/user-attachments/assets/8ca37513-a406-4c5a-bedb-b02a38519481)

- **Multiplayer Gameplay**
    - Multiplayer functionality implemented via **C# TCP server**.
    - Players can interact in real-time.
    - ![multi-player-game](https://github.com/user-attachments/assets/9809b580-c2ce-479a-8f4f-ba9748d51639)

---

## 🛠️ Technologies Used

- **Unity With C# Script**: Game engine for developing the client.
- **C# TCP Server**: Handles multiplayer networking and game logic.
- **A\* Algorithm**: Implements efficient pathfinding for enemy AI.

---

## 💡 Future Development (TODO)
- **Stat System**:
  - Implement comprehensive stats for players and enemies (e.g., HP, attack power, defense).
- **Server-Side AI**:
  - Move the A* pathfinding algorithm to the server for enhanced security and performance.
- **Game Server Deployment**:
  - Host the server on a cloud provider to enable global multiplayer access.