﻿using UnityEngine;
using UnityEditor;

using System.Collections.Generic;
using System.Reflection;
using System.Linq;

using BaseGameLogic.Management;

namespace BaseGameLogic
{
	[CustomEditor(typeof(BaseGameManager), true)]
	public class BaseGameManagerEditor : Editor 
	{
		private List<FieldInfo> _managerAttribute = new List<FieldInfo>();
		
        [MenuItem("BaseGameLogic/GameManager")]
        public static BaseGameManager CreateInputCollectorManager()
        {
            return GameObjectExtension.CreateInstanceOfAbstractType<BaseGameManager>();
        }

        private BaseGameManager _gameManagerInstance = null;

		private void OnEnable()
		{
			_gameManagerInstance = target as BaseGameManager;
		}

		public override void OnInspectorGUI ()
		{
			base.OnInspectorGUI ();

			if (Application.isPlaying) 
			{
				if (GUILayout.Button ("Pause")) 
				{
					_gameManagerInstance.PauseGame ();
				}

				if (GUILayout.Button ("Play")) 
				{
					_gameManagerInstance.ResumeGame ();
				}
			}
		}
	}
}