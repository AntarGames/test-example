using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Tower : MonoBehaviour
{
    [SerializeField] private Material blueMaterial;
    [SerializeField] private Material grayMaterial;

    [SerializeField] private Transform firstBlock;

    [SerializeField] private float flightSpeed;
    [SerializeField] private float minJumpForce,maxJumpForce;
    [SerializeField] private float jumpForceMult;

    [SerializeField] private float velocity;

    [SerializeField] private Transform tower;
    [SerializeField] private AnimationCurve jumpAnimationCurve;

    private Stack<Transform> towerStack = new Stack<Transform>();
    private float xCoord, yCoord, zCoord;
    private Collider[] nearbyCols;
    private bool isJumping;
    private float jumpForce;
    public bool isRaised;
    void Start()
    {
        towerStack.Push(firstBlock);
    }
    private void FixedUpdate()
    {
        zCoord += velocity * Time.fixedDeltaTime;
        xCoord = Mathf.Clamp(xCoord, -2, 2);
        MovePosition(xCoord,yCoord,zCoord);
    }
    public void SetXCoord(float x)
    {
        xCoord += x;
    }
    void Update()
    {
        RaycastHit hit;
        Ray ray = new Ray(towerStack.Peek().position, Vector3.forward);
        if (Physics.Raycast(ray,out hit, 0.5f))
        {
            if (hit.collider.tag == "Enemy")
            {
                isRaised = true;
                RemoveBlock();
            }
        }
        nearbyCols = Physics.OverlapBox(transform.position, Vector3.one *0.5f);
        if (isRaised)
        {    
            float enemyCounter = 0;
            for (int i = 0; i < nearbyCols.Length; i++)
            {
                if (nearbyCols[i].tag == "Enemy")
                {
                    enemyCounter++;
                }
            }
            if (enemyCounter == 0)
            {
                RaiseDown();
                isRaised = false;
            }
        }

    }
    public void JumpIncrease()
    {
        jumpForce += jumpForceMult * Time.deltaTime;
        jumpForce = Mathf.Clamp(jumpForce, minJumpForce, maxJumpForce);
    }
    public void Jump()
    {
        if (!isJumping)
        {
            StartCoroutine(JumpAnimation());
        }
    }
    public void AddBlock(Transform block)
    {
        block.parent = tower;
        block.position = towerStack.Peek().position + Vector3.down;
        towerStack.Push(block);
        block.GetComponent<MeshRenderer>().material = blueMaterial;
        RaiseUp();
    }
    public void RemoveBlock()
    {
        Transform removedBlock = towerStack.Pop();
        removedBlock.parent = null;
        removedBlock.GetComponent<MeshRenderer>().material = grayMaterial;
        if (towerStack.Count <= 0)
        {
            GameManager.RestartGame();
        }
    }
    public void RaiseUp()
    {
        tower.position += Vector3.up;
    }
    public void RaiseDown()
    {
        tower.position += Vector3.down;
    }
    public void MovePosition(float x,float y,float z)
    {
        Vector3 move = new Vector3(x, y, z);
        transform.position = move;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Neutral")
        {
            AddBlock(other.transform);
            Destroy(other);
        }
        else if (other.tag == "Finish")
        {
            GameManager.RestartGame();
        }
    }
    public IEnumerator JumpAnimation()
    {
        isJumping = true;
        float t = 0;
        while (t <= jumpAnimationCurve.keys[2].time)
        {
            t += flightSpeed /jumpForce;
            yCoord = jumpAnimationCurve.Evaluate(t)*jumpForce;
            yield return null;
        }
        jumpForce = minJumpForce;
        yCoord = 0;
        isJumping = false;

    }

}

