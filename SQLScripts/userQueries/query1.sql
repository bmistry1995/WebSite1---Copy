--show games where teamX has scored more/less/equal to Ygoals 
select * from tblFixtures 
where leagueID = 445 
and 
((homeTeam = 'Manchester United FC' and homeGoalsFT < 2)
or (awayTeam = 'Manchester United FC' and awayGoalsFT < 2))
and matchStatus = 'FINISHED'
order by fixtureDate