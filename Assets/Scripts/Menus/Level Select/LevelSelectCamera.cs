using UnityEngine;

public class LevelSelectCamera : MonoBehaviour
{
    [SerializeField] private float smoothTime = .5f;
    [SerializeField] private Transform target;
    private Vector3 _velocity = Vector3.one;

    public void AssignTarget(Transform target)
    {
        this.target = target;
    }

    private void LateUpdate()
    {
        if (target == null) return;

        transform.position = Vector3.SmoothDamp(transform.position, target.position, ref _velocity, smoothTime);
    }
}
