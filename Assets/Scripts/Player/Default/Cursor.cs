using UnityEngine;

public class Cursor : MonoBehaviour
{
    [SerializeField] Ray ray;
    [SerializeField] RuntimeUtils utils;
    [SerializeField] float localSize; // Serialized field for cursor size
    [SerializeField] GameObject cursorObject;
    [SerializeField] UnityEngine.UI.RawImage cursorImage;
    [SerializeField] Texture[] textures;
    [SerializeField] public float raycastDistance = 2f;

    void Start()
    { 
        raycastDistance = PlayerDetails.PlayerRaycastDistance;
        string hexColor = PlayerPrefs.GetString("cursorColor", "ffffff");
        cursorImage.color = HexToColor(hexColor);

        localSize = 0.3f * PlayerPrefs.GetFloat("curSizePercentage", 100) / 100;
        cursorObject.transform.localScale = new Vector2(localSize, localSize);
    }

    private Color HexToColor(string hex)
    {
        if (hex.StartsWith("#"))
        {
            hex = hex.Substring(1);
        }

        if (hex.Length != 6)
        {
            return Color.white;
        }
        byte r = byte.Parse(hex.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
        byte g = byte.Parse(hex.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
        byte b = byte.Parse(hex.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);

        return new Color(r / 255f, g / 255f, b / 255f);
    }

    public void Update()
    {
        if(!GameDetail.Instance.gameActive)
        {
            return;
        }
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        

        if (Physics.Raycast(ray, out RaycastHit hit, PlayerDetails.PlayerRaycastDistance))
        {
            string tagName = hit.collider.gameObject.tag; 

            switch(tagName) //use switch for efficiency
            {
                case "lockedCursor":
                {
                    cursorImage.texture = textures[2];
                    break;
                }
                case "lightCursor":
                {
                    cursorImage.texture = textures[1];
                    break;
                }
                default:
                {
                    cursorImage.texture = textures[0];
                    break;
                }
            }
        }
        else
        {
            if (textures.Length > 0)
            {
                cursorImage.texture = textures[0];
            }
        }
    }
}
