using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Movie_review
{
    internal class Review
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int IDReview { get; set; } 
        public int? IDMovie { get; set; }
        public string Moviereview { get; set; } = "";
        public virtual Movie? Movie { get; set; }
    }
}
