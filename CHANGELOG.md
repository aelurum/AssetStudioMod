# Changelog

## v0.20.0.0 [01-11-2026]
#### New features
- Improved Unity 6+ support (up to v7000.0)
- Improved Tuanjie support (up to v1.10) (still partial)
    - Improved auto-detection of Tuanjie meshes
    - [GUI] Added option to specify parsing mode of Tuanjie meshes
- Added support for legacy Unity versions (https://github.com/aelurum/AssetStudioMod/issues/103)
- Added JsonDump option
- Added support for parsing `MeshRenderer` and `SkinnedMeshRenderer` via typetree
- Added option to export images in "assetName@hash" name format
- Added option to dump material assets during model export (CLI flag: `--fbx-dump-materials`)
- Added option to keep original sprite sizes (https://github.com/aelurum/AssetStudioMod/issues/107; https://github.com/aelurum/AssetStudioMod/pull/115)
    - Enabled by default
    - [CLI] Added `--sprite-cropping` option to the Advanced options section
    - [GUI] Added "Keep original sprite sizes" option to the Export options
    - [GUI] Added `Ctrl + R` hotkey to change sprite rendering mode
    - [GUI] Added `Ctrl + B` hotkey to display sprite border
- Added option to extract raw byte array from MonoBehaviour assets (CLI flag: `--raw-array`)
- [GUI] Added "Studio options" window:
    - Added option to specify a custom cache directory
    - Renamed "Use lazy loading for Mesh assets" to "Pre-process Mesh assets" and moved to Studio options
- [GUI] Replaced "Import options" with "Game-specific options" window
- [GUI] Reworked "Export options" window
- [GUI] Added Unity version label to status strip
- [CLI] Added flag to allow removing container path prefix (`--strip-path-prefix`) (https://github.com/aelurum/AssetStudioMod/pull/113)
- [CLI] Added flag to allow filtering assets in blacklist mode (`--filter-exclude-mode`) (https://github.com/aelurum/AssetStudioMod/pull/113)
- [CLI] Added flag to allow exporting models in FBX ASCII format (`--fbx-ascii-format`) (https://github.com/aelurum/AssetStudioMod/pull/124)
- [CLI] Added support for `--fbx-uvs-as-diffuse` option in `--mode animator` (https://github.com/aelurum/AssetStudioMod/pull/124)

#### Fixes
- Improved normal maps support (https://github.com/aelurum/AssetStudioMod/issues/57)
- Fixed rendering of some packed sprites (https://github.com/aelurum/AssetStudioMod/issues/104)
- Fixed an error in case of unsupported sprites (https://github.com/aelurum/AssetStudioMod/issues/109)
- Fixed a bug in the bundle block reader (https://github.com/aelurum/AssetStudioMod/issues/119)
- Fixed null reference exception when bone frame is not found (https://github.com/aelurum/AssetStudioMod/pull/129)
- Fixed export interruption due to an error during mesh processing (https://github.com/aelurum/AssetStudioMod/issues/134)
- Fixed an error during parsing typetree data of Font assets (https://github.com/aelurum/AssetStudioMod/issues/137)
- Fixed m_IndexBuffer decompression when m_IndexFormat != 0 (https://github.com/aelurum/AssetStudioMod/issues/147)
- Fixed fbx export of some old models (regression v0.19.0)
- Fixed parallel export from a single stream
- Fixed some bugs with invalid names during export
- Changed cache logic:
    - Single cache file instead of separate cache files for each bundle
    - Program folder as a default cache directory
- Disabled ANSI codes support for console in Wine
- [GUI] Improved multi-monitor & highDpi support:
    - Improved multi-monitor support (.net builds)
    - Disabled dpi awareness for net472 build, via app.manifest, for better compatibility (with Wine as well)
- [CLI] Fixed `--mode animator` not exporting AnimationClips with `--fbx-animation all` (https://github.com/aelurum/AssetStudioMod/pull/124)

#### Live2D Changes
- Added support for Cubism 5.3 models
- Added option to filter Live2D textures when the "Search assets by filename" option is used
    - [CLI] Added `--l2d-filter-textures` flag to the Live2D options section
- Improved detection of Live2D models (https://github.com/aelurum/AssetStudioMod/issues/122)
    - Added an alternative way of model detection when a MonoScript asset is not loaded
- Fixed Live2D motion converting (https://github.com/aelurum/AssetStudioMod/issues/150)
- Fixed BlendType enum
- [exp3.json] Fixed format of Blend values (there should be strings according to the official specification)
- Improved support for Live2D physics and fade motion assets
- Disabled warnings from Live2D motion converter in non-debugging mode
- [CLI] Renamed "--l2d-force-bezier" to "--l2d-smooth-motions"

#### Other Changes
- Added support for swizzled rgba64 textures
- Changed display format of large byte arrays to base64 string
- Improved Dark mode support for .net10
- Improved AudioClip support
- [GUI] Improved right-click menu options for dump tree view
- [GUI] Improved lzma progress report
- Made some other minor fixes and improvements


## v0.19.0.0 [04-09-2025]
#### Breaking Changes
- Many asset fields of array type have been replaced with list type *(aka fixed the cause of most memory leaks)* ([35b2499](https://github.com/aelurum/AssetStudio/commit/35b24990c6a5d99c15b661578cf9da26aefe8d1b))

#### New features
- Added Unity 6 support
- Added support for Tuanjie 1.0 - 1.6 (partial)
    - Support for some Tuanjie-specific features, such as web streaming of texture data/virtual geometry meshes/acl clips, is not implemented yet
- Added multiBundle support (a single file with multiple asset bundles inside)
    - *Which also includes support for fake headers*
- Added options to control exported UV types
    - [GUI] Added option for manual binding of UV map types
    - [CLI] Added flag to export all UVs as diffuse maps
- [GUI] Added Dark mode support for .NET 9 (wip) (https://github.com/aelurum/AssetStudio/issues/9)
- [GUI] Added option to use lazy loading for `Mesh` assets
- [GUI] Added option to use tree view in the Dump tab
- [GUI] Added option to autoplay `AudioClip` assets
- [CLI] Added mode to export `Animator` assets
    - Added option to choose animation export mode (`--fbx-animation`)
- [CLI] Added mode to Extract/Decompress asset bundles (https://github.com/aelurum/AssetStudio/issues/62)
- [CLI] Added support for multiple input paths (separated by `,` or `;`)

#### Fixes
- Fixed support for `Mesh` assets with omitted weight values (https://github.com/aelurum/AssetStudio/issues/21)
- Fixed audio preview for some fmod formats (https://github.com/aelurum/AssetStudio/issues/53)
- Fixed bundle blocks read failed with ArchiveFlags.BlocksInfoAtTheEnd (https://github.com/aelurum/AssetStudio/pull/44)
- Fixed a bug that caused audio preview volume to reset when selecting an asset
- Fixed support of `Sprite` assets from Unity < 5.2
- Fixed support of `Sprite` assets with wrong `SpriteAtlas` pathID
- Fixed parsing of `AnimationClip` assets via typetree for Unity versions < 5
- Fixed calculation of totalPointCount and totalSegmentCount in Live2D motions (https://github.com/Live2D/CubismNativeFramework/pull/57)
- [GUI] Fixed displayed info for non-fmod audio clips from Unity < 5
- [CLI] Fixed `Mesh` export for some region formats (https://github.com/aelurum/AssetStudio/issues/38)

#### Other Changes
- Replaced Newtonsoft.Json with System.Text.Json for asset typetree serialization/deserialization
- Disabled animation converting if animation export is disabled in the options
- Added support for assets/bundles with obfuscated Unity version
- Added support of loading `Material` assets using their typetree
- Added Oodle compression support (unofficial)
- Improved `Animator` export in cases where `MeshRenderer` is missing a `Mesh` reference (https://github.com/aelurum/AssetStudio/pull/40)
- Improved support for some older Unity versions (< 3.0)
- Improved support of zipped files
- Improved ExportRaw option: External data will also be added to exported assets
- Updated UnityCN encryption detection method
- Updated bundle reader
    - Replaced creation of a duplicated file/memory stream with OffsetStream
    - Added separate processing of uncompressed bundles (including streamed bundles). They will be read directly from disk
    - Added progress report on LZMA decompression process
- Updated asset export logic
    - Disabled saving files with a unique id if they already exist. A unique id will only be added to files with identical names during export
    - Added option to overwrite existing files (`-r`, `--overwrite-existing` flag in CLI)
    - Fixed support of multiple model export using "Export selected objects (split)" option (https://github.com/aelurum/AssetStudio/issues/43)
- Improved integration with Live2D assets
    - Fixed Live2D model export for bundles with multiple models inside    
    - Added support of exporting Live2D poses (pose3.json)
    - Added support of grouping exported models by model name
    - Added container-independent method for searching `AnimationClip` assets
    > *However, it's not really universal, so it cannot completely replace the container-dependent method*
    - [GUI] Added ability to filter Live2D model assets using "Filter Type"
    - [CLI] Added name filtering support for Live2D export mode
- FMOD updated to v2.03.06
    - Added native libs for linux-arm64, win-arm64
- [GUI] AudioClip improvements:
    - Increased loading speed of `AudioClip` preview
    - Optimized memory consumption of `AudioClip` preview
    - Fixed incorrect length detection for some sound types
    - Added `AudioClip` loop point display (https://github.com/aelurum/AssetStudio/pull/37)
    - Added channel count info (audio channels)
- [GUI] Reworked some import options
    - Added feature to load and save import options to a file (*import options include Unity version*)
    - Added option to always decompress bundles to disk (related issue: https://github.com/aelurum/AssetStudio/issues/58)
- [GUI] Added exact search option for Scene Hierarchy (https://github.com/aelurum/AssetStudio/issues/49)
- [CLI] Added flag to always decompress bundles to disk (`--decompress-to-disk`)
- [CLI] Added flag to allow filtering assets using regex (`--filter-with-regex`) (https://github.com/aelurum/AssetStudio/issues/45)
- [CLI] The `--avoid-typetree-loading` flag replaced with `--ignore-typetree`
- Made some other minor fixes and improvements

## v0.18.0.0 [04-04-2024]
#### Breaking Changes
- Structure of the AnimationClip class has been changed a bit to match the structure of its type tree (`m_Clip = animationClip.m_MuscleClip.m_Clip` -> `m_Clip = animationClip.m_MuscleClip.m_Clip.data`)
- Types of Unity version fields have been changed from `int[]` to the `UnityVersion` class

#### New features
- Added option to export assets with PathID in filename (https://github.com/aelurum/AssetStudio/issues/25)
- Added support for swizzled Switch textures:
    - Ported from nesrak1's fork (https://github.com/nesrak1/AssetStudio/tree/switch-tex-deswizzle)
- Added support for `Texture2DArray` assets:
    - [GUI] Assets of fake asset type `Texture2DArrayImage` will be generated, to make it easier to work with images from array in the GUI
- Added support for assets with Zstd block compression:
    - Implemented as one of the option for custom compression type (5). Selected by default.
    - [GUI] "Options" -> "Custom compression type"
    - [CLI] "--custom-compression"
- Added support of Texture2D assets from Unity 2023.2+
- Added support of parallel asset export
    - It's also possible to specify the number of parallel tasks for export in both the GUI and CLI (the higher the number of parallel tasks, the more RAM may be needed for exporting)
- Added support for parsing assets using their type tree for some asset types (`Texture2D`, `Texture2DArray`, `AnimationClip`) (beta):
  > only suitable for asset bundles which contain typetree info

#### Fixes
- [GUI] Fixed compatibility with High Contrast modes
- Fixed Live2D export error due to wrong Blend type in Live2D expression parser
- Fixed AssetBundle structure for Unity v5.4.x (https://github.com/aelurum/AssetStudio/issues/31)
- Fixed loading of some Unity 2019.4 assets

#### Other changes
- [GUI] Preserve selection order of `AnimationClip` assets (https://github.com/aelurum/AssetStudio/issues/24)
- Improved integration with Live2D assets:
    - Improved export method of AnimationClip motions
    - Added support for generation of cdi3.json (beta)
    - [GUI] Added display of model info on the preview tab
    - [GUI] Added support for partial export:
        - selected models
        - model + selected AnimationClip motions
        - model + selected Fade motions
        - model + selected Fade Motion List
- Add more options to work with Scene Hierarchy: (https://github.com/aelurum/AssetStudio/issues/23)
    - Added option to group exported assets by node path in scene hierarchy
    - Added field with node path to exported xml asset list
- [CLI] Added colors to help message
- Changed Dump function to show/export object dump if type tree dump is not available
- Added more displayed information for non-fmod audio clips
- Added display of asset bundle's unity version in cases where asset's unity version is stripped but the asset bundle's unity version is not

## v0.17.4.0 [16-12-2023]
- Added support for Live2D Fade motions
    - [GUI] Added related settings to the Export Options window
    - [CLI] Added options section with related settings
- Added support for separate PreloadData (https://github.com/Perfare/AssetStudio/issues/690)
- [GUI] Added support for .lnk files via Drag&Drop (https://github.com/aelurum/AssetStudio/issues/13)
- [GUI] Added support for Drag&Drop files from a browser
- [GUI] Added console logger
    - Added option to save log to a file
- [GUI] Added option to not build a tree structure
- Made some other minor fixes and improvements

## v0.17.3.0 [13-09-2023]
- [CLI] Added support for exporting split objects (fbx) (https://github.com/aelurum/AssetStudio/pull/10)
- [CLI] Fixed display of asset names in the exported asset list in some working modes
- [CLI] Fixed a bug where the default output folder might not exist
- Added support of Texture2D assets from Unity 2022.2+
- Fixed AssemblyLoader (https://github.com/aelurum/AssetStudio/issues/6)
- [CLI] Added --load-all flag to load assets of all types
- [CLI] Improved option grouping on the help screen

## v0.17.2.0 [27-08-2023]
- [GUI] Improved Scene Hierarchy tab
   - Added "Related assets" item to the context menu (https://github.com/aelurum/AssetStudio/issues/7)
- [GUI] Added app.manifest for net472 build 
   - Added long paths support (win10 v1607+)
   - Fixed blurring at high DPI with scaling
- [CLI] Fixed sprite export in sprite only mode
- Made some changes to motion list for live2d models
   - Motion list is now sorted
   - Motions divided into groups (each motion is a separate group)
   - Motion names are used as group names
- Updated dependencies
- Made some other minor fixes and improvements

## v0.17.1.0 [12-07-2023]
#### Breaking Changes
- With the drag&drop fix (https://github.com/aelurum/AssetStudio/commit/2f8f57c1a63893c0b0d2a55349d6cb6d8f8a5a3b), functions `LoadFiles` and `LoadFolder` in AssetsManager have been replaced with one universal function `LoadFilesAndFolders`

#### Changes
- Fixed Texture2DDecoderNative compatibility issue with Linux/macOS (CLI preparation #1)
- Changed image library to ImageSharp (CLI preparation #2)
- Added support for sprites with alpha mask
   - Sprites with alpha mask can now be viewed and exported with transparency
   - Added hotkeys to control display of an alpha mask on the preview tab
   - Added an option to the export settings to enable/disable export with alpha mask as well
   - Prevented texture2D preview options from being changed with hotkeys outside of texture preview (e.g. when some other asset is selected)
- Added image export in WebP format
- Updated FMOD to 0.2.0.22 (CLI preparation #3)
- Added progress info about zip(apk) file loading process
- Added CLI version
- [GUI] Added context menu with "Select all", "Clear selection", "Expand all" and "Collapse all" options to the "Scene Hierarchy" tab
   - Selected objects count is now displayed in the status bar
- [GUI] Improved error handling
- [CLI] Added support for partial assets reading
- [GUI] Added some videoClip info to preview tab
- [GUI] Improved memory usage of image previews
- Disabled Shader support for Unity > 2020
- Added error message for bundles with UnityCN encryption
- Added error message on incorrect format of specified Unity version
- Block alignment fix for Unity 2019.4.X (source: https://github.com/K0lb3/UnityPy/commit/10346b4f02f2dbe0fa707799130c9f83c24f8e24)
- [GUI] Added "About" window
- Fixed cutout glitch in some packed sprites (https://github.com/Perfare/AssetStudio/issues/1015)
- Optimized drawing performance of packed sprites
- [GUI] Improved asset list filtering 
   - Added filter history
   - Added more filtering modes: Include, Exclude, Regex (Name/Container)
- Added grouping option with full container path (https://github.com/Perfare/AssetStudio/issues/815)
   - [GUI] - "container path full (with name)"
   - [CLI] - "containerFull"
- Improved "Restore TextAsset extension name" option 
   - If checked, AssetStudio will first try to find an extension in an asset's name and only then in its container. If no extension is found, ".txt" will be used
- [GUI] Fixed audio player position in maximized window
- [GUI] Improved file and folder loading (drag&drop) 
   - Added support for drag&drop of multiple folders
   - Open/Export dialog can now also use a path taken from drag&drop items
- [GUI] Added showing of progress bar in the taskbar button
- Added option to export Live2D Cubism 3 models

## v0.16.8.1 [25-11-2021]
- Uses System.Drawing lib instead of ImageSharp for process textures
- Added alphanumeric sorting to the column with asset names for more natural presentation of asset list
- Improved "Copy text" option in right click menu, to display what exactly to copy
- Added "Dump selected assets" option to right click menu
- Added 'selected assets count' info to status strip when you select assets
- Added 'exported count / total export count' info to status strip during export
- "Show error message" option on the "Debug" tab has been renamed to "Show all error messages" and is now disabled by default
- "Fixed" an issue with getting stuck during the "Building tree structure" step
- Fixed a bug with listSearch that could make it not work in some conditions
- Fixed a rare bug for resource files with the same name, that caused their data to be overwritten and become incorrect
