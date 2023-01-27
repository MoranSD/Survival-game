using UnityEngine;

namespace GameItems
{
	internal abstract class GameItemObject : MonoBehaviour
	{
		protected internal abstract IGameItemData Data { get; protected set; }
		internal abstract void InitData(IGameItemData data);
		protected internal abstract bool IsActive { get; protected set; }
		internal virtual void Enter()
		{

		}
		internal virtual void InteractUpdate()
		{

		}
		internal virtual void Exit()
		{
			
		}
	}
}
