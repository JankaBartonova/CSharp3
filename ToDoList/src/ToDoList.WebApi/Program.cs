var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => "Hello World!");
app.MapGet("/test", () => "Test");
app.MapGet("/czechitas", () => "Czechitas");
app.MapGet("/greetings/{name}", (string name) => $"Hello {name}!");
app.MapGet("/add/{a:int}/{b:int}", (int a, int b) => $"Sum: {a} + {b} = {a + b}");
app.MapGet("/nazdarSvete", () => "Nazdar světe!");

app.Run();
