using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using FMODUnity;
using FMOD.Studio;
using Unity.VisualScripting;

public class GameManager : MonoBehaviour
{
    public static GameManager _instance { get; private set; }

    public Database Database;

    public List<ItemData> ItemsUnlocked; 

    PlayerManager playerManager;
    ComputerManager computerManager;
    OpenSignManager openSignManager;
    NPCManager npcManager;

    [SerializeField] private TextMeshProUGUI transitionDayText;
    [SerializeField] private TextMeshProUGUI weatherText;

    public GameDetails gameDetails { get; private set; } = new();
    public GameDetailsVisuals gameDetailVisuals;

    public bool isGamePaused;

    public EventReference ShopClosedMusic;
    public EventReference ShopOpenMusic;

    EventInstance currentTrack;

    private void Awake()
    {
        Singleton();
        gameDetails.Weather = new();
        playerManager = FindAnyObjectByType<PlayerManager>();
        computerManager = FindAnyObjectByType<ComputerManager>();
        openSignManager = FindAnyObjectByType<OpenSignManager>();
        npcManager = FindAnyObjectByType<NPCManager>();
    }

    //Makes sure there's only one of this object
    private void Singleton()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        else
        {
            _instance = this;
        }
    }

    private void Start()
    {
        StartNewDay();
    }

    public void StartNewDay()
    {
        gameDetails.Day++;
        gameDetails.weekDayCount++;

        if (gameDetails.weekDayCount >= 8)
        {
            gameDetails.weekDayCount = 0;
            gameDetails.weekCount++;
            WeeklySummary();
        }

        ShopStatusChange();
        openSignManager.EnableSignChange(true);

        gameDetails.Hour = 8;
        gameDetails.Minute = 0;

        time = StartCoroutine(Time());

        NewShopInventory();
        WeatherUpdate();

        transitionDayText.text = $"Day : {gameDetails.Day}";
        gameDetailVisuals.UpdateEverything(gameDetails.Day, gameDetails.Hour, gameDetails.Minute, gameDetails.Weather.Icon, playerManager.inventory.Funds, gameDetails.IsShopOpen);
    }

    public void CloseUpShop()
    {
        npcManager.Leave();
    }

    public void EndDay()
    {

    }

    public void WeeklySummary()
    {

    }

    public void PlayMusic(EventReference musicRef)
    {
        if (!currentTrack.IsUnityNull())
        {
            currentTrack.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            currentTrack.release();
        }

        currentTrack = AudioManager._instance.CreateEventInstance(musicRef);
        currentTrack.start();
    }

    public void ShopStatusChange()
    {
        if (gameDetails.IsShopOpen) 
        {
            PlayMusic(ShopOpenMusic);
        }
        else 
        {
            PlayMusic(ShopClosedMusic);
        }
    }

    public void PauseGame()
    {
        if (isGamePaused)
        {

        }
        else
        {

        }

        PauseTime(isGamePaused);
    }

    Coroutine time;

    public void PauseTime(bool Pause)
    {
        if (Pause)
        {
            StopCoroutine(time);
        }
        else
        {
            time = StartCoroutine(Time());
        }
    }

    private IEnumerator Time()
    {
        while (!isGamePaused)
        {
            gameDetails.Minute++;
            gameDetails.NPCCounter++;

            if (gameDetails.NPCCounter >= 15)
            {
                gameDetails.NPCCounter = 0;
                SpawnNPC();
            }

            if (gameDetails.Minute >= 60)
            {
                gameDetails.Minute = 0;
                gameDetails.Hour++;
            }

            if (gameDetails.Hour >= 24)
            {
                gameDetails.Hour = 0;
            }

            if (gameDetails.Hour == 10 && !gameDetails.IsShopOpen)
            {
                openSignManager.ChangeSign();
                Debug.Log($"Player did not open shop manually on day: {gameDetails.Day}. Game did it automatically.");
            }

            if (gameDetails.IsShopOpen && gameDetails.Hour >= 18)
            {
                openSignManager.EnableSignChange(true);
            }

            if (gameDetails.Hour == 20 && gameDetails.IsShopOpen)
            {
                openSignManager.ChangeSign();
                Debug.Log($"Player did close shop manually on day: {gameDetails.Day}. Game did it automatically.");
            }

            if (gameDetails.Hour == 23 && !gameDetails.IsShopOpen)
            {
                EndDay();
                Debug.Log($"Player did not go home manually on day: {gameDetails.Day}. Game did it automatically.");
            }

            gameDetailVisuals.UpdateClock(gameDetails.Hour, gameDetails.Minute);

            yield return new WaitForSeconds(1);
        }
    }

    private void NewShopInventory()
    {
    }

    private void WeatherUpdate()
    {
        int randomWeather = Random.Range(0, Database.GetWeatherRange());
        gameDetails.Weather.GetWeatherData(Database,randomWeather);
        weatherText.text = $"{gameDetails.Weather.NameAndDescription.Name}";
        gameDetailVisuals.UpdateWeather(gameDetails.Weather.Icon);

        Debug.Log($"Today it is {gameDetails.Weather.NameAndDescription.Name}");
    }

    public void SpawnNPC()
    {
        if (!SpawnRoll() || gameDetails.NPCAtDesk || !gameDetails.IsShopOpen) { return; }

        npcManager.Spawn();
    }

    public bool SpawnRoll()
    {
        return true;
    }
}

[System.Serializable]
public class GameDetails
{
    public int Day;
    public int weekDayCount = 0;
    public int weekCount = 0;
    public Weather Weather = new();
    public int Hour;
    public int Minute;
    public bool IsShopOpen;
    public bool NPCAtDesk;
    public int NPCCounter;
}

[System.Serializable]
public class GameDetailsVisuals
{
    public TextMeshProUGUI Money;
    public TextMeshProUGUI DayCounter;
    public TextMeshProUGUI Clock;
    public Image WeatherIcon;
    public TextMeshProUGUI ShopOpenStatus;

    public void UpdateEverything(int day, int hour, int minutes, Sprite w_icon, int money, bool isShopOpen)
    {
        UpdateDayCounter(day);
        UpdateClock(hour, minutes);
        UpdateWeather(w_icon);
        UpdateMoney(money);
        UpdateShopOpenStatus(isShopOpen);
    }

    public void UpdateDayCounter(int day)
    {
        DayCounter.text = $"Day:{day}";
    }

    public void UpdateClock(int hour, int minutes)
    {
        Clock.text = string.Format("{00:00}:{01:00}",hour,minutes);
    }

    public void UpdateWeather(Sprite icon)
    {
        WeatherIcon.sprite = icon;
    }

    public void UpdateMoney(int money)
    {
        string negativeValue;

        if (money > 0) { Money.color = Color.white; negativeValue = null; }
        else { Money.color = Color.red; negativeValue = "-"; }

        Money.text = $"£{negativeValue}{money}";
    }

    public void UpdateShopOpenStatus(bool isOpen)
    {
        if (isOpen) { ShopOpenStatus.text = "Open"; ShopOpenStatus.color = Color.green; }
        else { ShopOpenStatus.text = "Closed"; ShopOpenStatus.color = Color.red; }
    }
}