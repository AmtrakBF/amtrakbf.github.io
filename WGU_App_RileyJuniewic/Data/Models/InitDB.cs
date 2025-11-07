using System.ComponentModel.DataAnnotations.Schema;

namespace WGU_App_RileyJuniewic.Data.Models;

[Table("InitDB")]
public class InitDB
{
    public bool IsInitialized { get; set; } = false;
}