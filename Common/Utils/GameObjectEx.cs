using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
public static class GameObjectEx
{
    static public T CheckAddComponet<T>(this GameObject go) where T : Component
    {
        T component = go.GetComponent<T>();
        if (component == null)
        {
            component = go.AddComponent<T>();
        }
        return component;
    }
    

    static public IEnumerable<GameObject> GetSubObject(this GameObject go)
    {
        for (int k = 0; k < go.transform.childCount; ++k)
        {
            yield return go.transform.GetChild(k).gameObject;
        }
    }

    static public XZSpaceListener AddSpaceClickListener(this GameObject go, VoidDelegate voidDelegate)
    {
        var zspaceClick = go.CheckAddComponet<XZSpaceListener>();
        zspaceClick.onZSpaceClick = voidDelegate;
        return zspaceClick;
    }

    static public XZSpaceListener AddSpaceEnterListener(this GameObject go, VoidDelegate voidDelegate)
    {
        var zspaceClick = go.CheckAddComponet<XZSpaceListener>();
        zspaceClick.onZSpaceEnter = voidDelegate;
        return zspaceClick;
    }

    static public XZSpaceListener AddSpaceExitListener(this GameObject go, VoidDelegate voidDelegate)
    {
        var zspaceClick = go.CheckAddComponet<XZSpaceListener>();
        zspaceClick.onZSpaceExit = voidDelegate;
        return zspaceClick;
    }

    static public XZSpaceListener AddSpaceDragListener(this GameObject go, VoidVectorDelegate voidVectorDelegate)
    {
        var zspaceDrag = go.CheckAddComponet<XZSpaceListener>();
        zspaceDrag.onZSpaceDragUI = voidVectorDelegate;
        return zspaceDrag;
    }

    static public XZSpaceListener AddSpacePressListener(this GameObject go, VoidVectorDelegate voidVectorDelegate)
    {
        var zspaceDrag = go.CheckAddComponet<XZSpaceListener>();
        zspaceDrag.onZSpacePressUI = voidVectorDelegate;
        return zspaceDrag;
    }

    static public XZSpaceListener AddSpaceReleaseListener(this GameObject go, VoidBoolDelegate voidBoolDelegate)
    {
        var zspaceDrag = go.CheckAddComponet<XZSpaceListener>();
        zspaceDrag.onZSpaceRelease = voidBoolDelegate;
        return zspaceDrag;
    }

    static public Coroutine<T> StartCoroutine<T>(this MonoBehaviour mb, IEnumerator c) where T : class
    {
        return null;
    }

    static public void SetChild(this GameObject parent, GameObject chid)
    {
        chid.transform.SetParent(parent.transform);
    }


    static public void IsUseGravity(this GameObject go, bool isUseGravity)
    {
        var rigibody = go.CheckAddComponet<Rigidbody>();
        rigibody.useGravity = isUseGravity;
    }

    public static Bounds GetBounds(this List<Vector3> vertices)
    {
        var min_x = vertices.Min(p => p.x);
        var max_x = vertices.Max(p => p.x);
        var min_y = vertices.Min(p => p.y);
        var max_y = vertices.Max(p => p.y);
        var min_z = vertices.Min(p => p.z);
        var max_z = vertices.Max(p => p.z);

        var min = new Vector3(min_x, min_y, min_z);
        var max = new Vector3(max_x, max_y, max_z);

        var center = (max + min) * 0.5F;
        var size = max - min;

        return new Bounds(center, size);
    }
    public static float MaxOfVector3(this Vector3 _v3)
    {
        List<float> list_size = new List<float> { _v3.x, _v3.y, _v3.z };
        return list_size.Max();
    }
}
