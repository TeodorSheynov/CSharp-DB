using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using Castle.DynamicProxy.Generators.Emitters;
using MusicHub.Data.Common;

namespace MusicHub.Data.Models
{
    public class Album
    {
        public Album()
        {
            this.Songs = new HashSet<Song>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(Constants.ALBUM_NAME_MAX_LENGTH)]
        public string Name { get; set; }

        public decimal Price => Songs.Sum(x => x.Price);

        [Required]
        public DateTime ReleaseDate { get; set; }

        [ForeignKey(nameof(Producer))]
        public int? ProducerId { get; set; }
        public virtual Producer Producer { get; set; }



        public virtual ICollection<Song> Songs { get; set; }   
        
    }
}