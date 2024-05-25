
public enum GameMode
{
    EditorMode,
    PackgeBundle,
    UpdateMode
}

public class AppConst
{
    public const string BundleExtension = ".ab";


    public const string FileListName = "fileList.txt";

    public static GameMode GameMode = GameMode.EditorMode;

    //热更新资源地址
    public const string ResourceUrl = "http://127.0.0.1/AssetBundles";
}
