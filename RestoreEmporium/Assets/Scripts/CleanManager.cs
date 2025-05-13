using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CleanManager : MonoBehaviour
{
    public static bool CleaningEnabled = false;

    [Header("Cursor Textures")]
    public Texture2D cleanCursor;
    public Texture2D defaultCursor;

    public Vector2 hotspot = Vector2.zero;

    public void ToggleCleanMode()
    {
        CleaningEnabled = !CleaningEnabled;
        UpdateCursor();
    }

    public void SetCleanMode(bool enabled)
    {
        CleaningEnabled = enabled;
        UpdateCursor();
    }

    private void UpdateCursor()
    {
        if (CleaningEnabled && cleanCursor != null)
        {
            Cursor.SetCursor(cleanCursor, hotspot, CursorMode.Auto);
        }
        else
        {
            Cursor.SetCursor(defaultCursor, Vector2.zero, CursorMode.Auto);
        }
    }
}
