# Corals & Seaweed Pack --- Documentation

## Overview

**Corals & Seaweed Pack** is a collection of decorative underwater
environment assets designed for ocean, reef, and seabed scenes. The pack
includes coral reef models, underwater vegetation, and an animated
seaweed shader that adds natural underwater motion.

All assets are optimized and ready for immediate use. A preview scene
and bubble VFX are included to demonstrate how the assets can be used in
a complete underwater environment.

------------------------------------------------------------------------

## Package Contents

-   9 decorative underwater models (corals, shells, starfish and
    vegetation)
-   Prefabs for each asset
-   LODs configured for every model
-   HDRP-ready materials
-   Animated seaweed shader
-   Underwater preview scene
-   Bubble particle VFX

------------------------------------------------------------------------

## Polygon Count

  Asset               Triangles
  ------------------- -----------
  Each model (LOD0)   \~1,500
  Lower LODs          Included

Each prefab contains a configured **LOD Group** for performance
optimization.

------------------------------------------------------------------------

## Render Pipeline Compatibility

  Pipeline      Supported
  ------------- ------------------------------------
  HDRP          ✔ Fully Supported (Primary Target)
  URP           ✖ Not Included
  Built-in RP   ✖ Not Included

------------------------------------------------------------------------

## Unity Version

Tested with:

**Unity 2022.3 LTS and newer (HDRP)**

Older versions may work but are not officially supported.

------------------------------------------------------------------------

## Installation

1.  Import the `.unitypackage` into your project.
2.  Ensure the project uses **HDRP**.
3.  Open:

`Assets/RSG_UnderWater_Pack/`

4.  Open the preview scene or drag prefabs into your environment scene.

No additional setup required.

------------------------------------------------------------------------

## Using the Assets

Prefab location:

`Assets/RSG_UnderWater_Pack/FPS/HDRP/Prefabs`

Simply drag any coral or vegetation prefab into your scene and adjust
scale and rotation as needed.

------------------------------------------------------------------------

## Animated Seaweed Shader

The pack includes a material with a built-in animation shader that
simulates underwater plant movement.

### Features

-   Vertex-based swaying animation
-   Adjustable movement speed
-   Adjustable movement strength
-   Suitable for underwater environments

### Adjusting the Animation

Select the seaweed material and modify the shader parameters: -
Animation Speed - Sway Strength - Wind/Direction

No scripting is required.

------------------------------------------------------------------------

## Materials & Textures

All materials use **HDRP Lit Shader**.

### Included Texture Maps

-   Base Color (Albedo)
-   Normal Map
-   Mask Map (AO, Metallic, Smoothness)

All textures are already configured with proper import settings.

------------------------------------------------------------------------

## Preview Scene

The package includes a demonstration scene showing: - Asset placement
examples - Lighting setup - Underwater atmosphere - Bubble VFX usage

This scene is intended as a usage reference and starting point for your
own environment.

------------------------------------------------------------------------

## Optimization

Designed for real-time environments.

Features: - Low polygon count - LOD system - Shared materials -
Efficient textures

Recommended usage: - Ocean floors - Reef environments - Underwater
caves - Coastal scenes

------------------------------------------------------------------------

## Important Notes / Limitations

-   Static environment assets
-   No physics interactions
-   No gameplay scripts included
-   Designed primarily for PC/Console HDRP projects

------------------------------------------------------------------------

## Customization

You can: - Re-scale prefabs - Recolor materials - Adjust seaweed
animation - Combine assets to create larger reefs

------------------------------------------------------------------------

## Folder Structure

    RSG_UnderWater_Pack
    │
    ├── FPS
    │   └── HDRP
    │       ├── Materials
    │       ├── Models
    │       ├── Prefabs
    │       ├── Scenes
    │       ├── Shaders
    │       └── Textures

------------------------------------------------------------------------

## Performance Tips

-   Use GPU Instancing
-   Use occlusion culling
-   Combine multiple assets into clusters
-   Use underwater fog volumes

------------------------------------------------------------------------

## Support

**Publisher:** RSG\
**Email:** your@email.com\
**Response Time:** 24--72 hours

Please include: - Unity version - HDRP version - Description of the
issue - Screenshot if possible

------------------------------------------------------------------------

## License

This asset is licensed under the **Unity Asset Store EULA**. You may use
it in commercial and non-commercial projects. Redistribution of the raw
files is not permitted.

------------------------------------------------------------------------

## Credits

All models, textures, and shaders were created by the publisher.
