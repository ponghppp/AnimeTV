namespace TVAnime.Models
{
    internal class Category
    {
        public string Id { get; set; }
        public string Title { get; set; }

        public Category(string id, string title)
        {
            Id = id;
            Title = title;
        }
    }
}
