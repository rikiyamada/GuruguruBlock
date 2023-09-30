using UnityEngine;

public class BGMManager : MonoBehaviour
{
    public static BGMManager Instance { get; private set; } // シングルトンのインスタンス

    private void Awake()
    {
        // 既存のインスタンスが存在する場合
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // この新しいインスタンスを破棄する
            return;
        }

        Instance = this; // このオブジェクトをシングルトンのインスタンスとして設定
        DontDestroyOnLoad(gameObject); // このオブジェクトを破棄しないようにする
    }


}
