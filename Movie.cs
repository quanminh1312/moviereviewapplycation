using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Movie_review
{
    internal class Movie
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int IDMovie { get; set; }
        public string Name { get; set; } = "";
        public string Year { get; set; } = "";
        public string Director { get; set; } = "";
        public string Country { get; set; } = "";

        public virtual List<Review>? reviews { get; set; }

    }
}
