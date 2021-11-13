using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelController : MonoBehaviour
{
    public static LevelController current;
    public bool gameActive = false;
    public Slider levelProgressBar;
    public float maxDistance;
    public GameObject finishLine;
    public AudioSource gameMusicAudioSource;
    public AudioClip victoryAudioClip, gameOverAudioClip;    
    public GameObject startMenu, gameMenu, gameOverMenu, finishMenu;
    public Text scoreText, finishScoreText, currentLevelText, nextLevelText, startingMenuMoneyText, gameOverMenuMoneyText, finishMenuMoneyText;
    public DailyReward dailyReward;

    int currentLevel, score, money;
    void Start()
    {
        current = this;
        currentLevel = PlayerPrefs.GetInt("currentLevel");//giriþ yoksa 0 döndürecek.        
        PlayerController.current = GameObject.FindObjectOfType<PlayerController>();// player controldeki tanýmlama start da olduðu için ve ilk olarak levelcontroller çalýþýrsa hata meydana gelir.bu yüzden buraya taþýdýk.
        GameObject.FindObjectOfType<MarketController>().InitializeMarketController(); 
        dailyReward.InitializDailyReward();
        currentLevelText.text = (currentLevel + 1).ToString();
        nextLevelText.text = (currentLevel + 2).ToString();
        UpdateMoneyText();            
        gameMusicAudioSource = Camera.main.GetComponent<AudioSource>();
    }

    void Update()
    {
        if (gameActive)
        {
            PlayerController player = PlayerController.current;
            float distance = finishLine.transform.position.z - PlayerController.current.transform.position.z;
            levelProgressBar.value = 1 - (distance / maxDistance);//slider ýn value deðeri 0-1 arasýndadýr.
        }
    }
    public void BackButton()
    {
        PlayerPrefs.SetInt("currentLevel", 0);
        LevelLoader.current.ChangeLevel("Level 0");
    }
    public void StartLevel()
    {
        maxDistance = finishLine.transform.position.z - PlayerController.current.transform.position.z;
        startMenu.SetActive(false);
        gameMenu.SetActive(true);
        gameActive = true;
        PlayerController.current.ChangeSpeed(PlayerController.current.currentRunningSpeed);
        PlayerController.current.animator.SetBool("running", true);
    }
    public void RestartLevel()
    {
        LevelLoader.current.ChangeLevel(this.gameObject.scene.name);
    }
    public void LoadNextLevel()
    {
        int level = PlayerPrefs.GetInt("currentLevel"); 
        if (level >= 3)
        {
            LevelLoader.current.ChangeLevel("Level 0");
        }
        LevelLoader.current.ChangeLevel("Level " + (currentLevel + 1));
    }
    public void GameOver()
    {
        UpdateMoneyText();
        gameMusicAudioSource.Stop();
        gameMusicAudioSource.PlayOneShot(gameOverAudioClip);
        gameMenu.SetActive(false);
        gameOverMenu.SetActive(true);
        gameActive = false;
    }
    public void FinishGame()
    {
        GiveMoneyToPlayer(score);
        gameMusicAudioSource.Stop();
        gameMusicAudioSource.PlayOneShot(victoryAudioClip);
        PlayerPrefs.SetInt("currentLevel", currentLevel + 1);
        finishScoreText.text = score.ToString();
        gameMenu.SetActive(false);
        finishMenu.SetActive(true);
        gameActive = false;

    }
    public void ChangeScore(int increment)
    {
        score += increment;
        scoreText.text = "SCORE: " + score.ToString();
    }
    public void UpdateMoneyText()
    {
        money = PlayerPrefs.GetInt("money");
        startingMenuMoneyText.text = money.ToString();
        gameOverMenuMoneyText.text = money.ToString();
        finishMenuMoneyText.text = money.ToString();
    }
    public void GiveMoneyToPlayer(int increment)
    {
        money = Mathf.Max(0, money + increment);
        PlayerPrefs.SetInt("money", money);
        UpdateMoneyText();
    }
}
