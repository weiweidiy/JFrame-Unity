using System;
using System.IO;
using UnityEngine;
using YooAsset;

/// <summary>
/// YooAsset 打包时对打包AB进行加密
///
///
///  暂时没有实现.... 后面需要测试
///
/// 
/// </summary>

namespace JFrame.Game
{
    public class EncryptionNone : IEncryptionServices
    {
        public EncryptResult Encrypt(EncryptFileInfo fileInfo)
        {
            EncryptResult result = new EncryptResult();
            result.LoadMethod = EBundleLoadMethod.Normal;
            return result;
        }
    }

    public class FileOffsetEncryption : IEncryptionServices
    {
        /// <summary>
        /// 所有ab都要加密
        /// </summary>
        /// <param name="fileInfo"></param>
        /// <returns></returns>
        public EncryptResult Encrypt(EncryptFileInfo fileInfo)
        {
            //Debug.LogError("Encrypt " + fileInfo.BundleName);
            int offset = 32;
            byte[] fileData = File.ReadAllBytes(fileInfo.FilePath);
            var encryptedData = new byte[fileData.Length + offset];
            Buffer.BlockCopy(fileData, 0, encryptedData, offset, fileData.Length);

            EncryptResult result = new EncryptResult
            {
                LoadMethod = EBundleLoadMethod.LoadFromFileOffset,
                EncryptedData = encryptedData
            };
            return result;

            //if (fileInfo.BundleName.Contains("____NONE____"))
            //{
            //    Debug.LogError("Encrypt " + fileInfo.BundleName);
            //    int offset = 32;
            //    byte[] fileData = File.ReadAllBytes(fileInfo.FilePath);
            //    var encryptedData = new byte[fileData.Length + offset];
            //    Buffer.BlockCopy(fileData, 0, encryptedData, offset, fileData.Length);

            //    EncryptResult result = new EncryptResult();
            //    result.LoadMethod = EBundleLoadMethod.LoadFromFileOffset;
            //    result.EncryptedData = encryptedData;
            //    return result;
            //}
            //else
            //{
            //    EncryptResult result = new EncryptResult();
            //    result.LoadMethod = EBundleLoadMethod.Normal;
            //    return result;
            //}
        }
    }

    public class FileStreamEncryption : IEncryptionServices
    {
        public EncryptResult Encrypt(EncryptFileInfo fileInfo)
        {
            var fileData = File.ReadAllBytes(fileInfo.FilePath);
            for (int i = 0; i < fileData.Length; i++)
            {
                fileData[i] ^= BundleStream.KEY;
            }

            EncryptResult result = new EncryptResult();
            result.LoadMethod = EBundleLoadMethod.LoadFromStream;
            result.EncryptedData = fileData;
            return result;

            //if (fileInfo.BundleName.Contains("____NONE____"))
            //{
            //    var fileData = File.ReadAllBytes(fileInfo.FilePath);
            //    for (int i = 0; i < fileData.Length; i++)
            //    {
            //        fileData[i] ^= BundleStream.KEY;
            //    }

            //    EncryptResult result = new EncryptResult();
            //    result.LoadMethod = EBundleLoadMethod.LoadFromStream;
            //    result.EncryptedData = fileData;
            //    return result;
            //}
            //else
            //{
            //    EncryptResult result = new EncryptResult();
            //    result.LoadMethod = EBundleLoadMethod.Normal;
            //    return result;
            //}
        }
    }
}