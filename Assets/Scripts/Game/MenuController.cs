using UnityEngine;

public class MenuController : MonoBehaviour
{
    public GameObject optionsPanel;
    public GameObject optionsButton;
    public GameObject player;

    void Start()
    {
        optionsPanel = GameObject.Find("OptionsPanel");
        if (optionsPanel != null)
        {
            optionsPanel.SetActive(false);
        }
        optionsButton = GameObject.Find("OptionsButton");
        player = GameObject.Find("Player");
    }
    public void LoadGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Level1");
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();
        PlayerPrefs.Save();
    }

    public void OpenOptionsPanel()
    {
        if (optionsPanel != null)
        {
            optionsPanel.SetActive(true);
            optionsButton.SetActive(false);
            Time.timeScale = 0;
            if (player != null)
            {
                player.GetComponent<PlayerShoot>().canShoot = false;
            }
        }
    }

    public void CloseOptionsPanel()
    {
        if (optionsPanel != null)
        {
            optionsPanel.SetActive(false);
            optionsButton.SetActive(true);
            Time.timeScale = 1;
            if (player != null)
            {
                player.GetComponent<PlayerShoot>().canShoot = true;
            }
        }
    }
}
