<div align="center">   
    
<h1>PlayerLoop customization API</h1>
Create your own loop system using interface built on top of <a href="https://docs.unity3d.com/ScriptReference/LowLevel.PlayerLoop.html">Unity's PlayerLoop</a>
</div>

# Features
- **No need to worry about managing array of PlayerLoopSystems**
- **Easy to use functionality** - think about what you want to add, everything else already done for you.

# Installation

### Add via package manager

```
https://github.com/skelitheprogrammer/PlayerLoop-Customization-API.git
```

### Add dependency in manifest.json
```
"com.skillitronic.playerloopcustomizationapi" : "https://github.com/skelitheprogrammer/PlayerLoop-Customization-API",
```

# Getting Started

## Create struct type which will act as a name for a custom PlayerLoopSystem

```c#
private struct CustomSystemName {}
```
## Create new [PlayerLoopSystem](https://docs.unity3d.com/ScriptReference/LowLevel.PlayerLoopSystem.html)

```c#
private struct CustomSystemName {}

[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
internal static class CustomPlayerLoopInitialization
{
    private static void Init()
    {
        PlayerLoopSystem customSystem = new()
        {
            type = typeof(CustomSystemName)
            updateDelegate = SomeMethodToRun
        }
    }
    
    private void SomeMethodToRun()
    {
        Debug.Log("Hi! I'm mock method!");
    }
}
```

## Get the current changes from API

```c#
private struct CustomSystemName {}

[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
internal static class CustomPlayerLoopInitialization
{
    private static void Init()
    {
        PlayerLoopSystem customSystem = new()
        {
            type = typeof(CustomSystemName),
            updateDelegate = SomeMethodToRun
        }
        
        ref PlayerLoopSystem copyLoop = ref PlayerLoopAPI.GetCustomPlayerLoop();
    }
    
    private void SomeMethodToRun()
    {
        Debug.Log("Hi! I'm mock method!");
    }
}
```

## Get interested subSystem

using PlayerLoopAPI class

```c#
private struct CustomSystemName {}

[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
internal static class CustomPlayerLoopInitialization
{
    private static void Init()
    {
        PlayerLoopSystem customSystem = new()
        {
            type = typeof(CustomSystemName),
            updateDelegate = SomeMethodToRun
        }
        
        ref PlayerLoopSystem copyLoop = ref PlayerLoopAPI.GetCustomPlayerLoop();
        
        ref PlayerLoopSystem updateLoop = PlayerLoopAPI.GetLoopSystem<Update>(); //traverse from the main PlayerLoop
        // or
        ref PlayerLoopSystem updateLoop = PlayerLoopAPI.GetLoopSystem<Update>(copyLoop); //traverse from selected PlayerLoopSystem
        // or using extension method
        ref PlayerLoopSystem updateLoop = copyLoop.GetLoopSystem<Update>();
    }
    
    private void SomeMethodToRun()
    {
        Debug.Log("Hi! I'm mock method!");
    }
}
```

## Insert your new [PlayerLoopSystem](https://docs.unity3d.com/ScriptReference/LowLevel.PlayerLoopSystem.html) inside of a selected subSystem using extension methods
```c#
private struct CustomSystemName {}

[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
internal static class CustomPlayerLoopInitialization
{
    private static void Init()
    {
        PlayerLoopSystem customSystem = new()
        {
            type = typeof(CustomSystemName),
            updateDelegate = SomeMethodToRun
        }
        
        ref PlayerLoopSystem copyLoop = ref PlayerLoopAPI.GetCustomPlayerLoop();
        
        ref PlayerLoopSystem updateLoop = PlayerLoopAPI.GetLoopSystem<Update>();
        // or
        ref PlayerLoopSystem updateLoop = PlayerLoopAPI.GetLoopSystem<Update>(copyLoop);
        // or use extension methods
        ref PlayerLoopSystem updateLoop = copyLoop.GetLoopSystem<Update>();
        
        updateLoop.InsertSystemBefore<Update.ScriptRunBehaviourUpdate>(customSystem);
        updateLoop.InsertSystemAfter<Update.ScriptRunBehaviourUpdate>(customSystem);
        updateLoop.InsertSystemAtBeginning(customSystem);
        updateLoop.InsertSystemAtEnd(customSystem);
    }
    
    private void SomeMethodToRun()
    {
        Debug.Log("Hi! I'm mock method!");
    }
}
```
- You should customize your systems in method marked [`[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]`](https://docs.unity3d.com/ScriptReference/RuntimeInitializeLoadType.SubsystemRegistration.html) attribute. 
  PlayerLoopAPI builds new PlayerLoop in [`[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSplashScreen)]`](https://docs.unity3d.com/ScriptReference/RuntimeInitializeLoadType.BeforeSplashScreen.html)
>If you don't have the ability to use this method, create the playerLoop yourself using [PlayerLoop.SetCurrentLoop(PlayerLoopAPI.GetCustomPlayerLoop())](https://docs.unity3d.com/ScriptReference/LowLevel.PlayerLoop.SetPlayerLoop.html)

# Utils
- `PlayerLoopUtils.ShowLoopSystems(PlayerLoopSystem playerLoopSystem, int inline = 0)` - Get string of all [PlayerLoopSystems](https://docs.unity3d.com/ScriptReference/LowLevel.PlayerLoopSystem.html)
- Follow `"PlayerLoopUtils/Log PlayerLoop" menu item` to log out to console current [PlayerLoop](https://docs.unity3d.com/ScriptReference/LowLevel.PlayerLoop.html)
# Experimental 
If you add [`PLAYERLOOPAPI_EXPERIMENTAL`](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/preprocessor-directives) in [`Scripting define symbols`](https://docs.unity3d.com/Manual/CustomScriptingSymbols.html)
you're opening up the possibility of using experimental features.

`PlayerLoopAPI.Query<T>()` - gives opportunity to traverse through the whole PlayerLoopSystem recursively.
> Can be expensive if PlayerLoop will have too many subSystems/nested subSystems
