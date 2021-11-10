using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using MusicHub.Data.Common;

namespace MusicHub.Data.Models
{
    public class Performer
    {
        public Performer()
        {
            this.PerformerSongs = new HashSet<SongPerformer>();
        }
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(Constants.PERFORMER_FIRST_NAME_MAX_LENGTH)]
        public string FirstName { get; set; }
        [Required]
        [MaxLength(Constants.PERFORMER_LAST_NAME_MAX_LENGTH)]
        public string LastName { get; set; }
        [Required]
        public int Age { get; set; }
        [Required]
        public decimal NetWorth { get; set; }

        public ICollection<SongPerformer> PerformerSongs { get; set; }

    }
}