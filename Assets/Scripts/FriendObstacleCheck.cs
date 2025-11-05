using UnityEngine;

public class FriendObstacleCheck : MonoBehaviour
{
    [SerializeField] FriendRole _friendRoleRequirement;
    
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
                Destroy(gameObject);
            }
            else
            {
                Debug.Log("Incorrect Friend");
            }
        }
    }
}