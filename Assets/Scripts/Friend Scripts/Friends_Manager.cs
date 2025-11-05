using System.Collections.Generic;
using UnityEngine;

public class Friends_Manager : MonoBehaviour
{
    public static List<Friend_Controller> Friends;

    private void Awake()
    {
        Friend_Controller[] friendsArray = FindObjectsByType<Friend_Controller>(FindObjectsSortMode.None);
        Friends = new List<Friend_Controller>(friendsArray);
    }
}
