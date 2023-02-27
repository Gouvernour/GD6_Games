using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tounge : MonoBehaviour
{
    public Vector3 Destination;
    public float speed = 1.0f;

    public float distance = 2;
    public GameObject nodePrefab;
    public GameObject Lastnode;
    public GameObject Player1;
    public GameObject Player2;
    GameObject ActivePlayer;
    List<GameObject> nodes = new List<GameObject>();

    bool done = false;
    void Start()
    {
        Player1 = GameObject.FindWithTag("Frog1");
        Player2 = GameObject.FindWithTag("Frog2");
        if(Player1.GetComponent<DuoFrogMovement>().Active)
        {
            ActivePlayer = Player1;
        }else
            ActivePlayer = Player2;

        Lastnode = transform.gameObject;
    }
    private void OnDestroy()
    {
        foreach (var node in nodes)
        {
            Destroy(node);
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, Destination, speed * Time.deltaTime);


        if (transform.position != Destination)
        {
            if (Vector2.Distance(ActivePlayer.transform.position, Lastnode.transform.position) > distance)
            {
                CreateNode();
            }
        }
        else if (!done)
        {
            done = true;
            Lastnode.GetComponent<HingeJoint2D>().connectedBody = ActivePlayer.GetComponent<Rigidbody2D>();
            AudioManager.instance.PlaySound(SoundGroup.Move);
        }
    }

    void CreateNode()
    {
        Vector2 CreatePos = ActivePlayer.transform.position - Lastnode.transform.position;
        CreatePos.Normalize();
        CreatePos *= distance;
        CreatePos += (Vector2)Lastnode.transform.position;

        GameObject newObject = (GameObject)Instantiate(nodePrefab, CreatePos, Quaternion.identity);
        nodes.Add(newObject);

        newObject.transform.SetParent(transform);

        Lastnode.GetComponent<HingeJoint2D>().connectedBody = newObject.GetComponent<Rigidbody2D>();

        Lastnode = newObject;
    }
}
