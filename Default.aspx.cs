using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;

public partial class _Default : System.Web.UI.Page
{
    String connString = @"workstation id=dbFootyStats.mssql.somee.com;packet size=4096;user id=bmistry1995_SQLLogin_1;pwd=amlcmx81b9;data source=dbFootyStats.mssql.somee.com;persist security info=False;initial catalog=dbFootyStats";
    protected void Page_Load(object sender, EventArgs e)
    {
    }

    protected void btnLogin_Click(object sender, EventArgs e)
    {
        //check txtboxes empty, if yes, notify
        if (String.IsNullOrWhiteSpace(txtUsername.Text) || String.IsNullOrWhiteSpace(txtPassword.Text))
        {
            lbNotification.Text = "Username and password cannot be empty";
        }
        else
        {
            //if not, check credentials
            using (SqlConnection conn = new SqlConnection(connString))
            {
                SqlDataAdapter da = new SqlDataAdapter("SELECT COUNT(*) FROM tblUsers WHERE username='" + txtUsername.Text + "' AND passwrd='" + txtPassword.Text + "'", conn);
                DataTable dt = new DataTable();
                da.Fill(dt);
                if (dt.Rows[0][0].ToString() == "1")
                {
                    //if credentials correct, proceed to next page, else notify
                    /*
                     * USING bmistry1995 AS ADMIN ACCOUNT
                     * IF USERNAME ISNT THIS, THEN ITS STANDARD ACCOUNT
                     */
                    if(txtUsername.Text == "bmistry1995")
                    {
                        //this is an admin account
                        Session["userSesh"] = txtUsername.Text;
                        Response.Redirect("AdminHome.aspx");
                    }
                    else
                    {
                        //not an admin account
                        Session["userSesh"] = txtUsername.Text;
                        Response.Redirect("StandardHome.aspx");
                    }
                    
                }
                else
                    lbNotification.Text = "Credentials not recognised";
            }
        }
    }

    protected void btnClear_Click(object sender, EventArgs e)
    {
        //clear textboxes
        txtNewUserName.Text = null;
        txtNewPassword.Text = null;
        txtNewName.Text = null;
    }

    protected void btnRegister_Click(object sender, EventArgs e)
    {
        //check textboxes are no empty, if so, notify
        if(String.IsNullOrWhiteSpace(txtNewUserName.Text) || String.IsNullOrWhiteSpace(txtNewPassword.Text) || String.IsNullOrWhiteSpace(txtNewName.Text))
        {
            lbRegisterNotification.Text = "Must fill in all fields";
        }
        else
        {
            //if not empty,check username exists in db
            int userExists = 0;
            String SQLCommand = @"SELECT COUNT(*) from tblUsers where username like @username";
            using (SqlConnection conn = new SqlConnection(connString))
            {
                using (SqlCommand comm = new SqlCommand(SQLCommand, conn))
                {
                    conn.Open();
                    comm.Parameters.AddWithValue("@username", txtNewUserName.Text);
                    userExists = (int)comm.ExecuteScalar();
                    conn.Close();
                    if (userExists > 0)
                    {
                        //username exists, so notify
                        lbRegisterNotification.Text = "Username already exists";
                        txtNewUserName.Text = null;
                        txtNewPassword.Text = null;
                        txtNewName.Text = null;
                    }
                    else
                    {
                        //username doesnt exist and we can add it to the database with the other data
                        lbRegisterNotification.Text = "new user added";
                        addNewUser();//call this method
                        Session["userSesh"] = txtNewUserName.Text;
                        Response.Redirect("StandardHome.aspx");
                    }
                }
            }
        }//end else
    }

    //add new user to db
    public void addNewUser()
    {
        String addNewUserCommand = @"INSERT INTO tblUsers (username, passwrd, usertype, name) VALUES (@userName, @password, 2, @name)";
        using (SqlConnection conn = new SqlConnection(connString))
        {
            using (SqlCommand comm = new SqlCommand(addNewUserCommand, conn))
            {
                comm.Parameters.AddWithValue("@userName", txtNewUserName.Text);
                comm.Parameters.AddWithValue("@password", txtNewPassword.Text);
                comm.Parameters.AddWithValue("@name", txtNewName.Text);
                conn.Open();
                int result = comm.ExecuteNonQuery();

                if (result < 0)
                    lbRegisterNotification.Text = "Error adding new user";
                else
                    lbRegisterNotification.Text = "New user added";

                conn.Close();
                txtNewUserName.Text = null;
                txtNewPassword.Text = null;
                txtNewName.Text = null;

            }
        }
    }
}