using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JFrame
{
    public interface IConfig<T>
    {
        T GetData(string id);

        T GetData(Predicate<T> predicate);

        List<T> GetDataList(Predicate<T> predicate);
    }

}
