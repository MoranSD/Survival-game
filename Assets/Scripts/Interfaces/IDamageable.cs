namespace Interfaces
{

	internal interface IDamageable
	{
		Enums.DamageableTargetType type { get; set; }
		void ApplyDamage(float damage);
	}
}
