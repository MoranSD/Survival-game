using System;
using UnityEngine;

namespace GameItems
{
    internal class Axe : GameItemObject
    {
        protected internal override IGameItemData Data { get; protected set; }

        private GameObject _model;

        private void OnEnable()
        {
            _model = transform.GetChild(0).gameObject;
        }
        internal override void InitData(IGameItemData data)
        {
            
        }
        internal override void Enter()
        {
            _model.SetActive(true);
        }
        internal override void InteractUpdate()
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                Debug.Log("ударил топором");
            }
        }
        internal override void Exit()
        {
            _model.SetActive(false);
        }
    }
}
