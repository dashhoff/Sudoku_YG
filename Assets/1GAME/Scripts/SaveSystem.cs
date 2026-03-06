using UnityEngine;
using YG;

public static class SaveSystem
{
    private const string _key = "sudoku_save";

    public static void Save(GameSave data)
    {
        string json = JsonUtility.ToJson(data);

        YG2.saves._sudokuSave = json;
        YG2.SaveProgress();
    }

    public static GameSave Load()
    {
        string json = YG2.saves._sudokuSave;

        if (string.IsNullOrEmpty(json))
            return null;

        return JsonUtility.FromJson<GameSave>(json);
    }

    public static void Clear()
    {
        YG2.saves._sudokuSave = "";
        YG2.SaveProgress();
    }
}