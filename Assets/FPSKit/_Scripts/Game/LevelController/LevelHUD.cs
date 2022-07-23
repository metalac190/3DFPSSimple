using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelHUD : MonoBehaviour
{
    [SerializeField]
    private HUDScreen _introScreen;
    [SerializeField]
    private HUDScreen _winScreen;
    [SerializeField]
    private HUDScreen _loseScreen;
    [SerializeField]
    private PlaytimeScreen _playtimeScreen;
    [SerializeField]
    private PlayerHUD _playerHUD;

    public HUDScreen IntroScreen => _introScreen;
    public HUDScreen WinScreen => _winScreen;
    public HUDScreen LoseScreen => _loseScreen;
    public PlaytimeScreen PlaytimeScreen => _playtimeScreen;
    public PlayerHUD PlayerHUD => _playerHUD;

    public void DisableAllCanvases()
    {
        _introScreen.Hide();
        _winScreen.Hide();
        _playtimeScreen.Hide();
        _playerHUD.Hide();
    }
}
