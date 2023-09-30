using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Board : MonoBehaviour
{
    [SerializeField]
    private Transform BoardSprite;
    private int height = 10, width = 10, totalDeleteLine = 0;
    private GameObject[,] grid;
    private GameManager gameManager;

    [SerializeField]
    private AudioSource blockSound, deleteSound;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        grid = new GameObject[width, height + 5];
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Transform clone = Instantiate(BoardSprite, new Vector3(x, y, 0), Quaternion.identity);
                clone.parent = transform;
            }
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            string text = "";
            for (int y = 9; y >= 0; y--)
            {
                for (int x = 0; x < 10; x++)
                {
                    if (grid[x, y] == null)
                    {
                        text += "0,";
                    }
                    else
                    {
                        text += "1, ";
                    }
                }
                text += "\n";
            }

            Debug.Log(text);
        }

        if (Input.GetKeyDown(KeyCode.R)) SceneManager.LoadScene(SceneManager.GetActiveScene().name);

    }

    public bool CheckMove(Transform parent, Vector2 direction)
    {
        foreach (Transform block in parent)
        {
            int x = Mathf.RoundToInt(block.position.x + direction.x);
            int y = Mathf.RoundToInt(block.position.y + direction.y);

            if (x < 0 || x > 9) return false;
            if (y < 0) return false;

            if (grid[x, y] != null)
            {
                if (grid[x, y].transform.parent != parent)
                {
                    return false;
                }
            }
        }

        return true;
    }

    public bool CheckRotate(Transform parent)
    {
        foreach (Transform block in parent)
        {
            int x = Mathf.RoundToInt(block.transform.position.x);
            int y = Mathf.RoundToInt(block.transform.position.y);

            if (x < 0 || x > 9 || y < 0) return false;
            if (grid[x, y]) return false;
        }

        return true;
    }

    public void LoadBlockToGrid(Transform parent)
    {
        foreach (Transform block in parent)
        {

            int x = Mathf.RoundToInt(block.transform.position.x);
            int y = Mathf.RoundToInt(block.transform.position.y);

            grid[x, y] = block.gameObject;
        }
    }


    public void CheckDeleteLine()
    {
        int deleteLines = 0;
        for (int y = 0; y < height; y++)
        {
            bool fullLine = true;
            for (int x = 0; x < width; x++)
            {
                if (grid[x, y] == null) fullLine = false;
            }

            if (fullLine)
            {
                deleteLines++;
                totalDeleteLine++;

                PlayerPrefs.SetInt("ClearLines", PlayerPrefs.GetInt("ClearLines", 0) + 1);
                PlayerPrefs.Save();

                StartCoroutine(DestroyCoroutine(y));
            }

        }

        if (deleteLines == 0)
        {
            CheckGameOver();
        }
        else
        {
            deleteSound.Play();
            gameManager.addScore(totalDeleteLine);
            totalDeleteLine = 0;
            StartCoroutine(ChangeGravityScaleCoroutine());

        }
    }

    private IEnumerator DestroyCoroutine(int deletey)
    {
        float time = 0f;

        Vector3 initialScale = transform.localScale;
        Quaternion initialRotation = Quaternion.identity;
        Quaternion finalRotation = Quaternion.Euler(0, 0, 180);

        while (time < 1f)
        {
            for (int x = 0; x < width; x++)
            {
                if (grid[x, deletey] != null)
                {
                    grid[x, deletey].transform.localScale = Vector3.Lerp(initialScale, Vector3.zero, time / 1f);
                    grid[x, deletey].transform.rotation = Quaternion.Lerp(initialRotation, finalRotation, time / 1f);
                }
            }
            time += Time.deltaTime;
            yield return null;
        }

        for (int x = 0; x < width; x++)
        {
            Destroy(grid[x, deletey]);
        }
    }


    private IEnumerator ChangeGravityScaleCoroutine()
    {
        yield return new WaitForSeconds(1.1f); //消えるアニメーションが終わるのを待機
        gameManager.ChangeGravityScale(1);
        yield return new WaitForSeconds(1.5f); //落下を待機
        gameManager.ChangeGravityScale(0);
        UpDateGrid();
        CheckDeleteLine();
    }


    private void CheckGameOver()
    {
        for (int x = 0; x < width; x++)
        {
            if (grid[x, 10])
            {
                gameManager.GameOver();
                return;
            }
        }

        gameManager.Spawn();
        
    }

    public void UpDateGrid()
    {

        for (int y = 0; y < height + 4; y++)
        {
            for (int x = 0; x < width; x++)
            {
                grid[x, y] = null;
            }

        }

        foreach (Block parent in FindObjectsOfType<Block>())
        {
            foreach (Transform block in parent.transform)
            {
                int x = Mathf.RoundToInt(block.transform.position.x);
                int y = Mathf.RoundToInt(block.transform.position.y);

                block.position = new Vector2(x, y);

                grid[x, y] = block.gameObject;
            }
        }

    }

    public void playBlockSound()
    {
        blockSound.Play();
    }


}
