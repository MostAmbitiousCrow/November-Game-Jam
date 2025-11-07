using System;
using System.Collections;
using UnityEngine;
using EditorAttributes;

public class Menu_Transition_Controller : MonoBehaviour
{
    // Delegates and Status variables
    public bool IsTransitioning { get { return _isTransitioning; } }

    /// <summary> Called whenever the transition has started </summary>
    public static event Action<int> OnTransitionStarted;
    /// <summary> Called whenever the transition has begun waiting </summary>
    public static event Action<int, int> OnTransitionWaiting;
    /// <summary> Called whenever the transition has finished waiting </summary>
    public static event Action<int> OnTransitionWaitCompleted;
    /// <summary> Called whenever the transition has been completed </summary>
    public static event Action OnTransitionCompleted;

    [Header("Transition Settings")]
    [SerializeField] bool _isTransitioning;
    [Space]
    [SerializeField] bool _doTransitionAnimation;
    [Space]
    [SerializeField, ShowField(nameof(_doTransitionAnimation))] float _transitionStartTime = .1f;
    [SerializeField, ShowField(nameof(_doTransitionAnimation))] float _transitionWaitTime = .3f;
    [SerializeField, ShowField(nameof(_doTransitionAnimation))] float _transitionEndTime = .1f;

    [Header("Dependency")]
    [SerializeField] Menu_Manager _mainMenuManager;

    /// <summary>
    /// Method to trigger the screen transition
    /// </summary>
    /// <param name="screenToOpen"> The Screen contents to enable (open) </param>
    /// <param name="screenToClose"> The Screen contents to disable (close)</param>
    /// <param name="multiplier"> Optional. Multiply the speed of the transition</param>
    public void TriggerTransition(int screenToOpen, int screenToClose,
        bool doTransition = true, float multiplier = 1f)
    {
        if(doTransition && _doTransitionAnimation) StartCoroutine(TransitionRoutine(screenToOpen, screenToClose, multiplier));
        else
        {
            OnTransitionStarted.Invoke(screenToOpen);
            OnTransitionWaiting.Invoke(screenToOpen, screenToClose);
            OnTransitionWaitCompleted.Invoke(screenToOpen);
            OnTransitionCompleted?.Invoke();
        }
    }

    IEnumerator TransitionRoutine(int screenToOpen, int screenToClose, float speed = 1f)
    {
        _isTransitioning = true;

        // Trigger Transition Started delegate
        OnTransitionStarted.Invoke(screenToOpen);

        float t = 0f;
        while (t * speed < _transitionStartTime)
        {
            // Insert your Start Transition Animation code here

            yield return t += Time.unscaledDeltaTime;
        }

        t = 0f;
        OnTransitionWaiting?.Invoke(screenToOpen, screenToClose);
        while (t * speed < _transitionWaitTime)
        {
            // Insert your Transition Animation code here

            yield return t += Time.unscaledDeltaTime;
        }
        OnTransitionWaitCompleted?.Invoke(screenToOpen);

        t = 0f;
        while (t * speed < _transitionEndTime)
        {
            // Insert your End Transition Animation code here

            yield return t += Time.unscaledDeltaTime;
        }

        // Trigger Transition Completed delegate
        OnTransitionCompleted?.Invoke();

        _isTransitioning = false;

        yield break;
    }
}
