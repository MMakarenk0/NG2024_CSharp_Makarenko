using Epic_2._2___Basic_LinQ_Training.Model;

namespace Epic_2._2___Basic_LinQ_Training;

public class Game
{
    public int Id { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
    public string Category { get; set; }
    public IEnumerable<Genre> Genres { get; set; }
}
