using System.ComponentModel.DataAnnotations;

namespace WPFPrism.Infrastructure.Base
{
    public abstract class ModelBase
    {
        [Key]
        public int Id { get; set; }
    }
}
