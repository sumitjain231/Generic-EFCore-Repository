namespace EFDomainModel
{
    public class SampleTableDTO
    {
        public long Id { get; set; }
        public string Name { get; set; } = null!;
        public List<SampleTableDetailDTO> SampleTableDetail { get; set; } = null!;
    }
}
