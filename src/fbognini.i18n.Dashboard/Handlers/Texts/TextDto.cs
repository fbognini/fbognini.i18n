namespace fbognini.i18n.Dashboard.Handlers.Texts
{
    public class TextDto
    {
        public string Id => $"{TextId}|{ResourceId}";
        public string TextId { get; set; }
        public string ResourceId { get; set; }
        public string Description { get; set; }
        public DateTime Created { get; set; }
    }
}
