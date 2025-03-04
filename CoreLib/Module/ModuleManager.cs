using System;
using System.Collections.Generic;
using UnityEngine;

namespace CoreLib.Module
{
    // ModuleBase의 수명 주기를 관리
    public static class ModuleManager
    {
        private static readonly Dictionary<Type, ModuleBase> Modules = new();

        private static readonly List<Type> UseModuleTypes = new();
        
        // Module의 초기 상태 혹은 Release된 상태로 보관
        private static readonly Dictionary<Type, ModuleBase> ReservedModules = new();

        private static ModulePhase _currentPhase = ModulePhase.None;

        private static bool _isInitialized;
        
        // 1. 최초에 사용할 Module의 목록 지정
        public static void Initialize(List<Type> useModuleTypes)
        {
            if (_isInitialized)
            {
                Debug.LogError("ModuleManager is already initialized");
                return;
            }
            
            for (var i = 0; i < useModuleTypes.Count; i++)
            {
                if (!useModuleTypes[i].IsAssignableFrom(typeof(ModuleBase)))
                    throw new Exception("ModuleManager: Please set only ModuleBase to UseModules.");
            }

            UseModuleTypes.Clear();
            UseModuleTypes.AddRange(useModuleTypes);

            _isInitialized = true;

            Application.quitting += Release;
        }
        
        // 2. Phase를 변경하며 Phase에 맞게 Module들을 Release 및 Initialize
        public static void InitializePhase(ModulePhase nextPhase)
        {
            Debug.Log($"Initialize Phase: {_currentPhase} => {nextPhase}");
            _currentPhase = nextPhase;

            for (var i = 0; i < UseModuleTypes.Count; i++)
            {
                var type = UseModuleTypes[i];
                
                ReleaseModule(type, nextPhase);
                InitializeModule(type, nextPhase);
            }
        }
        
        private static void Release()
        {
            Modules.Clear();
            UseModuleTypes.Clear();
            ReservedModules.Clear();
            _currentPhase = ModulePhase.None;

            _isInitialized = false;
            
            Application.quitting -= Release;
        }

        private static void ReleaseModule(Type moduleType, ModulePhase phase)
        {
            if (!Modules.TryGetValue(moduleType, out var module))
                return;

            if (module.GetInitializePhase() < phase)
                return;
            
            // 현재 사용 중인 Module이고 변경되는 Phase 혹은 이후 Phase에서 사용되는 경우 Release 후 Reserve
            module.OnRelease();
            Modules.Remove(moduleType);
            ReservedModules.Add(moduleType, module);
        }
        
        private static void InitializeModule(Type moduleType, ModulePhase phase)
        {
            // 정상적으로 사용 중인 Module은 return
            if (Modules.TryGetValue(moduleType, out var module))
                return;

            module = GetReservedModule(moduleType);

            if (module.GetInitializePhase() != phase)
                return;

            ReservedModules.Remove(moduleType);
            Modules.Add(moduleType, module);
            
            module.OnInitialize();
        }

        private static ModuleBase GetReservedModule(Type moduleType)
        {
            if (ReservedModules.TryGetValue(moduleType, out var module))
                return module;
            
            var obj = new GameObject(moduleType.Name);
            module = (ModuleBase)obj.AddComponent(moduleType);
            
            ReservedModules.Add(moduleType, module);

            return module;
        }
    
        public static T? GetModule<T>(Type type) where T : ModuleBase<T>
        {
            if (Modules.TryGetValue(type, out var module))
                return (T)module;
            
            Debug.LogError($"Module {type.Name} not found");
            
            return null;
        }
    }
}
