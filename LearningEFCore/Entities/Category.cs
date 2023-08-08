using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearningEFCore.Entities;
public class Category {
    //properties for map to columns
    [Required]
    public int CategoryId { get; set; }
    public string? CategoryName { get; set; }
    [Column(TypeName ="ntext")]
    public string? Description { get; set; }
    //navigation Property
    public  virtual ICollection<Product> Products { get; set; }
    public Category() {

        Products = new HashSet<Product>();
    }



}

