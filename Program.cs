using Raylib_cs;

Raylib.InitWindow(800, 480, "Corely Engine Test");
Raylib.SetTargetFPS(60);

while (!Raylib.WindowShouldClose())
{
    Raylib.BeginDrawing();
    Raylib.ClearBackground(Color.DarkBlue);
    Raylib.DrawText("Raylib is connected.", 12, 12, 20, Color.RayWhite);
    Raylib.EndDrawing();
}

Raylib.CloseWindow();