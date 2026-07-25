using AutoTest.Domain.Enums;

namespace AutoTest.Domain.StorageModels;

public record Vehicle(string Make = "", string Model = "", int Displacement = 0, Induction Induction = default, string Registration = "");
