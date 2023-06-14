using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using UnityEngine.UI;
public class GameManager : MonoBehaviour
{
    public static Action<bool> LevelEndStatus;
    public static Action startGame;
    private static GameManager _instance;

    private bool localMemoryEnable;
    private string savePath;
    private SaveData saveData;
    private string jsonSaveData;
    private bool tutorialPlayed = false;
    private static bool gameStarted = false;
    private bool levelEnd = false;
    private int targetPlayerLevel;

    
    public static bool GameStartReset
	{
        set { gameStarted = false; }
	}
    private GameManager() { }

    public static GameManager Instance
    {
        get { return _instance; }
    }

    #region Mono
    private void Awake()
	{
        _instance = this;
        gameStarted = false;
        savePath = Application.persistentDataPath;
        Screen.orientation = ScreenOrientation.Portrait;
        //Check if there is a local memory
        if (LoadGame())
		{
            //Wait for UI Response
            Debug.Log("Game Loaded");
            //Start game

        }
		else
		{
            //Create a new SaveData
            saveData = new SaveData();

            //Create the first level
            //LevelManager.Instance.ActiveLevelInfo = 0;
            //LevelManager.Instance.SpawnLevel();
        }
        //If there is not, do nothing
    }

	private void Start()
	{
        Player.PlayerLevelEvent += LevelUpEvent;
        LevelEndStatus += EndLevel;
        //LevelManager.levelChange += CloseMenuPanels;
        m_ExpBar.value = 0f;
        targetPlayerLevel = EntitySpawner.Instance.GetTargetPlayerLevel;
        
    }

    private void OnDestroy()
	{
        LevelEndStatus -= EndLevel;
        //LevelManager.levelChange -= CloseMenuPanels;
        Player.PlayerLevelEvent -= LevelUpEvent;

    }

    public void EndLevel(bool status)
    {
        levelEnd = status;
        OpenMenuPanel(status);
    }

    private void Update()
	{
        //If you see the panel, you are not in game
		if (m_MenuPanel.activeInHierarchy) return;

        m_LevelText.text = "Player Level:"+Player.Instance.PlayerLevel.ToString();
        m_ExpBar.value = Mathf.Clamp(Player.Instance.CurrentExperience / Player.Instance.MaxExperience,0,1);

        if ((Input.touchCount > 0 || Input.GetMouseButton(0)) && !gameStarted)
        {
            startGame.Invoke();
            m_Tutorial.SetActive(false);
            Debug.Log("GAME STARTED");
            //tutorialPlayed = true;
            gameStarted = true;
        }

        var enemies = FindObjectsOfType<Enemy>();

        if (levelEnd && enemies == null)
		{
            OpenMenuPanel(true);
		}

    }
	//For regular saves in-game
	//JsonUtility.FromJsonOverwrite(jsonSaveData, saveData);

	public void SaveGame()
	{
        //Make a save
        if (localMemoryEnable)
		{
            //Get important variables to make a save
            //saveData.levelIndex = LevelManager.Instance.ActiveLevelInfo;
			saveData.PlayerPos = Player.Instance.PlayerPosition;
            saveData.TutorialPlayed = tutorialPlayed;

            //Overwrite the local save data
            jsonSaveData = JsonUtility.ToJson(saveData).ToString();
            using (StreamWriter outputFile = new StreamWriter(Path.Combine(savePath, "Save.json")))
            {
                    outputFile.WriteLine(jsonSaveData);
            }

        }
        else
		{
            //Create a local save data
            //JsonUtility.FromJsonOverwrite(json, myObject);
            //saveData.levelIndex = LevelManager.Instance.ActiveLevelInfo;
            saveData.PlayerPos = Player.Instance.PlayerPosition;
            saveData.TutorialPlayed = tutorialPlayed;
            using (StreamWriter outputFile = new StreamWriter(Path.Combine(savePath, "Save.json")))
            {
                outputFile.WriteLine(jsonSaveData);
            }
            //Set local memory enable true
            localMemoryEnable = true;
        }
    }
    public bool LoadGame()
    {
        //Chack if there is a save file
        if (File.Exists(savePath))
        {
            //Read the existing file
            using (StreamReader sr = new StreamReader(savePath + "Save.json"))
            {
                saveData = JsonUtility.FromJson<SaveData>(sr.ReadToEnd().ToString());
                Debug.Log(sr);
                Debug.Log(sr.ToString());
            }
            //LevelManager.Instance.ActiveLevelInfo = saveData.levelIndex;
            Player.Instance.PlayerPosition = saveData.PlayerPos;
            tutorialPlayed = saveData.TutorialPlayed;
            //LevelManager.Instance.SpawnLevel();
            return localMemoryEnable = true;
        }
        else { return localMemoryEnable = false; }    
    }

    //Make a save before quitting the game
    public void ExitSave()
	{
        SaveGame();
	}

    private void ExitGame()
	{
        ExitSave();
        Application.Quit();
    }

    void OnApplicationQuit()
    {
        //ExitSave();
    }
    #endregion

    #region UI

    [SerializeField] private GameObject m_MenuPanel, m_NextButton, m_RetryButton, m_Tutorial;
    [SerializeField] private GameObject m_SkillsPanel;
    [SerializeField] private Text m_MaxExpText,m_CurExpText,m_LevelText;
    [SerializeField] private Slider m_ExpBar;


    private void OpenMenuPanel(bool status)
    {
        m_MenuPanel.SetActive(true);
        if (status) m_NextButton.SetActive(true);
        else m_RetryButton.SetActive(true);
    }

    public void CloseMenuPanels()
	{
        m_NextButton.SetActive(false);
        m_RetryButton.SetActive(false);
        m_MenuPanel.SetActive(false);
    }

    public void UpdateExperienceSystem(int newLevel)
	{
        m_LevelText.text = newLevel.ToString();
    }

    private void LevelUpEvent()
	{
        //Do not open panel after certain condition
        if (Player.Instance.PlayerLevel <= targetPlayerLevel)
		{
            SkillsPanel(true);
        }
        //Open skills panel
        else
        {
            Player.PlayerDeathEvent.Invoke();
            Player.Instance.PlayerIsDead = true;
            LevelEndStatus.Invoke(true);
            SkillsPanel(false);

        }
    }

    public void SkillsPanel(bool open)
	{
        //Stop time if panel appears
        if (open)
		{
            m_SkillsPanel.SetActive(true);
            Time.timeScale = 0;
        }
        else
        { 
            Time.timeScale = 1;
            m_SkillsPanel.SetActive(false);
        }
    }

    #endregion
}
