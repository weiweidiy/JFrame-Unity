using System;
using YooAsset;
using Cysharp.Threading.Tasks;

public class YooAssetsLoader 
{
	public async void LoadSceneAsync(string sceneName)
	{
		var handle = YooAssets.LoadSceneAsync(sceneName);
		await handle.ToUniTask();
	}


}

