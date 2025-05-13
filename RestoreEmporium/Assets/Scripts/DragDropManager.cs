using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragDropManager : MonoBehaviour
{
    public static bool DragDropEnabled = false;

    [Header("Cursor Textures")]
    public Texture2D dragCursor;
    public Texture2D defaultCursor;

    public Vector2 hotspot = Vector2.zero;

    public void ToggleDragMode()
    {
        DragDropEnabled = !DragDropEnabled;
        UpdateCursor();
    }

    public void SetDragMode(bool enabled)
    {
        DragDropEnabled = enabled;
        UpdateCursor();
    }

    private void UpdateCursor()
    {
        if (DragDropEnabled && dragCursor != null)
        {
            Cursor.SetCursor(dragCursor, hotspot, CursorMode.Auto);
        }
        else
        {
            Cursor.SetCursor(defaultCursor, Vector2.zero, CursorMode.Auto);
        }
    }
}