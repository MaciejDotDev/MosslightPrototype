# Manual Setup Notes

## Input

- Create a Unity Input Actions asset.
- Add action maps named `Driving`, `OnFoot`, `UI`, and `ActivityView`.
- Add the prototype actions listed in `InputActionNames`.
- Assign the Input Actions asset to an `InputReader` component in the scene.

## Road Prefabs

- Build road meshes/colliders in Blender or Unity.
- Create road prefabs under `Assets/_Project/Prefabs/Roads`.
- Add a `RoadChunk` component to each road prefab root.
- Add and assign child transforms named `EntryPoint` and `ExitPoint`.
- Make each `EntryPoint` and `ExitPoint` face along the road direction, with forward pointing from entry toward exit.
- Add optional `RoadSocket` components to child socket transforms for signs, trees, lay-bys, events, or side-road placeholders.

## PrototypeDrive Scene

- Add an `InputReader` scene object and assign the Unity Input Actions asset.
- Add a `RoadChunkSpawner` scene object.
- Add one or more `RoadChunk` prefabs to `Road Chunk Prefabs`.
- Add a `TruckSpawner` scene object.
- Assign the truck prefab to `Truck Prefab`, or leave it empty to spawn a plain prototype object.
- Assign `InputReader` and `RoadChunkSpawner` on the `TruckSpawner`.
- Use a child transform as `Spawn Point` if the truck should begin somewhere other than the spawner object's transform.
- If the truck prefab already has a cockpit camera, parent it inside the truck and add/let the spawner add `CockpitCameraController`.
- If the truck prefab does not have a camera, the spawner creates a `CockpitCamera` child at the fallback local position.
- Tune `Initial Chunk Count`, `Chunks Ahead`, and `Remove Behind Distance` after a first drive test.

## Demo Driving Checklist

- Road prefabs have `RoadChunk`, `EntryPoint`, `ExitPoint`, and colliders.
- Truck prefab has a collider, or `TruckSpawner` can add its temporary prototype box collider.
- Truck prefab forward axis points down the road direction.
- `Accelerate`, `BrakeReverse`, `Steer`, `Look`, and `ToggleHeadlights` are bound in the Input Actions asset.
- Main camera is the cockpit camera under the truck.
