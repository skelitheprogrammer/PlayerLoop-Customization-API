<div align="center">   
    
<h1>PlayerLoop customization API</h1>
Create your own loop system using interface built on top of <a href="https://docs.unity3d.com/ScriptReference/LowLevel.PlayerLoop.html">Unity's PlayerLoop</a>
</div>

# Installation
### Add via package manager
```
https://github.com/skelitheprogrammer/PlayerLoop-Customization-API.git
```
### Or add dependency in manifest.json
```
"com.skillitronic.playerloopcustomizationapi" : "https://github.com/skelitheprogrammer/PlayerLoop-Customization-API.git",
```

# Introduction
PlayerLoopAPI allows user to add additional systems into default Unity loop.

> [!NOTE]
> PlayerLoopAPI don't override PlayerLoop. It builds everything from map, that user populated using methods

> [!WARNING]
> PlayerLoopAPI setups himself at [`[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]`](https://docs.unity3d.com/ScriptReference/RuntimeInitializeLoadType.SubsystemRegistration.html) 
> timing and initiates at [`[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSplashScreen)]`](https://docs.unity3d.com/ScriptReference/RuntimeInitializeLoadType.BeforeSplashScreen.html)
> timing

### Methods to use
> [!NOTE]
> If the user leaves the type equal to null, then PlayerLoopAPI will treat this as root PlayerLoopSystem

Add a system before/after the selected system type name
```c#
PlayerLoopAPI.AddBefore(ref PlayerLoopSystem insertSystem, Type systemType = null);
PlayerLoopAPI.AddAfter(ref PlayerLoopSystem insertSystem, Type systemType = null); 
```
Adding a system as a child at the beginning/end of the selected system type name
```c#
PlayerLoopAPI.AddAtBeginning(ref PlayerLoopSystem insertSystem, Type systemType = null); 
PlayerLoopAPI.AddAtEnd(ref PlayerLoopSystem insertSystem, Type systemType = null);
```
Wrap systems around the selected system type name
```c#
PlayerLoopAPI.WrapAround(ref PlayerLoopSystem beforeSystem, ref PlayerLoopSystem afterSystem, Type systemType = null);
```
Wrap systems as children at the beginning and end of the selected system type name
```c#
PlayerLoopAPI.WrapInside(ref PlayerLoopSystem beforeSystem, ref PlayerLoopSystem afterSystem, Type systemType = null);
```

## Getting started

Create static class that will execute modifications before  [`[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSplashScreen)]`](https://docs.unity3d.com/ScriptReference/RuntimeInitializeLoadType.BeforeSplashScreen.html)
```c#
using UnityEngine;

public static class Registrar
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    private static void Init()
    {
    
    }
}
```

Create system yourself or use Utils class
```c#
using UnityEngine;
using PlayerLoopCustomizationAPI.Utils;

public static class Registrar
{
    private struct SystemName {}

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    private static void Init()
    {
        PlayerLoopSystem someSystem = new()
        {
            type = typeof(SystemName),
            updateDelegate = () => Debug.Log("Hi!")
        }
       
        // or
        
        PlayerLoopSystem someSystem = PlayerLoopUtils.CreateSystem<SystemName>(() => Debug.Log("Hi");
    }
}
```
Add created system wherever you want
```c#
using UnityEngine;
using PlayerLoopCustomizationAPI.Utils;

public static class Registrar
{
    private struct SystemName {}

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    private static void Init()
    {
        PlayerLoopSystem someSystem = new()
        {
            type = typeof(SystemName),
            updateDelegate = () => Debug.Log("Hi!")
        }
       
        // or
        
        PlayerLoopSystem someSystem = PlayerLoopUtils.CreateSystem<SystemName>(() => Debug.Log("Hi");
        
        PlayerLoopAPI.AddBefore(ref someSystem, typeof(Update.ScriptRunBehaviourUpdate));
    }
}
```

# Addons

- [PlayerLoop-customization-API.Runner](https://github.com/skelitheprogrammer/PlayerLoop-customization-API.Runner) - Add custom or Use predefined interfaces to run your custom Loop System!