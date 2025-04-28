using TMPro;
using UnityEngine;

public class ObjectiveDisplay : MonoBehaviour
{
    [SerializeField] TMP_Text ObjectiveText;
    public void Update()
    {
        if(GameDetail.Instance.GameObjective != "")
        {
            ObjectiveText.text = $"OBJECTIVE: {GameDetail.Instance.GameObjective.ToUpper()}";
        }
        else
        {
            ObjectiveText.text = $"OBJECTIVE: NO OBJECTIVE GIVEN...";
        }
    }
}
