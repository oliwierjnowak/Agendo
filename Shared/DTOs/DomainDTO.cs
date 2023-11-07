namespace Agendo.Server.Models
{
    public record DomainDTO
    {
        public int Nr { get; set; }
        public String Name{ get; set;}
        public sealed override string ToString() => Name;

    }
}
