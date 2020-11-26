using System.ComponentModel.DataAnnotations;

namespace VHacksWebstore.Core.Domain
{
    public class Image
    {
        [Key]
        public string Id { get; set; }
        public byte[] Content { get; set; }
    }
}
