using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI countDownText;
    [SerializeField] TextMeshProUGUI messageText;
    [SerializeField] TextMeshProUGUI levelText;
    [SerializeField] GameObject[] objectsMenu;
    [SerializeField] private GameObject[] instructions;

    public static UIManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void ShowCountDown(int count)
    {
        countDownText.text = count.ToString();
    }

    public void ShowLevel(int level)
    {
        levelText.text = level.ToString();
    }

    public void ShowMessage(string text)
    {
        messageText.text = text;
        messageText.enabled = true;
    }

    public void HideMessage()
    {
        messageText.enabled = false;
    }

    public void DeactiveObjectsMenu()
    {
        objectsMenu[0].SetActive(false);
        objectsMenu[1].SetActive(false);
    }

    public void ShowInstructions()
    {
        for (int i = 0; i < instructions.Length; i++)
        {
            instructions[i].SetActive(true);
        }
    }
}
