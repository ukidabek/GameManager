using UnityEngine;
using UnityEditor;

using System.Collections.Generic;
using System.Reflection;

namespace BaseGameLogic.Management
{
	[CustomEditor(typeof(GameManager), true)]
	public class BaseGameManagerEditor : Editor 
	{
		private List<FieldInfo> _managerAttribute = new List<FieldInfo>();
		
        [MenuItem("BaseGameLogic/GameManager")]
        public static GameManager CreateInputCollectorManager()
        {
            return GameObjectExtension.CreateInstanceOfAbstractType<GameManager>();
        }

        private GameManager _gameManagerInstance = null;

		private void OnEnable()
		{
			_gameManagerInstance = target as GameManager;
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