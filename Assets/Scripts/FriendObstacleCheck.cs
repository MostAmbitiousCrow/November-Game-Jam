using System.Collections;
using UnityEngine;
using CarterGames.Assets.AudioManager;

public class FriendObstacleCheck : MonoBehaviour
{
    [SerializeField] FriendRole _friendRoleRequirement;
    [SerializeField] Transform _artTransform;
    [SerializeField] InspectorAudioClipPlayer _audioPlayer;
    [SerializeField] string _hitSound;

    private bool _isShaking;
    private float _shakeTime;
    
    //Script goes on obstacle
    //When a friend triggers it, check if the current friend type matches the friend needed
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Friend"))
        {
            var friend = other.GetComponent<Friend_Controller>();
            if (_friendRoleRequirement == friend.Role)
            {
                Debug.Log("Correct Friend");
                _audioPlayer.Play();
                gameObject.SetActive(false);
                Destroy(gameObject, 1f);
            }
            else
            {
                if (_isShaking) _shakeTime = 0f;
                else StartCoroutine(Shake());

                //if(_hitSound != null)
                //    AudioManager.Play(_hitSound);
                _audioPlayer.Play();

                Debug.Log("Incorrect Friend");
            }
        }
    }

    IEnumerator Shake(int shakes = 10, float shakeAmount = 1f)
    {
        _shakeTime = 0f;
        _isShaking = true;

        Vector3 _initialPos = _artTransform.position;

        for (int i = 0; i < 10; i++)
        {
            transform.localPosition += new Vector3(shakeAmount, 0, 0);
            yield return new WaitForSeconds(0.01f);
            transform.localPosition -= new Vector3(shakeAmount, 0, 0);
            yield return new WaitForSeconds(0.01f);
        }
        _artTransform.position = _initialPos;
        _isShaking = false;
    }
}