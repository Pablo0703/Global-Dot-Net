public class UsuarioDTO
{
    public Guid Id { get; set; }

    public required string Email { get; set; }

    public required string FullName { get; set; }

    public required string Password { get; set; }

    public required string Role { get; set; }

    public string? Bio { get; set; }

    public string? AvatarUrl { get; set; }

    public required double TimeCredits { get; set; }

    public int TotalSessionsGiven { get; set; }

    public int TotalSessionsTaken { get; set; }

    public double AverageRating { get; set; }

    public string? Location { get; set; }

    public string? Timezone { get; set; }

    public string? LinkedinUrl { get; set; }

    public DateTime CreatedDate { get; set; }

    public DateTime UpdatedDate { get; set; }
}
