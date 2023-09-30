using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField]
    private Block[] Blocks;

    [SerializeField]
    private GameObject[] Pictures;

    [SerializeField]
    private GameObject WarningIcon;

    private GameManager gameManager;
    private int[] spawnOrder;
    private int spawnTimes = 0;
    private Vector3[] pictureSpawnPos, warningSpawnPos;
    private GameObject[] orderBlocks, warningIcons;
    // Start is called before the first frame update
    void Awake()
    {
        spawnOrder = new int[5];
        pictureSpawnPos = new Vector3[spawnOrder.Length];
        warningSpawnPos = new Vector3[spawnOrder.Length];
        orderBlocks = new GameObject[spawnOrder.Length];
        warningIcons = new GameObject[spawnOrder.Length];

        for (int i = 0; i < spawnOrder.Length; i++)
        {
            spawnOrder[i] = Random.Range(0, Blocks.Length);

            pictureSpawnPos[i] = new Vector2(0.5f + 2 * i, -2);
            warningSpawnPos[i] = new Vector2(0.5f + 2 * i, -4);
            orderBlocks[i] = Instantiate(Pictures[spawnOrder[i]], pictureSpawnPos[i], Quaternion.identity);
        }

        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

    }

    // Update is called once per frame
    public Block Spawn()
    {
        spawnTimes++;
        Vector2 blockSpawnPos;

        int spawnNum = spawnOrder[0];  //一旦保持

        Destroy(orderBlocks[0]);
        if (warningIcons[0])
        {
            Destroy(warningIcons[0]);
            gameManager.rotate = true;
        }
        else gameManager.rotate = false;

        for (int i = 0; i < spawnOrder.Length - 1; i++) //配列の中身を一個となりにずらす
        {
            spawnOrder[i] = spawnOrder[i + 1];
            orderBlocks[i] = orderBlocks[i + 1];
            orderBlocks[i].transform.position = pictureSpawnPos[i];

            warningIcons[i] = warningIcons[i + 1];
            if (warningIcons[i]) warningIcons[i].transform.position = warningSpawnPos[i];

        }

        spawnOrder[spawnOrder.Length - 1] = Random.Range(0, Blocks.Length);
        orderBlocks[orderBlocks.Length - 1] = Instantiate(Pictures[spawnOrder[spawnOrder.Length - 1]], pictureSpawnPos[pictureSpawnPos.Length - 1], Quaternion.identity);

        if (Random.Range(0, 5) == 1 || spawnTimes == 1) //ランダム　または　最初の５個目の保留に回転マークを生成
        {
            warningIcons[warningIcons.Length - 1] = Instantiate(WarningIcon, warningSpawnPos[warningSpawnPos.Length - 1], Quaternion.identity);
        }
        else
        {
            warningIcons[warningIcons.Length - 1] = null;
        }

        if (spawnNum == 0)
        {
            blockSpawnPos = new Vector2(5, 12);
        }
        else if (spawnNum == 1 || spawnNum == 2 || spawnNum == 4)
        {
            blockSpawnPos = new Vector2(4.5f, 11.5f);
        }
        else
        {
            blockSpawnPos = new Vector2(5, 12);
        }



        return Instantiate(Blocks[spawnNum], blockSpawnPos, Quaternion.identity);
    }
}