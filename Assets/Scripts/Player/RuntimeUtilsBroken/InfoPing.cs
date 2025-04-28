using System.Collections;
using TMPro;
using UnityEngine;

public class InfoPing : MonoBehaviour
{
    public static InfoPing Instance { get; private set; }

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
    }

    [SerializeField] TMP_Text DisplayedText;
    [SerializeField] Animator _Animator;
    [SerializeField] bool DeactivateAfterPing;
    [SerializeField] public int TimesActivated;

    void Start()
    {
        TimesActivated = 0;
        gameObject.SetActive(false);   
    }

    public void Ping(string PingedText, float OnScreenTime, bool DeactivateOnPing)
    {
        gameObject.SetActive(false);
        TimesActivated ++;
        
        if(DeactivateOnPing && TimesActivated > 1)
        {
            //No
        }
        else
        {
            print(TimesActivated);

            DisplayedText.text = PingedText.ToUpper();

            gameObject.SetActive(true);
            StartCoroutine(Close(OnScreenTime));
        }
    }

    IEnumerator Close(float Length)
    {
        yield return new WaitForSeconds(Length);
        _Animator.SetTrigger("Exit");
        yield return new WaitForSeconds(_Animator.GetCurrentAnimatorClipInfo(0).Length);
        gameObject.SetActive(false);
    }
}
