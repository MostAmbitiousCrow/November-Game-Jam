using UnityEngine;

public class FriendObstacleCheck : MonoBehaviour
{
    [SerializeField] private string friendNeeded; //Change to enum in final script!!!
    
    //Script goes on obstacle
    //When a friend triggers it, check if the current friend type matches the friend needed
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Friend"))
        {
            var script = other.GetComponent<FriendScript>();
            if (friendNeeded == script.friendType)
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
