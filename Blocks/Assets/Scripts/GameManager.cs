using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    #region Variables
    public static GameManager Instance { get; private set; }
    #endregion

    #region Unity
    private void Awake()
    {
        Application.targetFrameRate = 30;
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }
    #endregion

    #region Swipe On/Off
    public void DeactivateSwipe()
    {
        SwipeDetector.Instance.enabled = false;
    }

    public void ActivateSwipe()
    {
        SwipeDetector.Instance.enabled = true;
    }
    #endregion
}