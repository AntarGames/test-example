using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Control : MonoBehaviour
{
    [SerializeField] public float sensitivity;
    [SerializeField] private Tower tower;

    public bool jumpFlag;

    private void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                jumpFlag = true;
            }
            else if (touch.phase == TouchPhase.Stationary && jumpFlag)
            {
                tower.JumpIncrease();
            }
            else if (touch.phase == TouchPhase.Ended && jumpFlag)
            {
                tower.Jump();
                jumpFlag = false;
            }
            else if (Input.GetTouch(0).phase == TouchPhase.Moved)
            {
                jumpFlag = false;
                float deltaX = touch.deltaPosition.x*sensitivity*Time.deltaTime;
                tower.SetXCoord(deltaX);
            }
        }
    }
}
