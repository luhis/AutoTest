using System.ComponentModel.DataAnnotations;
using AutoTest.Domain.Enums;

namespace AutoTest.Web.Models.Save;

public class EntrantSaveModel
{
    [Required]
    public ushort DriverNumber { get; set; }

    [Required]
    public string GivenName { get; set; } = string.Empty;

    [Required]
    public string FamilyName { get; set; } = string.Empty;

    [Required]
    public string Class { get; set; } = string.Empty;

    [Required]
    public string Email { get; set; } = string.Empty;

    [Required]
    public Age Age { get; set; } = Age.Senior;

    [Required]
    public VehicleSaveModel Vehicle { get; set; } = new();

    [Required]
    public MsaMembershipSaveModel MsaMembership { get; set; } = new();

    [Required]
    public EmergencyContactSaveModel EmergencyContact { get; set; } = new();

    [Required]
    public AcceptDeclarationSaveModel AcceptDeclaration { get; set; } = new();

    [Required]
    public EntrantClubSaveModel EntrantClub { get; set; } = new();

    [Required]
    public bool IsLady { get; set; }

    public ulong? DoubleDrivenWith { get; set; }
}
