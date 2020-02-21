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
        
        if (rigidbody2D.velocity.magnitude > 0.1) moveDir = rigidbody2D.velocity.normalized;
    }

    private void UpdatePickup()
    {
        if ((playerID == PlayerID.Player1 && (Input.GetKeyDown(KeyCode.G) || Input.GetButtonDown("A1")))
            || (playerID == PlayerID.Player2 && (Input.GetKeyDown(KeyCode.Space) || Input.GetButtonDown("A2"))))
        {
            if (heldItem == null && overItems.Count > 0)
            {
                PickupItem();
            } else
            {
                DropItem();
            }
        }

        if (heldItem != null)
        {
            Vector2 newPos = transform.position;
            newPos += moveDir * heldItem.transform.parent.GetComponent<SpriteRenderer>().bounds.size.x;

            //Debug.Log("interiorplayer move dir = " + moveDir.ToString() + " sprite width = " + heldResource.transform.parent.GetComponent<SpriteRenderer>().bounds.size.x / 2f);

            if (heldItem.transform.parent != null) heldItem.transform.parent.position = newPos;
            else heldItem.transform.position = newPos;

            heldItem.transform.LookAt(transform.position + new Vector3(moveDir.x, moveDir.y, transform.position.z));

            if (heldItem.tag == "Tool") heldItem.GetComponent<Tool>().SetOn(((playerID == PlayerID.Player1 && (Input.GetKey(KeyCode.R) || Input.GetButton("B1")))
             || (playerID == PlayerID.Player2 && (Input.GetKey(KeyCode.B) || Input.GetButton("B2")))));

            /*
            if ((playerID == PlayerID.Player1 && (Input.GetKeyDown(KeyCode.R) || Input.GetButton("B1")))
             || (playerID == PlayerID.Player2 && (Input.GetKeyDown(KeyCode.B) || Input.GetButton("B2"))))
            {
                if (heldItem.tag == "Tool") heldItem.GetComponent<Tool>().SetOn(true);// ToggleOn();
            }
            */
        }
    }

    private void PickupItem()
    {
        if (heldItem == null)
        {
            ///Debug.Log("Picked up resource!");
            heldItem = overItems[0];
            overItems.RemoveAt(0);
            heldItem.GetComponent<PickupItem>().Pickup(this);
        }
    }

    public void DropItem()
    {
        if (heldItem != null)
        {
            //Debug.Log("Dropped resource!");
            overItems.Add(heldItem);
            heldItem.GetComponent<PickupItem>().Drop();
            heldItem = null;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((collision.tag == "Interior Resource" || collision.tag == "Tool") && !overItems.Contains(collision.gameObject))
        {
            overItems.Add(collision.gameObject);
            Debug.Log("Interior Player over item " + collision.tag.ToString() + " over items count = " + overItems.Count.ToString());
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Interior Resource" || collision.tag == "Tool")
        {
            overItems.Remove(collision.gameObject);
            Debug.Log("Interior Player no longer over item " + collision.tag.ToString() + " over items count = " + overItems.Count.ToString());
        }
    }

    public void RandomPush()
    {
        Vector2 v = new Vector2(UnityEngine.Random.Range(-1f,1f) * 1300f, UnityEngine.Random.Range(-1f, 1f) * 1300f);

        rigidbody2D.AddForce(v);
    }
}