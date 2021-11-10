using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using MusicHub.Data.Common;

namespace MusicHub.Data.Models
{
    public class Producer
    {
        public Producer()
        {
            this.Albums = new HashSet<Album>();
        }


        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(Constants.PRODUCER_NAME_MAX_LENGTH)]
        public string Name { get; set; }

        public string Pseudonym { get; set; }

        public string PhoneNumber { get; set; }

        public virtual ICollection<Album> Albums { get; set; }

    }
}