using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartGame()
    {
        SceneManager.LoadScene("Map");
    }

    public void StartPlayGround()
    {
        SceneManager.LoadScene("PlayGroundScene");
    }

    public void Menu()
    {
        SceneManager.LoadScene("Main Menu");
    }

    public void Tutorial()
    {
        SceneManager.LoadScene("Tutorial");
    }
}
