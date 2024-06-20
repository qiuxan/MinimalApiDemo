using MinimalApiDemo.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<IPostService, PostService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast = Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast")
.WithOpenApi();

var list = new List<Post>()
{
    new() { Id = 1, Title = "First Post", Content = "Hello World" },
    new() { Id = 2, Title = "Second Post", Content = "Hello Again" },
    new() { Id = 3, Title = "Third Post", Content = "Goodbye World" },
};


app.MapGet("/posts", async( IPostService postService) => {

    var posts = await postService.GetPosts();
    return posts;
}

).WithName("GetPosts").WithOpenApi().WithTags("Posts");

app.MapPost(
    "/posts",
    async(IPostService postService,Post post) =>
        {
            var newPost = await postService.CreatePost(post);
            return Results.Created($"/posts/{post.Id}", newPost);
        }
    ).WithName("CreatePost").WithOpenApi().WithTags("Posts");


app.MapGet(
    "/posts/{id}",
    async(IPostService postService,int id) =>
        {
            var post = await postService.GetPost(id);
            return post == null ? Results.NotFound() : Results.Ok(post);
        })
    .WithName("GetPost").WithOpenApi().WithTags("Posts");


app.MapPut(
    "/posts/{id}", 
    async(IPostService postService,int id, Post post) =>
        {
           var updatedPost = await postService.UpdatePost(id, post);
            return Results.Ok(updatedPost);
        })
    .WithName("UpdatePost").WithOpenApi().WithTags("Posts");


app.MapDelete(
    "/posts/{id}", 
    async(IPostService postService,int id) =>
        {
            postService.DeletePost(id);
            return Results.Ok();
        }).WithName("DeletePost").WithOpenApi().WithTags("Posts");

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}

public class Post
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
}




