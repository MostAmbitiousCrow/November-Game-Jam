using UnityEngine;

public class Letter_Controller : Character_Controller_Script
{
    public override void OnThrown()
    {
        base.OnThrown();

    }

    public override void PlayerDetected()
    {
        base.PlayerDetected();
        Friend_Chain_Controller.instance.AddFriend(this);
    }
}
