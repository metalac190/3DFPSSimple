using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class helps deal with time conversions and display
/// </summary>
public static class TimeHelper
{
    public static int ConvertToMin(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60);
        return minutes;
    }

    public static int ConvertToSec(float time)
    {
        int seconds = Mathf.FloorToInt(time % 60);
        return seconds;
    }

    public static string FormatTime(int minutes, int seconds)
    {
        // account for 0 position
        string formattedSeconds;
        if (seconds < 10)
            formattedSeconds = "0" + seconds.ToString();
        else
            formattedSeconds = seconds.ToString();

        string formattedTime = string.Format(minutes + ":" + formattedSeconds);
        return formattedTime;
    }
}
