using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildBlock : MonoBehaviour
{
    Rigidbody2D rb;
    GameManager gameManager;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!gameManager.isSleep) return;
        if (rb.velocity.y > 0)
        {
            rb.velocity = Vector2.zero;

            RoundPosition();
        }
    }

    private void RoundPosition()
    {
        int x = Mathf.RoundToInt(transform.position.x);
        int y = Mathf.RoundToInt(transform.position.y);

        transform.position = new Vector2(x, y);
    }
}
