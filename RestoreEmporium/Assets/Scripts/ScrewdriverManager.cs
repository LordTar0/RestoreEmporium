using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrewdriverManager : MonoBehaviour
{
    public static bool ScrewdriverEnabled = false;

    [Header("Cursor Textures")]
    public Texture2D screwdriverCursor;
    public Texture2D defaultCursor;

    public Vector2 hotspot = Vector2.zero;

    public void ToggleScrewdriverMode()
    {
        ScrewdriverEnabled = !ScrewdriverEnabled;
        UpdateCursor();
    }

    public void SetScrewdriverMode(bool enabled)
    {
        ScrewdriverEnabled = enabled;
        UpdateCursor();
    }

    private void UpdateCursor()
    {
        if (ScrewdriverEnabled && screwdriverCursor != null)
        {
            Cursor.SetCursor(screwdriverCursor, hotspot, CursorMode.Auto);
        }
        else
        {
            Cursor.SetCursor(defaultCursor, Vector2.zero, CursorMode.Auto);
        }
    }
}
