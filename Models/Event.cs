using System.ComponentModel.DataAnnotations;

namespace tower.Models
{
  public class TowerEvent
  {
    public int Id { get; set; }
    public string CreatedAt { get; set; }
    public string UpdatedAt { get; set; }
    [Required]
    [MinLength(3)]
    [MaxLength(25)]
    public string Name { get; set; }
    public string ImgUrl { get; set; }
    public string Location { get; set; }
    public string CreatorId { get; set; }
    // TODO populate Creator
    public Profile Creator { get; set; }
  }
}