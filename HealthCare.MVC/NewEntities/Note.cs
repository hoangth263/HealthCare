using System;
using System.Collections.Generic;

namespace HealthCare.MVC.Entities
{
    public partial class Note
    {
        public int Id { get; set; }
        public int AsignId { get; set; }
        public string Title { get; set; } = null!;
        public string Content { get; set; } = null!;
        public string Type { get; set; } = null!;
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }

        public virtual Asign Asign { get; set; } = null!;
    }
}
