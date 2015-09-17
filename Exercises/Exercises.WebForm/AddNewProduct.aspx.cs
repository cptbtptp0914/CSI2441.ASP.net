using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Exercises.WebForm
{
    public partial class AddNewProduct : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void ButtonAddProduct_Click(object sender, EventArgs e)
        {
            
        }

        // button event handler
        protected void ButtonAddProduct_OnClick(object sender, EventArgs e)
        {
            AddNewProductSQL.Insert();
        }
    }
}