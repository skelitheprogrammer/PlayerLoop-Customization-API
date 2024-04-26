<div align="center">   

<h1>PlayerLoop Extender</h1>
Add/Remove/Filter <a href="https://docs.unity3d.com/ScriptReference/LowLevel.PlayerLoop.html">Unity's PlayerLoop</a> systems!
</div>

# Installation

<details>
<summary>
<h2>Add via package manager</h2>
</summary>

```
https://github.com/skelitheprogrammer/PlayerLoopExtender.git
```
</details>

<details>
<summary>
<h2>
Or add dependency in manifest.json
</h2>
</summary>

```
"com.skillitronic.playerloopextender" : "https://github.com/skelitheprogrammer/PlayerLoopExtender.git",
```
</details>

# Quick Start

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

