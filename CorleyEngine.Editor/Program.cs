using System;

namespace CorleyEngine.Editor;

internal class Program {

    [STAThread]
    static void Main(string[] args) {
        using CorleyEditor game = new ();
        game.Run();
    }
}