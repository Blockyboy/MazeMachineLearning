using System.Drawing;
using System.Drawing.Imaging;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddHttpClient();

var app = builder.Build();


app.MapPost("/modify", async (IFormFile image, char agent, int generations, int maximumSteps, bool noRecursion, bool stepLimit, double alpha, double gamma, double epsillon, double epsillonDecay
) =>
{
    if (image.Length == 0)
        return Results.BadRequest("No image supplied.");

    await using var stream = image.OpenReadStream();

    using var original = new Bitmap(stream);

    Agent chosenAgent = new QLearningAgent(4);

    if (agent == 's')
    {
        chosenAgent = new SARSAAgent(4);        
    }

    Environment environment = new Environment(chosenAgent);

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