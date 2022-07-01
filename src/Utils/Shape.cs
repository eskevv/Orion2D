using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Orion2D;
public enum ShapeType {
	Circle,
	Polygon,
	Box
}

public class Shape {
	// __Fields__

	public ShapeType Type { get; set; }
}

public class CircleShape : Shape {
	// __Fields__

	public float Radius { get; set; }

	public CircleShape(float radius)
	{
		Type = ShapeType.Circle;
		Radius = radius;
	}
}

public class PolygonShape : Shape {
	// __Fields__

	private List<Vector2> _vertices;

	public PolygonShape(List<Vector2> vertices)
	{
		Type = ShapeType.Polygon;
		_vertices = vertices;
	}
}

public class BoxShape : Shape {
	// __Fields__

	private List<Vector2> _vertices;
	public int Width { get; set; }
	public int Height { get; set; }

	public BoxShape(int width, int height)
	{
		Type = ShapeType.Box;
		Width = width;
		Height = height;
	}
}