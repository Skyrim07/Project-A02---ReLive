using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using SKCell;

public sealed class PlayerModel : MonoBehaviour
{
    public float speed = 1;
    public bool accel = false;

    public bool wasd = true;

    private Rigidbody2D rb;
    private float axis_h, axis_v;
    private Vector2 mvector;
    private Vector3 oScale;
    private int lastOrient;

    [SerializeField] Animator anim;
    private void Start()
    {
        oScale=transform.localScale;
        rb = GetComponent<Rigidbody2D>();
    }
    private void Update()
    {
        GetAxes();
    }

    private void FixedUpdate()
    {
        Movement();
    }

    private void Movement()
    {
        if (UIManager.instance.inUI)
            return;

        mvector = new Vector2(axis_h, axis_v);
        if(accel)
            rb.AddForce(mvector * speed);
        else
           transform.Translate(mvector * speed * Time.fixedDeltaTime*.1f);

        if (axis_h > 0)
        {
            transform.localScale = new Vector3(-oScale.x,oScale.y,oScale.z);
            lastOrient = 0;
            anim.Play("leftWalk");
        }
        else if (axis_h < 0)
        {
            transform.localScale = new Vector3(oScale.x, oScale.y, oScale.z);
            lastOrient = 1;
            anim.Play("leftWalk");
        }
        else if (axis_v > 0)
        {
            transform.localScale = new Vector3(oScale.x, oScale.y, oScale.z);
            lastOrient = 2;
            anim.Play("upWalk");
        }
        else if (axis_v < 0)
        {
            transform.localScale = new Vector3(oScale.x, oScale.y, oScale.z);
            lastOrient = 3;
            anim.Play("downWalk");
        }


        if (mvector.sqrMagnitude == 0)
        {
            switch (lastOrient)
            {
                case 0:
                    anim.Play("leftIdle");
                    break;
                case 1:
                    anim.Play("leftIdle");
                    break;
                case 2:
                    anim.Play("upIdle");
                    break;
                case 3:
                    anim.Play("downIdle");
                    break;
                default:
                    break;
            }
        }

    }

    private void GetAxes()
    {
        if (wasd)
        {
            axis_h = (Input.GetKey(KeyCode.A) ? -1 : 0) + (Input.GetKey(KeyCode.D) ? 1 : 0);
            axis_v = (Input.GetKey(KeyCode.W) ? 1 : 0) + (Input.GetKey(KeyCode.S) ? -1 : 0);
        }
        else
        {
            axis_h = (Input.GetKey(KeyCode.LeftArrow) ? -1 : 0) + (Input.GetKey(KeyCode.RightArrow) ? 1 : 0);
            axis_v = (Input.GetKey(KeyCode.UpArrow) ? 1 : 0) + (Input.GetKey(KeyCode.DownArrow) ? -1 : 0);
        }
    }

    public void OnReachGoal()
    {
        gameObject.SetActive(false);
    }
}
