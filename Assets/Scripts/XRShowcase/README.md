# XR Showcase Scripts

This folder contains small XR examples. Each script is meant to be easy to inspect, reuse, and modify.

## Included Scripts

- `XRButtonLogger`
- `XRHapticPulseExample`
- `XRFlashlightToggle`
- `XRMaterialCycler`
- `XRSpawnPrefabOnButton`
- `XRScaleObjectWithThumbstick`
- `XRRotateObjectWithThumbstick`

## Recommended Input Actions

Create these actions in your XR input asset:

- `Trigger`
- `Grip`
- `PrimaryButton`
- `SecondaryButton`
- `Thumbstick`
- `ThumbstickClick`

Recommended action types:

- `Trigger`: `Value` or `Button`
- `Grip`: `Value` or `Button`
- `PrimaryButton`: `Button`
- `SecondaryButton`: `Button`
- `Thumbstick`: `Value (Vector2)`
- `ThumbstickClick`: `Button`

For Quest-style controllers:

- Left `primaryButton` = `X`
- Left `secondaryButton` = `Y`
- Right `primaryButton` = `A`
- Right `secondaryButton` = `B`

Typical Unity XR paths:

- `<XRController>{LeftHand}/primaryButton`
- `<XRController>{LeftHand}/secondaryButton`
- `<XRController>{RightHand}/primaryButton`
- `<XRController>{RightHand}/secondaryButton`

## General Workflow

1. Add one script to a GameObject.
2. Assign the needed `InputActionReference` fields.
3. Assign the target object, light, renderer, prefab, or transform.
4. Enter Play Mode and test in the XR Device Simulator or on a headset.

If nothing happens, check:

- The action is assigned.
- The action type matches the script.
- The action is bound to the expected controller input.
- The target field is assigned.

## Script Guide

### XRButtonLogger

Use this first when testing controller input.

Setup:

1. Add `XRButtonLogger` to a controller helper object.
2. Set the controller label.
3. Assign any of these actions:
- `Trigger Action`
- `Grip Action`
- `Primary Button Action`
- `Secondary Button Action`
- `Thumbstick Action`
- `Thumbstick Click Action`

What it shows:

- Button press and release messages
- Trigger and grip values
- Thumbstick direction in the Console

### XRHapticPulseExample

Use this to add controller vibration.

Setup:

1. Add `XRHapticPulseExample` to a GameObject.
2. Assign `Pulse Action`.
3. Set `Controller Node` to `Left Hand` or `Right Hand`.
4. Adjust `Amplitude` and `Duration`.

Tip:

- Start with small values like `0.3` amplitude and `0.1` duration.

### XRFlashlightToggle

Use this to switch a light or visible object on and off.

Setup:

1. Add `XRFlashlightToggle`.
2. Assign `Toggle Action`.
3. Assign `Target Light`, `Target Object`, or both.
4. Set `Start Enabled`.

### XRMaterialCycler

Use this to cycle through a list of materials.

Setup:

1. Add `XRMaterialCycler`.
2. Assign `Cycle Action`.
3. Add one or more renderers to `Target Renderers`.
4. Add materials to the `Materials` list.

Tip:

- Leave `Use Shared Materials` off for student experiments.

### XRSpawnPrefabOnButton

Use this to spawn a prefab from a controller or a fixed point.

Setup:

1. Add `XRSpawnPrefabOnButton`.
2. Assign `Spawn Action`.
3. Assign `Prefab To Spawn`.
4. Assign `Spawn Point`.
5. Optionally assign `Parent For Spawned Objects`.

### XRScaleObjectWithThumbstick

Use this to scale an object with thumbstick up and down.

Setup:

1. Add `XRScaleObjectWithThumbstick`.
2. Assign `Thumbstick Action`.
3. Assign `Target`.
4. Set `Scale Speed`, `Min Scale`, and `Max Scale`.

Tip:

- `Scale Uniformly` is the easiest mode for beginners.

### XRRotateObjectWithThumbstick

Use this to rotate an object with thumbstick left and right.

Setup:

1. Add `XRRotateObjectWithThumbstick`.
2. Assign `Thumbstick Action`.
3. Assign `Target`.
4. Set `Rotation Axis` and `Rotation Speed`.

Tip:

- `Vector3.up` is a good default axis for classroom demos.

## Suggested Lesson Order

1. `XRButtonLogger`
2. `XRHapticPulseExample`
3. `XRFlashlightToggle`
4. `XRMaterialCycler`
5. `XRSpawnPrefabOnButton`
6. `XRScaleObjectWithThumbstick`
7. `XRRotateObjectWithThumbstick`

