using UnityEngine;

public class Util2
{
	public static string RandomAlphaNumeric(int length)
	{
		string text = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
		string text2 = "";
		for (int i = 0; i < length; i++)
		{
			text2 += text[Random.Range(0, text.Length)].ToString();
		}
		return text2;
	}
}
