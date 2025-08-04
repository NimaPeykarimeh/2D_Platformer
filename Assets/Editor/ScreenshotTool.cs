using UnityEditor;
using UnityEngine;
using System.IO;

public class ScreenshotTool : EditorWindow
{
    private string folderName = "Screenshots";
    private string filePrefix = "screenshot_";
    private int superSize = 1; // 1 = normal res, 2 = 2x res, etc.

    [MenuItem("Tools/Screenshot Tool")]
    public static void ShowWindow()
    {
        GetWindow<ScreenshotTool>("Screenshot Tool");
    }

    private void OnGUI()
    {
        GUILayout.Label("Screenshot Settings", EditorStyles.boldLabel);

        folderName = EditorGUILayout.TextField("Folder Name", folderName);
        filePrefix = EditorGUILayout.TextField("File Prefix", filePrefix);
        superSize = EditorGUILayout.IntSlider("Resolution Multiplier", superSize, 1, 4);

        GUILayout.Space(10);

        if (GUILayout.Button("Take Screenshot"))
        {
            TakeScreenshot();
        }
    }

    private void TakeScreenshot()
    {
        string folderPath = Path.Combine(Application.dataPath, folderName);

        if (!Directory.Exists(folderPath))
            Directory.CreateDirectory(folderPath);

        string timestamp = System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
        string fileName = $"{filePrefix}{timestamp}.png";
        string filePath = Path.Combine(folderPath, fileName);

        ScreenCapture.CaptureScreenshot(filePath, superSize);
        Debug.Log($"Screenshot saved to: {filePath}");
    }
}
