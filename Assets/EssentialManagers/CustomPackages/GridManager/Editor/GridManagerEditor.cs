using UnityEngine;
using UnityEditor;
using EssentialManagers.CustomPackages.GridManager.Scripts;

[CustomEditor(typeof(GridManager))]
public class GridManagerEditor : Editor
{
    private const int cellSize = 60; // 2D grid hücre boyutu (2 kat daha büyük)
    private Color cellColor = Color.white;
    
    private SerializedProperty obstacleIcon;
    private SerializedProperty occupantIcon;
    private SerializedProperty setEmptyIcon;
    private bool isGridGenerated;

    private void OnEnable()
    {
        serializedObject.Update();
    
        obstacleIcon = serializedObject.FindProperty("obstacleIcon");
        occupantIcon = serializedObject.FindProperty("occupantIcon");
        setEmptyIcon = serializedObject.FindProperty("setEmptyIcon");

        if (obstacleIcon == null || occupantIcon == null || setEmptyIcon == null)
        {
            Debug.LogWarning("One or more SerializedProperties could not be found in GridManager.");
        }
    }

    public override void OnInspectorGUI()
    {
        GridManager gridManager = (GridManager)target;
        
        EditorGUILayout.LabelField("Grid Configuration", EditorStyles.boldLabel);
        gridManager.gridWidth = EditorGUILayout.IntField("Grid Width", gridManager.gridWidth);
        gridManager.gridHeight = EditorGUILayout.IntField("Grid Height", gridManager.gridHeight);
        gridManager.cellSpacing = EditorGUILayout.FloatField("Cell Spacing", gridManager.cellSpacing);
        
        if (GUILayout.Button("Generate Grid"))
        {
            gridManager.gridPlan = new System.Collections.Generic.List<CellController>();
            gridManager.gameObject.transform.position = Vector3.zero;
            isGridGenerated = true;
        }

        GUILayout.Space(10);
        DrawGridControlButtons();
        GUILayout.Space(10);
        
        if (isGridGenerated)
        {
            DrawGridPreview(gridManager);
        }
        
        DrawDefaultInspector();
    }

    private void DrawGridControlButtons()
    {
        GUILayout.BeginHorizontal();
    
        // Drawing the obstacle icon button with the current icon reference displayed
        if (GUILayout.Button(new GUIContent("Obstacle", GetScaledTexture((Texture)obstacleIcon.objectReferenceValue))))
        {
            SetIcon(obstacleIcon, "Select Obstacle Icon");
        }

        // Drawing the occupant icon button with the current icon reference displayed
        if (GUILayout.Button(new GUIContent("Occupant", GetScaledTexture((Texture)occupantIcon.objectReferenceValue))))
        {
            SetIcon(occupantIcon, "Select Occupant Icon");
        }

        // Drawing the set empty icon button with the current icon reference displayed
        if (GUILayout.Button(new GUIContent("Empty", GetScaledTexture((Texture)setEmptyIcon.objectReferenceValue))))
        {
            SetIcon(setEmptyIcon, "Select Set Empty Icon");
        }
    
        GUILayout.EndHorizontal();
        serializedObject.ApplyModifiedProperties();
    }

    private Texture GetScaledTexture(Texture original)
    {
        if (original == null) return null;
        
        // Scale the texture by 10x smaller (0.1 of the original size)
        Texture2D scaledTexture = new Texture2D(original.width / 10, original.height / 10);
        Color[] pixels = ((Texture2D)original).GetPixels(0, 0, original.width, original.height);
        scaledTexture.SetPixels(pixels);
        scaledTexture.Apply();
        return scaledTexture;
    }

    private void SetIcon(SerializedProperty iconProperty, string title)
    {
        // Allow the user to select the icon texture using an object field
        Texture2D selectedIcon = (Texture2D)EditorGUILayout.ObjectField(title, iconProperty.objectReferenceValue, typeof(Texture2D), false);
        iconProperty.objectReferenceValue = selectedIcon;
    }

    private void DrawGridPreview(GridManager gridManager)
    {
        GUILayout.Space(10);
        EditorGUILayout.LabelField("Grid Preview", EditorStyles.boldLabel);

        int gridWidth = gridManager.gridWidth;
        int gridHeight = gridManager.gridHeight;

        Rect gridRect = GUILayoutUtility.GetRect(gridWidth * cellSize, gridHeight * cellSize);
        GUI.Box(gridRect, "");

        for (int y = 0; y < gridHeight; y++)
        {
            for (int x = 0; x < gridWidth; x++)
            {
                Rect cellRect = new Rect(
                    gridRect.x + x * cellSize,
                    gridRect.y + y * cellSize,
                    cellSize, cellSize);
                EditorGUI.DrawRect(cellRect, cellColor);
                GUI.Box(cellRect, "", EditorStyles.helpBox);
            }
        }
    }
}
