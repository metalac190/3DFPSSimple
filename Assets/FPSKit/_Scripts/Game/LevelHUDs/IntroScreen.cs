using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IntroScreen : HUDScreen
{
    [SerializeField]
    private LevelData _levelData;
    [SerializeField]
    private Text _titleText;

    public override void Display()
    {
        base.Display();

        _titleText.text = _levelData.LevelName;
        DisplayForDuration(_levelData.LevelNameDuration);
    }
}
