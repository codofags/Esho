using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum ENUM_Device_Type
{
    Tablet,
    Phone
}

public static class DeviceChecker 
{
    public static bool isTablet;

    private static float DeviceDiagonalSizeInInches()
    {
        float screenWidth = Screen.width / Screen.dpi;
        float screenHeight = Screen.height / Screen.dpi;
        float diagonalInches = Mathf.Sqrt(Mathf.Pow(screenWidth, 2) + Mathf.Pow(screenHeight, 2));

        return diagonalInches;
    }

    private static bool DeviseIsTablet()
    {
#if UNITY_IOS
    bool deviceIsIpad = UnityEngine.iOS.Device.generation.ToString().Contains("iPad");
    return deviceIsIpad;
#elif UNITY_ANDROID
    float aspectRatio = Mathf.Max(Screen.width, Screen.height) / Mathf.Min(Screen.width, Screen.height);
    bool isTablet = (DeviceDiagonalSizeInInches() > 6.5f && aspectRatio < 2f);
    return isTablet;
#else
        return false;
#endif
    }

    internal static void SetupCanvas(List<Canvas> canvases)
    {
        var isTablet = DeviseIsTablet();

        if (isTablet)
        {
            foreach (var canvas in canvases)
            {
                var canvasScaler = canvas.GetComponent<CanvasScaler>();
                canvasScaler.matchWidthOrHeight = 1;
            }
        }
    }
}
