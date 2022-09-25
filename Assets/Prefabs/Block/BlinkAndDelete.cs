using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlinkAndDelete : MonoBehaviour
{

    private float blinkSeconds=1.75f;
    private float blinkInterval=0.1f;
    private float lifeTimeSeconds=35;

    private SpriteRenderer sr;

    // Start is called before the first frame update
    void Awake() {
        sr = GetComponent<SpriteRenderer>();
    }

    private void OnEnable() {
        StartCoroutine(Blink());
        GameObject.Destroy(this.gameObject, lifeTimeSeconds);
    }


    public IEnumerator Blink()
    {
        yield return new WaitForSeconds(lifeTimeSeconds-blinkSeconds);
        Color color = sr.color;
        Color colorBlink = color;
        colorBlink.a = 0f;
        for (int i = 0; i < blinkSeconds/(blinkInterval*2); i++)
        {
            yield return new WaitForSeconds(blinkInterval);
            sr.color = colorBlink;
            yield return new WaitForSeconds(blinkInterval);
            sr.color = color;
        }
        
    }
}
