using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    Board board;
    GameManager gameManager;
    private void Start()
    {
        board = GameObject.Find("Board").GetComponent<Board>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

    }
    public void MoveSide(int moveDirectionX)
    {
        if (!board.CheckMove(transform, new Vector2(moveDirectionX, 0))) return;
        transform.position = new Vector2(transform.position.x + moveDirectionX, transform.position.y);
    }


    public void MoveDown()
    {
        if (!board.CheckMove(transform, new Vector2(0, -1)))  //ブロックが下に落ちない　→　位置を確定させる！
        {
            gameManager.isSleep = true;
            if(!TitleManager.isMute) board.playBlockSound();
            VibrationMng.ShortVibration();
            RoundChildrenPos();
            board.LoadBlockToGrid(transform);
            board.CheckDeleteLine();
            return;
        }
        transform.position = new Vector2(transform.position.x, transform.position.y - 1);
    }

    private void OnTransformChildrenChanged()
    {
        // 子オブジェクトがなくなったら
        if (transform.childCount == 0)
        {
            // このGameObjectを破壊する
            Destroy(gameObject);
        }
    }

    public void RoundChildrenPos()
    {
        foreach (Transform block in transform)
        {
            block.position = new Vector2(Mathf.RoundToInt(block.position.x), Mathf.RoundToInt(block.position.y));  //座標の誤差を直す
        }
    }
}
