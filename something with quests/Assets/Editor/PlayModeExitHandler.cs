#if UNITY_EDITOR
using UnityEditor;

[InitializeOnLoad]
public static class PlayModeExitHandler
{
    static PlayModeExitHandler()
    {
        EditorApplication.playModeStateChanged += PlayModeStateChanged;
    }

    private static void PlayModeStateChanged(PlayModeStateChange state)
    {
        if (state == PlayModeStateChange.ExitingPlayMode)
        {
            ResetQuests();
        }
    }

    private static void ResetQuests()
    {
        string[] guids = AssetDatabase.FindAssets("t:QuestInfoSo");

        foreach (var guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            QuestInfoSo questObject = AssetDatabase.LoadAssetAtPath<QuestInfoSo>(path);

            if (questObject != null && questObject.resetQuest)
            {
                questObject.Reset();
                EditorUtility.SetDirty(questObject);
            }
        }

        AssetDatabase.SaveAssets();
    }
}
#endif
