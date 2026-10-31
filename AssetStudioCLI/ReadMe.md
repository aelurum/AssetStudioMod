## AssetStudioModCLI
CLI version of AssetStudioMod.
- Supported asset types for export: `Texture2D`, `Texture2DArray`, `Sprite`, `TextAsset`, `MonoBehaviour`, `Font`, `Shader`, `MovieTexture`, `AudioClip`, `VideoClip`, `Mesh`, `Animator`.

### Usage
```
AssetStudioModCLI <input path to asset file(s)/folder> [-m, --mode <value>]
                      [-t, --asset-type <value(s)>] [-g, --group-option <value>]
                      [-f, --filename-format <value>] [-o, --output <path>]
                      [-r, --overwrite-existing] [-h, --help]
                      [--log-level <value>] [--log-output <value>]
                      [--image-format <value>] [--audio-format <value>]
                      [--l2d-group-option <value>] [--l2d-motion-mode <value>]
                      [--l2d-smooth-motions] [--l2d-search-by-filename]
                      [--l2d-filter-textures] [--fbx-scale-factor <value>]
                      [--fbx-bone-size <value>] [--fbx-animation]
                      [--fbx-dump-materials] [--fbx-uvs-as-diffuse]
                      [--fbx-ascii-format] [--filter-by-name <text>]
                      [--filter-by-container <text>] [--filter-by-pathid <text>]
                      [--filter-by-text <text>] [--filter-with-regex]
                      [--filter-exclude-mode] [--unity-version <text>]
                      [--cache-dir <path>] [--blockinfo-comp <value>]
                      [--block-comp <value>] [--decompress-to-disk]
                      [--max-export-tasks <value>] [--export-asset-list <value>]
                      [--assembly-folder <path>] [--strip-path-prefix <path>]
                      [--sprite-cropping] [--not-restore-extension]
                      [--ignore-typetree] [--raw-array]
                      [--load-all]

General Options:
  -m, --mode <value>            Specify working mode
                                <Value: extract | export(default) | exportRaw | dump | jsonDump | info |
                                live2d | splitObjects | animator>
                                Extract - Extract(Decompress) asset bundles
                                Export - Convert and export assets
                                ExportRaw - Export raw assets
                                Dump - Generate txt dumps of loaded asset
                                JsonDump - Generate json dumps of loaded asset
                                Info - Load file(s) and show the number of available for export assets
                                Live2D - Export Live2D Cubism models
                                SplitObjects - Export all model objects (split) (fbx)
                                Animator - Export Animator assets (fbx)
                                Example: "-m info"

  -t, --asset-type <value(s)>   Specify asset type(s) to export
                                <Value(s): tex2d, tex2dArray, sprite, textAsset, monoBehaviour, font, shader
                                movieTexture, audio, video, mesh | all(default)>
                                All - Export all asset types listed in the values
                                *To specify multiple asset types, write them separated by ',' or ';' without spaces
                                Examples: "-t sprite" or "-t tex2d,sprite,audio" or "-t tex2d;sprite;font"

  -g, --group-option <value>    Specify the way in which exported assets should be grouped
                                <Value: none | type | container(default) | containerFull | fileName | sceneHierarchy>
                                None - Do not group exported assets
                                Type - Group exported assets by type name
                                Container - Group exported assets by container path
                                ContainerFull - Group exported assets by full container path (e.g. with prefab name)
                                FileName - Group exported assets by source file name
                                SceneHierarchy - Group exported assets by their node path in scene hierarchy
                                Example: "-g containerFull"

  -f, --filename-format <value> Specify the file name format for exported assets
                                <Value: assetName(default) | assetName_pathID | pathID | assetName_hash>
                                AssetName - Asset file names will look like "assetName.extension"
                                AssetName_pathID - Asset file names will look like "assetName @pathID.extension"
                                PathID - Asset file names will look like "pathID.extension"
                                AssetName_hash - (images only) image names will look like "assetName @hash.extension"
                                Example: "-f assetName_pathID"

  -o, --output <path>           Specify path to the output folder
                                If path isn't specified, 'ASExport' folder will be created in the program's work folder

  -r, --overwrite-existing      (Flag) If specified, Studio will overwrite existing files during asset export/dump

  -h, --help                    Display help and exit

Logger Options:
  --log-level <value>           Specify the log level
                                <Value: verbose | debug | info(default) | warning | error>
                                Example: "--log-level warning"

  --log-output <value>          Specify the log output
                                <Value: console(default) | file | both>
                                Example: "--log-output both"

Convert Options:
  --image-format <value>        Specify the format for converting image assets
                                <Value: none | jpg | png(default) | bmp | tga | webp>
                                None - Do not convert images and export them as texture data (.tex)
                                Example: "--image-format jpg"

  --audio-format <value>        Specify the format for converting FMOD audio assets
                                <Value: none | wav(default)>
                                None - Do not convert FMOD audios and export them in their own format
                                Example: "--audio-format wav"

Live2D Options:
  --l2d-group-option <value>    Specify the way in which exported models should be grouped
                                <Value: container(default) | fileName | modelName>
                                Container - Group exported models by container path
                                FileName - Group exported models by source file name
                                ModelName - Group exported models by model name
                                Example: "--l2d-group-option modelName"

  --l2d-motion-mode <value>     Specify Live2D motion export mode
                                <Value: monoBehaviour(default) | animationClip>
                                MonoBehaviour - Try to export motions from MonoBehaviour Fade motions
                                If no Fade motions are found, the AnimationClip method will be used
                                AnimationClip - Try to export motions using AnimationClip assets
                                Example: "--l2d-motion-mode animationClip"

  --l2d-smooth-motions          (Flag) If specified, Linear motion segments will be calculated as Bezier segments
                                (May help if the exported motions look jerky/not smooth enough)

  --l2d-search-by-filename      (Flag) If specified, Studio will search for model-related Live2D assets by file name
                                rather than by container
                                (Preferred option if all l2d assets of a single model are stored in a single file
                                or containers are obfuscated)

  --l2d-filter-textures         (Flag) If specified, Studio will only export model textures with "texture"
                                in their names
                                (Only when "--l2d-search-by-filename" flag is used)

FBX Options:
  --fbx-scale-factor <value>    Specify the FBX Scale Factor
                                <Value: float number from 0 to 100 (default=1)>
                                Example: "--fbx-scale-factor 50"

  --fbx-bone-size <value>       Specify the FBX Bone Size
                                <Value: integer number from 0 to 100 (default=10)>
                                Example: "--fbx-bone-size 10"

  --fbx-animation               Specify the FBX animation export mode
                                <Value: auto(default) | skip | all>
                                Auto - Search for model-related animations and export model with them
                                Skip - Don't export animations
                                All - Try to bind all loaded animations to each loaded model
                                Example: "--fbx-animation skip"

  --fbx-dump-materials          (Flag) If specified, Studio will dump model-related Material assets along with a model

  --fbx-uvs-as-diffuse          (Flag) If specified, Studio will export all UVs as Diffuse maps.
                                Сan be useful if you cannot find some UVs after exporting (e.g. in Blender)
                                (But can also cause some bugs with UVs)

  --fbx-ascii-format            (Flag) If specified, Studio will export FBX in ASCII format.
                                If not specified, Binary format will be used

Filter Options:
  --filter-by-name <text>       Specify the name or regexp by which assets should be filtered
                                *To specify multiple names write them separated by ',' or ';' without spaces
                                Example: "--filter-by-name char" or "--filter-by-name char,bg"

  --filter-by-container <text>  Specify the container or regexp by which assets should be filtered
                                *To specify multiple containers write them separated by ',' or ';' without spaces
                                Example: "--filter-by-container arts" or "--filter-by-container arts,icons"

  --filter-by-pathid <text>     Specify the PathID by which assets should be filtered
                                *To specify multiple PathIDs write them separated by ',' or ';' without spaces
                                Example: "--filter-by-pathid 7238605633795851352,-2430306240205277265"

  --filter-by-text <text>       Specify the text or regexp by which assets should be filtered
                                Looks for assets that contain the specified text in their names or containers
                                *To specify multiple values write them separated by ',' or ';' without spaces
                                Example: "--filter-by-text portrait" or "--filter-by-text portrait,art"

  --filter-with-regex           (Flag) If specified, the filter options will handle the specified text
                                as a regular expression (doesn't apply to --filter-by-pathid)

  --filter-exclude-mode         (Flag) If specified, the filter options will work as an exclusion
                                (i.e. assets that match the filter conditions will be excluded)

Import Options:
  --unity-version <text>        Specify Unity version
                                Example: "--unity-version 2022.3.2f1"

  --cache-dir <path>            Specify a directory for storing temporary files (default: program folder)

  --blockinfo-comp <value>      Specify the compression type of bundle's blockInfo data
                                <Value: auto(default) | zstd | oodle | lz4 | lzma>
                                Auto - Use compression type specified in an asset bundle
                                Zstd - Try to decompress as zstd archive
                                Oodle - Try to decompress as oodle archive
                                Lz4 - Try to decompress as lz4/lz4hc archive
                                Lzma - Try to decompress as lzma archive
                                Example: "--blockinfo-comp lz4"

  --block-comp <value>          Specify the compression type of bundle's block data
                                <Value: auto(default) | zstd | oodle | lz4 | lzma>
                                Auto - Use compression type specified in an asset bundle
                                Zstd - Try to decompress as zstd archive
                                Oodle - Try to decompress as oodle archive
                                Lz4 - Try to decompress as lz4/lz4hc archive
                                Lzma - Try to decompress as lzma archive
                                Example: "--block-comp zstd"

  --decompress-to-disk          (Flag) If not specified, only bundles larger than 2GB will be decompressed
                                to disk(cache dir) instead of RAM

Advanced Options:
  --max-export-tasks <value>    Specify the number of parallel tasks for asset export
                                <Value: integer number from 1 to max number of cores (default=max)>
                                Max - Number of cores in your CPU
                                Example: "--max-export-tasks 8"

  --export-asset-list <value>   Specify the format in which you want to export asset list
                                <Value: none(default) | xml>
                                None - Do not export asset list
                                Example: "--export-asset-list xml"

  --assembly-folder <path>      Specify the path to the assembly folder

  --strip-path-prefix <path>    Specify a path prefix to be stripped from exported asset paths
                                Example: "--strip-path-prefix assets/models/char/"

  --sprite-cropping             Specify the sprite cropping mode
                                <Value: spriteRect(default) | textureRect>
                                SpriteRect - Crop sprites keeping their own size and alignment
                                TextureRect - Crop sprites to their actual image size (aka trim)
                                Example: "--sprite-cropping textureRect"

  --not-restore-extension       (Flag) If specified, Studio will not try to use/restore original TextAsset extension,
                                and will just export all TextAssets with the ".txt" extension

  --ignore-typetree             (Flag) If specified, Studio will not try to parse assets at load time
                                using their type tree

  --raw-array                   (Flag) If specified, Studio will try to extract raw byte array from MonoBehaviour assets
                                (Only for ExportRaw mode)

  --load-all                    (Flag) If specified, Studio will load assets of all types
                                (Only for Dump, JsonDump, Info and ExportRaw modes)
```
