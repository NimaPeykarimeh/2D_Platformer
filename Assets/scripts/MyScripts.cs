using System.Collections.Generic;
using UnityEngine;

public static class MyScripts
{
    public static T SelectRandomFromThisList<T>(List<T> _list)
    {
        if (_list != null && _list.Count > 0) 
        {
            int _rand = Random.Range(0, _list.Count);
            return _list[_rand];
        }
        else{throw new System.ArgumentException("The List Is Null Or Empty");}
    }

    public static T SelectRandomFromThisArray<T>(T[] _array)
    {
        if (_array != null && _array.Length > 0)
        {
            int _rand = Random.Range(0,_array.Length);
            return _array[_rand];
        }
        else { throw new System.ArgumentException("The Array Is Nulll Or Empty"); }
    }

}

