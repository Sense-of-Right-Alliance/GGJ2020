using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ToolStation : Station
{
    [SerializeField] GameObject toolPrefab;

    [SerializeField] GameObject securedObject;

    protected override void InitStation()
    {
        if (securedObject != null)
        {
            TrySecureObject(securedObject, true);
        }
    }

    public bool TrySecureObject(GameObject obj, bool force=false)
    {
        PickupItem item = obj.GetComponent<PickupItem>();
        if (item == null) item = obj.GetComponentInChildren<PickupItem>();
        if (item == null && obj.transform.parent != null) item = obj.transform.parent.gameObject.GetComponentInChildren<PickupItem>();

        if (obj.tag != "Interior Resource" && item != null && (force || securedObject == null))
        {
            SecureObject(obj);
            return true;
        }

        return false;
    }

    public void SecureObject(GameObject securedObject)
    {
        this.securedObject = securedObject;
        PickupItem p = securedObject.GetComponent<PickupItem>();
        if (p == null && securedObject.transform.parent != null) p = securedObject.transform.parent.gameObject.GetComponent<PickupItem>();
        if (p == null) p = securedObject.GetComponentInChildren<PickupItem>();
        
        p.SetSecured(this);

        Transform t = securedObject.transform;
        if (securedObject.transform.parent != null) t = securedObject.transform.parent;

        t.position = transform.position;
        t.rotation = transform.rotation;
    }

    public void ReleaseObject()
    {
        this.securedObject = null;
    }

    protected override void ProcessResource(InteriorResource r)
    {
        base.ProcessResource(r);

        GameObject tool = Instantiate(toolPrefab, transform.position, transform.rotation);
        SecureObject(tool);
    }

    protected override bool TryCollectResource(InteriorResource r)
    {
        return securedObject == null;
    }
}
