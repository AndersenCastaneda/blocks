using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    #region Variables
    public Vector3 startPosition;

    [SerializeField] private GameObject player;
    [SerializeField] private GameObject center;
    [SerializeField] private GameObject front;
    [SerializeField] private GameObject back;
    [SerializeField] private GameObject right;
    [SerializeField] private GameObject left;

    private int step = 9;
    public float speed = 0.01f;

    [SerializeField] private bool isMoving = false;
    [SerializeField] private bool canMoveFront = true;
    [SerializeField] private bool canMoveBack = true;
    [SerializeField] private bool canMoveRight = true;
    [SerializeField] private bool canMoveLeft = true;
    public bool isFalling = false;


    public BoxCollider playerCollider;
    [SerializeField] private GameObject[] objectsInMyWay;
    [SerializeField] private PlayerMovement[] players;
    public bool isFinished = false;

    private bool askMoveFront = false;
    private bool askMoveBack = false;
    private bool askMoveLeft = false;
    private bool askMoveRight = false;
    #endregion

    #region Unity
    private void Start()
    {
        SuscribeSwipe();
    }

    private void Update()
    {
        CheckOtherPlayer();

        if (isFalling)
            GameManager.Instance.DeactivateSwipe();
        else
            GameManager.Instance.ActivateSwipe();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Fall"))
        {
            playerCollider.isTrigger = true;
            isFalling = true;
            GameManager.Instance.DeactivateSwipe();
        }

        if (other.CompareTag("Ground"))
        {
            playerCollider.isTrigger = false;
            isFalling = false;
        }

        if (other.CompareTag("End"))
        {
            isFinished = true;
            LevelManager.Instance.CheckLevel();
        }

        if (other.CompareTag("Restart"))
        {
            isFalling = false;
            LevelManager.Instance.ResetLevel();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("End"))
            isFinished = false;
    }
    #endregion

    #region Recive Data from SwipeDetector and CheckMove
    void SuscribeSwipe()
    {
        SwipeDetector.OnSwipe += CheckSwipeAndMove;
    }

    public void UnsuscribeSwipe()
    {
        SwipeDetector.OnSwipe -= CheckSwipeAndMove;
    }

    private void CheckSwipeAndMove(SwipeData data)
    {
        if (data.Direction == SwipeDirection.Up && canMoveFront && !isMoving && !isFalling)
            StartCoroutine(MoveFront());
        else if (data.Direction == SwipeDirection.Down && canMoveBack && !isMoving && !isFalling)
            StartCoroutine(MoveBack());
        else if (data.Direction == SwipeDirection.Left && canMoveLeft && !isMoving && !isFalling)
            StartCoroutine(MoveLeft());
        else if (data.Direction == SwipeDirection.Rigth && canMoveRight && !isMoving && !isFalling)
            StartCoroutine(MoveRigth());
    }
    #endregion

    #region Movement Coroutines
    private IEnumerator MoveFront()
    {
        isMoving = true;
        for (int i = 0; i < (90 / step); i++)
        {
            player.transform.RotateAround(front.transform.position, Vector3.right, step);
            yield return new WaitForSeconds(speed);
        }
        center.transform.position = player.transform.position;
        RoundPosition();
        ResetPlayersDirection();
        isMoving = false;
    }

    private IEnumerator MoveBack()
    {
        isMoving = true;
        for (int i = 0; i < (90 / step); i++)
        {
            player.transform.RotateAround(back.transform.position, Vector3.left, step);
            yield return new WaitForSeconds(speed);
        }
        center.transform.position = player.transform.position;
        RoundPosition();
        ResetPlayersDirection();
        isMoving = false;
    }

    private IEnumerator MoveLeft()
    {
        isMoving = true;
        for (int i = 0; i < (90 / step); i++)
        {
            player.transform.RotateAround(left.transform.position, Vector3.forward, step);
            yield return new WaitForSeconds(speed);
        }
        center.transform.position = player.transform.position;
        RoundPosition();
        ResetPlayersDirection();
        isMoving = false;
    }

    private IEnumerator MoveRigth()
    {
        isMoving = true;
        for (int i = 0; i < (90 / step); i++)
        {
            player.transform.RotateAround(right.transform.position, Vector3.back, step);
            yield return new WaitForSeconds(speed);
        }
        center.transform.position = player.transform.position;
        RoundPosition();
        ResetPlayersDirection();
        isMoving = false;
    }
    #endregion

    void ResetPlayersDirection()
    {
        for (int i = 0; i < players.Length; i++)
        {
            players[i].ResetDirections();
        }
    }

    #region Correct Position and Check for Empty Spot
    private void RoundPosition()
    {
        transform.position = new Vector3(Mathf.Round(transform.position.x), Mathf.Round(transform.position.y), Mathf.Round(transform.position.z));
        transform.rotation = Quaternion.Euler(0, 0, 0);
    }

    public void CheckOtherPlayer()
    {
        for (int i = 0; i < objectsInMyWay.Length; i++)
        {
            float distance = Mathf.Abs(Vector3.Distance(transform.position, objectsInMyWay[i].transform.position));

            if (distance == 4f)
            {
                if (!askMoveFront && (transform.position.z + 4) == objectsInMyWay[i].transform.position.z)
                {
                    askMoveFront = true;
                    canMoveFront = false;
                }

                if (!askMoveBack && (transform.position.z - 4) == objectsInMyWay[i].transform.position.z)
                {
                    askMoveBack = true;
                    canMoveBack = false;
                }

                if (!askMoveLeft && (transform.position.x - 4) == objectsInMyWay[i].transform.position.x)
                {
                    askMoveLeft = true;
                    canMoveLeft = false;
                }

                if (!askMoveRight && (transform.position.x + 4) == objectsInMyWay[i].transform.position.x)
                {
                    askMoveRight = true;
                    canMoveRight = false;
                }
            }
        }
    }

    public void ResetDirections()
    {
        askMoveFront = false;
        askMoveBack = false;
        askMoveLeft = false;
        askMoveRight = false;

        canMoveFront = true;
        canMoveBack = true;
        canMoveLeft = true;
        canMoveRight = true;
    }

    public void ResetPlayer()
    {
        ResetDirections();
        transform.position = startPosition;
        playerCollider.isTrigger = false;
        isFalling = false;
    }
    #endregion
}