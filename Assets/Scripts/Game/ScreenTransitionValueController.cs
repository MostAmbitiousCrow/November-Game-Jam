using UnityEngine;
using UnityEngine.UI;
using EditorAttributes;

public class ScreenTransitionValueController : MonoBehaviour
{
    [SerializeField] private Material transitionMaterial;
    [SerializeField] private Image transitionImage;
    [Space]
    [SerializeField] private bool isActive;
    // Scale
    [SerializeField, MinMaxSlider(0f, 10f)] private Vector2 scaleMinMax;
    [SerializeField] private float scaleValue;
    // Rotation
    [SerializeField, MinMaxSlider(0f, 6.28f)] private Vector2 rotationMinMax;
    [SerializeField] private float rotationValue;

    void Start()
    {
        transitionMaterial = new Material(transitionMaterial);
        transitionImage.material = transitionMaterial;
    }

    private void OnEnable()
    {
        SceneLoader.SceneLoadTransitionStarted += () => isActive = true;
        SceneLoader.SceneLoadTransitionEnded += () => isActive = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isActive) return;

        var sV = Mathf.Lerp(scaleMinMax.y, scaleMinMax.x, scaleValue);
        var rV = Mathf.Lerp(rotationMinMax.y, rotationMinMax.x, rotationValue);
        transitionMaterial.SetFloat("_Scale", sV);
        transitionMaterial.SetFloat("_Rotation", rV);
    }
}
