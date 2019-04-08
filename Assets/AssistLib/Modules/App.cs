using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class App : BehaviourSingleton<App> {


    public static event System.Action modulesLoaded;

    private Dictionary<AppModule, GameObject> _appModules;
    private Dictionary<GameModule, GameObject> _gameModules;

    private Transform _appModulesContainer;
    private Transform _gameModulesContainer;

    IEnumerator Start() {

        DontDestroyOnLoad(gameObject);
        
        var gameModules = DB.GetAll<GameModule>().SelectNotNull().ToList();
        gameModules.Sort(new System.Comparison<GameModule>((m1, m2) => {
            return m1.priority.CompareTo(m2.priority);
        }));
        yield return StartCoroutine(C_InitAppModules());
        yield return StartCoroutine(C_InitGameModules());

        modulesLoaded.SafeInvoke();
    }

    IEnumerator C_InitAppModules() {
        var appModules = DB.GetAll<AppModule>().SelectNotNull().ToList();
        appModules.Sort(new System.Comparison<AppModule>((m1, m2) => {
            return m1.priority.CompareTo(m2.priority);
        }));
        _appModules = new Dictionary<AppModule, GameObject>();
        _appModulesContainer = new GameObject("AppModules").transform;
        _appModulesContainer.Reset(transform);
        foreach (var module in appModules) {
            var moduleGO = module.LoadPrefab<GameObject>().Clone();
            moduleGO.name = module.name;
            moduleGO.transform.parent = _appModulesContainer;
            _appModules.Add(module, moduleGO);
            var moduleComponent = moduleGO.GetComponent<ModuleComponent>();
            if (moduleComponent != null) {
                while (!moduleComponent.inited) {
                    moduleComponent.inited = true;
                    yield return null;
                }
            }
        }
    }

    IEnumerator C_InitGameModules() {
        var gameModules = DB.GetAll<GameModule>().SelectNotNull().ToList();
        gameModules.Sort(new System.Comparison<GameModule>((m1, m2) => {
            return m1.priority.CompareTo(m2.priority);
        }));
        _gameModules = new Dictionary<GameModule, GameObject>();
        _gameModulesContainer = new GameObject("GameModules").transform;
        _gameModulesContainer.Reset(transform);
        foreach (var module in gameModules) {
            var moduleGO = module.LoadPrefab<GameObject>().Clone();
            moduleGO.name = module.name;
            moduleGO.transform.parent = _gameModulesContainer;
            _gameModules.Add(module, moduleGO);
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

        if (_appModules != null) {
            foreach (var kvp in _appModules) {
                var module = kvp.Value.GetComponentInChildren<T>();
                if (module != null) {
                    return module;
                }
            }
        }


        if (_gameModules != null) {
            foreach (var kvp in _gameModules) {
                var module = kvp.Value.GetComponentInChildren<T>();
                if (module != null) {
                    return module;
                }
            }
        }

        return default(T);
    }
}
