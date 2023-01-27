using System.Collections.Generic;
using UnityEngine;

namespace Weapon
{
	internal class BladeCollider : MonoBehaviour
	{
		[SerializeField] private Vector3 _boundsSize;

		internal List<T> GetAllFoundedTargets<T>()
        {
			Collider[] colliders = Physics.OverlapBox(transform.position, _boundsSize, transform.rotation);

			List<T> targets = new List<T>();

            for (int i = 0; i < colliders.Length; i++)
            {
				if (colliders[i].TryGetComponent(out T target))
					targets.Add(target);
            }

			return targets;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.green;

            Gizmos.DrawWireCube(transform.position, _boundsSize);
        }
    }
}
