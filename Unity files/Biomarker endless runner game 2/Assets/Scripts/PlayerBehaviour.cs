using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PlayerBehaviour : MonoBehaviour
{
    [SerializeField] SkinnedMeshRenderer skinnedRenderer;
    [SerializeField] List<Material> wireframeMaterials = new List<Material>();
    [SerializeField] AudioSource audioSource;
    [SerializeField] Volume volume;

    private Vignette vignette;

    public int lives;
    public int score;
    private int startLives = 3;
    private float hitProtection = 0.2f;
    private float lastHit = 0f;
    

    public event Action increaseScore;
    public event Action takeDamage;
    public event Action <string,string> changeColor;

    void Start()
    {
        volume.profile.TryGet(out vignette);
        vignette.intensity.value = 0;
        score = 0;
        lives = startLives;
        gameObject.tag = "red";
        skinnedRenderer.material = wireframeMaterials[0];
        lastHit = Time.time;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (Time.time - lastHit < hitProtection)
            return;
        lastHit = Time.time;

        if(other.tag != gameObject.tag)
        {
            if(other.tag == "Entrance")
            {
                return;
            }

            TakeDamage();
            Destroy(other.transform.parent.parent.gameObject);
        }
        else
        {
            increaseScore();
            Destroy(other.transform.parent.gameObject);
        }

    }
    private void TakeDamage()
    {
        takeDamage();
        PlayerStats.lives--;
        audioSource.Play();
        StartCoroutine(VignetteBlink(0.5f, 2));
        //StartCoroutine(ColorBlink(4, 1f));
    }

    public void ChangeColor(string color)
    {
        if(changeColor != null)
        {
            changeColor(gameObject.tag, color);
        }
        
        PlayerStats.totalColorChanges++;
        if (color == gameObject.tag)
        {
            PlayerStats.falseColorChanges++;
        }
        switch (color)
        {
            case "red":
                skinnedRenderer.material = wireframeMaterials[0];
                gameObject.tag = color;
                break;
            case "green":
                skinnedRenderer.material = wireframeMaterials[1];
                gameObject.tag = color;
                break;
            case "blue":
                skinnedRenderer.material = wireframeMaterials[2];
                gameObject.tag = color;
                break;
        }
    }

    #region animations
    private IEnumerator VignetteBlink(float max,float speed)
    {
        float min = 0;
        float t = 0;
        while (t < 1)
        {
            float intensity = Mathf.Lerp(min, max, t);
            vignette.intensity.value = intensity;
            t += speed * Time.deltaTime;
            yield return new WaitForEndOfFrame();
            
        }

        while(t > 0)
        {
            float intensity = Mathf.Lerp(min, max, t);
            vignette.intensity.value = intensity;
            t -= speed * Time.deltaTime;
            yield return new WaitForEndOfFrame();
            
        }

        
        
    }
    private IEnumerator ColorBlink(int times, float timeToBlink)
    {
        Color startColor = skinnedRenderer.material.color;
        Color midColor = Color.white;
        float startTime = Time.time;
        while (Time.time - timeToBlink < startTime)
        {
            Color lerpedColor = Color.LerpUnclamped(Color.red, Color.white, Mathf.PingPong(Time.time*4, 1));
            skinnedRenderer.material.color = lerpedColor;
            yield return new WaitForEndOfFrame();
        }

        skinnedRenderer.material.color = startColor;
        
    }
    #endregion
}
