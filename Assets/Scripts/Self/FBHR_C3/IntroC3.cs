using UnityEngine;
using UnityEngine.UI;

public class IntroC3 : MonoBehaviour
{
    float transparency;
    GameObject staticGameObject;
    RawImage rawImage;
    void Start()
    {
        rawImage = staticGameObject.GetComponent<RawImage>();
    }
    void Update()
    {
        rawImage.color = new Color(rawImage.color.r, rawImage.color.g, rawImage.color.b, transparency);
        while (rawImage.color.a > 0)
        {
            transparency -= 2 * Time.deltaTime;
        }    
    }
}
