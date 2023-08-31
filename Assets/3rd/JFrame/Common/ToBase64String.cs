﻿using System;
using System.Collections.Generic;
using System.Text;
using JFrame.Common.Interface;

namespace JFrame.Common
{
    public class ToBase64String : IProcesser
    {
        Encoding _encoding;
        public ToBase64String(Encoding encoding = null)
        {
            _encoding = encoding ?? Encoding.UTF8;
        }

        public byte[] Process(byte[] bytes)
        {
            return _encoding.GetBytes(Convert.ToBase64String(bytes));
        }
    }
}
