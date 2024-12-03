using UnityEditor;
using UnityEditor.EditorTools;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapAreaTool : EditorTool, IDrawSelectedHandles
{
    public bool isDragging = false;
    public bool forceDraw = false;
    public bool ignoreClick = false;
    public Vector3Int start;
    public Vector3Int stop;

    protected Tilemap map;

    private GUIContent icon;

    public override GUIContent toolbarIcon => icon;

    public virtual void OnEnable()
    {
        icon = new GUIContent(EditorGUIUtility.GetIconForObject(target));
        map = FindObjectOfType<Tilemap>();
    }

    public override void OnWillBeDeactivated()
    {
        forceDraw = false;
    }

    public override void OnToolGUI(EditorWindow window)
    {
        if (!(window is SceneView)) return;

        HandleUtility.AddDefaultControl(GUIUtility.GetControlID(FocusType.Keyboard));

        Event e = Event.current;
        Vector3Int? mouseCell = GetMouseCell(e);

        switch (e.type)
        {
            case EventType.MouseDown:
                if (e.button == 0 && !ignoreClick && mouseCell.HasValue)
                {
                    isDragging = true;
                    start = stop = mouseCell.Value;
                }
                break;

            case EventType.MouseDrag:
                if (isDragging && mouseCell.HasValue)
                {
                    stop = mouseCell.Value;
                }
                break;

            case EventType.MouseUp:
                if (isDragging && e.button == 0)
                {
                    isDragging = false;
                    OnFinish();
                }
                break;
        }
    }

    public virtual Color handleColor => Color.white;

    public void OnDrawHandles()
    {
        if (isDragging || forceDraw)
        {
            Handles.color = handleColor;
            TileSelectionHelper.DrawBounds(map, start, stop);
            HandleUtility.Repaint();
        }
    }

    private Vector3Int? GetMouseCell(Event e)
    {
        Matrix4x4 worldMatrix = map.transform.localToWorldMatrix;
        Plane tilemapPlane = new Plane(
            worldMatrix.MultiplyPoint3x4(Vector3.zero),
            worldMatrix.MultiplyPoint3x4(Vector3.one),
            worldMatrix.MultiplyPoint3x4(new Vector3(1, 0, 0))
        );

        Ray mouseRay = HandleUtility.GUIPointToWorldRay(e.mousePosition);
        if (tilemapPlane.Raycast(mouseRay, out float enter))
        {
            return map.layoutGrid.WorldToCell(mouseRay.GetPoint(enter));
        }
        return null;
    }

    public virtual void OnFinish()
    {
        
    }
}
