using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for TeamObject
/// </summary>
public class TeamObject
{
    public int leagueID { get; set; }
    public string teamName { get; set; }
    public TeamObject(int _leagueId, string _teamName)
    {
        leagueID = _leagueId;
        teamName = _teamName;
    }
}