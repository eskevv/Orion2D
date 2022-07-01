using System;

namespace Orion2D;

public static class Program {
	[STAThread]
	static void Main()
	{
		using (var game = new CoreGame())
			game.Run();
	}
}
