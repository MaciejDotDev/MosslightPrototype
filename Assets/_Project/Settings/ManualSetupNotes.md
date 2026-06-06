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

- Add a `RoadChunkSpawner` scene object.
- Assign the player or vehicle transform to `Player Or Vehicle`.
- Add one or more `RoadChunk` prefabs to `Road Chunk Prefabs`.
- Tune `Initial Chunk Count`, `Chunks Ahead`, and `Remove Behind Distance` after a first drive test.
