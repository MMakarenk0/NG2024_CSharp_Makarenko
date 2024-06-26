using Epic_2._2___Basic_LinQ_Training.Model;

namespace Epic_2._2___Basic_LinQ_Training
{
    public class GameManager
    {
        public IEnumerable<Game> Games { get; private set; }
        public GameManager(IEnumerable<Game> games)
        {
            Games = games;
        }
        public Game? GetGameById(int id)
        {
            return Games.FirstOrDefault(g => g.Id == id);
        }
        public List<Game> GetGamesInPriceRange(decimal minPrice, decimal maxPrice)
        {
            return Games.Where(g => g.Price >= minPrice && g.Price <= maxPrice).ToList();
        }
        public List<Genre> GetGenresByGame(int gameId)
        {
            var game = GetGameById(gameId);
            return game?.Genres.ToList() ?? new List<Genre>();
        }
        public List<string> GetUniqueCategories()
        {
            return Games.Select(g => g.Category).Distinct().ToList();
        }
        public List<Game> FilterByCategoryAndGenres(string filterCategory, List<String> filterGenresNames)
        {
            return Games.Where(g => g.Category == filterCategory && g.Genres.Any(genre => filterGenresNames.Contains(genre.Name))).ToList();
        }
        public List<Game> GetGamesPaginated(int pageNumber, int pageSize = 5)
        {
            return Games.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
        }
    }
}
