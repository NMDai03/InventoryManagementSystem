using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace InventoryManagementSystem
{
    public partial class ProductModuleForm : Form
    {
        SqlConnection con = new SqlConnection(@"Data Source=NGUYENMINHDAI;Initial Catalog=dbIMS;User ID=sa;Password=sa;Integrated Security=True;TrustServerCertificate=Yes");
        SqlCommand cm = new SqlCommand();
        SqlDataReader dr;
        public ProductModuleForm()
        {
            InitializeComponent();
            LoadCategory();
        }

        public void LoadCategory()
        {
            comboCat.Items.Clear();
            List<CategoryItem> categories = new List<CategoryItem>();

            cm = new SqlCommand("SELECT catid, catname FROM tbCategory", con);
            con.Open();
            dr = cm.ExecuteReader();
            while (dr.Read())
            {
                int categoryId = Convert.ToInt32(dr["catid"]);
                CategoryItem category = new CategoryItem
                {
                    CategoryId = categoryId,
                    CategoryName = dr["catname"].ToString()
                };
                categories.Add(category);
            }
            dr.Close();
            con.Close();

            comboCat.DisplayMember = "CategoryName";
            comboCat.ValueMember = "CategoryId";
            comboCat.DataSource = categories;
        }

        

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (MessageBox.Show("Are you sure you want to save this product?", "Saving Record", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    cm = new SqlCommand("INSERT INTO tbProduct(pname,pqty,pprice,pdescription,pcategory)VALUES(@pname,@pqty,@pprice,@pdescription,@pcategory)", con);
                    cm.Parameters.AddWithValue("@pname", txtPName.Text);
                    cm.Parameters.AddWithValue("@pqty", Convert.ToInt16(txtPQty.Text));
                    cm.Parameters.AddWithValue("@pprice", Convert.ToInt16(txtPPrice.Text));
                    cm.Parameters.AddWithValue("@pdescription", txtPDes.Text);
                    cm.Parameters.AddWithValue("@pcategory", comboCat.SelectedValue.ToString());
                    con.Open();
                    cm.ExecuteNonQuery();
                    con.Close();
                    MessageBox.Show("save successfully");
                    Clear();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void Clear()
        {
            txtPName.Clear();
            txtPQty.Clear();
            txtPPrice.Clear();
            txtPDes.Clear();
            comboCat.Text = "";
        }

        private void comboQty_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void pictureBoxClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            Clear();
            btnSave.Enabled = true;
            btnUpdate.Enabled = false;
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                if (MessageBox.Show("Are you sure you want to update this product?", "Update Record", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    cm = new SqlCommand("UPDATE tbProduct SET pname = @pname,pqty = @pqty,pprice = @pprice,pdescription = @pdescription,pcategory = @pcategory WHERE pid LIKE '" + lblPid.Text + "'", con);
                    cm.Parameters.AddWithValue("@pname", txtPName.Text);
                    cm.Parameters.AddWithValue("@pqty", Convert.ToInt16(txtPQty.Text));
                    cm.Parameters.AddWithValue("@pprice", Convert.ToInt16(txtPPrice.Text));
                    cm.Parameters.AddWithValue("@pdescription", txtPDes.Text);
                    cm.Parameters.AddWithValue("@pcategory", comboCat.Text);
                    con.Open();
                    cm.ExecuteNonQuery();
                    con.Close();
                    MessageBox.Show("update successfully");
                    this.Dispose();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void ProductModuleForm_Load(object sender, EventArgs e)
        {

        }
    }
}
