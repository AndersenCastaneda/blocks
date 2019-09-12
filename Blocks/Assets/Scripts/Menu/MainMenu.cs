using UnityEngine;
using TMPro;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI instructor;


    public bool isMenuHide = false;

    public void HideMenu()
    {
        isMenuHide = true;
    }

    public void ShowMenu()
    {
        isMenuHide = false;
    }

    public void TextSwipe(string swipeText)
    {
        instructor.text = swipeText;
    }

    public void RestartInstructor()
    {
        instructor.text = "swipe up";
    }
}