namespace DataLayer.Entities
{
    public class Item : IEntity
    {
        public Guid? SenderId { get; set; }
        public Client? Sender { get; set; }
        public Guid? ReceiverId { get; set; }
        public Client? Receiver { get; set; }
        public float Weight { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public ICollection<ItemCategory> ItemCategories { get; set; } = new List<ItemCategory>();
        public Guid? StorageId { get; set; }
        public Storage? Storage { get; set; }
    }
}

