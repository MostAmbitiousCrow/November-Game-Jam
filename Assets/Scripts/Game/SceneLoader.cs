using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Animator))]
public class SceneLoader : MonoBehaviour
{
    [SerializeField] private float startTransitionTime, endTransitionTime = 1f;

    private Animator _animator;

    // Events
    public static Action SceneLoadTransitionStarted; // When the loading 
    public static Action SceneLoadStarted; // When the loading process starts (after transition)
    public static Action SceneLoadFinished; // When the scene has actually loaded
    public static Action SceneLoadTransitionEnded; // When the transition has finished (after scene load)

    public static Action<string> LoadSceneRequest;
    public static bool IsLoadingScene { get; private set; }

    void Start()
    {
        // Prevent Duplicate Scene Loaders
        var count = FindObjectsByType<SceneLoader>(FindObjectsInactive.Include, FindObjectsSortMode.InstanceID).Length;
        if (count > 1)
        {
            Destroy(gameObject);
            return;
        }

        SceneLoadTransitionStarted += () => _animator.SetTrigger("Started");
        SceneLoadStarted += () => _animator.SetTrigger("Loaded");
        SceneLoadFinished += () => _animator.SetTrigger("Finished");
        SceneLoadTransitionEnded += () => _animator.SetTrigger("Ended");

        LoadSceneRequest += LoadScene;

        DontDestroyOnLoad(gameObject);
        _animator = GetComponent<Animator>();
    }

    private void OnDestroy()
    {
        SceneLoadTransitionStarted -= () => _animator.SetTrigger("Started");
        SceneLoadStarted -= () => _animator.SetTrigger("Loading");
        SceneLoadFinished -= () => _animator.SetTrigger("Finished");
        SceneLoadTransitionEnded -= () => _animator.SetTrigger("Ended");

        LoadSceneRequest -= LoadScene;
    }

    private Coroutine _currentLoadingRoutine;
    public void LoadScene(string sceneName)
    {
        if (_currentLoadingRoutine != null)
        {
            return;
        }
        if (SceneManager.GetSceneByName(sceneName) == null)
        {
            Debug.LogError($"Scene '{sceneName}' not found. Make sure it is added to the build settings!");
            return;
        }
        _currentLoadingRoutine = StartCoroutine(LoadSceneRoutine(sceneName));
    }

    private IEnumerator LoadSceneRoutine(string sceneName)
    {
        IsLoadingScene = true;
        SceneLoadTransitionStarted?.Invoke();

        yield return new WaitForSeconds(startTransitionTime);

        SceneLoadStarted?.Invoke();
        var async = SceneManager.LoadSceneAsync(sceneName);

        yield return new WaitUntil(() => async.progress > .9f);
        async.allowSceneActivation = true;
        yield return new WaitForSeconds(.5f);
        SceneLoadFinished?.Invoke();

        yield return new WaitForSeconds(endTransitionTime);

        SceneLoadTransitionEnded?.Invoke();

        IsLoadingScene = false;

        _currentLoadingRoutine = null;
    }

}
