using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Friend_Chain_Controller : MonoBehaviour
{
    //public List<Hand_Connector> ConnectedHands { get { return _connectedHands; } }
    [SerializeField] List<Hand_Connector> _connectedHands;

    public void AddFriend(Hand_Connector hand)
    {
        _connectedHands.Insert(_connectedHands.Count, hand);
    }

    public void RemoveFriend(Hand_Connector hand)
    {
        if (_connectedHands.Contains(hand))
            _connectedHands.Remove(hand);
    }

    public Hand_Connector GetLastFriend()
    {
        if (_connectedHands != null)
            return _connectedHands[^1];
        else
            return null;
    }
}
