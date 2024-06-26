using Epic_2._2___Basic_LinQ_Training;
using Epic_2._2___Basic_LinQ_Training.Data;

var gameManager = new GameManager(DataSource.games);

var game1 = gameManager.GetGameById(1);
Console.WriteLine(game1.Name);

Console.WriteLine();

var cheapGames = gameManager.GetGamesInPriceRange(0m, 20m);
cheapGames.ForEach(game => Console.WriteLine($"{game.Name}: ${game.Price}"));

Console.WriteLine();

var gameGenres = gameManager.GetGenresByGame(1);
gameGenres.ForEach(game => Console.WriteLine(game.Name));

Console.WriteLine();

var uniqueCategories = gameManager.GetUniqueCategories();
uniqueCategories.ForEach(category => Console.WriteLine(category));

Console.WriteLine();

var filteredGames = gameManager.FilterByCategoryAndGenres("Horror", new List<string>
{
    "Psychological Horror"
});
filteredGames.ForEach(game => Console.WriteLine(game.Name));

Console.WriteLine();

var paginatedGames = gameManager.GetGamesPaginated(1);
paginatedGames.ForEach(game => Console.WriteLine(
    $"Name: {game.Name}\n" +
    $"Price: ${game.Price}\n" +
    $"Category: {game.Category}\n" +
    $"Genres: {string.Join(", ", game.Genres.Select(g => g.Name))}\n" +
    $"-----------------------------"));
