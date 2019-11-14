using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class App : BehaviourSingleton<App> {


    public static event System.Action modulesLoaded;

    private Dictionary<ModuleDBEntry, GameObject> _modules;

    private Transform _appModulesContainer;
    private Transform _gameModulesContainer;

    IEnumerator Start() {

        DontDestroyOnLoad(gameObject);
        
        yield return StartCoroutine(C_Modules());

        modulesLoaded.SafeInvoke();
    }

    IEnumerator C_Modules() {
        var appModules = DB.GetAll<ModuleDBEntry>().Where(m => m.enabled).SelectNotNull().ToList();
        appModules.Sort(new System.Comparison<ModuleDBEntry>((m1, m2) => {
            return m1.priority.CompareTo(m2.priority);
        }));
        _modules = new Dictionary<ModuleDBEntry, GameObject>();
        _appModulesContainer = new GameObject("AppModules").transform;
        _appModulesContainer.Reset(transform);
        foreach (var module in appModules) {
            var moduleGO = module.LoadPrefab<GameObject>().Clone();
            moduleGO.name = module.name;
            moduleGO.transform.parent = _appModulesContainer;
            _modules.Add(module, moduleGO);
            var moduleComponent = moduleGO.GetComponent<ModuleComponent>();
            if (moduleComponent != null) {
                while (!moduleComponent.inited) {
                    moduleComponent.inited = true;
                    yield return null;
                }
            }
        }
    }

    public T Get<T>() {

        if (_modules != null) {
            foreach (var kvp in _modules) {
                var module = kvp.Value.GetComponentInChildren<T>();
                if (module != null) {
                    return module;
                }
            }
        }

        return default(T);
    }
}
