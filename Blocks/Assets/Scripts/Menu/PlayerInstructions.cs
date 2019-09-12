using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInstructions : MonoBehaviour
{
    #region Variables
    public Vector3 startPosition;

    [SerializeField] private GameObject player;
    [SerializeField] private GameObject center;
    [SerializeField] private GameObject front;
    [SerializeField] private GameObject back;
    [SerializeField] private GameObject rigth;
    [SerializeField] private GameObject left;

    public int step = 9;
    public float speed = 0.01f;

    public bool isMoving = false;
    [SerializeField] private bool canMoveFront = true;
    [SerializeField] private bool canMoveBack = false;
    [SerializeField] private bool canMoveLeft = false;
    [SerializeField] private bool canMoveRigth = false;
    [SerializeField] private bool isTutorialFinished = false;
    [SerializeField] private MainMenu mainMenu;
    #endregion

    #region Unity
    private void Awake()
    {
        SwipeDetector.OnSwipe += CheckSwipeAndMove;
    }
    #endregion

    public void RestartInstructor()
    {
        canMoveFront = true;
        canMoveBack = false;
        canMoveLeft = false;
        canMoveRigth = false;
        isTutorialFinished = false;
        transform.position = startPosition;
    }

    #region Recive Data from Swipe Detector
    private void CheckSwipeAndMove(SwipeData data)
    {
        if (canMoveFront && data.Direction == SwipeDirection.Up && !isMoving && mainMenu.isMenuHide && !isTutorialFinished)
        {
            isMoving = true;
            StartCoroutine(MoveFront());
        }
        if (canMoveBack && data.Direction == SwipeDirection.Down && !isMoving && mainMenu.isMenuHide && !isTutorialFinished)
        {
            isMoving = true;
            StartCoroutine(MoveBack());
        }
        if (canMoveLeft && data.Direction == SwipeDirection.Left && !isMoving && mainMenu.isMenuHide && !isTutorialFinished)
        {
            isMoving = true;
            StartCoroutine(MoveLeft());
        }
        if (canMoveRigth && data.Direction == SwipeDirection.Rigth && !isMoving && mainMenu.isMenuHide && !isTutorialFinished)
        {
            isMoving = true;
            StartCoroutine(MoveRigth());
            isTutorialFinished = true;
        }
    }
    #endregion

    public void SkipTutorial()
    {
        isTutorialFinished = true;
    }
    #region Movement Coroutines
    public IEnumerator MoveFront()
    {
        for (int i = 0; i < (90 / step); i++)
        {
            player.transform.RotateAround(front.transform.position, Vector3.right, step);
            yield return new WaitForSeconds(speed);
        }
        center.transform.position = player.transform.position;
        RoundPosition();
        isMoving = false;
        canMoveFront = false;
        canMoveBack = true;
        mainMenu.TextSwipe("swipe down");
    }

    public IEnumerator MoveBack()
    {
        for (int i = 0; i < (90 / step); i++)
        {
            player.transform.RotateAround(back.transform.position, Vector3.left, step);
            yield return new WaitForSeconds(speed);
        }
        center.transform.position = player.transform.position;
        RoundPosition();
        isMoving = false;
        canMoveBack = false;
        canMoveLeft = true;
        mainMenu.TextSwipe("swipe left");
    }

    public IEnumerator MoveLeft()
    {
        for (int i = 0; i < (90 / step); i++)
        {
            player.transform.RotateAround(left.transform.position, Vector3.forward, step);
            yield return new WaitForSeconds(speed);
        }
        center.transform.position = player.transform.position;
        RoundPosition();
        isMoving = false;
        canMoveLeft = false;
        canMoveRigth = true;
        mainMenu.TextSwipe("swipe rigth");
    }

    public IEnumerator MoveRigth()
    {
        for (int i = 0; i < (90 / step); i++)
        {
            player.transform.RotateAround(rigth.transform.position, Vector3.back, step);
            yield return new WaitForSeconds(speed);
        }
        center.transform.position = player.transform.position;
        RoundPosition();
        isMoving = false;
        canMoveRigth = false;
        mainMenu.TextSwipe("you finished the tutorial");
    }
    #endregion

    #region Correct Position and Check for Empty Spot
    private void RoundPosition()
    {
        transform.position = new Vector3(Mathf.Round(transform.position.x), Mathf.Round(transform.position.y), Mathf.Round(transform.position.z));
        transform.rotation = Quaternion.Euler(0, 0, 0);
    }
    #endregion

    private void OnDisable()
    {
        SwipeDetector.OnSwipe -= CheckSwipeAndMove;
    }
}
