using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using com.jesusnoseq.util;

public class GenerateBlock :MonoBehaviourSingleton<GenerateBlock>
{
    private int maxSteps=100;
    public GameObject blockPrefab;

    private int nSteps=0;

    private float xLastBlockAt=0;
    private float yLastBlockAt=0;

    private float minSizeX=5f;
    private float maxSizeX=10;
    private float minSizeY=0.5f;
    private float maxSizeY=5;


    private float minXReductionPerStep=0.031f;
    private float maxXReductionPerStep=0.05f;


    private float minYIncreasePerStep=0.03f;
    private float maxYIncreasePerStep=0.08f;

  

    private float nextActionTime = 0.0f;
    private float period=2;
    private float periodIncrease = 1f;


    // Start is called before the first frame update
    void Start()
    {
        GenNewBlock();
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.timeSinceLevelLoad > nextActionTime ) {
            Debug.Log("GenNewBlock because of time");
            GenNewBlock();
        }
    }

    public void GenNewBlock(){
        if(nSteps>maxSteps){
            return;
        }
        
        nextActionTime += period;
        period+=periodIncrease;

        nSteps++;
        GameObject b=Instantiate(blockPrefab, new Vector3(xLastBlockAt, yLastBlockAt, 0), Quaternion.identity) as GameObject;
        float x=Random.Range(minSizeX, maxSizeX);
        float y=Random.Range(minSizeY, maxSizeY);
        b.transform.localScale = new Vector3(x, y, 0);
        xLastBlockAt= b.transform.position.x+x;
        yLastBlockAt= b.transform.position.y+y;
        b.GetComponent<SpriteRenderer>().color=Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
        GameManager.Instance.AddStep(yLastBlockAt, nSteps);

        minSizeX-=minXReductionPerStep;
        maxSizeX-=maxXReductionPerStep;
        minSizeY+=minYIncreasePerStep;
        maxSizeY+=maxYIncreasePerStep;
    }
}
