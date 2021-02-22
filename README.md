# Gorilla Cosmetics

A PC cosmetic mod for Gorilla Tag that allows customizing the look of gorillas with materials and hats.

![An example of a custom gorilla](https://user-images.githubusercontent.com/34404266/108779287-7c9a8400-751b-11eb-8a9c-e279aaddf6dc.png)

# Usage

If your game isn't modded with BepinEx, DO THAT FIRST! Simply go to the [latest BepinEx release](https://github.com/BepInEx/BepInEx/releases) and extract BepinEx_x64_VERSION.zip directly into your game's folder, then run the game once to install BepinEx properly.

Next, go to the [latest release of this mod](https://github.com/legoandmars/GorillaCosmetics/releases/latest) and extract it directly into your game's folder. Make sure it's extracted directly into your game's folder and not into a subfolder!

The mod is now installed! If you'd like to change what cosmetics you have selected, run the game once to generate a config found in `Gorilla Tag/BepInEx/config/GorillaCosmetics.cfg`

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

# For Model Creators

If you'd like to create Materials or Hats, the unity project can be found here: https://github.com/legoandmars/GorillaCosmeticsUnityProject

More detailed information is found in the Unity Project's README.

# For Developers
This project is built with C# using .NET Standard.

For references, create a Libs folder in the same folder as the project solution. Inside of this folder you'll need to copy:

```
0Harmony.dll
BepInEx.dll
BepInEx.Harmony.dll
``` 
from `Gorilla Tag\BepInEx\plugins`, and
```
Assembly-CSharp.dll
Photon3Unity3D.dll
PhotonRealtime.dll
PhotonUnityNetworking.dll
UnityEngine.dll
UnityEngine.AssetBundleModule.dll
UnityEngine.CoreModule.dll
UnityEngine.PhysicsModule.dll
UnityEngine.VRModule.dll 
UnityEngine.XRModule.dll 
``` 
from `Gorilla Tag\Gorilla Tag_Data\Managed`.