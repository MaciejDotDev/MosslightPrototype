# AGENTS.md - Mosslight / LowBeamPrototype

## Project identity

This is a Unity first-person cozy horror delivery-driving game set in late-1990s Poland.

Working prototype/project name: `LowBeamPrototype`
World/game title direction: `Mosslight Delivery`

The player drives an old FSC Żuk-inspired van between small Polish towns, delivers cargo, camps, explores roads, and experiences subtle strange events. The vehicle is the player’s home, storage, shelter, and main interface.

Do not turn this into a generic horror game, a tycoon upgrade game, a full truck simulator, or a survival spreadsheet.

## Core pillars

1. Driving is the core gameplay.
2. The Żuk is home.
3. Controller and keyboard/mouse support must be considered from the start.
4. UI should be physical/diegetic where practical: dashboard, binder, satchel, radio, signs.
5. Horror is subtle, rare, and memorable.
6. The game should feel cozy as often as it feels strange.
7. Upgrades must be physical, believable, and limited.
8. Roads should feel continuous and consistent, not fully random chaos.
9. Avoid floating markers unless absolutely necessary.
10. Prefer simple staged systems over over-engineered simulation.

## Current Unity setup assumptions

- Unity version: Unity 6.3 LTS.
- Render pipeline: URP.
- Main target: Windows PC.
- Input: Unity New Input System.
- Source control: GitHub with Git LFS.
- Asset serialization should be Force Text.
- Version control mode should use Visible Meta Files.

## Repo layout

Keep project-owned files under `Assets/_Project/`.

Preferred structure:

```text
Assets/
  _Project/
    Art/
      Models/
      Textures/
      Materials/
      Sprites/
      Source/
    Audio/
      Music/
      SFX/
      Ambience/
      Radio/
    Code/
      Core/
      Input/
      Player/
      Vehicle/
      Interaction/
      UI/
      World/
      Roads/
      Towns/
      Delivery/
      Cargo/
      Camping/
      Events/
      Saving/
      Audio/
      Debug/
    Prefabs/
      Vehicle/
      Roads/
      UI/
      Props/
      Buildings/
      Camping/
      Cargo/
    Scenes/
      PrototypeDrive/
      Boot/
      MainMenu/
      TownPrototype/
    ScriptableObjects/
      Jobs/
      DeliveryLocations/
      Cargo/
      Routes/
      Towns/
      Items/
      Upgrades/
    Settings/
```

Do not create random top-level folders unless there is a clear reason.

## Naming conventions

Use clear boring names. Boring names save lives.

C#:
- Classes: `PascalCase`
- Methods: `PascalCase`
- Private fields: `_camelCase`
- Serialized private fields: `[SerializeField] private Type _fieldName;`
- Interfaces: `IInteractable`, `ISaveable`

Assets:
- Vehicles: `veh_zuk_blockout_01`
- Roads: `road_forest_straight_01`
- Props: `prop_tree_pine_01`
- UI: `ui_satchel_panel_01`
- Materials: `mat_road_wet_tarmac_01`
- Textures: `tex_zuk_body_01`
- Audio: `sfx_wiper_loop_01`, `amb_rain_light_01`

## Coding style

Use small, focused components.

Prefer composition over large manager scripts.

Do not create god classes such as:
- `GameManager` doing everything
- `PlayerManager` doing everything
- `VehicleManager` doing everything
- `StoryManager` with all event logic

Use specific classes:
- `InputReader`
- `SimpleVehicleController`
- `VehicleLightsController`
- `VehicleSignalController`
- `RoadChunk`
- `RoadChunkSpawner`
- `InteractionDetector`
- `CampingManager`
- `CargoBay`
- `EventDirector`

Keep MonoBehaviours thin where possible.
Use ScriptableObjects for definitions/data:
- jobs
- delivery locations
- cargo definitions
- item definitions
- route definitions
- town definitions
- upgrade definitions

## Input requirements

Controller and keyboard/mouse must be supported.

Use separate input contexts/action maps:
- Driving
- OnFoot
- UI
- ActivityView

Early action list:
- Steer
- Accelerate
- BrakeReverse
- Look
- Interact
- ExitEnterVehicle
- ToggleHeadlights
- ToggleWipers
- LeftIndicator
- RightIndicator
- ToggleHazards
- OpenSatchel
- OpenBinder
- RadioNext
- RadioPrevious
- RadioToggle
- Pause

Do not hardcode keyboard-only input in gameplay scripts.
Gameplay scripts should read input through `InputReader` or equivalent abstraction.

## First milestone

Milestone 1 is not menus.

Milestone 1 is:

The player can sit in a first-person Żuk blockout and drive down a road made from streamed road chunks.

Required:
- Basic Żuk blockout support.
- First-person cockpit camera.
- Basic vehicle movement.
- Keyboard and controller input.
- Road straight prefab support.
- Road curve prefab support.
- Road chunks have entry/exit points.
- Road chunk spawner keeps chunks ahead and removes old chunks behind.
- Basic roadside environment sockets.
- Headlights toggle.
- Optional debug day/night toggle.

Avoid in Milestone 1:
- camping
- inventory
- delivery jobs
- cargo damage
- towns
- NPCs
- saving
- full menus
- spooky events

Build the road fantasy first.

## Road system

Roads are modular prefabs.

Each road chunk prefab should have:
- `RoadChunk` component
- `EntryPoint` transform
- `ExitPoint` transform
- road mesh/collider
- optional environment root
- optional socket root

Recommended hierarchy:

```text
road_forest_straight_01
  Mesh
  Colliders
  EntryPoint
  ExitPoint
  Environment
  Sockets
    SignSocket_01
    EventSocket_01
    TreeSocket_01
    LaybySocket_01
    SideRoadSocket_01
```

Roads are streamed/seeded route chunks.
Towns are handmade compact hubs.
Do not build one huge terrain as the main road world.

## Town system direction

Towns are small handmade areas, not procedural cities.

Town ingredients:
- shop row
- housing block / klatka entrances
- cigarette kiosk
- chapel/church
- bus stop
- poster pillar / job board
- mechanic/garage
- standalone cottages/farmhouses
- warm lights and neon signs

NPCs:
- max 5 outdoor NPCs during day
- 1 to 2 at night, often none
- day behaviour: walk to shop, café, window, klatka
- night behaviour: smoke on klatka steps, stand under awning, rare sightings

Do not build complex schedules early. Fake life convincingly.

## Vehicle direction

Vehicle is an FSC Żuk-inspired first-person van/truck.

Supported upgrades:
- covered rear
- cargo padding
- front bumper
- better tyres
- better radio

Do not add for now:
- roof rack
- larger fuel tank
- extra headlights
- suspension
- cabin heater
- straps
- better map
- better wipers
- spare tyre holder

Damage:
- flat tyre
- broken headlight
- cosmetic damage later

No engine/battery/suspension/cargo-bed simulation for now.

Vehicle needs states:
- Driving
- Stopped
- Parked
- Disabled
- Recovering

Exiting/camping should require parked state.

## Mirrors

Use a low-resolution mirror camera rendering to a RenderTexture for functional mirrors.

For horror events, mirrors may lie:
- overlay silhouettes
- fake headlights
- passenger-seat figure
- impossible reflections

Mirror event logic should not require the physical world to contain the thing shown in the mirror.

## Wipers

Use:
- animated wiper arms
- rain overlay on windscreen
- simplified cleared-mask effect
- visibility modifier

Do not simulate fluid realistically.

## Signals/hazards

Use a dedicated `VehicleSignalController`.

Indicators and hazards are mostly immersion early, but may later affect:
- roadside stops
- NPC car behaviour
- event triggers

## Visibility

Use a central `VisibilityManager`.

Visibility should combine modifiers from:
- time of day
- fog
- rain
- darkness
- headlights
- headlight damage
- wipers
- fatigue
- road lights
- event effects

Do not let multiple scripts independently fight over fog/camera visibility.

## Engine audio

Use a dedicated `EngineAudioController`, not the global `AudioManager`.

Early states:
- idle
- accelerating/load
- coasting
- braking/reverse later

No stalling for now.
Pedal and gear-stick animations can come later and should be driven from input/vehicle state.

## Inventory

The player has a satchel.

First pass:
- XMB-style horizontal inventory UI.
- Not literal PlayStation icons.
- Left/right item browsing.
- Focused item shows name, icon or 3D preview, slow rotating preview, and short description.

Later:
- physical animation of player looking into satchel.

Inventory layers:
1. Player satchel: small items.
2. Vehicle storage: large gear and tools.
3. Cargo bay: delivery cargo.
4. Active camp: deployed camping objects.

No grid inventory. No item Tetris.

## Camping

Camping kit is stored in a dedicated crate in the Żuk.

Camping only works in valid `CampingZone`s:
- lay-bys
- forest clearings
- lakeside pull-offs
- campsites
- quiet dirt-road clearings

Flow:
1. Park near camping zone.
2. Exit vehicle.
3. Retrieve camping kit from rear crate.
4. Carry kit into camping zone.
5. Hold/press setup input.
6. Fade to black briefly with setup sounds.
7. Spawn `CampSetup` prefab.
8. Stay first-person.
9. Use fixed cozy first-person activity views for coffee/cooking/sitting/sleeping.

Do not make manual tent peg placement early.

## Cargo

Cargo types:
- vegetable crate
- standard cardboard box
- fragile cardboard box
- mystery parcel

Staged implementation:
1. condition only, no physics
2. limited Rigidbody physics in cargo bay
3. upgrades affect damage/security

Covered rear prevents cargo falling out and protects from rain.
Cargo padding reduces damage.
No straps.

Cargo can have:
- condition percent
- weight
- fragility
- destination
- damage rules

## Delivery / jobs

No normal job deadlines.
Deadlines hurt the cozy driving identity.

Job states:
- Available
- Accepted
- CargoCollected
- InTransit
- Delivered
- Completed
- Failed
- Cancelled

Failure only from:
- destroyed cargo
- story-specific conditions
- deliberate abandonment

Use ScriptableObjects for delivery locations and job definitions.

Pickup/drop-off signalling should avoid floating markers.
Use:
- warm lit entrance
- neon/window sign
- polaroid clue
- loading bay markings
- shop signage
- landmark hints

## Events

Support both one-off events and chain events.

Event awareness is hidden, not a quest log.

Awareness states:
- Unseen
- Seen
- Ignored
- Approached
- Acknowledged
- Resolved
- Escalated

Reflect awareness through:
- binder notes
- radio mentions
- Polaroids
- receipts
- NPC rumours
- strange changes in future events

## Saving/checkpoints

Early rule:
- save at accepting job
- save at completing job
- later maybe save at sleeping/camping or entering/leaving towns

If player quits mid-route, reload at last town/checkpoint.
Transient roadside props do not need exact persistence unless event-chain relevant.

## Rescue / stuck states

Player must not be permanently softlocked.

Rescue layers:
1. self-rescue: spare tyre, walking, hazards, resting until morning
2. roadside help: NPC car/farmer/mechanic
3. emergency recovery: fade to nearest known town/garage with penalty

Getting stuck should create stories, not rage quits.

## Money / shops

Use a simple `PlayerWallet`.
Optional transaction ledger later.

Shop stock refreshes by day index, usually after sleep/day transition.

Basic supplies should always be available.
Rare items can depend on story progress or refresh policy.

## Road surfaces

Road chunks or surface zones should provide `RoadSurfaceData`.

Surface types:
- tarmac
- dirt
- gravel
- mud
- wet tarmac

Surface affects:
- grip
- puncture chance
- road noise
- possibly cargo movement

## Debug tools

Early development should include debug controls for:
- spawn next road
- toggle day/night
- toggle rain/fog
- damage tyre
- damage headlight
- teleport to town
- trigger event
- add cargo
- set fatigue level

Put debug tools behind a clear debug component or dev-only UI.

## Testing / verification

For every task, state how it was verified.

Examples:
- Play Mode test in `PrototypeDrive`
- road chunks connect without visible gap
- controller input works
- keyboard input works
- headlights toggle
- vehicle can drive for 60 seconds without errors
- no console errors introduced

If automated tests are not available, provide manual verification steps.

## Git / PR expectations

Make small focused commits.

Do not mix unrelated systems in one change.
Do not reformat unrelated files.
Do not rename folders/files unless required.
Do not delete `.meta` files manually.
Preserve Unity `.meta` files with assets.

Before final response, summarise:
- files changed
- what was added
- how to test it
- known limitations

## Hard boundaries

Do not implement the whole game in one task.
Do not add towns, camping, cargo, saving, and events all at once.
Do not introduce large frameworks without asking.
Do not add paid/store assets.
Do not use copyrighted music, branding, or logos.
Do not clone Easy Delivery Co.’s exact assets, UI, layout, story, or style.
Use original assets and original identity.

When unsure, choose the smallest implementation that supports the next milestone.
