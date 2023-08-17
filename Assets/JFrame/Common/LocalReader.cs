﻿using JFrame.Common.Interface;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

namespace JFrame.Common
{
    /// <summary>
    /// 本地文件加载器
    /// </summary>
    public class LocalReader : Reader
    {
        /// <summary>
        /// 无参构造函数
        /// </summary>
        public LocalReader() : this(null) { }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="processer"></param>
        public LocalReader(ProcesserManager processer) : base(processer) { }

        /// <summary>
        /// 同步读取路径文件
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public override byte[] Read(string filePath)
        {
            try
            {
                byte[] bytes = null;
                using (FileStream fs = new FileStream(filePath, FileMode.Open))
                {
                    bytes = new byte[fs.Length];
                    fs.Read(bytes, 0, bytes.Length);

                    bytes = GetProcessResult(bytes);

                    return bytes;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }


        /// <summary>
        /// 异步读取路径文件
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public override async Task<byte[]> ReadAsync(string filePath)
        {
            try
            {
                byte[] bytes = null;
                using (FileStream fs = new FileStream(filePath, FileMode.Open))
                {
                    bytes = new byte[fs.Length];
                    await fs.ReadAsync(bytes, 0, bytes.Length);

                    //数据加工
                    bytes = GetProcessResult(bytes);

                    return bytes;
                }
            }catch(Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }

        }
    }
}
