using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SpaceStationPlayer : MonoBehaviour
{
    [SerializeField] float speed = 5f;

    [SerializeField] Vector2 velocity = Vector2.zero;

    [SerializeField] PlayerID playerID = PlayerID.Player1;

    Rigidbody2D rigidbody2D;

    private GameObject heldResource;
    private List<GameObject> overResources = new List<GameObject>();
    private AudioSource audioSource;

    private void Awake()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {

    }

    private void Update()
    {
        UpdatePickup();
    }

    private void FixedUpdate()
    {
        if (!ready) UpdateMovement();
    }

    private void UpdateMovement()
    {
        velocity = new Vector2(0, 0);

        if ((playerID == PlayerID.Player2 && Input.GetKey(KeyCode.LeftArrow))
            || (playerID == PlayerID.Player1 && Input.GetKey(KeyCode.A)))
        {
            velocity.x -= speed;
        }
        else if ((playerID == PlayerID.Player2 && Input.GetKey(KeyCode.RightArrow))
            || (playerID == PlayerID.Player1 && Input.GetKey(KeyCode.D)))
        {
            velocity.x += speed;
        }
        else
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
        }
        else
        {
            velocity.y = (playerID == PlayerID.Player1 ? Input.GetAxis("Vertical1") : Input.GetAxis("Vertical2")) * speed;
        }

        rigidbody2D.AddForce(velocity);
    }

    private bool ready = false;

    private void UpdatePickup()
    {
        if ((playerID == PlayerID.Player1 && (Input.GetKeyDown(KeyCode.G) || Input.GetButtonDown("A1")))
            || (playerID == PlayerID.Player2 && (Input.GetKeyDown(KeyCode.Space) || Input.GetButtonDown("A2"))))
        {
            if (heldResource == null && overResources.Count > 0)
            {
                PickupResource();
            }
            else
            {
                DropResource();
            }

            if (!ready && overPad)
            {
                ready = true;
                overPad.Ready(playerID);
            }
            else if (ready && overPad)
            {
                ready = false;
                overPad.Unready();
            }
        }

        if (heldResource != null)
        {
            Vector2 newPos = transform.position;

            if (heldResource.transform.parent != null) heldResource.transform.parent.position = newPos;
            else heldResource.transform.position = newPos;
        }
    }

    private void PickupResource()
    {
        if (heldResource == null)
        {
            ///Debug.Log("Picked up resource!");
            heldResource = overResources[0];
            overResources.RemoveAt(0);
        }
    }

    public void DropResource()
    {
        if (heldResource != null)
        {
            //Debug.Log("Dropped resource!");
            overResources.Add(heldResource);
            heldResource = null;
        }
    }

    private LaunchPad overPad = null;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Interior Resource")
        {
            overResources.Add(collision.gameObject);
        }
        else if (collision.tag == "Launch Pad")
        {
            overPad = collision.gameObject.GetComponent<LaunchPad>();
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Interior Resource")
        {
            overResources.Remove(collision.gameObject);
        }
        else if (collision.tag == "Launch Pad")
        {
            overPad = null;
        }
    }

    public void RandomPush()
    {
        Vector2 v = new Vector2(UnityEngine.Random.Range(-1f, 1f) * 1300f, UnityEngine.Random.Range(-1f, 1f) * 1300f);

        rigidbody2D.AddForce(v);
    }
}
