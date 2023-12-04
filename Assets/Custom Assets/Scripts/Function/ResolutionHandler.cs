using UnityEngine;

public class ResolutionHandler : MonoBehaviour
{

    [SerializeField]
    int refWidth = 1920, refHeight = 1080;

    [SerializeField]
    bool fullScreenMode = true;

    private void Awake()
    {
        // check valid of refWidth and refHeight
        refWidth = refWidth <= 0 ? 1920 : refWidth;
        refHeight = refHeight <= 0 ? 1080 : refHeight;

        // get main display resolution
        int displayWidth = Display.main.systemWidth;
        int displayHeight = Display.main.systemHeight;

        // calculate ref resolution ratio and display resolution ration
        float refRatio = (float)refHeight / (float)refWidth;
        float displayRatio = (float)displayHeight / (float)displayWidth;

        // calculate desired resolution
        int desiredWidth = 0;
        int desiredHeight = 0;

        if (refRatio <= displayRatio)
        {
            desiredWidth = displayWidth;
            desiredHeight = Mathf.FloorToInt(displayWidth * refRatio);
        }
        else
        {
            desiredHeight = displayHeight;
            desiredWidth = Mathf.FloorToInt(displayHeight / refRatio);
        }

        // set resolution
        Screen.SetResolution(desiredWidth, desiredHeight, fullScreenMode);
    }

}
