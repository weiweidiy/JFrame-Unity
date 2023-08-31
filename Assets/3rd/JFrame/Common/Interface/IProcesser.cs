﻿using System;
using System.Collections.Generic;
using System.Text;

namespace JFrame.Common.Interface
{
    /// <summary>
    /// 数据加工器
    /// </summary>
    public interface IProcesser
    {
        byte[] Process(byte[] bytes);
    }
}
