using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearningEFCore.Entities;
public class Product {
    public int ProductId { get; set; }
    [Required]
    [StringLength(40)]
    public string ProductName { get; set; } = null!;
    // property name != column name
    [Column("UnitPrice", TypeName = "money")]
    public decimal? Cost { get; set; } 
    [Column("UnitsInStock")]
    public short? Stock { get; set; }
    public bool Discontinued { get; set; }
    //the foreign key relationship to Category table
    public int CategoryId { get; set; }
    public virtual Category Category { get; set; } = null!;

}

