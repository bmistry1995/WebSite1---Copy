using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class StandardHome : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["userSesh"] == null)
            Response.Redirect("Default.aspx");


        List<string> leaguesLogos = new List<string>();
        leaguesLogos.Add("https://upload.wikimedia.org/wikipedia/en/4/42/Campeonato_Brasileiro_S%C3%A9rie_A_logo.png");
        leaguesLogos.Add("http://www.freelogovectors.net/wp-content/uploads/2012/08/epl-premier-league-logo.png");
        leaguesLogos.Add("https://upload.wikimedia.org/wikipedia/en/c/c3/EFL_Championship.svg");
        leaguesLogos.Add("https://www.efl.com/siteassets/logos/efl-logos/league-one-.png");
        leaguesLogos.Add("https://www.efl.com/siteassets/logos/efl-logos/league-two.png");
        leaguesLogos.Add("https://upload.wikimedia.org/wikipedia/commons/0/0f/Eredivisie_nieuw_logo_2017-.svg");
        leaguesLogos.Add("https://upload.wikimedia.org/wikipedia/commons/thumb/5/5e/Ligue1.svg/630px-Ligue1.svg.png");
        leaguesLogos.Add("https://upload.wikimedia.org/wikipedia/en/thumb/a/a9/Ligue_2_logo.svg/1200px-Ligue_2_logo.svg.png");
        leaguesLogos.Add("https://upload.wikimedia.org/wikipedia/en/thumb/d/df/Bundesliga_logo_%282017%29.svg/1024px-Bundesliga_logo_%282017%29.svg.png");
        leaguesLogos.Add("https://upload.wikimedia.org/wikipedia/en/thumb/7/7b/2._Bundesliga_logo.svg/300px-2._Bundesliga_logo.svg.png");
        leaguesLogos.Add("https://vignette.wikia.nocookie.net/fifa/images/c/c3/Primera_Division_Logo.png/revision/latest?cb=20161117172930");
        leaguesLogos.Add("https://upload.wikimedia.org/wikipedia/en/f/f7/LegaSerieAlogoTIM.png");
        leaguesLogos.Add("https://upload.wikimedia.org/wikipedia/commons/thumb/e/ef/Liga_NOS_logo_white.svg/2000px-Liga_NOS_logo_white.svg.png");
        leaguesLogos.Add("https://upload.wikimedia.org/wikipedia/en/9/9f/DFB-Pokal_logo_2016.svg");
        leaguesLogos.Add("http://www.forza27.com/wp-content/uploads/2017/07/Serie-B-ConTe.it-Imc.jpg");
        leaguesLogos.Add("http://sabonner.beinsports.com/wp-content/uploads/2015/06/ucl_2015-18_logo_silver_on_light1.jpg");
        leaguesLogos.Add("https://upload.wikimedia.org/wikipedia/en/thumb/8/85/Hyundai_A-League_logo_%282017%E2%80%93%29.svg/1200px-Hyundai_A-League_logo_%282017%E2%80%93%29.svg.png");

        List<LeagueObject> leagues = selectLeagues();

        for (int i = 0; i < leagues.Count; i++)
        {
            leagues[i].crestURL = leaguesLogos[i];
        }

        printLeagues.DataSource = leagues;
        printLeagues.DataBind();
    }

    protected void btnLogout_Click(object sender, EventArgs e)
    {
        Session["userSesh"] = null;
        Response.Redirect("Default.aspx");
    }

    public List<LeagueObject> selectLeagues()
    {
        string selectLeaguesConnString = @"workstation id=dbFootyStats.mssql.somee.com;packet size=4096;user id=bmistry1995_SQLLogin_1;pwd=amlcmx81b9;data source=dbFootyStats.mssql.somee.com;persist security info=False;initial catalog=dbFootyStats";
        string selectLeaguesCommString = "SELECT * FROM tblLeagues";

        List<LeagueObject> listLeagues = new List<LeagueObject>();

        using (SqlConnection selectLeaguesConn = new SqlConnection(selectLeaguesConnString))
        {
            using (SqlCommand selectLeaguesComm = new SqlCommand(selectLeaguesCommString, selectLeaguesConn))
            {
                selectLeaguesConn.Open();
                using (SqlDataReader selectLeaguesReader = selectLeaguesComm.ExecuteReader())
                {
                    int indexOfLeagueID = selectLeaguesReader.GetOrdinal("id");
                    int indexOfLeagueName = selectLeaguesReader.GetOrdinal("leagueName");
                    while (selectLeaguesReader.Read())
                    {
                        listLeagues.Add(new LeagueObject(Convert.ToInt32(selectLeaguesReader.GetValue(indexOfLeagueID)) ,selectLeaguesReader.GetValue(indexOfLeagueName).ToString(), ""));
                    }//end while
                }//end reader
            }//end using
        }//end using
        return listLeagues;
    }
}