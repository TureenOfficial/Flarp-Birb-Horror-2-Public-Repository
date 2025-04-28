using UnityEngine;

public class InheritBehaviour : MonoBehaviour
{
    public string behaviourToInherit;
    public void Click()
    {
        PlayerPrefs.SetString("inheritedBehaviour", behaviourToInherit);
    }
}
