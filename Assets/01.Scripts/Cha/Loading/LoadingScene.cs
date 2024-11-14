using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingScene : MonoBehaviour
{
    [Header("UIs")] 
    public Slider progressBar;
   // public Text textPercent;

    private void Start()
    {
        StartCoroutine(LoadNextSceneAsync());
    }
    
    private IEnumerator LoadNextSceneAsync()
    {
        string sceneName = GameSceneDataManager.Instance.LoadSceneName;
        
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        asyncLoad.allowSceneActivation = false;

        while (!asyncLoad.isDone)
        {
            float progress = Mathf.Clamp01(asyncLoad.progress / 0.9f);
            progressBar.value = progress;
          
            if (asyncLoad.progress >= 0.9f)
            { 
                asyncLoad.allowSceneActivation = true;
            }

            yield return null;
        }

        // 로딩이 완료된 후 `Scene` 객체를 가져와서 전달
        Scene loadedScene = SceneManager.GetSceneByName(sceneName);
        GameSceneDataManager.Instance.OnSceneLoaded(loadedScene, LoadSceneMode.Single); // 로딩 완료 후 씬 이동
    }
}
