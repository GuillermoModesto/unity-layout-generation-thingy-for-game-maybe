# 🔁 Procedural Room Layouts in Unity

A Unity-based system for generating multiple grid-based room layouts using procedural logic. Each layout is composed of interconnected rooms with dynamically placed doorways and optional triggers that allow seamless switching between layouts during runtime.

---

## 🛠️ Features

- Procedural generation of N × M room grids.
- Automatic doorway placement between connected rooms.
- Triggers placed randomly in doorways to switch layouts.
- Modular architecture using `RoomController`, `GridSpawner`, and `LayoutManager`.
- Built with Unity’s Input System for smooth player movement and interaction.

---

## 📦 Project Structure

```bash
Assets/
├── Scripts/
│   ├── LayoutManager.cs
│   ├── GridSpawner.cs
│   ├── RoomController.cs
│   └── PlayerMovement.cs
├── Prefabs/
│   ├── RoomPrefab (with walls/floor)
│   ├── DoorwayPrefab
│   └── DoorwayTriggerPrefab 
```

## 🧩 Components Overview

---

### `LayoutManager.cs`
Responsible for generating and managing multiple room layouts.

#### 🔑 Key Public Fields
- `int numberOfLayouts`: Number of layout variations to generate.
- `GameObject roomPrefab`: Room template prefab.
- `GameObject doorWayPrefab`: Doorway prefab to insert between rooms.
- `Transform origin`: Start point of the grid.
- `float spacing`: Distance between rooms.
- `int rows`, `columns`: Grid dimensions.

#### 🔧 Key Methods
- `void Start()`: Generates all layouts and shows the first one.
- `void NextLayout()`: Switches to the next layout.
- `void GenerateLayouts()`: Creates N layouts using `GridSpawner`.

---

### `GridSpawner.cs`
Generates a 2D grid of rooms and connects them with procedural doorways.

#### 🔑 Key Public Fields
- `int rows`, `columns`: Grid size.
- `float spacing`: Room spacing.
- `float density`: Controls number of extra connections beyond the minimum.
- `GameObject roomPrefab`, `doorWayPrefab`: Prefabs for room and doorway.
- `Transform origin`: World-space origin for grid placement.

#### 🔧 Key Methods
- `void Generate()`: Main entry point for generating and connecting rooms.
- `void SpawnGrid()`: Instantiates room grid.
- `void GenerateConnections()`: Uses DFS to ensure all rooms are reachable.
- `void ApplyConnections()`: Places doorways between connected rooms.

---

### `RoomController.cs`
Manages an individual room's structure and modifies its walls.

#### 🔑 Key Public Fields
- `GameObject northWall`, `southWall`, `eastWall`, `westWall`: Wall references.
- `GameObject doorWayPrefab`: Door to place.
- `GameObject doorWayTriggerPrefab`: Optional trigger object.
- `float triggerChance`: Probability of doorway getting a trigger (0–1).

#### 🔧 Key Method
- `void AddDoorway(string wall, bool spawnTrigger = true)`: Replaces a wall with a doorway, optionally spawning a trigger.

---

### `PlayerMovement.cs`
Handles first-person movement and layout switching on trigger exit.

#### 🔑 Key Public Fields
- `float moveSpeed`, `sprintAmount`: Movement settings.
- `Transform cameraHolder`: Camera pivot for look control.

#### 🔧 Key Method
- `void OnTriggerExit(Collider other)`: If exiting a trigger tagged `DoorTrigger`, switches to the next layout via `LayoutManager`.

---
