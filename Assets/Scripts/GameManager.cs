using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    bool isPaused;

    private static bool created = false;

    void Awake()
    {
        if (!created)
        {
            DontDestroyOnLoad(gameObject);
            created = true;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            if (SceneManager.GetActiveScene().name == "menu") { Application.Quit(); }
          //  else{
          //      SceneManager.LoadScene("menu");
                //pause();
           //     }
        }
    }

    public void MenuPlay() { SceneManager.LoadScene("game"); }
    public void MenuInfo() { }
    public void MenuQuit() { Application.Quit(); }

    public void pause() 
    {
    isPaused = !isPaused;

    }
}
