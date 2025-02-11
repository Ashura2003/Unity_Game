using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [Header ("Main Menu")]
    [SerializeField] private GameObject mainMenuScreen;
    [SerializeField] private AudioClip mainMenuSound;


    private void Awake()
    {
        mainMenuScreen.SetActive(true);
    }

    #region Main Menu
    public void Start()
    {
        mainMenuScreen.SetActive(false);
        SceneManager.LoadScene(1);
    }

    public void Quit()
    {
        Application.Quit(); //Quits the game (only works in build)

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; //Exits play mode (will only be executed in the editor)
#endif
    }


    #endregion
}
