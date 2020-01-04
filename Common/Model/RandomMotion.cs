using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomMotion : MonoBehaviour {
    [SerializeField]
    float rangeX=0.4f;
    [SerializeField]
    float rangeY = 0.5f;
    [SerializeField]
    float rangeZ= 0.5f;
    Vector3 originalPos;
    Tween tween;
    // Use this for initialization
    void Start () {
        originalPos = transform.position;
        rangeX = transform.parent.localScale.x/2;
        rangeY = transform.parent.localScale.y/ 2;
        rangeZ = transform.parent.localScale.z / 2;
    }

    private void OnEnable()
    {
        originalPos = transform.parent.position;
    }
    // Update is called once per frame
    float time = 0f;
    public float timeLimit= 10f;
    void UpdateCube () {

        time += Time.deltaTime;
        if (time >= timeLimit)
        {
            time = 0;
            Vector3 moveDir = new Vector3(UnityEngine.Random.Range(-rangeX, rangeX), UnityEngine.Random.Range(-rangeY, rangeY), UnityEngine.Random.Range(-rangeZ, rangeZ));
            Vector3 newPos = transform.position + moveDir;
            if (newPos.x < originalPos.x - rangeX || newPos.x > originalPos.x + rangeX)
            {
                moveDir.x = -moveDir.x;
            }
            if (newPos.y < originalPos.y - rangeY || newPos.y > originalPos.y + rangeY)
            {
                moveDir.y = -moveDir.y;
            }
            if (newPos.z < originalPos.z - rangeZ || newPos.z > originalPos.z + rangeZ)
            {
                moveDir.z = -moveDir.z;
            }
            Vector3 endpos = transform.position + moveDir;
            if (tween != null)
            {
                tween.Complete();
            }
            tween = transform.DOMove(endpos, timeLimit);
            tween.SetEase(Ease.Linear);
        }      
    }

    private void UpdateSphere()
    {      
        time += Time.deltaTime;
        if (time >=timeLimit)
        {
            time = 0;
            Vector3 moveDir = Quaternion.Euler(new Vector3(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360))) * Vector3.forward*Random.Range(0,0.5f);
            Vector3 endpos = originalPos + moveDir;
            //Debug.DrawLine(originalPos, endpos, Color.red, 10);
            tween = transform.DOMove(endpos, timeLimit);
        }
    }

    private void Update()
    {
        UpdateCube();
    }

    private void OnDrawGizmos1()
    {
        Gizmos.DrawSphere(originalPos, 0.5f);
        Gizmos.color =new Color(255,255,100,10);
    }
}
