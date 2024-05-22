using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    // Start is called before the first frame update
    IEnumerator Start()
    {
        AssetBundleCreateRequest request = AssetBundle.LoadFromFileAsync(Application.streamingAssetsPath+ "/assets/ui/prefab/loadingui.prefab.ab");
        yield return request;

        AssetBundleCreateRequest request1 = AssetBundle.LoadFromFileAsync(Application.streamingAssetsPath + "/assets/ui/res/background.png.ab");
        yield return request1;

        AssetBundleCreateRequest request2 = AssetBundle.LoadFromFileAsync(Application.streamingAssetsPath + "/assets/ui/res/img_jindutiaodikuang.png.ab");
        yield return request2;  
        
        AssetBundleCreateRequest request3 = AssetBundle.LoadFromFileAsync(Application.streamingAssetsPath + "/assets/ui/res/img_jindutiaoxuanzhong.png.ab");
        yield return request3;

        AssetBundleRequest bundleRequest = request.assetBundle.LoadAssetAsync("Assets/BuildResources/UI/Prefab/loadingui.prefab");
        yield return bundleRequest;

        GameObject go = Instantiate(bundleRequest.asset) as GameObject;
        go.transform.SetParent(transform);
        go.SetActive(true);
        go.transform.localPosition = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
