# AssetStudioMod

[![Release](https://img.shields.io/github/v/release/aelurum/AssetStudio?color=blue)](https://github.com/aelurum/AssetStudio/releases/latest) [![Downloads](https://img.shields.io/github/downloads/aelurum/AssetStudio/total?color=blue)](https://github.com/aelurum/AssetStudio/releases/latest) [![Download latest release](https://img.shields.io/badge/Download_latest_release-blue)](https://github.com/aelurum/AssetStudio/releases/latest)

[![Build status](https://ci.appveyor.com/api/projects/status/5qyai0hqs0ktyara/branch/AssetStudioMod?svg=true)](https://ci.appveyor.com/project/aelurum/assetstudiomod/branch/AssetStudioMod) [![Download latest build](https://img.shields.io/badge/Download_latest_build-brightgreen)](https://ci.appveyor.com/project/aelurum/assetstudiomod/branch/AssetStudioMod/artifacts)

**AssetStudioMod** - modified version of Perfare's [AssetStudio](https://github.com/Perfare/AssetStudio), mainly focused on UI optimization and some functionality enhancements.

**Neither the repository, nor the tool, nor the author of the tool, nor the author of the modification is affiliated with, sponsored, or authorized by Unity Technologies or its affiliates.**

## Game specific modifications

- [ArknightsStudio](https://github.com/aelurum/AssetStudio/tree/ArknightsStudio)

## AssetStudio Features

- Support Unity version:
  - 1.2 - 7000.0
- Support Tuanjie version (partial):
  - 1.0 - 1.10
- Support asset types:
  - **Texture2D**, **Texture2DArray** : convert to png, tga, jpeg, bmp, webp
  - **Sprite** : crop Texture2D to png, tga, jpeg, bmp, webp
  - **AudioClip** : mp3, ogg, wav, m4a, fsb. Support converting FSB file to WAV(PCM)
  - **Font** : ttf, otf
  - **Mesh** : obj
  - **TextAsset**
  - **Shader** (for Unity < 2021)
  - **MovieTexture**
  - **VideoClip**
  - **MonoBehaviour** : json
  - **Animator** : export to FBX file with bound AnimationClip
 
## AssetStudioMod Features

- CLI version (for Windows, Linux, Mac)
- Support of sprites with alpha mask
- Support of image export in WebP format
- Support of Live2D Cubism model export
   - Ported from my fork of Perfare's [UnityLive2DExtractor](https://github.com/aelurum/UnityLive2DExtractor)
- Support of swizzled Switch textures
    - Ported from nesrak1's [AssetStudio fork](https://github.com/nesrak1/AssetStudio/tree/switch-tex-deswizzle)
- Detecting bundles with UnityCN encryption
   - Detection only. If you want to open them, please use Razmoth's [Studio](https://github.com/RazTools/Studio) or Escartem's [AnimeStudio](https://github.com/Escartem/AnimeStudio)
- Some UI optimizations and bug fixes (See [CHANGELOG](https://github.com/aelurum/AssetStudio/blob/AssetStudioMod/CHANGELOG.md) for details)

## Requirements

- AssetStudioMod.net472
   - GUI/CLI - [.NET Framework 4.7.2](https://dotnet.microsoft.com/download/dotnet-framework/net472)
- AssetStudioMod.net9
   - GUI/CLI (Windows) - [.NET Desktop Runtime 9.0](https://dotnet.microsoft.com/download/dotnet/9.0)
   - CLI (Linux/Mac) - [.NET Runtime 9.0](https://dotnet.microsoft.com/download/dotnet/9.0)
- AssetStudioMod.net10
   - GUI/CLI (Windows) - [.NET Desktop Runtime 10.0](https://dotnet.microsoft.com/download/dotnet/10.0)
   - CLI (Linux/Mac) - [.NET Runtime 10.0](https://dotnet.microsoft.com/download/dotnet/10.0)

## GUI Usage

### Load Assets/AssetBundles

Use **File->Load file** or **File->Load folder**.

When AssetStudio loads AssetBundles, it decompresses and reads it directly in memory, which may cause a large amount of memory to be used. You can use **File->Extract file** or **File->Extract folder** to extract AssetBundles to another folder, and then read.

### Extract/Decompress AssetBundles

Use **File->Extract file** or **File->Extract folder**.

### Export Assets, Live2D Models

Use **Export** menu.

### Export 3D Models (FBX)

Export model from "Scene Hierarchy" using the **Model** menu.

Export Animator from "Asset List" using the **Export** menu.

#### With AnimationClip

Select model from "Scene Hierarchy" then select the AnimationClip from "Asset List", using **Model->Export selected objects with AnimationClip** to export.

Export Animator will export bound AnimationClip or use **Ctrl** to select Animator and AnimationClip from "Asset List", using **Export->Export Animator with selected AnimationClip** to export.

### Export MonoBehaviour

When you select an asset of the MonoBehaviour type for the first time, AssetStudio will ask you the directory where the assembly is located, please select the directory where the assembly is located, such as the `Managed` folder.

#### For Il2Cpp

First, use [Il2CppDumper](https://github.com/Perfare/Il2CppDumper) (or [Il2CppInspectorRedux](https://github.com/LukeFZ/Il2CppInspectorRedux) / [AtiDev17/Il2CppDumper](https://github.com/AtiDev17/Il2CppDumper)) to generate dummy dll, then when using AssetStudio to select the assembly directory, select the dummy dll folder.

## CLI Usage

You can read CLI readme [here](https://github.com/aelurum/AssetStudio/blob/AssetStudioMod/AssetStudioCLI/ReadMe.md).

### Run

- Command-line: `AssetStudioModCLI <asset folder path>`
- Command-line for Portable versions (.NET 6+): `dotnet AssetStudioModCLI.dll <asset folder path>`

### Basic Samples

- Show a list with a number of assets of each type available for export
```
AssetStudioModCLI <asset folder path> -m info
```
- Export assets of all supported for export types
```
AssetStudioModCLI <asset folder path>
```
- Export assets of specific types
```
AssetStudioModCLI <asset folder path> -t tex2d
```
```
AssetStudioModCLI <asset folder path> -t tex2d,sprite,audio
```
- Export assets grouped by type
```
AssetStudioModCLI <asset folder path> -g type
```
- Export assets to a specified output folder
```
AssetStudioModCLI <asset folder path> -o <output folder path>
```
- Dump assets to a specified output folder
```
AssetStudioModCLI <asset folder path> -m dump -o <output folder path>
```
- Export Live2D Cubism models
```
AssetStudioModCLI <asset folder path> -m live2d
```
> When running in live2d mode, the only filter option supported is `--filter-by-name`.
- Export all FBX objects (similar to "Export all objects (split)" option in the GUI)
```
AssetStudioModCLI <asset folder path> -m splitObjects
```
> When running in splitObjects mode, the only filter option supported is `--filter-by-name`.
- Export Animator assets
```
AssetStudioModCLI <asset folder path> -m animator
```

### Advanced Samples
- Export image assets converted to webp format to a specified output folder
```
AssetStudioModCLI <asset folder path> -o <output folder path> -t sprite,tex2d --image-format webp
```
- Show the number of audio assets that have "voice" in their names
```
AssetStudioModCLI <asset folder path> -m info -t audio --filter-by-name voice
```
- Export audio assets that have "voice" in their names
```
AssetStudioModCLI <asset folder path> -t audio --filter-by-name voice
```
- Export audio assets that have "music" or "voice" in their names
```
AssetStudioModCLI <asset folder path> -t audio --filter-by-name music,voice
```
```
AssetStudioModCLI <asset folder path> -t audio --filter-by-name music --filter-by-name voice
```
- Export audio assets that have "char" in their names **or** containers
```
AssetStudioModCLI <asset folder path> -t audio --filter-by-text char
```
- Export audio assets that have "voice" in their names **and** "char" in their containers
```
AssetStudioModCLI <asset folder path> -t audio --filter-by-name voice --filter-by-container char
```
- Export FBX objects that have "model" or "scene" in their names and set the scale factor to 10
```
AssetStudioModCLI <asset folder path> -m splitObjects --filter-by-name model,scene --fbx-scale-factor 10
```
- Export MonoBehaviour assets that require an assembly folder to read and create a log file
```
AssetStudioModCLI <asset folder path> -t monobehaviour --assembly-folder <assembly folder path> --log-output both
```
- Export assets that require to specify a Unity version
```
AssetStudioModCLI <asset folder path> --unity-version 2022.3.2f1
```
- Load assets of all types and show them (similar to "Display all assets" option in the GUI)
```
AssetStudioModCLI <asset folder path> -m info --load-all
```
- Load assets of all types and dump Material assets
```
AssetStudioModCLI <asset folder path> -m dump -t material --load-all
```

## Build

* Visual Studio 2026 or newer
* **AssetStudioFBXNative** uses [FBX SDK 2020.2.1](https://www.autodesk.com/developer-network/platform-technologies/fbx-sdk-2020-2-1), before building, you need to install the FBX SDK and modify the project file, change include directory and library directory to point to the FBX SDK directory

## Open source libraries used

### Texture2DDecoder
* [Ishotihadus/mikunyan](https://github.com/Ishotihadus/mikunyan)
* [BinomialLLC/crunch](https://github.com/BinomialLLC/crunch)
* [Unity-Technologies/crunch](https://github.com/Unity-Technologies/crunch/tree/unity)

### LZMA compression
* [7-zip/sdk](https://www.7-zip.org/sdk.html)

### Oodle compression
* [zao/ooz](https://github.com/zao/ooz)
