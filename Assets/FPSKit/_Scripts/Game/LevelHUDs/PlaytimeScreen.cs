using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlaytimeScreen : HUDScreen
{
    [Header("Playtime Screen")]
    [SerializeField]
    private Text _playtimeTextGUI;
    // tracking min/sec state for optimization
    private int _elapsedMinutes = 0;
    private int _elapsedSeconds = 0;

    public void IncrementPlaytimeDisplay(float elapsedTime)
    {
        int minutes = TimeHelper.ConvertToMin(elapsedTime);
        int seconds = TimeHelper.ConvertToSec(elapsedTime);
        // only reformat time if a value has changed since last update, for optimization
        if (minutes != _elapsedMinutes || seconds != _elapsedSeconds)
        {
            _playtimeTextGUI.text = TimeHelper.FormatTime(minutes, seconds);
        }
        _elapsedMinutes = minutes;
        _elapsedSeconds = seconds;
    }
}
