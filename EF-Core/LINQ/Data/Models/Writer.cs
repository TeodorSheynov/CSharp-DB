using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using MusicHub.Data.Common;

namespace MusicHub.Data.Models
{
    public class Writer
    {
        public Writer()
        {
            this.Songs = new HashSet<Song>();
        }
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(Constants.WRITER_NAME_MAX_LENGTH)]
        public string Name { get; set; }

        public string Pseudonym { get; set; }

        public virtual ICollection<Song> Songs { get; set; }

    }
}