namespace Agendo.Shared.DTOs
{
    public record DomainDTO
    {
        public int Nr { get; set; }
        public string Name{ get; set;}
        public sealed override string ToString() => Name;

    }
}
