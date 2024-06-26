using Epic_2._2___Basic_LinQ_Training.Model;

namespace Epic_2._2___Basic_LinQ_Training.Data;

public static class DataSource
{
    public static IEnumerable<Game> games { get; set; } = new List<Game>()
    {
        new Game
            {
                Id = 1,
                Name = "The Witcher 3: Wild Hunt",
                Price = 39.99m,
                Category = "RPG",
                Genres = new List<Genre>
                {
                    new Genre { Name = "Fantasy", Description = "Games set in a fictional universe, often with magical elements." },
                    new Genre { Name = "Adventure", Description = "Games that focus on exploration and puzzle-solving." }
                }
            },
            new Game
            {
                Id = 2,
                Name = "Cyberpunk 2077",
                Price = 59.99m,
                Category = "RPG",
                Genres = new List<Genre>
                {
                    new Genre { Name = "Sci-Fi", Description = "Games set in a futuristic universe with advanced technology." },
                    new Genre { Name = "Open World", Description = "Games that provide a large, open environment for players to explore." }
                }
            },
            new Game
            {
                Id = 3,
                Name = "Minecraft",
                Price = 26.95m,
                Category = "Sandbox",
                Genres = new List<Genre>
                {
                    new Genre { Name = "Creative", Description = "Games that emphasize creativity and building." },
                    new Genre { Name = "Survival", Description = "Games that involve resource gathering and survival mechanics." }
                }
            },
            new Game
            {
                Id = 4,
                Name = "Dark Souls III",
                Price = 49.99m,
                Category = "Action RPG",
                Genres = new List<Genre>
                {
                    new Genre { Name = "Dark Fantasy", Description = "Games set in a grim, dark fantasy world." },
                    new Genre { Name = "Action", Description = "Games that emphasize physical challenges and combat." }
                }
            },
            new Game
            {
                Id = 5,
                Name = "Metal Gear Rising: Revengeance",
                Price = 19.99m,
                Category = "Action",
                Genres = new List<Genre>
                {
                    new Genre { Name = "Hack and Slash", Description = "Games that focus on melee combat." },
                    new Genre { Name = "Sci-Fi", Description = "Games set in a futuristic universe with advanced technology." }
                }
            },
            new Game
            {
                Id = 6,
                Name = "Factorio",
                Price = 30.00m,
                Category = "Simulation",
                Genres = new List<Genre>
                {
                    new Genre { Name = "Management", Description = "Games that involve managing resources and production." },
                    new Genre { Name = "Strategy", Description = "Games that require planning and tactical thinking." }
                }
            },
            new Game
            {
                Id = 7,
                Name = "Subnautica",
                Price = 29.99m,
                Category = "Adventure",
                Genres = new List<Genre>
                {
                    new Genre { Name = "Survival", Description = "Games that involve resource gathering and survival mechanics." },
                    new Genre { Name = "Exploration", Description = "Games that focus on exploring unknown worlds." }
                }
            },
            new Game
            {
                Id = 8,
                Name = "Cry of Fear",
                Price = 0.00m,
                Category = "Horror",
                Genres = new List<Genre>
                {
                    new Genre { Name = "Psychological Horror", Description = "Games that focus on psychological fear and tension." },
                    new Genre { Name = "Survival Horror", Description = "Games that involve surviving in a horror environment." }
                }
            },
            new Game
            {
                Id = 9,
                Name = "Outlast",
                Price = 19.99m,
                Category = "Horror",
                Genres = new List<Genre>
                {
                    new Genre { Name = "Survival Horror", Description = "Games that involve surviving in a horror environment." },
                    new Genre { Name = "First-Person", Description = "Games that are played from a first-person perspective." }
                }
            },
            new Game
            {
                Id = 10,
                Name = "Battlefield 4",
                Price = 19.99m,
                Category = "FPS",
                Genres = new List<Genre>
                {
                    new Genre { Name = "Shooter", Description = "Games that involve shooting enemies and other targets." },
                    new Genre { Name = "Multiplayer", Description = "Games that are played with or against other players." }
                }
            }
    };
}
