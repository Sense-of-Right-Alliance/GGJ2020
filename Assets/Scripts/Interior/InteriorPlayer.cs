using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;



public class InteriorPlayer : MonoBehaviour
{
    [SerializeField] float speed = 5f;

    [SerializeField] Vector2 velocity = Vector2.zero;

    [SerializeField] PlayerID playerID = PlayerID.Player1;

    Rigidbody2D rigidbody2D;

    private GameObject heldItem;
    private List<GameObject> overItems = new List<GameObject>();
    private AudioSource audioSource;

    private Vector2 moveDir = Vector2.up;
    public Vector2 LookDirection { get { return moveDir; } }

    private void Awake()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        int savedPlayerID = PlayerPrefs.GetInt("Engineer",-1);
        if (savedPlayerID == 0) playerID = PlayerID.Player1;
        else if (savedPlayerID == 1) playerID = PlayerID.Player2;
    }

    private void Update()
    {
        UpdatePickup();
        UpdateUse();
    }

    private void FixedUpdate()
    {
        UpdateMovement();
    }

    private void UpdateMovement()
    {
        velocity = new Vector2(0,0);

        if ((playerID == PlayerID.Player2 && Input.GetKey(KeyCode.LeftArrow))
            || (playerID == PlayerID.Player1 && Input.GetKey(KeyCode.A)))
        {
            velocity.x -= speed;
        }
        else if ((playerID == PlayerID.Player2 && Input.GetKey(KeyCode.RightArrow))
            || (playerID == PlayerID.Player1 && Input.GetKey(KeyCode.D)))
        {
            velocity.x += speed;
        } else
        {
            velocity.x = (playerID == PlayerID.Player1 ? Input.GetAxis("Horizontal1") : Input.GetAxis("Horizontal2")) * speed;
        }


        if ((playerID == PlayerID.Player2 && Input.GetKey(KeyCode.UpArrow))
            || (playerID == PlayerID.Player1 && Input.GetKey(KeyCode.W)))
        {
            velocity.y += speed;
        }
        else if ((playerID == PlayerID.Player2 && Input.GetKey(KeyCode.DownArrow))
            || (playerID == PlayerID.Player1 && Input.GetKey(KeyCode.S)))
        {
            velocity.y -= speed;
        } else
        {
            velocity.y = (playerID == PlayerID.Player1 ? Input.GetAxis("Vertical1") : Input.GetAxis("Vertical2")) * speed;
        }

        rigidbody2D.AddForce(velocity);
        
        if (/*rigidbody2D.velocity.magnitude*/velocity.magnitude > 0.1) moveDir = rigidbody2D.velocity.normalized;
    }

    private void UpdatePickup()
    {
        if ((playerID == PlayerID.Player1 && (Input.GetKeyDown(KeyCode.G) || Input.GetButtonDown("A1")))
            || (playerID == PlayerID.Player2 && (Input.GetKeyDown(KeyCode.Space) || Input.GetButtonDown("A2"))))
        {
            if (heldItem == null && overItems.Count > 0)
            {
                audioSource.Play();
                PickupItem();
            } else
            {
                audioSource.Play();
                DropItem();
            }
        }
    }

    private bool pushing = false;
    private void UpdateUse()
    {
        bool buttonDown = ((playerID == PlayerID.Player1 && (Input.GetKey(KeyCode.R) || Input.GetButton("B1")))
             || (playerID == PlayerID.Player2 && (Input.GetKey(KeyCode.B) || Input.GetButton("B2"))));

        if (heldItem != null)
        {
            if (heldItem.tag == "Tool") heldItem.GetComponent<Tool>().SetOn(buttonDown);
        } else
        {
            pushing = buttonDown;
        }
    }

    private void PickupItem()
    {
        if (heldItem == null)
        {
            //Debug.Log("Picked up resource!");

            heldItem = overItems[0];
            overItems.RemoveAt(0);
            while ((heldItem == null || heldItem.activeSelf) && overItems.Count > 0)
            {
                heldItem = overItems[0];
                overItems.RemoveAt(0);
            }
            
            if (heldItem != null && heldItem.activeSelf) heldItem.GetComponent<PickupItem>().Pickup(this);
        }
    }

    public void DropItem()
    {
        if (heldItem != null)
        {
            //Debug.Log("Dropped resource!");
            if (overStation != null)
            {
                overStation.TryProcessItem(heldItem.GetComponent<PickupItem>());
            }

            if (heldItem != null) // wasn't consumed. So just drop it
            {
                overItems.Add(heldItem);
                heldItem.GetComponent<PickupItem>().Drop();
                heldItem = null;
            }
        }
    }

    private Station overStation;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((collision.tag == "Interior Resource" || collision.tag == "Tool") && !overItems.Contains(collision.gameObject) && heldItem != collision.gameObject)
        {
            overItems.Add(collision.gameObject);
            //Debug.Log("Interior Player over item " + collision.tag.ToString() + " over items count = " + overItems.Count.ToString());
        }
        else
        {
            Station station = collision.GetComponent<Station>();
            if (station != null) overStation = station;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Interior Resource" || collision.tag == "Tool")
        {
            overItems.Remove(collision.gameObject);
            //Debug.Log("Interior Player no longer over item " + collision.tag.ToString() + " over items count = " + overItems.Count.ToString());
        } else
        {
            Station station = collision.GetComponent<Station>();
            if (station != null && station == overStation) overStation = null;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Debris" && pushing)
        {
            collision.gameObject.GetComponent<Rigidbody2D>().AddForce((collision.transform.position - transform.position).normalized * 5000f);
        }
    }

    public void RandomPush()
    {
        Vector2 v = new Vector2(UnityEngine.Random.Range(-1f,1f) * 1300f, UnityEngine.Random.Range(-1f, 1f) * 1300f);

        rigidbody2D.AddForce(v);
    }

    public void PushInDir(Vector2 dir, float force)
    {
        rigidbody2D.AddForce(dir * force);
    }
}