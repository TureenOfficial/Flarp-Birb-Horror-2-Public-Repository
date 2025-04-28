using UnityEngine;
using TMPro;
using Unity.VisualScripting;

public class PlayerMap : MonoBehaviour
{
    public static PlayerMap Instance { get; set; }

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
    }

    [Header("UI")]
    [SerializeField] bool UIEnabled;
    [SerializeField] GameObject MapButtonUI;

    [SerializeField] GameObject ForceMapUI;
    [SerializeField] GameObject ForcePlayerUI;
    [SerializeField] TMP_Text MapNameText;

    [Header("Mechanics")]
    [SerializeField] GameObject mapCameraObject;
    Camera MapCamera;
    [SerializeField] GameObject playerCameraObject;
    [SerializeField] public bool mapActive;

    [SerializeField] float zoomMin, zoomMax;

    [Header("Input Handling")]
    [SerializeField] KeyCode PlayerMapKeyCode = KeyCode.M;

    void Start()
    {
        MapNameText.text = $"IN MAP: {GameDetail.Instance.GameTitle} CHAPTER {GameDetail.Instance.GameChapterNumber}";
        MapCamera = mapCameraObject?.GetComponent<Camera>();
        mapActive = false;

        if(playerCameraObject == null) playerCameraObject = GameObject.Find("Main Camera"); 
        if(mapCameraObject == null) mapCameraObject = GameObject.Find("Map Camera");

        if(!GameDetail.Instance.playerHasMap)
        {
            UIEnabled = false;
            ForceMapUI.SetActive(false);
            MapButtonUI.SetActive(false);
        }
    }

    void Update()
    {
        if(!GameDetail.Instance.playerHasMap || !PhotoModeScript.Instance.PhotoModeActive || !GameDetail.Instance.gameActive || PauseMenu.GamePaused)
        {
            mapCameraObject.SetActive(false);
            mapActive = false;
            return;
        }
        else
        {
            mapCameraObject.SetActive(mapActive);
            playerCameraObject.SetActive(!mapActive);
            if(mapActive && Input.GetKeyDown(PlayerMapKeyCode))
                {
                    Time.timeScale = 1;
                    ForceMapUI.SetActive(false);
                    ForcePlayerUI.SetActive(true);
                    mapActive = false; // MAP IS NOT ACTIVE
                    PlayerController.canMove = true;
                }
            else if(!mapActive && Input.GetKeyDown(PlayerMapKeyCode))
                {
                    Time.timeScale = 0;
                    ForceMapUI.SetActive(true);
                    ForcePlayerUI.SetActive(false);
                    mapActive = true; // MAP IS ACTIVE
                    PlayerController.canMove = false;
                }

            if(mapActive)
            {
                float zoomSpeed = 10000f;
                float targetFOV = MapCamera.fieldOfView;

                if (Input.GetKey(KeyCode.E)) targetFOV += zoomSpeed * Time.unscaledDeltaTime;
                if (Input.GetKey(KeyCode.Q)) targetFOV -= zoomSpeed * Time.unscaledDeltaTime;

                targetFOV = Mathf.Clamp(targetFOV, zoomMin, zoomMax);
                MapCamera.fieldOfView = Mathf.Lerp(MapCamera.fieldOfView, targetFOV, 5f * Time.unscaledDeltaTime);
            }

            if(UIEnabled)
            {
                MapButtonUI.SetActive(true);
                MapButtonUI.GetComponentInChildren<TMP_Text>().text = PlayerMapKeyCode.ToString().ToUpper();
            }
            else if(!UIEnabled)
            {
                MapButtonUI.SetActive(false);
            }
        }
    }
}
