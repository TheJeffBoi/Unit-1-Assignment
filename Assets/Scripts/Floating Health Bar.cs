using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FloatingHealthBar : MonoBehaviour
{
    public Image background1;
    public Image background2;
    public Image health;
    public GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        background1.transform.position = new Vector2(player.transform.position.x, player.transform.position.y + 1.9f);
        background2.transform.position = new Vector2(player.transform.position.x, player.transform.position.y + 1.9f);
        health.transform.position = new Vector2(player.transform.position.x, player.transform.position.y + 1.9f);
    }
}
