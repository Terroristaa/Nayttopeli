using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;

using UnityEngine.UI;
using TMPro;
public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance;

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Animator animator;

    [SerializeField] private float moveSpeed;
    public Vector3 playerMoveDirection;
    public Vector3 lastMoveDirection;
    public float playerMaxHealth;
    public float playerHealth;

    public int experience;
    public int currentLevel;
    public int maxLevel;

    private bool isImmune;
    [SerializeField] private float immunityDuration;
    [SerializeField] private float immunityTimer;

    public List<int> playerLevels;

    public Weapon activeWeapon;
    public static bool shouldLoadGame = false;
    public List<Weapon> allWeapons = new List<Weapon>();

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

    void Start()
    {
        allWeapons = GetComponentsInChildren<Weapon>().ToList();

        lastMoveDirection = new Vector3(0, -1);
        for (int i = playerLevels.Count; i < maxLevel; i++)
        {
            playerLevels.Add(Mathf.CeilToInt(playerLevels[playerLevels.Count - 1] * 1.1f + 15));
        }
        playerHealth = playerMaxHealth;
        UIController.Instance.UpdateHealthSlider();
        UIController.Instance.UpdateExperienceSlider();

        if (shouldLoadGame)
        {
            LoadPlayerData();
            shouldLoadGame = false;
        }
    }

    void Update()
    {
        float inputX = Input.GetAxisRaw("Horizontal");
        float inputY = Input.GetAxisRaw("Vertical");
        playerMoveDirection = new Vector3(inputX, inputY).normalized;
        if (playerMoveDirection == Vector3.zero)
        {
            animator.SetBool("moving", false);
        }
        else if (Time.timeScale != 0)
        {
            animator.SetBool("moving", true);
            animator.SetFloat("moveX", inputX);
            animator.SetFloat("moveY", inputY);
            lastMoveDirection = playerMoveDirection;
        }

        if (immunityTimer > 0)
        {
            immunityTimer -= Time.deltaTime;
        }
        else
        {
            isImmune = false;
        }
    }

    void FixedUpdate()
    {
        rb.linearVelocity = new Vector3(playerMoveDirection.x * moveSpeed, playerMoveDirection.y * moveSpeed);
    }

    public void TakeDamage(float damage)
    {
        if (!isImmune)
        {
            isImmune = true;
            immunityTimer = immunityDuration;
            playerHealth -= damage;
            UIController.Instance.UpdateHealthSlider();
            if (playerHealth <= 0)
            {
                // Delete the save file on death
                string path = Application.persistentDataPath + "/playerdata.json";
                if (File.Exists(path))
                {
                    File.Delete(path);
                }

                AudioController.Instance.PlaySound(AudioController.Instance.gameOver);
                gameObject.SetActive(false);
                GameManager.Instance.GameOver();
            }
        }
    }
    public void GetExperience(int experienceToGet)
    {
        experience += experienceToGet;
        UIController.Instance.UpdateExperienceSlider();
        if (experience >= playerLevels[currentLevel - 1])
        {
            LevelUp();
        }
    }
    public void LevelUp()
    {
        experience -= playerLevels[currentLevel - 1];
        currentLevel++;
        UIController.Instance.UpdateExperienceSlider();
        UIController.Instance.levelUpButtons[0].ActivateButton(activeWeapon);
        UIController.Instance.LevelUpPanelOpen();
    }

    public void SavePlayerData()
    {
        PlayerData data = new PlayerData
        {
            playerHealth = playerHealth,
            playerMaxHealth = playerMaxHealth,
            experience = experience,
            currentLevel = currentLevel,
            maxLevel = maxLevel,
            position = new float[] { transform.position.x, transform.position.y, transform.position.z },
            weaponLevels = allWeapons.Select(w => w.weaponLevel).ToList(),
            waveNumber = EnemySpawner.Instance.waveNumber,
            timerValue = GameManager.Instance.gameTime
        };
        string json = JsonUtility.ToJson(data, true);
        string path = Application.persistentDataPath + "/playerdata.json";
        File.WriteAllText(path, json);
    }

    public void LoadPlayerData()
    {
        string path = Application.persistentDataPath + "/playerdata.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            PlayerData data = JsonUtility.FromJson<PlayerData>(json);
            playerHealth = data.playerHealth;
            playerMaxHealth = data.playerMaxHealth;
            experience = data.experience;
            currentLevel = data.currentLevel;
            maxLevel = data.maxLevel;
            transform.position = new Vector3(data.position[0], data.position[1], data.position[2]);
            UIController.Instance.UpdateHealthSlider();
            GameManager.Instance.gameTime = data.timerValue;
            UIController.Instance.UpdateTimer(data.timerValue);

            if (data.weaponLevels != null && allWeapons.Count == data.weaponLevels.Count)
            {
                for (int i = 0; i < allWeapons.Count; i++)
                {
                    allWeapons[i].weaponLevel = data.weaponLevels[i];
                }
            }
            UIController.Instance.UpdateExperienceSlider();

            // Restore the wave system
            if (EnemySpawner.Instance != null)
            {
                EnemySpawner.Instance.waveNumber = data.waveNumber;
                // EnemySpawner.Instance.UpdateWaveState();
            }
        }
    }
    public void SaveAndQuitToMenu()
    {
        SavePlayerData();
        SceneManager.LoadScene("Main Menu");
        Time.timeScale = 1f;
    }
    public void LoadGame()
    {
        // Do not call LoadPlayerData() here!
        SceneManager.LoadScene("Game");
        shouldLoadGame = true;
    }
}
