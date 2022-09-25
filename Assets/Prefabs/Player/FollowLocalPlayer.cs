using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using com.jesusnoseq.util;


[RequireComponent(typeof(Camera))]
public class FollowLocalPlayer : MonoBehaviourSingleton<FollowLocalPlayer> {

    Transform playerTransform;
    private float yOffset;
    private float xOffset;

    private void Start()
    {
    }

    void Update()
    {
        if (playerTransform != null)
        {
            transform.position = new Vector3(
                playerTransform.position.x - xOffset,
                playerTransform.position.y - yOffset,
                transform.position.z);
        }
    }

    public void SetTarget(Transform target)
    {
        playerTransform = target;
        xOffset = playerTransform.position.x - transform.position.x;
        yOffset = playerTransform.position.y - transform.position.y;
    }

    public void Shake(float time=0.5f, float force=1f)
    {
        transform.DOShakePosition(time, force);
    }
}

