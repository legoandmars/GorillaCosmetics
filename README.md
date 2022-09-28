# Gorilla Cosmetics

A PC cosmetic mod for Gorilla Tag that allows customizing the look of gorillas with materials and hats.

![An example of a custom gorilla](https://user-images.githubusercontent.com/34404266/108779287-7c9a8400-751b-11eb-8a9c-e279aaddf6dc.png)

## Installation

### Automatic installation
If you don't want to manually install, you can install this mod with the [Monke Mod Manager](https://github.com/DeadlyKitten/MonkeModManager/releases/latest)

### Manual Installation

If your game isn't modded with BepinEx, DO THAT FIRST! Simply go to the [latest BepinEx release](https://github.com/BepInEx/BepInEx/releases) and extract BepinEx_x64_VERSION.zip directly into your game's folder, then run the game once to install BepinEx properly.

This mod also depends on Newtonsoft.Json, so go to my latest [Newtonsoft.Json release](https://github.com/legoandmars/Newtonsoft.Json/releases/latest) and follow the instructions there.

It also depends on Utilla, so install the latest [Utilla release](https://github.com/legoandmars/Utilla/releases/latest) as well

Next, go to the [latest release of this mod](https://github.com/legoandmars/GorillaCosmetics/releases/latest) and extract it directly into your game's folder. Make sure it's extracted directly into your game's folder and not into a subfolder!

The mod is now installed!

If you'd like to install gorilla materials that people have made that don't come with the mod, drag the `.gmat` file into your `Gorilla Tag/BepInEx/Materials` folder.

If you'd like to install hats that people have made that don't come with the mod, drag the `.hat` file into your `Gorilla Tag/BepInEx/Hats` folder.

## Configuration

Gorilla Cosmetics currently comes with an in-game mirror that allows you to select your hat and material.

If you'd like to change one of the options not available ingame or manually change what cosmetics you have selected, you can edit the config found in `Gorilla Tag/BepInEx/config/GorillaCosmetics.cfg` (The game must be run *at least once* after installing the mod for this file to be generated)

The config should be mostly self-explanatory, but here's a quick rundown of what each option does:
```
[Config]

# Whether or not other players should use your selected material.
# Valid Options: true/false
ApplyMaterialsToOtherPlayers = false

# Whether or not other players should use your selected infected material when tagged/infected.
# Valid Options: true/false
ApplyInfectedMaterialsToOtherPlayers = false

# Whether or not other players should use your selected hat.
# Valid Options: true/false
ApplyHatsToOtherPlayers = false

[Cosmetics]

# What material to use from the BepInEx/plugins/GorillaCosmetics/Materials folder.
# Valid Options: Default/Camo/Gold/Matrix/Rainbow/Unity Default Material
SelectedMaterial = Rainbow

# What material to use from the BepInEx/plugins/GorillaCosmetics/Materials folder for tagged/infected players.
# Valid Options: Default/Camo/Gold/Matrix/Rainbow/Unity Default Material
SelectedInfectedMaterial = Default

# What hat to use from the BepInEx/plugins/GorillaCosmetics/Hats folder. Use Default for none
# Valid Options: Amongus/Crown/Top Hat
SelectedHat = Top Hat
```

## For Model Creators

If you'd like to create Materials or Hats, the unity project can be found here: https://github.com/legoandmars/GorillaCosmeticsUnityProject

More detailed information is found in the Unity Project's README.

## For Developers
This project is built with C# using .NET Standard.

Make sure to install the mod first so you have all of the required files. For references, create a Libs folder in the same folder as the project solution. Inside of this folder you'll need to copy:

```
0Harmony.dll
BepInEx.dll
BepInEx.Harmony.dll
Newtonsoft.Json
``` 
from `Gorilla Tag\BepInEx\core`,
```
Utilla.dll
``` 
from `Gorilla Tag\BepInEx\plugins\Utilla`, and
```
Assembly-CSharp.dll
PhotonRealtime.dll
PhotonUnityNetworking.dll
UnityEngine.dll
UnityEngine.AssetBundleModule.dll
UnityEngine.CoreModule.dll
UnityEngine.PhysicsModule.dll
``` 
from `Gorilla Tag\Gorilla Tag_Data\Managed`.

## Disclaimers
This product is not affiliated with Gorilla Tag or Another Axiom LLC and is not endorsed or otherwise sponsored by Another Axiom LLC. Portions of the materials contained herein are property of Another Axiom LLC. ©2021 Another Axiom LLC.
