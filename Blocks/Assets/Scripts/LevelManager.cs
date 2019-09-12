using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public int count = 20;
    public int second = 1;
    private int level;
    private int levels;
    public bool isLevelComplete = false;

    [SerializeField] private PlayerMovement[] players;

    public static LevelManager Instance { get; private set; }

    #region Unity
    private void Awake()
    {
        levels = SceneManager.sceneCountInBuildSettings;
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        Scene scene = SceneManager.GetActiveScene();
        level = scene.buildIndex;
        if (scene.buildIndex > 0)
        {
            StartCoroutine(CountDown());
            UIManager.Instance.ShowLevel(level);
            DataManager.Instance.SaveLevel(level);
        }
    }
    #endregion

    public void LoadMenu()
    {
        AudioManager.Instance.StopAudio();
        for (int i = 0; i < players.Length; i++)
        {
            players[i].UnsuscribeSwipe();
        }

        if (isLevelComplete)
        {
            DataManager.Instance.SaveLevel(level++);
        }
        SceneManager.LoadScene("Menu");
    }

    public void SkipTutorial()
    {
        StartCoroutine(LoadNextLevel());
    }

    public IEnumerator LoadNextLevel()
    {
        level++;
        if (level < levels)
        {
            for (int i = 0; i < players.Length; i++)
            {
                players[i].UnsuscribeSwipe();
            }
            yield return new WaitForSeconds(2f);
            SceneManager.LoadScene(level, LoadSceneMode.Single);
        }
    }

    #region Level Logic
    public IEnumerator CountDown()
    {
        count = 20;
        isLevelComplete = false;
        AudioManager.Instance.PlayAudioLevel();
        UIManager.Instance.ShowMessage("starts");
        UIManager.Instance.ShowCountDown(count);
        GameManager.Instance.ActivateSwipe();

        while (count > 0)
        {
            count -= second;
            yield return new WaitForSeconds(second);
            UIManager.Instance.ShowCountDown(count);
            UIManager.Instance.HideMessage();
        }

        if (count == 0)
            GameManager.Instance.DeactivateSwipe();

        yield return new WaitForSeconds(2f);

        if (count == 0 && !isLevelComplete)
        {
            StopCoroutine(CountDown());
            AudioManager.Instance.StopAudio();
            ResetLevel();
        }
    }

    public void CheckLevel()
    {
        for (int i = 0; i < players.Length; i++)
        {
            if (!players[i].isFinished)
            {
                isLevelComplete = false;
                break;
            }
            else
                isLevelComplete = true;
        }

        if (isLevelComplete)
        {
            StopAllCoroutines();
            AudioManager.Instance.PLayAudioWin();
            GameManager.Instance.DeactivateSwipe();
            StartCoroutine(LoadNextLevel());
            UIManager.Instance.ShowMessage("level complete");
        }
    }

    public void ResetLevel()
    {
        StopAllCoroutines();
        AudioManager.Instance.StopAudio();
        for (int i = 0; i < players.Length; i++)
        {
            players[i].ResetPlayer();
        }
        StartCoroutine(CountDown());
    }

    public void Load()
    {
        DataManager.Instance.LoadData();
        if (DataManager.Instance.MaxLevelPLayed() > level)
        {
            level = DataManager.Instance.MaxLevelPLayed();
            SceneManager.LoadScene(level, LoadSceneMode.Single);
        }
        else
        {
            UIManager.Instance.DeactiveObjectsMenu();
            UIManager.Instance.ShowInstructions();
        }
    }
    #endregion
}
