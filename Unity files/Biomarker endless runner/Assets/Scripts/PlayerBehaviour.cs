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


    // Enum for keeping track of the player character's current color
    private enum CurrentColor
    {
        red,
        green,
        blue
    }

    CurrentColor currentColor = CurrentColor.red;


    #region variables
    public int lives;
    public int score;
    private int startLives = 3;
    private float hitProtection = 0.2f;
    private float lastHit = 0f;
    #endregion

    public event Action increaseScore;
    public event Action takeDamage;
    public event Action<string, string> changeColor;


    // Start is called before the first frame update
    void Start()
    {
        volume.profile.TryGet(out vignette);
        vignette.intensity.value = 0;
        score = 0;
        lives = startLives;
        gameObject.tag = "green";
        skinnedRenderer.material = wireframeMaterials[1];
        lastHit = Time.time;
    }

    private void OnTriggerEnter(Collider other)
    {        
        
        switch (other.tag)
        {
            // If the target is missed, take damage and update hitprotection
            case "Obstacle":
                if (Time.time - lastHit < hitProtection)
                    return;
                lastHit = Time.time;
                TakeDamage();
                Destroy(other.transform.parent.gameObject);
                break;
            //If the target is hit, compare colors and act accordingly
            case "Target":
                CompareColor(other.transform.parent.tag);
                Destroy(other.transform.parent.gameObject);
                break;
            default:
                // do nothing
                break;
        }

    }
    private void TakeDamage()
    {
        takeDamage();
        PlayerStats.lives--;
        audioSource.Play();
        StartCoroutine(VignetteBlink(0.5f, 2));
    }

    public void ChangeColor(string color)
    {
        // Add the correct data about color changes to player stats
        if(changeColor != null)
        {
            changeColor(gameObject.tag, color);
        }
        PlayerStats.totalColorChanges++;
        if(color == gameObject.tag)
        {
            PlayerStats.falseColorChanges++;
        }

        // Change color by switching out the material
        // Would work with wireframe materials but WebGL doesn't support geometry based shaders
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

    private void CompareColor(string tag)
    {
        
        if (gameObject.tag == tag)
        {
            increaseScore();
        }
        else
        {
            TakeDamage();
        }
    }

    #region animations

    //Simple fade animation for the vignette around the border of the screen for taking damages
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
    #endregion
}
