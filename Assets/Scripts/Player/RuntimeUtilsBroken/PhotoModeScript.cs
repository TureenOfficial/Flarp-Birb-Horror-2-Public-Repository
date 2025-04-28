using UnityEngine;

public class PhotoModeScript : MonoBehaviour
{
    public static PhotoModeScript Instance { get; set; }

    [Header("UI")]
    [SerializeField] bool UIEnabled;
    [SerializeField] GameObject PhotoButtonUI;
    [SerializeField] GameObject DefaultUI;

    [Header("Mechanics")]
    [SerializeField] public bool PhotoModeActive;

    [Header("Input Handling")]
    [SerializeField] KeyCode PhotoModeKeyCode = KeyCode.F1;

    private void Awake()
    {
        PhotoModeActive = true; 
        if(Instance == null)
        {
            Instance = this;
        }
    }

    void Start()
    {
        if(!GameDetail.Instance.playerHasCamera)
        {
            PhotoButtonUI.SetActive(false);
        }
    }

    void Update()
    {
        if(GameDetail.Instance.gameActive && !PlayerMap.Instance.mapActive && GameDetail.Instance.playerHasCamera && !PauseMenu.GamePaused)
        {
            if(Input.GetKeyDown(PhotoModeKeyCode) && PhotoModeActive)
            {
                Time.timeScale = 0;
                PhotoModeActive = false;
                DefaultUI.SetActive(false);
            }
            else if(Input.GetKeyDown(PhotoModeKeyCode) && !PhotoModeActive)
            {
                Time.timeScale = 1;
                PhotoModeActive = true;
                DefaultUI.SetActive(true);
            }
        }

    }
}
