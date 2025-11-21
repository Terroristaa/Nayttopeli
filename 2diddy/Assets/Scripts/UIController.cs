using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public static UIController Instance;
    [SerializeField] private Slider playerHealthSlider;
    [SerializeField] private Slider playerExperienceSlider;
    public GameObject gameOverPanel;
    public GameObject PausePanel;
    public GameObject levelUpPanel;
    [SerializeField] private TMP_Text timerText;


    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    public void UpdateHealthSlider()
    {
        playerHealthSlider.maxValue = PlayerController.Instance.playerMaxHealth;
        playerHealthSlider.value = PlayerController.Instance.playerHealth;
    }
    public void UpdateExperienceSlider()
    {
        playerExperienceSlider.maxValue = PlayerController.Instance.playerLevels[PlayerController.Instance.currentLevel - 1];// -1 makes the player start off from lvl 1 and not 0

        playerExperienceSlider.value = PlayerController.Instance.experience;
    }

    public void UpdateTimer(float timer)
    {
        float min = Mathf.FloorToInt(timer / 60f);
        float sec = Mathf.FloorToInt(timer % 60f);
        timerText.text = min + ":" + sec.ToString("00");
    }


}
