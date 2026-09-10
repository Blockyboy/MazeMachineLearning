using System.Drawing;
using System.Drawing.Imaging;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("TestWebsite", policy =>
    {
        policy
            .AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});


builder.Services.AddControllers();
builder.Services.AddHttpClient();

var app = builder.Build();

app.UseCors("TestWebsite");

app.MapPost("/modify", async ([FromForm] IFormFile image,
    [FromForm] string agent,
    [FromForm] int generations,
    [FromForm] int maximumSteps,
    [FromForm] bool noRecursion,
    [FromForm] bool stepLimit,
    [FromForm] double alpha,
    [FromForm] double gamma,
    [FromForm] double epsillon,
    [FromForm] double epsillonDecay
) =>
{
    if (image.Length == 0)
        return Results.BadRequest("No image supplied.");

    if(!(0 > alpha && alpha >= 1) ||
    !(0 > gamma && gamma >= 1) ||
    !(0 > epsillon && epsillon >= 1) ||
    !(0 > epsillonDecay && epsillonDecay >= 1) )
    {
        return Results.BadRequest("Alpha, gamma, epsillon and epsillonDecay must be between 0 and 1 inclusive");
    }

    if(!(generations > 0))
    {
        return Results.BadRequest("Must be 1 or more generations");
    }

    await using var stream = image.OpenReadStream();

    using var original = new Bitmap(stream);

    Agent chosenAgent = new QLearningAgent(4, alpha, gamma, epsillon, epsillonDecay);

    if (agent == "s")
    {
        chosenAgent = new SARSAAgent(4, alpha, gamma, epsillon, epsillonDecay);        
    }

    Environment environment = new Environment(chosenAgent);

    environment.noRecursion = noRecursion;
    environment.stepLimit = stepLimit;
    environment.maximumSteps = maximumSteps;

    Bitmap result = environment.APILearning(original);

    using var outputStream = new MemoryStream();

    result.Save(outputStream, ImageFormat.Bmp);

    return Results.File(
        outputStream.ToArray(),
        "image/bmp",
        "modified.bmp"
    );
})
.DisableAntiforgery();


app.Run();