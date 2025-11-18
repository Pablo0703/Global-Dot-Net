using Presentation.Hateoas;

public class UsuarioDTO
{
    public int Id { get; set; }
    public string NomeCompleto { get; set; }
    public string Email { get; set; } = "";
    public string Password { get; set; } = "";

    public string Role { get; set; } = "USER";
    public string? Bio { get; set; }
    public string? AvatarUrl { get; set; }
    public string? Location { get; set; }
    public string? Timezone { get; set; }
    public string? LinkedinUrl { get; set; }

    public double TimeCredits { get; set; } = 0;
    public int TotalSessionsGiven { get; set; }
    public int TotalSessionsTaken { get; set; }
    public double AverageRating { get; set; }

    public DateTime CreatedDate { get; set; }

    // 🔗 HATEOAS LINKS
    public List<LinkDTO>? Links { get; set; }
}
