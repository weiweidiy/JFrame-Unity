﻿using System;
using System.Collections.Generic;
using System.Text;

namespace JFrame.Common.Interface
{
    public interface IUnCompress
    {
        byte[] UnCompress(byte[] bytes, Encoding coding = null);
    }
}
