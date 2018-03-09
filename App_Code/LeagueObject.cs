using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for LeagueObject
/// </summary>
public class LeagueObject
{
    public int leagueID { get; set; }
    public string leagueName { get; set; }
    public string crestURL { get; set; }
    public LeagueObject(int _leagueID, string _leagueName, string _crestURL)
    {
        leagueID = _leagueID;
        leagueName = _leagueName;
        crestURL = _crestURL;
    }
}