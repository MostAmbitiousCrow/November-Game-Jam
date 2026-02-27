using System.Collections.Generic;
using UnityEngine;

public class Friends_Manager : MonoBehaviour
{
    public static List<NewFriendController> Friends;

    private void Awake()
    {
        NewFriendController[] friendsArray = FindObjectsByType<NewFriendController>(FindObjectsSortMode.None);
        Friends = new List<NewFriendController>(friendsArray);
    }
}
