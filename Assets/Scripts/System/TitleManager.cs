using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{
    [SerializeField]
    private TMP_Text HighScoreText, CountText, LinesText;
    PlayFabController playFabController;
    AdManager adManager;
    ButtonSound buttonSound;
    public GameObject RankingPanel, ProfilePanel, SoundOnButton, SoundOffButton;
    public InputField inputField;
    public Text placeHolder;
    AudioSource BGMsource;
    private string playerName;
    public static bool isMute;


    void Start()
    {
        playFabController = GameObject.Find("PlayFabController").GetComponent<PlayFabController>();
        buttonSound = GameObject.Find("ButtonSound").GetComponent<ButtonSound>();
        adManager = GameObject.Find("AdManager").GetComponent<AdManager>();
        BGMsource = GameObject.Find("BGMSource").GetComponent<AudioSource>();

        HighScoreText.text = PlayerPrefs.GetInt("HighScore", 0) + "";
        CountText.text = PlayerPrefs.GetInt("PlayCount", 0) + "";
        LinesText.text = PlayerPrefs.GetInt("ClearLines", 0) + "";

        placeHolder.text = PlayerPrefs.GetString("playerName", "なまえを入力");

        if (isMute)
        {
            SoundOffButton.SetActive(true);
        }
        else
        {
            SoundOnButton.SetActive(true);
        }
    }

    public void SendButton()
    {
        playFabController.SendPlayScore(10);
        buttonSound.PlaySound();
    }

    public void RankingButton()
    {
        RankingPanel.SetActive(true);
        playFabController.GetRanking();
        buttonSound.PlaySound();
    }

    public void BannerButton()
    {
        adManager.RequestBanner();
        buttonSound.PlaySound();
    }

    public void InterstitalButton()
    {
        adManager.showInterstitialAd();
        buttonSound.PlaySound();
    }

    public void RewardButton()
    {
        adManager.showRewardAd();
        buttonSound.PlaySound();
    }

    public void CloseRankingButton()
    {
        RankingPanel.SetActive(false);
        buttonSound.PlaySound();
    }

    public void AroundRankingButton()
    {
        playFabController.GetAroundRanking();
        buttonSound.PlaySound();
    }

    public void LoadNameText()
    {
        playerName = inputField.text;
    }

    public void LoadFinish()
    {
        PlayerPrefs.SetString("playerName", playerName);
        PlayerPrefs.Save();
        playFabController.SetUserName(playerName);
        buttonSound.PlaySound();
    }

    public void ProfileButton()
    {
        ProfilePanel.SetActive(true);
        Text AvailableText = GameObject.Find("AvailableText").GetComponent<Text>();
        AvailableText.text = "";
        buttonSound.PlaySound();
    }

    public void ProfileCloseButton()
    {
        ProfilePanel.SetActive(false);
        buttonSound.PlaySound();
    }

    public void SoundOnButtonClicked()
    {
        SoundOnButton.SetActive(false);
        SoundOffButton.SetActive(true);
        isMute = true;
      //  BGMsource.volume = 0;
        buttonSound.PlaySound();
    }

    public void SoundOffButtonClicked()
    {
        SoundOnButton.SetActive(true);
        SoundOffButton.SetActive(false);
        isMute = false;
        //BGMsource.volume = 1;
        buttonSound.PlaySound();
    }

    public void StartButton()
    {
        SceneManager.LoadScene("Gamescene");
    }
}
