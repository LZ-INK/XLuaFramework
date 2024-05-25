using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MySceneManager : MonoBehaviour
{
    private string m_LogicName = "[SceneLogic]";

    private void Awake()
    {
        SceneManager.activeSceneChanged += OnActiveSceneChanged;
    }

    private void OnActiveSceneChanged(Scene arg0, Scene arg1)
    {
        if (!arg0.isLoaded|| !arg1.isLoaded)
        {
            return;
        }

        SceneLogic logic1 = GetSceneLogic(arg0);
        SceneLogic logic2 = GetSceneLogic(arg1);
        logic1?.OnInActive();
        logic2?.OnActive();
    }

    /// <summary>
    /// 激活场景
    /// </summary>
    /// <param name="sceneName"></param>
    public void SetActive(string sceneName)
    {
        Scene scene = SceneManager.GetSceneByName(sceneName);
        SceneManager.SetActiveScene(scene);
    }
    /// <summary>
    /// 叠加加载场景
    /// </summary>
    /// <param name="scenenName"></param>
    /// <param name="luaName"></param>
    public void LoadScene(string scenenName, string luaName)
    {
        Manager.Resource.LoadScene(scenenName, (UnityEngine.Object obj) =>
        {
            StartCoroutine(StartLoadScene(scenenName, luaName,LoadSceneMode.Additive));
        });
    }

   

    /// <summary>
    /// 切换场景
    /// </summary>
    /// <param name="scenenName"></param>
    /// <param name="luaName"></param>
    public void ChangeScene(string scenenName, string luaName)
    {
        Manager.Resource.LoadScene(scenenName, (UnityEngine.Object obj) =>
        {
            StartCoroutine(StartLoadScene(scenenName, luaName, LoadSceneMode.Single));
        });
    }


    IEnumerator StartLoadScene(string sceneName,string luaName,LoadSceneMode mode)
   {
        if (IsLoadScene(sceneName))
        {
            yield break;
        }
        AsyncOperation async = SceneManager.LoadSceneAsync(sceneName,mode);
        async.allowSceneActivation = true; //设置为true 加载完全
        yield return async;

        Scene scene = SceneManager.GetSceneByName(sceneName);
        GameObject go = new GameObject(m_LogicName);

        SceneManager.MoveGameObjectToScene(go, scene);

        SceneLogic logic = go.AddComponent<SceneLogic>();
        logic.SceneName = sceneName;
        logic.Init(luaName);
        logic.OnEnter();
    }
    /// <summary>
    /// 判断加载
    /// </summary>
    /// <param name="sceneName"></param>
    /// <returns></returns>
    private bool IsLoadScene(string sceneName)
    {
        Scene scene = SceneManager.GetSceneByName(sceneName);
        return scene.isLoaded;
    }

    public IEnumerator UnLoadScene(string sceneName)
    {
        Scene scene = SceneManager.GetSceneByName(sceneName);
        if (!scene.isLoaded)
        {
            Debug.Log("scene not load");
            yield break;
        }
        SceneLogic logic = GetSceneLogic(scene);
        logic?.OnQuit();
        AsyncOperation async = SceneManager.UnloadSceneAsync(sceneName);

        yield return async;

    }

    private SceneLogic GetSceneLogic(Scene scene)
    {
        GameObject[] gameObjects = scene.GetRootGameObjects();
        foreach (GameObject go in gameObjects)
        {
            if (go.name.CompareTo(m_LogicName) ==0)
            {
                SceneLogic sceneLogic = go.GetComponent<SceneLogic>();
                return sceneLogic;
            }
        }
        return null;
    }
}
