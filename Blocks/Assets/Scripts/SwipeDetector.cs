using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SwipeDirection { Up, Down, Left, Rigth }

public struct SwipeData
{
    public Vector2 StartPosition;
    public Vector2 EndPosition;
    public SwipeDirection Direction;
}

public class SwipeDetector : MonoBehaviour
{
    #region Variables
    Vector2 fingerDownPosition;
    Vector2 fingerUpPosition;

    private float minDistanceForSwipe = 20f;
    public static event Action<SwipeData> OnSwipe = delegate { };

    public static SwipeDetector Instance { get; private set; }
    #endregion

    #region Unity
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Update()
    {
        foreach (Touch touch in Input.touches)
        {
            if (touch.phase == TouchPhase.Began)
            {
                fingerDownPosition = touch.position;
                fingerUpPosition = touch.position;
            }

            if (touch.phase == TouchPhase.Moved) fingerDownPosition = touch.position;

            if (touch.phase == TouchPhase.Ended)
            {
                fingerDownPosition = touch.position;
                DetectSwipe();
            }
        }
    }
    #endregion

    #region SwipeLogic
    private void DetectSwipe()
    {
        if (SwipeDistanceCheckMet())
        {
            if (IsVerticalSwipe())
            {
                SwipeDirection direction = fingerDownPosition.y - fingerUpPosition.y > 0 ? SwipeDirection.Up : SwipeDirection.Down;
                SendSwipe(direction);
            }
            else
            {
                SwipeDirection direction = fingerDownPosition.x - fingerUpPosition.x > 0 ? SwipeDirection.Rigth : SwipeDirection.Left;
                SendSwipe(direction);
            }
        }
    }

    private bool SwipeDistanceCheckMet()
    {
        return VerticalMovementDistance() > minDistanceForSwipe || HorizontalMovementDistance() > minDistanceForSwipe;
    }

    private float VerticalMovementDistance()
    {
        return Mathf.Abs(fingerDownPosition.y - fingerUpPosition.y);
    }

    private float HorizontalMovementDistance()
    {
        return Mathf.Abs(fingerDownPosition.x - fingerUpPosition.x);
    }

    private bool IsVerticalSwipe()
    {
        return VerticalMovementDistance() > HorizontalMovementDistance();
    }

    private void SendSwipe(SwipeDirection direction)
    {
        SwipeData swipeData = new SwipeData()
        {
            Direction = direction,
            StartPosition = fingerDownPosition,
            EndPosition = fingerUpPosition
        };
        OnSwipe(swipeData);
    }
    #endregion
}
