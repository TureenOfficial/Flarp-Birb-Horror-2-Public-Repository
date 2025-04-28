using UnityEngine;
using System.Collections;
using System;

public class TutorialTrigger : MonoBehaviour
{
    [Header("Trigger Settings")]
    public string[] attributeToChange;
    public bool SetTrue;
    public float[] SolidColorTagRGB;


    public void OnTriggerEnter(Collider collider)
    {
        if(collider.CompareTag("Player"))
        {
            RuntimeUtils utils =collider.GetComponent<RuntimeUtils>();


                foreach (string att in attributeToChange)
                {
                    switch(att.ToLower())
                    {
                        case "map":
                        {
                            utils.isMapActive = SetTrue;
                            break;
                        }
                        case "flashlight":
                        {
                            utils.isFlashlight = SetTrue;
                            break;
                        }
                        case "screenshot":
                        {
                            utils.isScreenshot = SetTrue;
                            break;
                        }
                        case "skyboxoff":
                        {
                            Camera cam = utils.playerCameraObject.GetComponent<Camera>();

                            RenderSettings.ambientLight = new Color(SolidColorTagRGB[0], SolidColorTagRGB[1], SolidColorTagRGB[2]);
                            cam.clearFlags = CameraClearFlags.SolidColor;
                            cam.backgroundColor = new Color(SolidColorTagRGB[0], SolidColorTagRGB[1], SolidColorTagRGB[2]);
                            break;
                        }
                }
            }       
        }
    }
}
