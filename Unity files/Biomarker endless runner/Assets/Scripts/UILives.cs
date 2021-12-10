using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UILives : MonoBehaviour
{
    public Sprite fullHeart;
    public Sprite emptyHeart;
    private Image[] hearts;


    // Start is called before the first frame update
    void Start()
    {
        hearts = GetComponentsInChildren<Image>();
        ResetLives();
    }


    //Reset the sprites for the lives back to full
    public void ResetLives()
    {
        foreach(Image h in hearts)
        {
            h.sprite = fullHeart;
        }
    }

    //Take the last full heart, play the damage animation and set it to empty
    public void TakeDamage()
    {
        for(int i = hearts.Length-1; i >= 0; i--)
        {
            if(hearts[i].sprite == fullHeart)
            {
                StartCoroutine(HeartBlink(hearts[i],1));
                
                break;
            }
        }

    }

    // Blinking animation for the hearts
    private IEnumerator HeartBlink(Image heart, float timeToBlink)
    {
        Color startColor = new Color(1,1,1,1);
        Color endColor = new Color(1,1,1,0);
        float startTime = Time.time;
        while (Time.time - timeToBlink < startTime)
        {
            Color lerpedColor = Color.LerpUnclamped(startColor, endColor, Mathf.PingPong(Time.time * 4, 1));
            heart.color = lerpedColor;
            yield return new WaitForEndOfFrame();
        }

        heart.color = startColor;
        heart.sprite = emptyHeart;

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
