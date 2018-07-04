using UnityEngine;
using UnityEngine.Events;

using System;

using BaseGameLogic.Singleton;
using BaseGameLogic.Management.Interfaces;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

namespace BaseGameLogic.Management
{
	public class GameManager : Singleton<GameManager> 
	{
		public InitializeObjects ObjectInitializationCallBack = new InitializeObjects();

        [SerializeField] private GameObject[] managersList = null;

		[SerializeField] private GameStatusEnum _gameStatus = GameStatusEnum.Play;
		public GameStatusEnum GameStatus { get { return this._gameStatus; } }

        private ITimeManager _timeManager = null;

        protected virtual void CreateManagersInstance()
		{
            for (int i = 0; i < managersList.Length; i++)
            {
                var instance = GameObject.Instantiate(managersList[i], transform);
                IInitialize initialize = instance.GetComponentInChildren<IInitialize>();
                if (initialize != null)
                    ObjectInitializationCallBack.AddListener(initialize.Initialize);

                if(_timeManager == null)
                    _timeManager = instance.GetComponentInChildren<ITimeManager>();
            }
		}

		protected virtual void InitializeOtherObjects()
		{
			ObjectInitializationCallBack.Invoke (this);
            ObjectInitializationCallBack.RemoveAllListeners();
        }

		protected override void Awake()
		{
            base.Awake();

            transform.ResetPosition();
            transform.ResetRotation();

			CreateManagersInstance ();

            GatherObjectToInitialize();

            Cursor.visible = false;
        }

        private void GatherObjectToInitialize()
        {
            for (int i = 0; i < SceneManager.sceneCount; i++)
            {
                var rootObjects = SceneManager.GetSceneAt(i).GetRootGameObjects();
                for (int j = 0; j < rootObjects.Length; j++)
                    foreach (var item in rootObjects[j].GetComponents<IInitialize>())
                        ObjectInitializationCallBack.AddListener(item.Initialize);
            }
        }

        protected virtual void Update()
		{
            if(_gameStatus != GameStatusEnum.Loading)
            {
			    this.enabled = false;
			    InitializeOtherObjects ();
            }
		}

		public virtual void PauseGame()
		{
			_gameStatus = GameStatusEnum.Pause;
            if (_timeManager != null)
                _timeManager.Factor = 0f;
        }

        public void LoadGame()
        {
            _gameStatus = GameStatusEnum.Loading;
        }

        public virtual void ResumeGame()
		{
			_gameStatus = GameStatusEnum.Play;
            if (_timeManager != null)
                _timeManager.Factor = 1f;
        }
    }

	[Serializable] public class InitializeObjects : UnityEvent<GameManager> {}
}
