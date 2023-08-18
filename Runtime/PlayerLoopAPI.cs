using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using UnityEngine;
using UnityEngine.LowLevel;

namespace PlayerLoopCustomizationAPI
{
    internal enum InsertionType
    {
        None = 0,
        Before = 1,
        After = 2,
        AtBeginning = 4,
        AtEnd = 8,
    }

    public static class PlayerLoopAPI
    {
        //Purely for null avoidance. Not used in the actual loop
        internal struct MainLoop
        {
        }

        private static Dictionary<Type, List<SystemData>> _map;
        private static LoopComponent _component;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void Setup()
        {
            HandleReset();

            PlayerLoopSystem copyLoop = PlayerLoop.GetCurrentPlayerLoop();

            _component = new LoopComponent(ref copyLoop);

            IEnumerable<LoopComponent> descriptors = copyLoop.subSystemList.Select(x => new LoopComponent(ref x, _component));
            Queue<LoopComponent> queue = new(descriptors);

            while (queue.TryDequeue(out LoopComponent result))
            {
                if (result.System.subSystemList == null)
                {
                    continue;
                }

                for (int index = 0; index < result.System.subSystemList.Length; index++)
                {
                    ref PlayerLoopSystem playerLoopSystem = ref result.System.subSystemList[index];
                    queue.Enqueue(new LoopComponent(ref playerLoopSystem, result));
                }
            }

            static void HandleReset()
            {
                _component = null;
                _map = new();
                Query.Reset();

                Application.quitting -= Reset;
                Application.quitting += Reset;

                static void Reset()
                {
                    _component = null;
                    _map = new();
                    Query.Reset();
                    PlayerLoop.SetPlayerLoop(PlayerLoop.GetDefaultPlayerLoop());
                }
            }
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSplashScreen)]
        private static void Build()
        {
            InsertSystems();

            Convert(ref _component.System, _component);
            PlayerLoop.SetPlayerLoop(_component.System);

            static void Convert(ref PlayerLoopSystem system, LoopComponent component)
            {
                system.subSystemList = new PlayerLoopSystem[component.ChildrenCount];

                for (int i = 0; i < system.subSystemList.Length; i++)
                {
                    system.subSystemList[i] = component[i].System;

                    Convert(ref system.subSystemList[i], component[i]);
                }
            }
        }

        public static void AddBefore(ref PlayerLoopSystem insertSystem, Type systemType = null)
        {
            AddSystemData(ref insertSystem, InsertionType.Before, systemType);
        }

        public static void AddAfter(ref PlayerLoopSystem insertSystem, Type systemType = null)
        {
            AddSystemData(ref insertSystem, InsertionType.After, systemType);
        }

        public static void AddAtBeginning(ref PlayerLoopSystem insertSystem, Type systemType = null)
        {
            AddSystemData(ref insertSystem, InsertionType.AtBeginning, systemType);
        }

        public static void AddAtEnd(ref PlayerLoopSystem insertSystem, Type systemType = null)
        {
            AddSystemData(ref insertSystem, InsertionType.AtEnd, systemType);
        }

        public static void WrapAround(ref PlayerLoopSystem beforeSystem,ref PlayerLoopSystem afterSystem, Type systemType = null)
        {
            AddBefore(ref beforeSystem, systemType);
            AddAfter(ref afterSystem, systemType);
        }

        public static void WrapInside(ref PlayerLoopSystem beforeSystem, ref PlayerLoopSystem afterSystem, Type systemType = null)
        {
            AddAtBeginning(ref beforeSystem, systemType);
            AddAtEnd(ref afterSystem, systemType);
        }

        private static void AddSystemData(ref PlayerLoopSystem insertSystem, InsertionType insertionType, Type systemType = null)
        {
            systemType ??= typeof(MainLoop);

            if (!_map.ContainsKey(systemType))
            {
                _map.Add(systemType, new List<SystemData>());
            }

            _map[systemType].Add(new SystemData
            {
                System = insertSystem,
                InsertionType = insertionType
            });
        }

        private static void InsertSystems()
        {
            foreach (Type mapKey in _map.Keys)
            {
                LoopComponent result = Query.Traverse(mapKey, _component);

                for (int i = 0; i < _map[mapKey].Count; i++)
                {
                    SystemData addSystemData = _map[mapKey][i];
                    int index = result.PositionIndex;

                    InsertionType insertionType = addSystemData.InsertionType;
                    index = insertionType switch
                    {
                        InsertionType.None => throw new InvalidEnumArgumentException(),
                        InsertionType.Before => index,
                        InsertionType.After => index + 1,
                        InsertionType.AtBeginning => 0,
                        InsertionType.AtEnd => result.ChildrenCount,
                        _ => throw new ArgumentOutOfRangeException()
                    };

                    if (insertionType is InsertionType.AtBeginning or InsertionType.AtEnd)
                    {
                        new LoopComponent(ref addSystemData.System, result, index);
                    }
                    else
                    {
                        if (result.Parent == null)
                        {
                            throw new InvalidOperationException($"Can't {addSystemData.System.type.Name} system to non existing parent of {result.SystemType.Name}");
                        }

                        new LoopComponent(ref addSystemData.System, result.Parent, index);
                    }
                }
            }
        }

        //todo: make the return type a collection type because of the possibility that duplicates may exist
        private static class Query
        {
            private static Dictionary<Type, LoopComponent> _cache = new();

            internal static void Reset()
            {
                _cache = new();
            }

            internal static LoopComponent Traverse(Type seekType, LoopComponent parent)
            {
                if (seekType == typeof(MainLoop))
                {
                    return _component;
                }

                if (_cache.TryGetValue(seekType, out LoopComponent component))
                {
                    return component;
                }

                Queue<LoopComponent> queue = new();
                queue.Enqueue(parent);

                while (queue.TryDequeue(out LoopComponent result))
                {
                    if (result.SystemType == seekType)
                    {
                        _cache.Add(seekType, result);
                        return result;
                    }

                    foreach (LoopComponent playerLoopSystem in result.Children)
                    {
                        queue.Enqueue(playerLoopSystem);
                    }
                }

                throw new NullReferenceException();
            }
        }
    }

    internal class LoopComponent
    {
        internal LoopComponent Parent { get; private set; }
        internal PlayerLoopSystem System;

        private readonly List<LoopComponent> _children;
        internal IReadOnlyList<LoopComponent> Children => _children;
        internal Type SystemType => System.type;

        internal LoopComponent this[int i] => _children[i];

        internal int PositionIndex
        {
            get
            {
                if (Parent == null)
                {
                    return 0;
                }

                return Parent._children.IndexOf(this);
            }
        }

        internal int ChildrenCount => _children.Count;

        internal LoopComponent(ref PlayerLoopSystem system, LoopComponent parent = null, int index = -1)
        {
            System = system;
            
            _children = new();
            SetParent(parent, index);
        }

        private void SetParent(LoopComponent newParent, int index = -1)
        {
            if (Parent != null)
            {
                Parent._children.Remove(this);
                Parent = null;
            }

            if (newParent != null)
            {
                if (index == -1)
                {
                    index = newParent.ChildrenCount;
                }

                newParent.Insert(this, index);
                Parent = newParent;
            }
        }

        private void Insert(LoopComponent component, int index)
        {
            _children.Insert(index, component);
        }
    }

    internal class SystemData
    {
        public PlayerLoopSystem System;
        public InsertionType InsertionType;
    }
}