using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HammerManager : MonoBehaviour
{
    public static bool HammerEnabled = false;

    [Header("Cursor Textures")]
    public Texture2D hammerCursor;
    public Texture2D defaultCursor;

    public Vector2 hotspot = Vector2.zero;

    public void ToggleHammerMode()
    {
        HammerEnabled = !HammerEnabled;
        UpdateCursor();
    }

    public void SetHammerMode(bool enabled)
    {
        HammerEnabled = enabled;
        UpdateCursor();
    }

    private void UpdateCursor()
    {
        if (HammerEnabled && hammerCursor != null)
        {
            Cursor.SetCursor(hammerCursor, hotspot, CursorMode.Auto);
        }
        else
        {
            Cursor.SetCursor(defaultCursor, Vector2.zero, CursorMode.Auto);
        }
    }
}
