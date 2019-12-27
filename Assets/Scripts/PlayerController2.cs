using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController2 : MonoBehaviour
{
    public float speed;
    public float jumpForce;
    public Text countText;
    //public Text winText;
    public GameObject game;

    private Rigidbody rb;
    private int count;
    private bool inAir;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        count = 0;
        SetCountText();
        //winText.text = "";

        inAir = false;
    }

    // Update is called once per frame
    void Update()
    {
        float moveUp = 0f;

        if (Input.GetButtonDown("Jump_2") && !inAir)
        {
            moveUp = jumpForce;
        }
        else
        {
            moveUp = 0f;
        }

        rb.AddForce(new Vector3(0f, moveUp, 0f));

        if (transform.position.y < -10f)
        {
            SceneManager.LoadScene("MiniGame"); //Load scene called Game
        }
    }

    void FixedUpdate()
    {
        inAir = !Physics.Raycast(transform.position, Vector3.down, 0.5f + 0.1f); //0.1 for error, 0.5 is radius of sphere

        float moveHorizontal = Input.GetAxis("Horizontal_2");
        float moveVertical = Input.GetAxis("Vertical_2");

        Vector3 movement = new Vector3(moveHorizontal, 0f, moveVertical);

        if (!inAir)
        {
            rb.AddForce(movement * speed);
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Pick Up"))
        {
            other.gameObject.SetActive(false);
            count++;
            SetCountText();

            if (game.GetComponent<GameHandler>().FindPickUpsLeft() == 0)
            {
                if (count*2 >= game.GetComponent<GameHandler>().length)
                {
                    game.GetComponent<GameHandler>().winText.text = "Player 2 Wins!";
                }
                else
                {
                    game.GetComponent<GameHandler>().winText.text = "Player 1 Wins!";
                }
            }
        }
    }

    void SetCountText()
    {
        countText.text = "Count: " + count.ToString();
    }
}
