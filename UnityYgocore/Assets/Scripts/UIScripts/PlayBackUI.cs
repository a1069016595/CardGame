using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayBackUI : BaseMonoBehivour
{
    Button startButton;
    Button stopButton;
    Button exitButton;

    DuelEventSys eventSys;

    void Awake()
    {
        gameObject.SetActive(false);
        eventSys = DuelEventSys.GetInstance;

        startButton = transform.GetChild<Button>("StartPlayButton");
        stopButton = transform.GetChild<Button>("StopPlayButton");
        exitButton = transform.GetChild<Button>("ExitButton");

        startButton.onClick.AddListener(ClickStartButton);
        stopButton.onClick.AddListener(ClickStopButton);
        exitButton.onClick.AddListener(ClickExitButton);

        AddHandler(DuelEvent.playBackEvent_StartGame, Show);
    }

    private void Show(params object[] args)
    {
        gameObject.SetActive(true);
        startButton.gameObject.SetActive(false);
    }

    private void ClickExitButton()
    {
        eventSys.SendEvent(DuelEvent.gameEvent_ExitPlayBack);
    }

    private void ClickStopButton()
    {
        eventSys.SendEvent(DuelEvent.playBackEvent_StopPlay);
        stopButton.gameObject.SetActive(false);
        startButton.gameObject.SetActive(true);
    }

    private void ClickStartButton()
    {
        eventSys.SendEvent(DuelEvent.playBackEvent_StartPlay);
        stopButton.gameObject.SetActive(true);
        startButton.gameObject.SetActive(false);
    }
}
