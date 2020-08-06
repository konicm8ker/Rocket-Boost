using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlayerStats
{

	private static int lives = 3;

	public static int Lives
	{
		get
		{
			return lives;
		}
		set
		{
			lives = value;
		}
	}
}
