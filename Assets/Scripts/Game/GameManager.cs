using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private TMP_Text scoreText, deleteText, hiscoreText;
    [SerializeField]
    private GameObject ResultPanel, RankingPanel;
    private PlayFabController playFabController;
    private Animator deleteAnimator, resultScoreAnimator, WarningAnimator;
    public Spawner spawner;
    public Board board;
    public GameObject Floor;
    Block activeBlock;
    private float dropInterval = 2f, nextDropTime = 0, tapTime = 0;
    public bool isSleep = true, rotate = false, scoreUp = false;
    Vector3 startPos;
    private int score = 0;

    void Start()
    {
        Spawn();
        WarningAnimator = GameObject.Find("WarningAnimator").GetComponent<Animator>();
        deleteAnimator = GameObject.Find("DeleteAnimator").GetComponent<Animator>();
        resultScoreAnimator = GameObject.Find("ScoreText").GetComponent<Animator>();
        playFabController = GameObject.Find("PlayFabController").GetComponent<PlayFabController>();

        PlayerPrefs.SetInt("PlayCount", PlayerPrefs.GetInt("PlayCount",0) + 1);
        PlayerPrefs.Save();
    }


    void Update()
    {
        if (isSleep) return;
        if (!activeBlock) return;

        if (Input.GetMouseButtonDown(0))
        {
            startPos = Input.mousePosition;
            tapTime = Time.time;
        }

        if (Input.GetMouseButton(0))
        {
            Vector3 mousePos = Input.mousePosition;
            if(startPos.y - mousePos.y > 250)
            {
                nextDropTime -= Time.deltaTime * 30f;
            }
            else
            {
                if(mousePos.x - startPos.x > 100)
                {
                    startPos.x = mousePos.x;
                    activeBlock.MoveSide(Mathf.RoundToInt(1));
                }
                else if(mousePos.x - startPos.x < -100)
                {
                    startPos.x = mousePos.x;
                    activeBlock.MoveSide(Mathf.RoundToInt(-1));
                }
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (tapTime - Time.time < 0.1 && (startPos - Input.mousePosition).magnitude < 1)  //すぐ指を離す&&指の位置があまり変わってない　→　タップの検出
            {
                RotateBlock();
            }
        }

        if (Time.time > nextDropTime)
        {
            nextDropTime = Time.time + dropInterval;
            activeBlock.MoveDown();
        }
    }

    public void Spawn()
    {
        if (rotate) //回転するならスポーン処理を一旦中断
        {
            rotate = false;
            StartCoroutine("RotateBoard");
            return;
        }

        nextDropTime = Time.time + dropInterval;
        activeBlock = spawner.Spawn();
        activeBlock.transform.parent = board.transform;
        startPos = new Vector3(0,0,0);
        if (rotate)
        {
            WarningAnimator.SetBool("isWarning", true);
        }
        isSleep = false;
    }

    private void RotateBlock()
    {
        activeBlock.transform.Rotate(0, 0, 90);
        if (!board.CheckRotate(activeBlock.transform)) activeBlock.transform.Rotate(0, 0, -90);

    }

    private IEnumerator RotateBoard() 
    {
        scoreUp = true;
        Vector3 beforeRotation = board.transform.rotation.eulerAngles;
        Floor.SetActive(false);
        float rotationAngle = 0f;
        float rotationSpeed = 90f / 2f; // 90度を2秒間で分割

        WarningAnimator.Play("Warning", 0, 0f); // アニメーションの再生位置をリセット
        WarningAnimator.SetBool("isWarning", false);



        while (rotationAngle < 90f)
        {
            float step = rotationSpeed * Time.deltaTime;
            board.transform.Rotate(0, 0, step);
            rotationAngle += step;
            yield return null;
        }

        board.transform.rotation = Quaternion.Euler(beforeRotation.x, beforeRotation.y, beforeRotation.z + 90f);

        Floor.SetActive(true);
        
        ChangeGravityScale(1);

        yield return new WaitForSeconds(1.5f);

        ChangeGravityScale(0);
        board.UpDateGrid();
        board.CheckDeleteLine();
        scoreUp = false;
    }

    public void ChangeGravityScale(int gravity)
    {
        bool isCollider2D = gravity == 1;

        foreach (Block parent in FindObjectsOfType<Block>())
        {
            foreach (Transform block in parent.transform)
            {
                Rigidbody2D rb = block.GetComponent<Rigidbody2D>();
                rb.gravityScale = gravity;

                block.GetComponent<BoxCollider2D>().enabled = isCollider2D;
                if(gravity == 0)
                rb.velocity = Vector2.zero;
            }
        }
    }

    public void addScore(int lines)
    {
        int addPoint =  10 * (int)Math.Pow(2, lines - 1);
        if (scoreUp)
        {
            addPoint *= 2;
        }

        score += addPoint;
        scoreText.text = score + "";

        deleteText.text = "+" + addPoint;
        deleteAnimator.Play("DeleteAnimation", 0, 0);
    }

    public void GameOver()
    {
        int highScore = PlayerPrefs.GetInt("HighScore", 0);

        ResultPanel.SetActive(true);
        hiscoreText.text = "HighSocre\n" + highScore;

        resultScoreAnimator.transform.SetParent(ResultPanel.transform, false);
        resultScoreAnimator.transform.SetAsFirstSibling();
        resultScoreAnimator.Play("ResultScore",0,0);

        playFabController.SendPlayScore(score);

        if (highScore < score)
        {
            PlayerPrefs.SetInt("HighScore", score);
            PlayerPrefs.Save();
        }
    }

    public void RetryButton()
    {
        VibrationMng.ShortVibration();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void HomeButton()
    {
        VibrationMng.ShortVibration();
        SceneManager.LoadScene("TitleScene");
    }

    public void RankingButton()
    {
        RankingPanel.SetActive(true);
        playFabController.GetRanking();
        VibrationMng.ShortVibration();
    }

    public void RankingCloseButton()
    {
        RankingPanel.SetActive(false);
        VibrationMng.ShortVibration();
    }

    public void AroundButton()
    {
        playFabController.GetAroundRanking();
        VibrationMng.ShortVibration();
    }
}