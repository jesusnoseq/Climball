using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using com.jesusnoseq.util;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviourSingleton<GameManager>
{

    private float[] stepsAltitude;

    public TMP_Text jumpsCounter;
    public TMP_Text stepCounter;

    private int exptectedSteps=100;
    private int currentStep;
    private int maxStep;
    private int lastStepAdded;

    public FollowLocalPlayer fpCamera;
    public BallPlayer player;

    public GameObject celebration;
    public GameObject endPanel;

    // Start is called before the first frame update
    void Start()
    {
        fpCamera.SetTarget(player.transform);
        stepsAltitude = new float[exptectedSteps];
    }


    // Update is called once per frame
    void Update()
    {
        int newStep=CaluclateCurrentStep();
        if(newStep!=currentStep && !BallPlayer.Instance.IsMoving()){
            if(newStep>currentStep){
                maxStep=newStep;
                if(lastStepAdded==(newStep+1)){
                    Debug.Log("new step by jump");
                    GenerateBlock.Instance.GenNewBlock();
                }
            }
            if(newStep==exptectedSteps){
                celebration.SetActive(true);
                endPanel.SetActive(true);
                SetScore();
            }
            currentStep=newStep;
            stepCounter.text=""+currentStep;
        }

        if(player.transform.position.y<-1){
            endPanel.SetActive(true);
            SetScore();
            if(player.transform.position.y<-500){
                EndGame();
            }
        }           
    }

    IEnumerator EndGameFinish()
    {
        yield return new WaitForSeconds(5f);
        EndGame();
    }

    private void SetScore(){
        if (PlayerPrefs.GetInt("SCORE", 0)<maxStep){
            PlayerPrefs.SetInt("SCORE", maxStep);
            PlayerPrefs.Save();
        }
    }

    private void EndGame(){
        SceneManager.LoadScene("Menu");
    }


    int CaluclateCurrentStep() {
        int currentStep=0;
        for(int i = 0; i < exptectedSteps; i++)
        {
            if(stepsAltitude[i]>player.transform.position.y){
                currentStep=i;
                break;
            }
        }
        return currentStep;
    }

    public void AddStep(float altitude, int step){
        lastStepAdded=step;
        stepsAltitude[step-1]=altitude;
    }
}
