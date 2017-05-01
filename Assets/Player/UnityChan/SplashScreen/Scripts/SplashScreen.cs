using UnityEngine;
using System.Collections;

namespace UnityChan
{
	[ExecuteInEditMode]
	public class SplashScreen : MonoBehaviour
	{
		void NextLevel ()
		{
#pragma warning disable CS0618 // 型またはメンバーが古い形式です
            Application.LoadLevel (Application.loadedLevel + 1);
#pragma warning restore CS0618 // 型またはメンバーが古い形式です
        }
	}
}