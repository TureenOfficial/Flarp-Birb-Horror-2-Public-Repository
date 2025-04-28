using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.UIElements;
using TMPro;

public class CheatsGUI : MonoBehaviour
{
    public GameObject birb;
    public TextMeshProUGUI debugText;
    [SerializeField] bool CheatsEnabled;
    bool ConsoleActive;
    KeyCode cheatKey = KeyCode.P;
    string inputCortex= "";
    public List<object> commandList;
    [SerializeField] bool helper;
    [SerializeField] TMP_FontAsset customFont;

    void Update()
    {
        if(CheatsEnabled)
        {
            if(Input.GetKeyDown(cheatKey))
            {
                DebugToggled();
            }
            if(Input.GetKeyDown(KeyCode.Return))
            {
                if(ConsoleActive) OnReturn();
            }
        }
    }
    public void DebugToggled()
    {
        ConsoleActive = !ConsoleActive;

        if(!ConsoleActive)
        {
            UnityEngine.Cursor.visible = false;
            UnityEngine.Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            UnityEngine.Cursor.visible = true;
            UnityEngine.Cursor.lockState = CursorLockMode.None;
        }
    }
    public void OnReturn()
    {
        UnityEngine.Cursor.visible = false;
        UnityEngine.Cursor.lockState = CursorLockMode.Locked;
        if(ConsoleActive)
        {
            HandleInput();
            inputCortex = "";
        }
    }
    private void Awake()
    {
        var TOGGLE_BIRB = new CheatCommand<bool>("birb_on", "Sets the birb active or not, birb has to be active first", "birb_on <bool>", (birbOn) =>
        {
            Debug.Log($"birb_on: {birbOn}");

            if(birb != null) birb?.SetActive(birbOn);

            if (birbOn)
            {
                var flarp = birb.GetComponent<New_FlarpBehaviour>();
                if (flarp != null)
                {
                    flarp.Summon();
                }
                else
                {
                    Debug.LogWarning("New_FlarpBehaviour component not found on the birb object.");
                }
            }
        });

        var WE_HAVE_TO_GET = new CheatCommand<int>("we_have_to_get", "Adds amount to reward", "we_have_to_get <Amount>", (moneyAmount)=>
        {
            print($"we_have_to_get: {moneyAmount}");
            MoneyManager.Instance.AddTotal(moneyAmount);
        });

        var MAP_ACTIVE = new CheatCommand<bool>("where_am_i", "Toggles the map", "where_am_i <bool>", (mapIsOn) =>
        {
            print($"where_am_i: {mapIsOn}");
            GameDetail.Instance.playerHasMap = mapIsOn;
        });

        var DURANDURAN = new CheatCommand<bool>("duranduran", "INVISIBLE", "duranduran <bool>", (invisible) =>
        {
            print($"duranduran: {invisible}");

            if(invisible)
            {
                birb.GetComponent<New_FlarpBehaviour>().detectionRange = 0;
            }
            else
            {
                birb.GetComponent<New_FlarpBehaviour>().detectionRange = birb.GetComponent<New_FlarpBehaviour>().SavedDetectionRange;
            }
        });

        var COLLECTMYPAGES = new CheatCommand<int>("collect_my_pages", "Sets collectable amount", "collect_my_pages <int>", (amount) =>
        {
            GameDetail.Instance.ScoreCurrent = amount;
        });

        var WHATHOWMANY = new CheatCommand<int>("what_how_many", "Sets amount needed", "what_how_many <int>", (amount) =>
        {
            GameDetail.Instance.ScoreNeeded = amount;
        });


        commandList = new List<object>
        {
            TOGGLE_BIRB,
            WE_HAVE_TO_GET,
            MAP_ACTIVE,
            DURANDURAN,
            COLLECTMYPAGES,
            WHATHOWMANY
        };
    }

    UnityEngine.Vector2 scroll;
    private void OnGUI()
    {
        if (!ConsoleActive) return;
        
        
        GUI.contentColor = Color.green;
        GUI.color = Color.green;

        GUIStyle customStyle = new GUIStyle(GUI.skin.label);
        customStyle.fontSize = 12;

        
        GUIStyle backgroundStyle = new GUIStyle(GUI.skin.box);
        Texture2D bgTexture = new Texture2D(1, 1);
        bgTexture.SetPixel(0, 0, new Color32(30, 30, 30, 255));
        bgTexture.Apply();
        backgroundStyle.normal.background = bgTexture;

        float y = 0f;

        UnityEngine.Cursor.visible = true;
        UnityEngine.Cursor.lockState = CursorLockMode.None;

        if (helper)
        {
            GUI.Box(new Rect(0, y, Screen.width, 100), GUIContent.none, backgroundStyle);

            Rect viewport = new Rect(0, 0, Screen.width - 30, 20 * commandList.Count);
            scroll = GUI.BeginScrollView(new Rect(0, y + 5f, Screen.width, 90), scroll, viewport);

            for (int i = 0; i < commandList.Count; i++)
            {
                CheatBase command = commandList[i] as CheatBase;
                if (command == null) continue;

                string labeler = $"{command.commandFormat} | {command.commandDesc}";
                Rect labelRect = new Rect(5, 20 * i, viewport.width - 100, 20);

                GUI.Label(labelRect, labeler, customStyle);
            }

            GUI.EndScrollView();
            y += 100;
        }

        // Draw solid input area background
        GUI.Box(new Rect(0, y, Screen.width, 40), GUIContent.none, backgroundStyle);

        inputCortex = GUI.TextField(new Rect(10f, y + 5f, Screen.width - 20f, 20f), inputCortex);
    }


    private void HandleInput()
    {
        string[] property = inputCortex.Split(' ');
        for(int i=0; i<commandList.Count; i++)
        {
            CheatBase cheatBase = commandList[i] as CheatBase;

            if(inputCortex.Contains(cheatBase.commandID))
            {
                if(commandList[i] as CheatCommand != null) //Fit cast?
                {
                    (commandList[i] as CheatCommand).Invoke();
                    return;
                }
                else if(commandList[i] as CheatCommand<int> != null)
                {
                    (commandList[i] as CheatCommand<int>).Invoke(int.Parse(property[1]));
                    return;
                }
                else if (commandList[i] is CheatCommand<bool> && property.Length > 1 && bool.TryParse(property[1], out bool boolVal))
                {
                    (commandList[i] as CheatCommand<bool>).Invoke(boolVal);
                    return;
                }
            }
        }
    }
}
