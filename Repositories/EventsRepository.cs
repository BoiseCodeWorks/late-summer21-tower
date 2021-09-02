using System.Collections.Generic;
using System.Data;
using System.Linq;
using Dapper;
using tower.Models;

namespace tower.Repositories
{
  public class EventsRepository
  {
    private readonly IDbConnection _db;

    public EventsRepository(IDbConnection db)
    {
      _db = db;
    }

    internal List<TowerEvent> Get()
    {
      string sql = @"
      SELECT 
        a.*,
        e.*
      FROM events e
      JOIN accounts a ON e.creatorId = a.id
      ";
      // data type 1, data type 2, return type
      return _db.Query<Profile, TowerEvent, TowerEvent>(sql, (profile, towerEvent) =>
      {
        towerEvent.Creator = profile;
        return towerEvent;
      }, splitOn: "id").ToList();
    }

    internal TowerEvent Get(int id)
    {
      string sql = @"
      SELECT 
        a.*,
        e.*
      FROM events e
      JOIN accounts a ON e.creatorId = a.id
      WHERE e.id = @id
      ";
      // data type 1, data type 2, return type
      return _db.Query<Profile, TowerEvent, TowerEvent>(sql, (profile, towerEvent) =>
      {
        towerEvent.Creator = profile;
        return towerEvent;
      }, new { id }, splitOn: "id").FirstOrDefault();
    }

    // NOTE GET BY ONE TO MANY
    internal TowerEvent GetByCreatorId(string id)
    {
      string sql = @"
      SELECT 
        a.*,
        e.*
      FROM events e
      JOIN accounts a ON e.creatorId = a.id
      WHERE e.creatorId = @id
      ";
      // data type 1, data type 2, return type
      return _db.Query<Profile, TowerEvent, TowerEvent>(sql, (profile, towerEvent) =>
      {
        towerEvent.Creator = profile;
        return towerEvent;
      }, new { id }, splitOn: "id").FirstOrDefault();
    }

    internal TowerEvent Create(TowerEvent newTowerEvent)
    {
      string sql = @"
        INSERT INTO events
        (name, imgUrl, location, creatorId)
        VALUES
        (@Name, @ImgUrl, @Location, @CreatorId);
        SELECT LAST_INSERT_ID();
        ";
      int id = _db.ExecuteScalar<int>(sql, newTowerEvent);
      return Get(id);
    }

    internal void Delete(int id)
    {
      string sql = "DELETE FROM events WHERE id = @id LIMIT 1";
      _db.Execute(sql, new { id });
    }
  }
}