using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ReTimeEditorWindow : EditorWindow
{
    ReTimeEditorWindow()
    {
        this.titleContent = new GUIContent("ˢ��ʱ�����ô���");
    }

    [MenuItem("Tool/ˢ��ʱ�����ô��� %h")]
    static void ShowWindow()
    {
        GetWindow(typeof(ReTimeEditorWindow));
    }

    private void OnGUI()
    {
        
    }
}
