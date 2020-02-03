using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class InteriorPlayer : MonoBehaviour
{
    [SerializeField] float speed = 5f;

    [SerializeField] Vector2 velocity = Vector2.zero;

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
        UpdateMovement();

        UpdatePickup();
    }

    void UpdateMovement()
    {
        velocity = new Vector2(0,0); //*= 0.5f;

        if (Input.GetKey(KeyCode.A))
        {
            velocity.x -= speed;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            velocity.x += speed;
        } else
        {
            velocity.x = Input.GetAxis("Horizontal1") * speed;// * Time.deltaTime;
        }


        if (Input.GetKey(KeyCode.W))
        {
            velocity.y += speed;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            velocity.y -= speed;
        } else
        {
            velocity.y = Input.GetAxis("Vertical1") * speed;// * Time.deltaTime;
        }

        rigidbody2D.AddForce(velocity);
    }

    void UpdatePickup()
    {
        if (Input.GetKeyDown(KeyCode.G) || Input.GetButtonDown("A1"))
        {
            if (heldResource == null && overResources.Count > 0)
            {
                //Debug.Log("Interior Player Picking Up Resource!");
                PickupResource();
            } else
            {
                //Debug.Log("Interior Player Dropping Resource!");
                DropResource();
            }
        }

        if (heldResource != null)
        {
            Vector2 newPos = transform.position;
            heldResource.transform.position = newPos;
        }
    }

    void PickupResource()
    {
        if (heldResource == null)
        {
            heldResource = overResources[0];
            overResources.RemoveAt(0);
        }
    }

    public void DropResource()
    {
        if (heldResource != null)
        {
            overResources.Add(heldResource);
            heldResource = null;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Interior Resource")
        {
            overResources.Add(collision.gameObject);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Interior Resource")
        {
            overResources.Remove(collision.gameObject);
        }
    }

    public void RandomPush()
    {
        Vector2 v = new Vector2(Random.Range(-1f,1f) * 1500f, Random.Range(-1f, 1f) * 1500f);

        //Debug.Log("pushing player " + v.ToString());

        rigidbody2D.AddForce(v);
    }
}
