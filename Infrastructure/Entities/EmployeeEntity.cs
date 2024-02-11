using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Infrastructure.Dtos;

namespace Infrastructure.Entities
{
    [Index(nameof(Email), IsUnique = true)]
    public class EmployeeEntity
    {
        [Key]
        public int EmployeeId { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(50)")]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [Column(TypeName = "nvarchar(50)")]
        public string LastName { get; set; } = string.Empty;

        [Required]
        [Column(TypeName = "nvarchar(100)")]
        public string Email { get; set; } = string.Empty;

        public virtual ICollection<EmployeePhoneNumberEntity> EmployeePhoneNumbers { get; set; } = new HashSet<EmployeePhoneNumberEntity>();
        public virtual ICollection<EmployeeAddressEntity> EmployeeAddresses { get; set; } = new HashSet<EmployeeAddressEntity>();

        [DataType(DataType.Date)]
        public DateTime BirthDate { get; set; } = default;

        public char Gender { get; set; } = default;

        [Required]
        [ForeignKey(nameof(Department))]
        public int DepartmentId { get; set; }
        public virtual DepartmentEntity? Department { get; set; } = null!;

        [Required]
        [ForeignKey(nameof(Position))]
        public int PositionId { get; set; }
        public virtual PositionEntity? Position { get; set; } = null!;

        [Required]
        [ForeignKey(nameof(Salary))]
        public int SalaryId { get; set; }
        public virtual SalaryEntity? Salary { get; set; } = null!;

        [Required]
        [ForeignKey(nameof(Skill))]
        public int SkillId { get; set; }
        public virtual SkillEntity? Skill { get; set; } = null!;

        public static implicit operator EmployeeEntity(CreateEmployeeDto createEmployeeDto)
        {
            var employeeEntity = new EmployeeEntity
            {
                FirstName = createEmployeeDto?.FirstName ?? string.Empty,
                LastName = createEmployeeDto?.LastName ?? string.Empty,
                Email = createEmployeeDto?.Email ?? string.Empty,
                EmployeePhoneNumbers = (createEmployeeDto?.PhoneNumbers ?? new List<string>())
                    .Select(number => new EmployeePhoneNumberEntity { PhoneNumber = number })
                    .ToList(),

                EmployeeAddresses = (createEmployeeDto?.Addresses ?? new List<AddressDto>())
                    .Select(address => new EmployeeAddressEntity
                    {
                        Address = new AddressEntity
                        {
                            StreetName = address?.StreetName ?? string.Empty,
                            StreetNumber = address?.StreetNumber ?? string.Empty,
                            PostalCode = address?.PostalCode ?? string.Empty,
                            City = address?.City ?? string.Empty
                        }
                    })
                    .ToList(),
                BirthDate = createEmployeeDto?.BirthDate ?? DateTime.MinValue,
                Gender = createEmployeeDto?.Gender ?? 'U',
                DepartmentId = createEmployeeDto?.DepartmentId ?? 0,
                PositionId = createEmployeeDto?.PositionId ?? 0,
                SalaryId = createEmployeeDto?.Salary?.SalaryId ?? 0,
                SkillId = createEmployeeDto?.SkillDto?.SkillId ?? 0,
            };


            return employeeEntity;
        }

        public static implicit operator UpdatedEmployeeDto(EmployeeEntity? entity)
        {
            var updatedEmployeeDto = new UpdatedEmployeeDto
            {
                Id = entity?.EmployeeId ?? 0,
                FirstName = entity?.FirstName ?? string.Empty,
                LastName = entity?.LastName ?? string.Empty,
                Email = entity?.Email ?? string.Empty,
                PhoneNumbers = entity?.EmployeePhoneNumbers?.Select(x => x.PhoneNumber),
                Address = entity?.EmployeeAddresses?.FirstOrDefault()?.Address != null
                    ? new AddressDto
                    {
                        StreetName = entity.EmployeeAddresses.First().Address.StreetName ?? string.Empty,
                        StreetNumber = entity.EmployeeAddresses.First().Address.StreetNumber ?? string.Empty,
                        PostalCode = entity.EmployeeAddresses.First().Address.PostalCode ?? string.Empty,
                        City = entity.EmployeeAddresses.First().Address.City ?? string.Empty
                    }
                    : null,
                BirthDate = entity?.BirthDate ?? DateTime.MinValue,
                Gender = entity?.Gender ?? 'U',
                Salary = entity?.Salary,
                DepartmentName = entity?.Department?.DepartmentName ?? string.Empty,
                SkillName = entity?.Skill?.SkillName ?? string.Empty,
                PositionName = entity?.Position?.PositionName ?? string.Empty
            };

            return updatedEmployeeDto;
        }

        public static implicit operator EmployeeDto(EmployeeEntity? entity)
        {
            return new EmployeeDto
            {
                Id = entity?.EmployeeId ?? 0,
                FirstName = entity?.FirstName ?? string.Empty,
                LastName = entity?.LastName ?? string.Empty,
                EmployeePhoneNumbers = entity?.EmployeePhoneNumbers?.Select(x => new EmployeePhoneNumberEntity { PhoneNumber = x.PhoneNumber }).ToList(),
                EmployeeAddresses = entity?.EmployeeAddresses?.Select(x => new EmployeeAddressEntity { Address = x.Address }).ToList(),
                BirthDate = entity?.BirthDate ?? DateTime.MinValue,
                Gender = entity?.Gender ?? default,
                DepartmentId = entity?.DepartmentId ?? 0,
                PositionId = entity?.PositionId ?? 0,
                SalaryId = entity?.SalaryId ?? 0,
                SkillId = entity?.SkillId ?? 0,
            };
        }

        public static IEnumerable<EmployeeDto> Create(IEnumerable<EmployeeEntity> employeeEntities)
        {
            List<EmployeeDto> employeeDtos = new List<EmployeeDto>();

            foreach (var entity in employeeEntities)
            {
                var employeeDto = new EmployeeDto
                {
                    Id = entity.EmployeeId,
                    FirstName = entity.FirstName,
                    LastName = entity.LastName,
                    Email = entity.Email,
                    EmployeePhoneNumbers = entity?.EmployeePhoneNumbers?.Select(x => new EmployeePhoneNumberEntity { PhoneNumber = x.PhoneNumber }).ToList(),
                    EmployeeAddresses = entity?.EmployeeAddresses?.Select(x => new EmployeeAddressEntity { Address = x.Address }).ToList(),
                    BirthDate = entity?.BirthDate ?? DateTime.MinValue,
                    Gender = entity?.Gender ?? default,
                    DepartmentId = entity?.DepartmentId ?? 0,
                    PositionId = entity?.PositionId ?? 0,
                    SalaryId = entity?.SalaryId ?? 0,
                    SkillId = entity?.SkillId ?? 0,
                };

                employeeDtos.Add(employeeDto);
            }

            return employeeDtos;
        }
    }
}
