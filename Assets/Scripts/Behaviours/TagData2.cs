[System.Serializable]
public class TagData2
{
	public string name;

    public int count;

	public string Name => name;

	public int Count => count;

	public TagData2(string name, int count)
	{
		this.name = name;
		this.count = count;
	}

	public TagData2(string name)
	{
		this.name = name;
		count = count;
	}

	public new string ToString()
	{
		return "Name: " + name + ", Count: " + count;
	}
}
