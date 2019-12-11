using System;

namespace BalloonPop {
  public static class Program {
    [STAThread]
    static void Main() {
      using (var game = new BalloonPopGame())
        game.Run();
    }
  }
}