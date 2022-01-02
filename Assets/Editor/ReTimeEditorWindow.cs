using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ReTimeEditorWindow : EditorWindow
{
    ReTimeEditorWindow()
    {
        this.titleContent = new GUIContent("刷新时间设置窗口");
    }

    [MenuItem("Tool/刷新时间设置窗口 %h")]
    static void ShowWindow()
    {
        GetWindow(typeof(ReTimeEditorWindow));
    }

    private void OnGUI()
    {
        
    }
}
