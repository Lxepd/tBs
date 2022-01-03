using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Npc))]
public class NpcEditor : Editor
{
    private SerializedObject npcData;
    private SerializedProperty npcType;

    private SerializedProperty items, equipmentNpc, craftsMan;
    private SerializedProperty checkRadius;

    private void OnEnable()
    {
        npcData = new SerializedObject(target);
        npcType = npcData.FindProperty("type");

        checkRadius = npcData.FindProperty("checkPlayerHereRadius");

        // ��������
        items = npcData.FindProperty("items");
        // װ������
        equipmentNpc = npcData.FindProperty("equipmentNpc");
        // ����
        craftsMan = npcData.FindProperty("craftsMan");
    }

    public override void OnInspectorGUI()
    {
        npcData.Update();
        EditorGUILayout.PropertyField(npcType);
        EditorGUILayout.PropertyField(checkRadius);

        switch (npcType.enumValueIndex)
        {
            case 0:
                break;
            case 1:
                EditorGUILayout.PropertyField(items);
                break;
            case 2:
                EditorGUILayout.PropertyField(equipmentNpc);
                break;
            case 3:
                EditorGUILayout.PropertyField(craftsMan);
                break;
        }

        npcData.ApplyModifiedProperties();
    }
}
