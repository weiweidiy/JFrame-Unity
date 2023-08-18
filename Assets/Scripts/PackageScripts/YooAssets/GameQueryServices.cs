using YooAsset;

/// <summary>
/// 资源文件查询服务类
/// </summary>
public class GameQueryServices : IQueryServices
{
    public DeliveryFileInfo GetDeliveryFileInfo(string packageName, string fileName)
    {
        throw new System.NotImplementedException();
    }

    public bool QueryDeliveryFiles(string packageName, string fileName)
    {
        return false;
    }

    public bool QueryStreamingAssets(string packageName, string fileName)
    {
        // 注意：fileName包含文件格式
        return StreamingAssetsHelper.FileExists(packageName, fileName);
    }
}
