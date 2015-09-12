using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

/**
 * Validation happens here.
 */
namespace MVC.Web.Tutorial.Models
{
    public class TestFormModel
    {
        // required tag used for validation client or server side
        [Required(ErrorMessage = "Required")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Required")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Required")]
        public string Phone { get; set; }

        // can do regex here!
//        [RegularExpression("regexPattern", ErrorMessage = "message")]
        // some attribute
    }
}