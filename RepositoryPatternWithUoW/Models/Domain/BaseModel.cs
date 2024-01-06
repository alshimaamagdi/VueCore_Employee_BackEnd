using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryPatternWithUoW.Core.Models.Domain
{
    public class BaseModel
    {
        public BaseModel()
        {
            CreatedOn = DateTime.Now.ToUniversalTime();
        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public DateTime CreatedOn { get; set; }
        public bool? IsModified { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public bool? IsDeleted { get; set; }
        public DateTime? DeletedOn { get; set; }
        public string? DeletedBy { get; set; }
        public string? CreatedBy { get; set; }
        public string? ModifiedBy { get; set; }

        [ForeignKey("DeletedBy")]
        public virtual Users DeletedUser { get; set; }
        [ForeignKey("CreatedBy")]
        public virtual Users CreatedUser { get; set; }
        [ForeignKey("ModifiedBy")]
        public virtual Users ModifiedUser { get; set; }
    }
}
