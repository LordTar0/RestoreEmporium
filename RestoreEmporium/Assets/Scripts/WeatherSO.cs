using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Restore Emporium/New Weather")]
public class WeatherSO : ScriptableObject
{
    public int ID = -1;
    public Weather data;
}

[System.Serializable]
public class Weather
{
    public Sprite Icon;
    public NameAndDescription NameAndDescription = new();
    [Space(10)]

    [Range(0.01f, 2)] public float NPCMultiplier;
    [Range(0.01f, 2)] public float ShippingMultipler;
    public weatherType weatherType;

    public void GetWeatherData(Database database,int ID)
    {
        WeatherSO weather = database.GetWeatherData(ID);

        Icon = weather.data.Icon;
        NameAndDescription.Name = weather.data.NameAndDescription.Name;
        NameAndDescription.Description = weather.data.NameAndDescription.Description;
        NPCMultiplier = weather.data.NPCMultiplier;
        ShippingMultipler = weather.data.ShippingMultipler;
        weatherType = weather.data.weatherType;
    }
}

public enum weatherType
{
    Normal,
    Sunny,
    Wet,
    Cold
}