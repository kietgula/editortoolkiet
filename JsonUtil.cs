using System;
using System.Collections.Generic;

public class JsonUtil
{
	public static float ParseFloat(object val, float defVal = 0f)
	{
		return (val == null) ? defVal : float.Parse(val.ToString());
	}

	public static int ParseInt(object val, int defVal = 0)
	{
		return (val == null) ? defVal : int.Parse(val.ToString());
	}

	public static bool ParseBool(object val, bool defVal = false)
	{
		if (val == null)
		{
			return defVal;
		}
		try
		{
			return ParseInt(val, defVal ? 1 : 0) != 0;
		}
		catch (Exception)
		{
			try
			{
				return bool.Parse(val.ToString());
			}
			catch (Exception)
			{
				return defVal;
			}
		}
	}

	public static DateTime ParseDateTime(object val)
	{
		return DateTime.Parse(val.ToString());
	}

	public static string ReadString(IDictionary<string, object> dic, string key, string defVal = "")
	{
		if (dic != null && dic.ContainsKey(key))
		{
			return (dic[key] == null) ? defVal : dic[key].ToString();
		}
		return defVal;
	}

	public static int ReadInt(IDictionary<string, object> dic, string key, int defVal = 0)
	{
		if (dic != null && dic.ContainsKey(key))
		{
			return ParseInt(dic[key], defVal);
		}
		return defVal;
	}

	public static bool ReadBool(IDictionary<string, object> dic, string key, bool defVal = false)
	{
		if (dic != null && dic.ContainsKey(key))
		{
			return ParseBool(dic[key], defVal);
		}
		return defVal;
	}

	public static float ReadFloat(IDictionary<string, object> dic, string key, float defVal = 0f)
	{
		if (dic != null && dic.ContainsKey(key))
		{
			return ParseFloat(dic[key], defVal);
		}
		return defVal;
	}

	public static DateTime ReadDateTime(IDictionary<string, object> dic, string key)
	{
		return ReadDateTime(dic, key, new DateTime(1, 1, 1, 0, 0, 0));
	}

	public static DateTime ReadDateTime(IDictionary<string, object> dic, string key, DateTime defVal)
	{
		if (dic != null && dic.ContainsKey(key))
		{
			try
			{
				return ParseDateTime(dic[key]);
			}
			catch (Exception ex)
			{
				return defVal;
			}
		}
		return defVal;
	}

	public static string ListToJsonStr(List<string> list)
	{
		if (list == null || list.Count < 1)
		{
			return "[]";
		}
		bool flag = true;
		string text = "[";
		foreach (string item in list)
		{
			if (flag)
			{
				flag = false;
			}
			else
			{
				text += ",";
			}
			text += item;
		}
		return text + "]";
	}
}
