using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameHandler : MonoBehaviour
{
    public GameObject pickUps;
    public int pickUpsLeft, length;
    public Text winText;

    // Start is called before the first frame update
    void Awake()
    {
        length = pickUps.transform.childCount;
        pickUpsLeft = length;
    }

    private void Start()
    {
        winText.text = "";
    }

    // Update is called once per frame
    public int FindPickUpsLeft()
    {
        pickUpsLeft = 0;
        for (int i = 0; i < length; i++)
        {
            if (pickUps.transform.GetChild(i).gameObject.activeSelf)
            {
                pickUpsLeft++;
            }
        }
        //Debug.Log("Left: " + pickUpsLeft);
        return pickUpsLeft;
    }
}
