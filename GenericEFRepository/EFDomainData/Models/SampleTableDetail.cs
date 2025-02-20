using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFDomainData.Models
{
    public partial class SampleTableDetail
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        [StringLength(50)]
        [Unicode(false)]
        public string Detail { get; set; } = null!;     
        public long SampleTableId { get; set; }
        [ForeignKey("SampleTableId")]
        public virtual SampleTable SampleTable { get; set; } = null!;
    }
}
