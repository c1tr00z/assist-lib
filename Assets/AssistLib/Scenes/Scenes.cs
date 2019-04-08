using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Scenes : BehaviourSingleton<Scenes> {

    private SceneItem _currentSceneItem;

    private void OnEnable() {
        App.modulesLoaded += App_modulesLoaded;
    }

    private void OnDisable() {
        App.modulesLoaded -= App_modulesLoaded;
    }

    private void App_modulesLoaded() {
        if (AppSettings.instance.startScene == null) {
            return;
        }
        LoadScene(AppSettings.instance.startScene);
    }

    public void LoadScene(SceneItem newScene) {

        _currentSceneItem = newScene;

        SceneManager.LoadScene(_currentSceneItem.name, LoadSceneMode.Single);
    }

    public void LoadSceneAsync(SceneItem newScene) {
        StartCoroutine(C_LoadSceneAsync(newScene));
    }

    public IEnumerator C_LoadSceneAsync(SceneItem newScene) {
        
        _currentSceneItem = newScene;

        var asyncOperation = SceneManager.LoadSceneAsync(_currentSceneItem.name);

        while (asyncOperation.isDone) {
            yield return 0;
        }
    }
}
