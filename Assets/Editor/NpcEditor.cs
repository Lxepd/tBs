using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Npc))]
public class NpcEditor : Editor
{
    private SerializedObject npcData;
    private SerializedProperty npcType;

    // 道具商人商店刷新时间
    private SerializedProperty itemShopReTime;
    // 各商人商店列表
    private SerializedProperty items, equipmentNpc, craftsMan;
    // 检测玩家距离
    private SerializedProperty checkRadius;

    private void OnEnable()
    {
        npcData = new SerializedObject(target);
        npcType = npcData.FindProperty("type");

        checkRadius = npcData.FindProperty("checkPlayerHereRadius");
        itemShopReTime = npcData.FindProperty("reInitTime");
        // 道具商人
        items = npcData.FindProperty("items");
        // 装备商人
        equipmentNpc = npcData.FindProperty("equipmentNpc");
        // 工匠
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
                EditorGUILayout.PropertyField(itemShopReTime);
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
