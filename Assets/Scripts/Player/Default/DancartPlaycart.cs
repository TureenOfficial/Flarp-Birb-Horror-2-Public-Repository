 using UnityEngine;

public class DancartPlaycart : MonoBehaviour
{
    //NOT USED ANYMORE
    [SerializeField] int wrongGuess = 0;
    [SerializeField] int attempts = 9;
    public void UpdateWrongGuess(int amt)
    {
        wrongGuess += amt;
        if(wrongGuess > attempts)
        {
            RuntimeUtils utils = GameObject.Find("Player").GetComponent<RuntimeUtils>();
            utils.OnDeath();
        }
    }
}
