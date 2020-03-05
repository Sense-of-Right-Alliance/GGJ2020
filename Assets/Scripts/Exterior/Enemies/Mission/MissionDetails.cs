using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MissionDetails : MonoBehaviour
{
    protected string _name = "Untitled Mission";
    public string Name { get { return _name; } }

    protected string _description = "";
    public string Description { get { return _description; } }

    protected List<Wave> _waves;
    public List<Wave> Waves { get { return _waves; } }

    private void Start()
    {
        
    }

    private void Update()
    {
        
    }
}
