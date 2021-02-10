using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BackEnd.Models.Models
{
    public abstract class Record
    {
        [Key]
        public string Id { get; set; }

        public Record()
        {
            Id = Guid.NewGuid().ToString();
        }
    }
}
