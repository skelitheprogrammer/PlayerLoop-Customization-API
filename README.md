<div align="center">   

<h1>PlayerLoop Extender</h1>
<b>Insert✨Remove✨Filter <a href="https://docs.unity3d.com/ScriptReference/LowLevel.PlayerLoop.html">Unity's PlayerLoop</a> systems!</b>
</div>

# 💾 Installation

<details>
<summary>
<h3>Add via package manager</h3>
</summary>

```
https://github.com/skelitheprogrammer/PlayerLoopExtender.git
```

</details>

<details>
<summary>
<h3>Or add dependency in manifest.json</h3>
</summary>

```
"com.skillitronic.playerloopextender" : "https://github.com/skelitheprogrammer/PlayerLoopExtender.git",
```

</details>

# ⚡ Quick Use

```c#
    internal struct TestSystemName
    {
        public static PlayerLoopSystem Create(PlayerLoopSystem.UpdateFunction updateFunction = null) => new()
        {
            type = typeof(TestSystemName),
            updateDelegate = updateFunction
        };
    }

    public static class Context
    {
        [MenuItem("PlayerLoop/Action")]
        private static void InsertSystems()
        {
            Type anchorType = typeof(Update.ScriptRunBehaviourUpdate);
    
            PlayerLoopSystem copyLoop = PlayerLoop.GetDefaultPlayerLoop();
    
            copyLoop.InsertSystem(TestSystemName.Create(), anchorType, PlayerLoopSystemExtensions.InsertType.BEFORE);
            copyLoop.InsertSystem(TestSystemName.Create(), anchorType, PlayerLoopSystemExtensions.InsertType.AFTER);
        
            PlayerLoop.SetPlayerLoop(copyLoop);
            
            LogLoopSystem(copyLoop);
        }
    }
```

# ➕ Insert

Using [InsertSystem](https://github.com/skelitheprogrammer/PlayerLoopExtender/blob/a3d84c438d6e7350f3954e31978302344a0e4f98/Runtime/PlayerLoopSystemExtensions.Insert.cs#L14)
extension method you can add your own system into PlayerLoop

```c#
public struct SomeSystemName {}

PlayerLoopSystem copyLoop = PlayerLoop.GetDefaultPlayerLoop();
PlayerLoopSystem customSystem = new ()
{
    type = typeof(SomeSystemName)
};

copyLoop.InsertSystem(customSystem, typeof(Update), InsertType.BEFORE);
```

# ➖ Remove

Using [TryRemoveSystem](https://github.com/skelitheprogrammer/PlayerLoopExtender/blob/a3d84c438d6e7350f3954e31978302344a0e4f98/Runtime/PlayerLoopSystemExtensions.Remove.cs#L14)
extension method you can remove any PlayerLoopSystem.

```c#
PlayerLoopSystem copyLoop = PlayerLoop.GetDefaultPlayerLoop();
copyLoop.TryRemoveSystem(typeof(Update));
```

# 🗑️ Filter

There is
partial [PlayerLoopSystemFilter](https://github.com/skelitheprogrammer/PlayerLoopExtender/blob/a3d84c438d6e7350f3954e31978302344a0e4f98/Runtime/Filter/PlayerLoopSystemFilter.cs)
class with predefined array's of different Unity PlayerLoopSystems which you can remove
using [TryRemoveSystem](https://github.com/skelitheprogrammer/PlayerLoopExtender/blob/a3d84c438d6e7350f3954e31978302344a0e4f98/Runtime/PlayerLoopSystemExtensions.Remove.cs#L14)
extension method

```c#
PlayerLoopSystem copyLoop = PlayerLoop.GetDefaultPlayerLoop();

foreach (Type type in PlayerLoopSystemFilter.XR)
{
    copyLoop.TryRemoveSystem(type);
}
```
> [!NOTE]
> You can extend this class, by adding your own filter.

# Misc.
> [!Tip]
> You can Check out [Tests](https://github.com/skelitheprogrammer/PlayerLoopExtender/tree/a3d84c438d6e7350f3954e31978302344a0e4f98/Tests/Editor) to see how you can use this methods.



