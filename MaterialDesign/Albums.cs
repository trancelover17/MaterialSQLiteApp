namespace MaterialDesign
{
    public class Albums
    {
        [System.ComponentModel.DataAnnotations.Key]
        public int AlbumId { get; set; }
        public string Title { get; set; }
        public int ArtistId { get; set; }
    }
}
