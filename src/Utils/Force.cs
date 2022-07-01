using Microsoft.Xna.Framework;

namespace Orion2D;
public class Force {
	//--Methods

	public static Vector2 GenerateDragForce(Body p, float k)
	{
		float velocity_lenS = p.Velocity.LengthSquared();
		if (velocity_lenS == 0) return Vector2.Zero;

		Vector2 force_direction = Vector2.Normalize(p.Velocity) * -1f;
		float magnitude = k * velocity_lenS;

		return force_direction * magnitude;
	}

	public static Vector2 GenerateFrictionForce(Body p, float k)
	{
		if (p.Velocity.LengthSquared() == 0) return Vector2.Zero;

		Vector2 force_direction = Vector2.Normalize(p.Velocity) * -1f;

		return force_direction * k;
	}

	public static Vector2 GenerateGravitationalForce(Body a, Body b, float g)
	{
		Vector2 direction = a.Position - b.Position;
		if (direction.LengthSquared() == 0) return Vector2.Zero;

		float distance_squared = direction.LengthSquared();
		float attraction_magnitude = g * (a.Mass * b.Mass) / distance_squared;

		return Vector2.Normalize(direction) * attraction_magnitude;
	}

	public static Vector2 GenerateSpringForce(Body a, Vector2 anchor, float restLength, float k)
	{
		Vector2 direction = a.Position - anchor;
		if (direction.LengthSquared() == 0) return Vector2.Zero;

		float displacement = direction.Length() - restLength;
		float spring_magnitude = -k * displacement;

		return Vector2.Normalize(direction) * spring_magnitude;
	}
}