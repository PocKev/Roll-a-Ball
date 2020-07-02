using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour {

    public float speed;
    public float jumpForce;
    public Text countText;
    //public Text winText;
    public GameObject game;
    private GameObject ChildGameObject;
    private GameObject ParentGameObject;

    private Rigidbody rb;
    private int count;
    private bool inAir;

    private float xForce, prevXError, xError;
    private float zForce, prevZError, zError;


    public float p=.16f;
    public float d = 0f;

    private int checker = 10;

  

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        count = 0;
        checker = 0;
        SetCountText();
        //winText.text = "";
        //Debug.Log("Count: " + game.GetComponent<GameHandler>().pickUpsLeft);


        inAir = false;
    }

    private void Update()
    {
        
        float moveUp = 0f;

        if (Input.GetButtonDown("Jump") && !inAir)
        {
            moveUp = jumpForce;
        }
        else
        {
            moveUp = 0f;
        }

        rb.AddForce(new Vector3(0f, moveUp, 0f));

        ParentGameObject = GameObject.FindGameObjectWithTag("Drop It");
      
        
        // NEW assignment: create IEnumerator AutoPilot() to collect all the pickups
        if (Input.GetKeyDown(KeyCode.F))
        {
            
            StartCoroutine(AutoPilot(ParentGameObject));
        }

        if (transform.position.y < -10f)
        {
            SceneManager.LoadScene("MiniGame"); //Load scene called Game
        }
        if (Input.GetKey("escape"))
        {
            Application.Quit();
        }
    }

    IEnumerator AutoPilot(GameObject ParentGameObject)
    {


        while (checker < ParentGameObject.transform.childCount)
        {
            print(ParentGameObject.transform.GetChild(checker).name);

            if (ParentGameObject.transform.GetChild(checker).gameObject.activeSelf)
            {
                StartCoroutine(moveTo(ParentGameObject.transform.GetChild(checker).gameObject));
            }
            else
            {
                checker++;
            }
            yield return null;
        }
        StopCoroutine(AutoPilot(ParentGameObject));
        yield return null;
    }


    IEnumerator moveTo(GameObject TargetObject)
    {
        if (transform.position != ParentGameObject.transform.GetChild(checker).gameObject.transform.position)
        {
            xError = TargetObject.transform.position.x - transform.position.x;
            zError = TargetObject.transform.position.z - transform.position.z;
            xForce = p * (xError) + d * (xError - prevXError) / (float)Time.deltaTime;
            zForce = p * (zError) + d * (zError - prevZError) / (float)Time.deltaTime;
            Vector3 movement = new Vector3(xForce, 0f, zForce);
            rb.AddForce(movement * speed);
            prevXError = xError;
            prevZError = zError;
        }
        yield return null;
    }
  

    void FixedUpdate ()
    {
        inAir = !Physics.Raycast(transform.position, Vector3.down, 0.5f + 0.1f); //0.1 for error, 0.5 is radius of sphere

        float moveHorizontal = Input.GetAxis ("Horizontal");
        float moveVertical = Input.GetAxis ("Vertical");

        Vector3 movement = new Vector3 (moveHorizontal, 0f, moveVertical);

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
                if (count * 2 >= game.GetComponent<GameHandler>().length)
                {
                    game.GetComponent<GameHandler>().winText.text = "Player 1 Wins!";
                }
                else
                {
                    game.GetComponent<GameHandler>().winText.text = "Player 2 Wins!";
                }
            }
        }
    }


    void SetCountText()
    {
        countText.text = "Count: " + count.ToString();
    }
}
