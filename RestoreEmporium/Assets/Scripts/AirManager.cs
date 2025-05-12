using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirManager : MonoBehaviour
{
    public static bool AirEnabled = false;

    [Header("Cursor Textures")]
    public Texture2D airCursor;
    public Texture2D defaultCursor;

    public Vector2 hotspot = Vector2.zero;

    public void ToggleAirMode()
    {
        AirEnabled = !AirEnabled;
        UpdateCursor();
    }

    public void SetAirMode(bool enabled)
    {
        AirEnabled = enabled;
        UpdateCursor();
    }

    private void UpdateCursor()
    {
        if (AirEnabled && airCursor != null)
        {
            Cursor.SetCursor(airCursor, hotspot, CursorMode.Auto);
        }
        else
        {
            Cursor.SetCursor(defaultCursor, Vector2.zero, CursorMode.Auto);
        }
    }
}