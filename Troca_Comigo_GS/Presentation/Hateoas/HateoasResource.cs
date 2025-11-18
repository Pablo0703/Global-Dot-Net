namespace Presentation.Hateoas
{
    public class LinkDTO
    {
        public string Href { get; set; }
        public string Rel { get; set; }
        public string Method { get; set; }

        public LinkDTO(string href, string rel, string method)
        {
            Href = href;
            Rel = rel;
            Method = method;
        }
    }

    public class ResourceDTO<T>
    {
        public T Data { get; set; }
        public List<LinkDTO> Links { get; set; } = new();

        public ResourceDTO(T data)
        {
            Data = data;
        }

        public void AddLink(string href, string rel, string method)
        {
            if (href != null)
                Links.Add(new LinkDTO(href, rel, method));
        }
    }
}
