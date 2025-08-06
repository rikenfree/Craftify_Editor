[System.Serializable]
public class MapData2
{
    public string mapid;

    public string name;

    public string imageUrl;

    public string mapFileUrl;

	public string mapFileSize;

	public string Id => mapid;

	public string Name => name;

	public string ImageUrl => imageUrl;

	public string MapFileUrl => mapFileUrl;

	public string MapFileSize=> mapFileSize;
	public MapData2(string id, string name, string imageUrl, string mapFileUrl,string mapFileSize)
	{
		this.mapid = id;
		this.name = name;
		this.imageUrl = imageUrl;
		this.mapFileUrl = mapFileUrl;
		this.mapFileSize = mapFileSize;
	}

	public new string ToString()
	{
		return "ID: " + mapid + ", Name: " + name;
	}
}
