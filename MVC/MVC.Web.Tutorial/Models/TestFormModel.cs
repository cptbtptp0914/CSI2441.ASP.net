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
        [Required]
        public string Name { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Phone { get; set; }
    }
}