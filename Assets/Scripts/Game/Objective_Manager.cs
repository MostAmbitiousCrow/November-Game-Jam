using UnityEngine;

public class Objective_Manager : MonoBehaviour
{
    public static Objective_Manager Instance;

    [Header("Stats")]
    [SerializeField] int _friendsSaved;
    [SerializeField] int _savedFriendsRequirement = 10;
    [SerializeField] int _deliveredLettersRequirement = 9;
    [Space]
    [SerializeField] bool _isAllFriendsSaved;
    [SerializeField] bool _isAllLettersDelivered;
    [Header("Components")]
    [SerializeField] Canvas _victoryCanvas;

    private void Awake()
    {
        Instance = this;
    }

    public void SavedFriend()
    {
        _friendsSaved++;

        if(_friendsSaved >= _savedFriendsRequirement)
        {

            print("All friends saved!");
        }

    }

    public void DeliveredLetter()
    {

    }

    public bool CheckVictoryCondition()
    {
        return _isAllFriendsSaved && _isAllLettersDelivered;
    }

}
