# 🦁 Project Roary
### A 2D FIU-Themed RPG Built in Godot 4

> *Explore a Florida International University-inspired world, battle bosses, manage your inventory, and uncover the story of FIU.*

---

## 📖 About the Project

**Project Roary** is a 2D action RPG built over a 10-week development sprint by a 14-member cross-disciplinary team of beginner game developers at **Florida International University**, as part of the **INIT Game Dev 2D program**. The game features a vibrant FIU-themed world complete with custom art, original music, NPC dialogue, a shop and inventory system, dynamic day/night cycles, and multiple multi-phase boss encounters.

The project was presented at **INIT Demo Day (December 2025)** as a fully playable demo, delivering **50+ features** and an estimated **8,000–15,000 lines of C# code**.

---

## 🎮 Gameplay Features

### 🧍 Player
- Smooth movement with stamina and health systems
- Dodge mechanic for combat evasion
- Real-time HUD displaying health, stamina, time, and temperature

### 🎒 Inventory & Hotbar
- 16-slot inventory with drag-and-drop and item stacking
- 4-slot hotbar with `1–4` shortcuts
- Press `I` to open/close the inventory
- Items can be dropped back into the world

### 🛒 Shop
- Interactable ShopKeeper NPC located in the Graham Center
- Coin-based economy with live balance display
- Cart/receipt system for purchasing multiple items at once

### 💬 Dialogue & NPCs
- Interactable NPCs with multi-line dialogue sequences
- Unique characters like the WiseTurtleBrother scattered throughout the world
- Proximity-based interaction prompts

### 🐉 Bosses

| Boss | Description |
|---|---|
| **Logo** | First phase of the Mermaid Boss encounter — an intro-difficulty fight |
| **Mermaid** | A mid-game boss inspired by the Starbucks mermaid with 4 unique attacks |
| **Roary** | The final boss — a full multi-phase fight with 8+ distinct attack patterns |

**Roary's Attacks include:**
- Arena roaming between attack phases
- Direct charges and lateral dashes at the player
- Telegraphed ground slams, firework spreads, orbital hazards, and a football stampede

### 🌙 Day/Night Cycle
- Dynamic in-game clock with days of the week, hours, minutes, and temperature
- Visual lighting transitions between day and night
- All displayed live on the HUD

### ⏸️ Menus
- FIU-themed pause menu, main menu, and death screen
- Animated transitions between states

### 🔊 Audio
- Scene-specific background music that changes as you move through areas
- Sound effects for combat, item pickups, and UI interactions

---

## 🛠️ Tech Stack

| Technology | Role |
|---|---|
| **Godot 4.5** | Game engine |
| **C# (.NET)** | Primary scripting language |
| **GitHub** | Version control and project management |
| **Discord** | Team communication and coordination |

---

## 🚀 Getting Started

### Prerequisites
- [Godot 4.5](https://godotengine.org/download) (with .NET / C# support)
- [.NET SDK 8.0+](https://dotnet.microsoft.com/download)

### Installation

1. **Clone the repository:**
```bash
git clone https://github.com/Xtreak-XD/2DGameDev.git
cd 2DGameDev
```

2. **Open in Godot:**
   - Launch Godot 4.5
   - Click **Import**, navigate to the cloned folder, and select `project.godot`

3. **Build the C# solution:**
   - In Godot, go to **Build → Build Solution** (or press `Ctrl+B`)

4. **Run the game:**
   - Press `F5` or click the **Play** button

> **Note:** Make sure you are using Godot 4.5 with .NET support enabled. The standard non-.NET build of Godot will not run this project.

---

## 🎯 Controls

| Action | Key |
|---|---|
| Move | `W A S D` |
| Dodge | `Shift` |
| Interact / Talk | `E` |
| Open Inventory | `I` |
| Hotbar Slot 1–4 | `1` `2` `3` `4` |
| Pause | `Esc` |

---

## 👥 Team

Project Roary was built by a 14-member team through FIU's INIT Game Dev 2D program:

- **Build Leads:** Miguel A., Chris P.
- **Artists:** Monica B., Matthew, Fer P.
- **Musicians:** Haruki H.
- **Developers:** Max, Alex P., Alex V., Anhel, Cris A., Iadd C., Monica B.

---

## 📄 License

This project was created for educational purposes through FIU's INIT Game Dev 2D program. All original artwork, music, and code belong to their respective contributors.

---

<p align="center">
  <em>Made with ❤️ at Florida International University — Go Panthers! 🐾</em>
</p>
